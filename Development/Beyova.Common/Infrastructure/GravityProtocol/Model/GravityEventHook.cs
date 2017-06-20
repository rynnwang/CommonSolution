using Beyova.ExceptionSystem;

namespace Beyova.Gravity
{
    /// <summary>
    /// Class GravityEventHook.
    /// </summary>
    public abstract class GravityEventHook
    {
        /// <summary>
        /// Called when [secured message processed completed].
        /// </summary>
        /// <param name="feature">The feature.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="exception">The exception.</param>
        public virtual void OnSecuredMessageProcessedCompleted(string feature, object parameter, BaseException exception)
        {
            //Do nothing;
        }

        /// <summary>
        /// Called when [processing command].
        /// </summary>
        /// <param name="invoker">The invoker.</param>
        /// <param name="command">The command.</param>
        public virtual void OnProcessingCommand(IGravityCommandInvoker invoker, GravityCommandRequest command)
        {
            //Do nothing;
        }
    }
}