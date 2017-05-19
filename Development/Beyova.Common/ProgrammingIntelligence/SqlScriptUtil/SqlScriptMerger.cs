using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Beyova.ProgrammingIntelligence
{
    /// <summary>
    /// 
    /// </summary>
    public class SqlScriptMerger
    {
        /// <summary>
        /// Gets or sets the source directory.
        /// </summary>
        /// <value>
        /// The source directory.
        /// </value>
        public DirectoryInfo SourceDirectory { get; protected set; }

        /// <summary>
        /// Gets or sets the destination directory.
        /// </summary>
        /// <value>
        /// The destination directory.
        /// </value>
        public DirectoryInfo DestinationDirectory { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlScriptMerger" /> class.
        /// </summary>
        /// <param name="sourceDirectory">The source directory.</param>
        /// <param name="destinationDirectory">The destination directory.</param>
        public SqlScriptMerger(string sourceDirectory, string destinationDirectory)
            : this(new DirectoryInfo(sourceDirectory), new DirectoryInfo(destinationDirectory))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlScriptMerger" /> class.
        /// </summary>
        /// <param name="sourceDirectory">The source directory.</param>
        /// <param name="destinationDirectory">The destination directory.</param>
        public SqlScriptMerger(DirectoryInfo sourceDirectory, DirectoryInfo destinationDirectory)
        {
            this.SourceDirectory = sourceDirectory;
            this.DestinationDirectory = destinationDirectory;
        }

        /// <summary>
        /// Merges the specified type filters.
        /// </summary>
        /// <param name="typeFilters">The type filters.</param>
        /// <param name="viewBasedFunctionaNames">The view based functiona names.</param>
        public int Merge(SqlScriptType typeFilters = SqlScriptType.All, IEnumerable<string> viewBasedFunctionaNames = null)
        {
            int counter = 0;

            CheckDirectory();
            this.DestinationDirectory.ClearFiles();

            int index = 1;

            if (typeFilters.HasFlag(SqlScriptType.Table))
            {
                counter += MergeSqlScript("Table", index);
                index++;
            }

            if (typeFilters.HasFlag(SqlScriptType.Function))
            {
                counter += MergeBasicFunctions(index, viewBasedFunctionaNames);
                index++;
            }

            if (typeFilters.HasFlag(SqlScriptType.View))
            {
                counter += MergeSqlScript("View", index);
                index++;
            }

            if (typeFilters.HasFlag(SqlScriptType.Function))
            {
                counter += MergeViewFunctions(index, viewBasedFunctionaNames);
                index++;
            }

            if (typeFilters.HasFlag(SqlScriptType.StoredProcedure))
            {
                counter += MergeSqlScript("StoredProcedure", index);
                index++;
            }

            if (typeFilters.HasFlag(SqlScriptType.Data))
            {
                counter += MergeSqlScript("Data", index);
                index++;
            }

            return counter;
        }

        /// <summary>
        /// Merges the SQL script.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="sequence">The sequence.</param>
        /// <returns></returns>
        protected int MergeSqlScript(string name, int sequence)
        {
            int counter = 0;

            try
            {
                name.CheckEmptyString(nameof(name));

                var source = this.SourceDirectory.GetSubDirectory(name);
                source.CheckDirectoryExist();
                this.DestinationDirectory.CheckDirectoryExist();

                StringBuilder builder = new StringBuilder(1024);

                foreach (var file in source.GetFiles("*.sql", SearchOption.AllDirectories))
                {
                    builder.AppendLine(file.ReadFileContents());
                }

                File.WriteAllBytes(Path.Combine(this.DestinationDirectory.FullName, string.Format("{0}.{1}.sql", sequence, name)), Encoding.UTF8.GetBytes(builder.ToString()));
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { counter });
            }

            return counter;
        }

        /// <summary>
        /// Merges the basic functions.
        /// </summary>
        /// <param name="sequence">The sequence.</param>
        /// <param name="viewBasedFunctionaNames">The view based functiona names.</param>
        /// <returns></returns>
        protected int MergeBasicFunctions(int sequence, IEnumerable<string> viewBasedFunctionaNames)
        {
            const string name = "Function";
            int counter = 0;

            try
            {
                var source = this.SourceDirectory.GetSubDirectory(name);
                source.CheckDirectoryExist();
                this.DestinationDirectory.CheckDirectoryExist();

                StringBuilder builder = new StringBuilder(1024);

                foreach (var file in source.GetFiles("*.sql", SearchOption.AllDirectories))
                {
                    if (!(viewBasedFunctionaNames?.Contains(file.Name) ?? false))
                    {
                        builder.AppendLine(file.ReadFileContents());
                    }
                }

                File.WriteAllBytes(Path.Combine(this.DestinationDirectory.FullName, string.Format("{0}.{1}.sql", sequence, name)), Encoding.UTF8.GetBytes(builder.ToString()));
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { counter, viewBasedFunctionaNames });
            }

            return counter;
        }

        /// <summary>
        /// Merges the view functions.
        /// </summary>
        /// <param name="sequence">The sequence.</param>
        /// <param name="viewBasedFunctionaNames">The view based functiona names.</param>
        /// <returns></returns>
        protected int MergeViewFunctions(int sequence, IEnumerable<string> viewBasedFunctionaNames)
        {
            const string name = "Function";
            int counter = 0;

            try
            {
                var source = this.SourceDirectory.GetSubDirectory(name);
                source.CheckDirectoryExist();
                this.DestinationDirectory.CheckDirectoryExist();

                StringBuilder builder = new StringBuilder(1024);

                foreach (var file in source.GetFiles("*.sql", SearchOption.AllDirectories))
                {
                    if (viewBasedFunctionaNames?.Contains(file.Name) ?? false)
                    {
                        builder.AppendLine(file.ReadFileContents());
                    }
                }

                File.WriteAllBytes(Path.Combine(this.DestinationDirectory.FullName, string.Format("{0}.{1}.sql", sequence, name)), Encoding.UTF8.GetBytes(builder.ToString()));
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { counter, viewBasedFunctionaNames });
            }

            return counter;
        }

        /// <summary>
        /// Checks the directory.
        /// </summary>
        protected void CheckDirectory()
        {
            if (this.SourceDirectory == null || !this.SourceDirectory.Exists)
            {
                ExceptionFactory.CreateInvalidObjectException(nameof(this.SourceDirectory), this.SourceDirectory?.ToString());
            }

            if (this.DestinationDirectory == null || !this.DestinationDirectory.Exists)
            {
                ExceptionFactory.CreateInvalidObjectException(nameof(this.DestinationDirectory), this.DestinationDirectory?.ToString());
            }
        }

        /// <summary>
        /// Checks the sub directory.
        /// </summary>
        /// <param name="subFolder">The sub folder.</param>
        protected void CheckSubDirectory(string subFolder)
        {
            subFolder.CheckEmptyString(nameof(subFolder));

            var sourceSubDirectory = this.SourceDirectory.GetDirectories().FirstOrDefault(x => x.Name.Equals(subFolder, StringComparison.OrdinalIgnoreCase));
            if (sourceSubDirectory == null || !sourceSubDirectory.Exists)
            {
                ExceptionFactory.CreateInvalidObjectException(nameof(sourceSubDirectory), sourceSubDirectory?.ToString());
            }

            var denstinationSubDirectory = this.DestinationDirectory.GetDirectories().FirstOrDefault(x => x.Name.Equals(subFolder, StringComparison.OrdinalIgnoreCase));
            if (denstinationSubDirectory == null || !denstinationSubDirectory.Exists)
            {
                ExceptionFactory.CreateInvalidObjectException(nameof(denstinationSubDirectory), denstinationSubDirectory?.ToString());
            }
        }
    }
}
