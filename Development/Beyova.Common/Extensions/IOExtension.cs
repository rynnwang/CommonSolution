using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Beyova
{
    /// <summary>
    /// Extensions forIOExtension
    /// </summary>
    public static class IOExtension
    {
        #region IO

        /// <summary>
        /// The dot
        /// </summary>
        const string dot = ".";

        /// <summary>
        /// Gets the sub directory.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <param name="subDirectoryName">Name of the sub directory.</param>
        /// <returns></returns>
        public static DirectoryInfo GetSubDirectory(this DirectoryInfo directory, string subDirectoryName)
        {
            return string.IsNullOrWhiteSpace(subDirectoryName) ? directory : directory?.GetDirectories()?.FirstOrDefault(x => x.Name.Equals(subDirectoryName, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Clears the files.
        /// </summary>
        /// <param name="directory">The directory.</param>
        public static void ClearFiles(this DirectoryInfo directory)
        {
            if (directory != null && directory.Exists)
            {
                foreach (var one in directory.GetFiles())
                {
                    one.Delete();
                }
            }
        }

        /// <summary>
        /// Checks the directory exist.
        /// </summary>
        /// <param name="directory">The directory.</param>
        public static void CheckDirectoryExist(this DirectoryInfo directory)
        {
            if (directory == null || !directory.Exists)
            {
                ExceptionFactory.CreateInvalidObjectException(nameof(directory), directory?.FullName);
            }
        }

        /// <summary>
        /// Combines the extension.
        /// </summary>
        /// <param name="pureFileName">Name of the pure file.</param>
        /// <param name="extension">The extension.</param>
        /// <returns>System.String.</returns>
        public static string CombineExtension(this string pureFileName, string extension)
        {
            if (!string.IsNullOrWhiteSpace(extension))
            {
                extension = dot + extension.Replace(dot, string.Empty);
            }

            return pureFileName.SafeToString() + extension;
        }

        /// <summary>
        /// Gets the name of the pure file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>System.String.</returns>
        public static string GetPureFileName(this string fileName)
        {
            if (!string.IsNullOrWhiteSpace(fileName))
            {
                return fileName.SubStringBeforeLastMatch(dot);
            }

            return fileName;
        }

        /// <summary>
        /// Gets the temporary folder.
        /// If the tempIdentity is null, method would assign one.
        /// If the folder of tempIdentity is not existed, it would be created.
        /// </summary>
        /// <param name="anyObject">Any object.</param>
        /// <param name="tempIdentity">The temporary identity.</param>
        /// <returns>DirectoryInfo.</returns>
        public static DirectoryInfo GetTempFolder(this object anyObject, ref Guid? tempIdentity)
        {
            if (tempIdentity == null)
            {
                tempIdentity = Guid.NewGuid();
            }

            var path = Path.Combine(Path.GetTempPath(), tempIdentity.Value.ToString());
            var directory = new DirectoryInfo(path);

            if (!directory.Exists)
            {
                directory.Create();
            }

            return directory;
        }

        /// <summary>
        /// Reads the file contents.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>
        /// System.String.
        /// </returns>
        public static string ReadFileContents(this string path, Encoding encoding = null)
        {
            try
            {
                path.CheckEmptyString(nameof(path));

                using (var stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (var streamReader = new StreamReader(stream, encoding ?? Encoding.UTF8))
                    {
                        return streamReader.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(path);
            }
        }

        /// <summary>
        /// Reads the file contents.
        /// </summary>
        /// <param name="fileInfo">The file information.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns></returns>
        public static string ReadFileContents(this FileInfo fileInfo, Encoding encoding = null)
        {
            return (fileInfo != null && fileInfo.Exists) ? ReadFileContents(fileInfo.FullName, encoding) : null;
        }

        /// <summary>
        /// Reads the file bytes.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>System.Byte[].</returns>
        public static byte[] ReadFileBytes(this string path)
        {
            try
            {
                using (var stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    return stream.ToBytes();
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(path);
            }
        }

        /// <summary>
        /// Reads the file bytes.
        /// </summary>
        /// <param name="fileInfo">The file information.</param>
        /// <returns></returns>
        public static byte[] ReadFileBytes(this FileInfo fileInfo)
        {
            return (fileInfo != null && fileInfo.Exists) ? ReadFileBytes(fileInfo.FullName) : null;
        }

        /// <summary>
        /// Gets the absolute URI.
        /// If the relative Uri is started with ~/ or /, then directory should be the root directory of site.
        /// If the relative Uri is started with ../ or ./, then directory should be the current directory.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <param name="relativeUri">The relative URI.</param>
        /// <returns>System.String.</returns>
        public static string GetAbsoluteUri(this DirectoryInfo directory, string relativeUri)
        {
            string result = string.Empty;

            if (directory != null && !string.IsNullOrWhiteSpace(relativeUri))
            {
                if (relativeUri.StartsWith("../"))
                {
                    result = GetAbsoluteUri(directory.Parent, relativeUri.Substring(3));
                }
                else if (relativeUri.StartsWith("./"))
                {
                    result = GetAbsoluteUri(directory, relativeUri.Substring(2));
                }
                else if (relativeUri.StartsWith("~/"))
                {
                    result = directory.FullName(false) + relativeUri.Substring(1).Replace('/', '\\');
                }
                else if (relativeUri.StartsWith("/"))
                {
                    result = directory.FullName(false) + relativeUri.Replace('/', '\\');
                }
                // If relativeUri is like C:\, http://, ftp://, etc.
                else if (relativeUri.Contains(':'))
                {
                    result = relativeUri;
                }
                else
                {
                    result = directory.FullName(true) + relativeUri.Replace('/', '\\');
                }
            }

            return result;
        }

        /// <summary>
        /// Generates the relative resource URI.
        /// </summary>
        /// <param name="currentDirectory">The current directory.</param>
        /// <param name="targetAbsoluteUri">The target absolute URI.</param>
        /// <returns>System.String.</returns>
        public static string GenerateRelativeResourceUri(this DirectoryInfo currentDirectory, string targetAbsoluteUri)
        {
            string result = string.Empty;

            if (currentDirectory != null && !string.IsNullOrWhiteSpace(targetAbsoluteUri))
            {
                var currentFolder = currentDirectory.ToString();
                var baseFolder = currentFolder.FindCommonStartSubString(targetAbsoluteUri, true);

                if (!string.IsNullOrWhiteSpace(baseFolder))
                {
                    var depth = currentFolder.Substring(baseFolder.Length).Split(new char[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries).Length;
                    result = String.Concat(Enumerable.Repeat("../", depth)) + targetAbsoluteUri.Substring(baseFolder.Length).TrimStart('/', '\\');
                }
            }

            return result;
        }

        /// <summary>
        /// Fulls the name.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <param name="endWithSlash">if set to <c>true</c> [end with slash].</param>
        /// <returns>System.String.</returns>
        public static string FullName(this DirectoryInfo directory, bool endWithSlash = true)
        {
            string result = string.Empty;

            if (directory != null)
            {
                if (endWithSlash)
                {
                    result = directory.FullName.EndsWith("\\") ? directory.FullName : (directory.FullName + "\\");
                }
                else
                {
                    result = directory.FullName.TrimEnd('\\');
                }
            }

            return result;
        }

        #endregion

        #region Bytes

        /// <summary>
        /// To the hexadecimal.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns>System.String.</returns>
        public static string ToHex(this byte[] bytes)
        {
            return bytes != null ? BitConverter.ToString(bytes).Replace("-", "") : string.Empty;
        }

        /// <summary>
        /// Reads the stream to bytes.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="closeWhenFinish">if set to <c>true</c> [close when finish].</param>
        /// <returns>System.Byte[][].</returns>
        public static byte[] ReadStreamToBytes(this Stream stream, bool closeWhenFinish = false)
        {
            long originalPosition = 0;

            try
            {
                stream.CheckNullObject(nameof(stream));

                if (stream.CanSeek)
                {
                    originalPosition = stream.Position;
                    stream.Position = 0;
                }

                byte[] readBuffer = new byte[4096];

                int totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead == readBuffer.Length)
                    {
                        int nextByte = stream.ReadByte();
                        if (nextByte != -1)
                        {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }

                byte[] buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }

                return buffer;
            }
            finally
            {
                if (stream != null)
                {
                    if (stream.CanSeek)
                    {
                        stream.Position = originalPosition;
                    }

                    if (closeWhenFinish)
                    {
                        stream.Close();
                        stream.Dispose();
                    }
                }
            }
        }

        /// <summary>
        /// To the bytes.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>The <see cref="Byte" />  array of stream.</returns>
        public static byte[] ToBytes(this Stream stream)
        {
            return ReadStreamToBytes(stream, true);
        }

        /// <summary>
        /// To the stream.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns>The <see cref="Stream" />  for byte array.</returns>
        public static Stream ToStream(this byte[] bytes)
        {
            try
            {
                bytes.CheckNullObject(nameof(bytes));

                return new MemoryStream(bytes);
            }
            catch (Exception ex)
            {
                throw ex.Handle();
            }
        }

        #endregion        

        /// <summary>
        /// Determines whether [is relative path] [the specified path].
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns><c>true</c> if [is relative path] [the specified path]; otherwise, <c>false</c>.</returns>
        public static bool IsRelativePath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return false;
            }

            var root = Path.GetPathRoot(path);
            return root.IsInString("", "\\", "/");
        }

        /// <summary>
        /// Copies the stream.
        /// </summary>
        /// <param name="sourceStream">The source stream.</param>
        /// <param name="destinationStream">The destination stream.</param>
        public static void CopyStream(this Stream sourceStream, Stream destinationStream)
        {
            if (sourceStream != null && destinationStream != null)
            {
                sourceStream.CopyTo(destinationStream);
            }
        }
    }
}
