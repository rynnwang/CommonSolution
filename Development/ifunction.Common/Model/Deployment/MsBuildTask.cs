using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ifunction.Model
{
    /// <summary>
    /// Class MsBuildTask.
    /// </summary>
    public class MsBuildTask
    {
        /// <summary>
        /// Gets or sets the build target. Target can be solution name (*.sln) or project name (*.csproj, *.vbproj).
        /// Example: test.sln, test.csproj.
        /// </summary>
        /// <value>The build target.</value>
        public string BuildTarget { get; set; }

        /// <summary>
        /// Gets or sets the target details.
        /// </summary>
        /// <value>The target details.</value>
        public List<string> TargetDetails { get; set; }

        /// <summary>
        /// Gets or sets the dot net version. Example: 3.5, 4.5
        /// </summary>
        /// <value>The dot net version.</value>
        public string DotNetVersion { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is outpu directory git deployment.
        /// </summary>
        /// <value><c>true</c> if this instance is outpu directory git deployment; otherwise, <c>false</c>.</value>
        public bool IsOutpuDirectoryGitDeployment { get; set; }

        #region BuildProperties

        const string outputDirectory = "OutDir";

        const string wariningLevel = "WarningLevel";

        const string deployOnBuild = "DeployOnBuild";

        const string publishProfile = "PublishProfile";

        const string buildConfigurationName = "Configuration";

        /// <summary>
        /// Gets or sets the output directory.
        /// </summary>
        /// <value>The output directory.</value>
        public string OutputDirectory
        {
            get { return BuildProperties.SafeGetValue(outputDirectory); }
            set
            {
                BuildProperties.Merge(outputDirectory, value);
            }
        }

        /// <summary>
        /// Gets or sets the publish profile.
        /// </summary>
        /// <value>The publish profile.</value>
        public string PublishProfile
        {
            get { return BuildProperties.SafeGetValue(publishProfile); }
            set
            {
                BuildProperties.Merge(publishProfile, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [deploy on build].
        /// </summary>
        /// <value><c>true</c> if [deploy on build]; otherwise, <c>false</c>.</value>
        public bool DeployOnBuild
        {
            get { return BuildProperties.SafeGetValue(deployOnBuild).ToBoolean(); }
            set { BuildProperties.Merge(deployOnBuild, value.ToString().ToLowerInvariant()); }
        }

        /// <summary>
        /// Gets or sets the warning level.
        /// </summary>
        /// <value>The warning level.</value>
        public string WarningLevel
        {
            get { return BuildProperties.SafeGetValue(wariningLevel); }
            set
            {
                BuildProperties.Merge(wariningLevel, value);
            }
        }

        /// <summary>
        /// Gets or sets the name of the build configuration.
        /// </summary>
        /// <value>The name of the build configuration.</value>
        public string BuildConfigurationName
        {
            get { return BuildProperties.SafeGetValue(buildConfigurationName); }
            set
            {
                BuildProperties.Merge(buildConfigurationName, value);
            }
        }

        #endregion

        /// <summary>
        /// Gets or sets the build properties.
        /// </summary>
        /// <value>The build properties.</value>
        public Dictionary<string, string> BuildProperties { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MsBuildTask" /> class.
        /// </summary>
        /// <param name="buildTarget">The build target.</param>
        public MsBuildTask(string buildTarget)
        {
            BuildProperties = new Dictionary<string, string>();
            this.TargetDetails = new List<string>();
            this.BuildTarget = buildTarget;
        }

        /// <summary>
        /// To the command line.
        /// </summary>
        /// <returns>System.String.</returns>
        public string ToCommandLine()
        {
            this.BuildTarget.CheckEmptyString("BuildTarget");

            StringBuilder builder = new StringBuilder(100);

            builder.AppendFormat("msbuild.exe {0}", BuildTarget);

            if (!string.IsNullOrWhiteSpace(DotNetVersion))
            {
                builder.AppendFormat("/toolsversion:{0}", DotNetVersion);
            }

            if (this.TargetDetails.HasItem())
            {
                builder.AppendFormat("/t:{0}", string.Join(";", this.TargetDetails));
            }

            if (BuildProperties.HasItem())
            {
                builder.AppendFormat("/property:{0}", string.Join(";", this.TargetDetails));
            }

            return builder.ToString();
        }
    }
}