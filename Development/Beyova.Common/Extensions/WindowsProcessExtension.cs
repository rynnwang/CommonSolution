using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Xml.Linq;
using Beyova.ExceptionSystem;

namespace Beyova
{
    /// <summary>
    /// Extensions ProcessExtension
    /// </summary>
    public static class WindowsProcessExtension
    {
        /// <summary>
        /// Runs the command. (*.cmd, *.bat, *.exe, etc.)
        /// </summary>
        /// <param name="commandFilePath">The command file path. (*.cmd, *.bat, *.exe, etc.)</param>
        /// <param name="outputDelegate">The output delegate.</param>
        public static void RunCommand(string commandFilePath, Action<string> outputDelegate)
        {
            if (!string.IsNullOrWhiteSpace(commandFilePath) && File.Exists(commandFilePath))
            {
                Process batProcess = new Process();
                // Redirect the output stream of the child process.
                batProcess.StartInfo.UseShellExecute = false;
                batProcess.StartInfo.RedirectStandardOutput = true;
                batProcess.StartInfo.RedirectStandardError = true;

                batProcess.StartInfo.FileName = commandFilePath;

                if (outputDelegate != null)
                {
                    batProcess.OutputDataReceived += (sender, args) => outputDelegate(args.Data);
                    batProcess.ErrorDataReceived += (sender, args) => outputDelegate(args.Data);
                }
                batProcess.Start();

                batProcess.BeginOutputReadLine();

                batProcess.WaitForExit();
                batProcess.Close();
            }
        }
    }
}
