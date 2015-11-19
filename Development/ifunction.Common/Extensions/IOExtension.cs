using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Xml.Linq;
using ifunction.ExceptionSystem;

namespace ifunction
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
        /// Reads the file content lines.
        /// This method would not impact the conflict for reading and writing.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>System.String[][].</returns>
        /// <exception cref="OperationFailureException">GetFileContentLines</exception>
        public static string[] ReadFileContentLines(this string path, Encoding encoding)
        {
            Stream stream = null;

            StreamReader streamReader = null;
            List<string> lines = new List<string>();

            try
            {
                stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                streamReader = new StreamReader(stream, encoding); string stringLine = string.Empty;

                do
                {
                    stringLine = streamReader.ReadLine();
                    lines.Add(stringLine);
                }
                while (stringLine != null);

                if (lines.Count > 0)
                {
                    lines.RemoveAt(lines.Count - 1);
                }
            }
            catch (Exception ex)
            {
                throw new OperationFailureException("GetFileContentLines", ex, path);
            }
            finally
            {
                streamReader.Close();
                stream.Close();
            }

            return lines.ToArray();
        }

        /// <summary>
        /// Reads the file content lines.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>System.String[][].</returns>
        public static string[] ReadFileContentLines(this string path)
        {
            return ReadFileContentLines(path, Encoding.UTF8);
        }

        /// <summary>
        /// Reads the file contents.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="OperationFailureException">ReadFileContents</exception>
        public static string ReadFileContents(this string path, Encoding encoding)
        {
            Stream stream = null;
            StreamReader streamReader = null;

            try
            {
                stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);
                streamReader = new StreamReader(stream, encoding);
                return streamReader.ReadToEnd();
            }
            catch (Exception ex)
            {
                throw new OperationFailureException("ReadFileContents", ex, path);
            }
            finally
            {
                if (streamReader != null)
                {
                    streamReader.Close();
                }
                if (stream != null)
                {
                    stream.Close();
                }
            }
        }

        /// <summary>
        /// Reads the file bytes.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>System.Byte[][].</returns>
        /// <exception cref="OperationFailureException">ReadFileBytes</exception>
        public static byte[] ReadFileBytes(this string path)
        {
            Stream stream = null;

            try
            {
                stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);
                return stream.ToBytes();
            }
            catch (Exception ex)
            {
                throw new OperationFailureException("ReadFileBytes", ex, path);
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
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
                var baseFolder = currentFolder.FindCommonStartString(targetAbsoluteUri, true);

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

        /// <summary>
        /// Creates the file stream writer.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="mode">The mode.</param>
        /// <param name="fileAccess">The file access.</param>
        /// <param name="share">The share.</param>
        /// <param name="bufferSize">Size of the buffer.</param>
        /// <param name="options">The options.</param>
        /// <returns>StreamWriter.</returns>
        public static StreamWriter CreateFileStreamWriter(this string path, FileMode mode, FileAccess fileAccess = FileAccess.ReadWrite, FileShare share = FileShare.Write, int bufferSize = 4096, FileOptions options = FileOptions.None)
        {
            var fileStream = new FileStream(path, mode, fileAccess, share, bufferSize, options);
            return new StreamWriter(fileStream);
        }

        /// <summary>
        /// Clones the data.
        /// </summary>
        /// <param name="anyStream">Any stream.</param>
        /// <returns>MemoryStream.</returns>
        public static MemoryStream CloneData(this Stream anyStream)
        {
            if (anyStream != null)
            {

                var stream = new MemoryStream();
                anyStream.CopyTo(stream);

                return stream;
            }

            return null;
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
                stream.CheckNullObject("stream");

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
        /// <exception cref="OperationFailureException">StreamToBytes</exception>
        public static byte[] ToBytes(this Stream stream)
        {
            return ReadStreamToBytes(stream, true);
        }

        /// <summary>
        /// To the stream.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns>The <see cref="Stream" />  for byte array.</returns>
        /// <exception cref="OperationFailureException">BytesToStream</exception>
        public static Stream ToStream(this byte[] bytes)
        {
            Stream stream = null;

            try
            {
                if (bytes != null)
                {
                    stream = new MemoryStream(bytes);
                }
            }
            catch (Exception ex)
            {
                throw new OperationFailureException("BytesToStream", ex);
            }

            return stream;
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
    }
}
