using System;
using System.Diagnostics;
using System.IO;

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
            RunCommand(new WindowsProcessCommandParameter { CommandPath = commandFilePath, OutputDelegate = outputDelegate });
        }

        /// <summary>
        /// Runs the command. (*.cmd, *.bat, *.exe, etc.)
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        public static void RunCommand(WindowsProcessCommandParameter parameter)
        {
            if (parameter != null && !string.IsNullOrWhiteSpace(parameter.CommandPath) && File.Exists(parameter.CommandPath))
            {
                try
                {
                    using (Process processToRun = new Process())
                    {
                        // Redirect the output stream of the child process.
                        processToRun.StartInfo.UseShellExecute = false;
                        processToRun.StartInfo.RedirectStandardOutput = true;
                        processToRun.StartInfo.RedirectStandardError = true;

                        parameter.FillProcessStartInfo(processToRun.StartInfo);

                        if (parameter.OutputDelegate != null)
                        {
                            processToRun.OutputDataReceived += (sender, args) => parameter.OutputDelegate(args.Data);
                            processToRun.ErrorDataReceived += (sender, args) => parameter.OutputDelegate(args.Data);
                        }

                        processToRun.Start();

                        processToRun.BeginOutputReadLine();

                        processToRun.WaitForExit();
                        processToRun.Close();
                    }
                }
                catch (Exception ex)
                {
                    throw ex.Handle(parameter);
                }
            }
        }
    }
}