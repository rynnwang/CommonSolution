using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using Beyova.Configuration;
using Beyova.ProgrammingIntelligence;
using Newtonsoft.Json.Linq;

namespace Beyova.Gravity
{
    /// <summary>
    /// Class GravityShell.
    /// </summary>
    internal class GravityShell
    {
        /// <summary>
        /// The _client key
        /// </summary>
        private Guid? _clientKey = null;

        /// <summary>
        /// Gets or sets the client.
        /// </summary>
        /// <value>The client.</value>
        public GravityClient Client { get; protected set; }

        /// <summary>
        /// Gets or sets the entry.
        /// </summary>
        /// <value>The entry.</value>
        public GravityEntryObject Entry { get; protected set; }

        /// <summary>
        /// Gets or sets the command invokers.
        /// </summary>
        /// <value>The command invokers.</value>
        public Dictionary<string, IGravityCommandInvoker> CommandInvokers { get; protected set; }

        /// <summary>
        /// Gets or sets the configuration reader.
        /// </summary>
        /// <value>The configuration reader.</value>
        public RemoteConfigurationReader ConfigurationReader { get; protected set; }

        /// <summary>
        /// Gets or sets the component attribute.
        /// </summary>
        /// <value>The component attribute.</value>
        public BeyovaComponentAttribute ComponentAttribute { get; protected set; }

        /// <summary>
        /// Gets or sets the watcher thread.
        /// </summary>
        /// <value>The watcher thread.</value>
        public Thread WatcherThread { get; internal protected set; }

        /// <summary>
        /// Gets or sets the event hook.
        /// </summary>
        /// <value>The event hook.</value>
        internal GravityEventHook EventHook { get; private set; }

        /// <summary>
        /// Gets the information.
        /// </summary>
        /// <value>The information.</value>
        internal dynamic Info
        {
            get
            {
                return new
                {
                    Uri = this.Client.Entry.GravityServiceUri,
                    ConfigurationName = this.Client.Entry.ConfigurationName,
                    Actions = this.CommandInvokers.Keys
                };
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GravityShell" /> class.
        /// </summary>
        /// <param name="componentAttribute">The component attribute.</param>
        /// <param name="entry">The entry.</param>
        /// <param name="commandInvokers">The command invokers.</param>
        /// <param name="eventHook">The event hook.</param>
        protected GravityShell(BeyovaComponentAttribute componentAttribute, GravityEntryObject entry, IEnumerable<IGravityCommandInvoker> commandInvokers, GravityEventHook eventHook)
        {
            this.Entry = entry;
            this.Client = new GravityClient(entry);
            this.CommandInvokers = commandInvokers.AsDictionary((x) => x?.Action, StringComparer.OrdinalIgnoreCase);
            this.EventHook = eventHook;

            this.ComponentAttribute = componentAttribute;
            this.ConfigurationReader = new RemoteConfigurationReader(this.Client, entry.ConfigurationName);
            this.WatcherThread = new Thread(new ThreadStart(Watch))
            {
                IsBackground = true
            };

            this.WatcherThread.Start();
        }

        /// <summary>
        /// Invokes the command.
        /// </summary>
        /// <param name="invoker">The invoker.</param>
        /// <param name="command">The command.</param>
        /// <returns>GravityCommandResult.</returns>
        protected void InvokeCommand(IGravityCommandInvoker invoker, GravityCommandRequest command)
        {
            JToken result = null;

            if (invoker != null && command != null && command.Key.HasValue)
            {
                EventHook.OnProcessingCommand(invoker, command);

                try
                {
                    result = invoker.Invoke(command.Parameters);
                }
                catch (Exception ex)
                {
                    result = ex.Handle(command).ToExceptionInfo().ToJson();
                }

                Client.CommitCommandResult(new GravityCommandResult
                {
                    Key = command.Key,
                    Content = result,
                    ClientKey = _clientKey,
                    RequestKey = command.Key
                });
            }
        }

        /// <summary>
        /// Processes the commands.
        /// </summary>
        /// <param name="commands">The commands.</param>
        protected void ProcessCommands(List<GravityCommandRequest> commands)
        {
            if (commands.HasItem())
            {
                foreach (var command in commands)
                {
                    if (command != null && !string.IsNullOrWhiteSpace(command.Action))
                    {
                        IGravityCommandInvoker invoker;
                        if (CommandInvokers.TryGetValue(command.Action, out invoker))
                        {
                            InvokeCommand(invoker, command);
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Watches this instance.
        /// </summary>
        protected void Watch()
        {
            while (true)
            {
                try
                {
                    var machineHealth = new Heartbeat
                    {
                        ConfigurationName = this.ConfigurationReader?.ConfigurationName
                    };
                    machineHealth.FillHealthData();
                    var echo = Client.Heartbeat(machineHealth);

                    _clientKey = echo.ClientKey;
                    ProcessCommands(echo.CommandRequests);
                }
                catch { }

                //5 min
                Thread.Sleep(300000);
            }
        }

        #region Static

        /// <summary>
        /// The host
        /// </summary>
        private static GravityShell host = InitializeGravityHost();

        /// <summary>
        /// Gets the host.
        /// </summary>
        /// <value>The host.</value>
        internal static GravityShell Host { get { return host; } }

        /// <summary>
        /// Initializes the gravity host.
        /// </summary>
        /// <returns>Beyova.Gravity.GravityHost.</returns>
        private static GravityShell InitializeGravityHost()
        {
            HashSet<IGravityCommandInvoker> invokers = new HashSet<Gravity.IGravityCommandInvoker>();
            invokers.Add(new UpdateConfigurationCommandInvoker());
            invokers.Add(new FeatureModuleSwitchCommandInvoker());

            GravityEntryObject entryObject = null;
            BeyovaComponentAttribute componentAttribute = null;
            GravityEventHook gravityEventHook = null;

            bool findMoreEntry = true;
            foreach (var assembly in EnvironmentCore.AscendingAssemblyDependencyChain)
            {
                var commandActionAttribute = assembly.GetCustomAttribute<GravityCommandActionAttribute>();
                if (commandActionAttribute != null)
                {
                    foreach (var one in commandActionAttribute.Invokers)
                    {
                        invokers.Add(one);
                    }
                }

                if (findMoreEntry)
                {
                    var protocolAttribute = assembly.GetCustomAttribute<GravityProtocolAttribute>();
                    if (protocolAttribute != null)
                    {
                        entryObject = protocolAttribute.Entry;
                        componentAttribute = assembly.GetCustomAttribute<BeyovaComponentAttribute>();
                        gravityEventHook = (assembly.GetCustomAttribute<GravityEventHookAttribute>()?.Hook) ?? gravityEventHook;

                        if (protocolAttribute.IsSealed)
                        {
                            findMoreEntry = false;
                        }
                    }
                }
            }

            return entryObject == null ? null : new GravityShell(componentAttribute, entryObject, invokers, gravityEventHook);
        }

        #endregion Static
    }
}