using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using ifunction.ExceptionSystem;

namespace ifunction
{
    /// <summary>
    /// Class CompressionExtension.
    /// </summary>
    public static class CompressionExtension
    {
        /// <summary>
        /// Compresses the specified bytes object.
        /// </summary>
        /// <param name="bytesObject">The bytes object.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="OperationFailureException">Compress</exception>
        public static string CompressBytesToString(this  byte[] bytesObject)
        {
            if (bytesObject != null && bytesObject.Length > 0)
            {
                try
                {
                    var memoryStream = new MemoryStream();
                    using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
                    {
                        gZipStream.Write(bytesObject, 0, bytesObject.Length);
                    }

                    memoryStream.Position = 0;

                    var compressedData = new byte[memoryStream.Length];
                    memoryStream.Read(compressedData, 0, compressedData.Length);

                    var gZipBuffer = new byte[compressedData.Length + 4];
                    Buffer.BlockCopy(compressedData, 0, gZipBuffer, 4, compressedData.Length);
                    Buffer.BlockCopy(BitConverter.GetBytes(bytesObject.Length), 0, gZipBuffer, 0, 4);
                    return Convert.ToBase64String(gZipBuffer);
                }
                catch (Exception ex)
                {
                    throw ex.Handle("Compress", bytesObject);
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Compresses the specified string object.
        /// </summary>
        /// <param name="stringObject">The string object.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>System.String.</returns>
        public static string Compress(this string stringObject, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }
            var buffer = string.IsNullOrWhiteSpace(stringObject) ? null : encoding.GetBytes(stringObject);
            return CompressBytesToString(buffer);
        }

        /// <summary>
        /// Compresses the object automatic string.
        /// </summary>
        /// <param name="anyObject">Any object.</param>
        /// <returns>System.String.</returns>
        public static string CompressObjectToString(this object anyObject)
        {
            if (anyObject != null)
            {
                var binaryFormatter = new BinaryFormatter();
                byte[] bytes = null;

                using (var memoryStream = new MemoryStream())
                {
                    binaryFormatter.Serialize(memoryStream, anyObject);
                    bytes = memoryStream.ToArray();
                }

                return bytes.CompressBytesToString();
            }

            return string.Empty;
        }

        /// <summary>
        /// Decompresses the specified string object.
        /// </summary>
        /// <param name="stringObject">The string object.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>System.String.</returns>
        public static string Decompress(this string stringObject, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            return encoding.GetString(DecompressStringToBytes(stringObject));
        }

        /// <summary>
        /// Decompresses the string automatic object.
        /// </summary>
        /// <param name="stringObject">The string object.</param>
        /// <returns>System.Object.</returns>
        public static object DecompressStringToObject(this string stringObject)
        {
            object result = null;
            var bytes = stringObject.DecompressStringToBytes();

            if (bytes != null && bytes.Length > 0)
            {
                var binaryFormatter = new BinaryFormatter();
                using (var memoryStream = new MemoryStream(bytes))
                {
                    result = binaryFormatter.Deserialize(memoryStream);
                }
            }

            return result;
        }

        /// <summary>
        /// Decompresses the string automatic bytes.
        /// </summary>
        /// <param name="stringObject">The string object.</param>
        /// <returns>System.Byte[][].</returns>
        /// <exception cref="OperationFailureException">Decompress</exception>
        public static byte[] DecompressStringToBytes(this string stringObject)
        {
            if (!string.IsNullOrWhiteSpace(stringObject))
            {
                try
                {
                    byte[] gZipBuffer = Convert.FromBase64String(stringObject);
                    return Decompress(gZipBuffer);
                }
                catch (Exception ex)
                {
                    throw ex.Handle("Decompress", stringObject);
                }
            }

            return new byte[] { };
        }

        /// <summary>
        /// Decompresses the specified bytes.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns>System.Byte[][].</returns>
        /// <exception cref="OperationFailureException">Decompress</exception>
        public static byte[] Decompress(this byte[] bytes)
        {
            if (bytes != null && bytes.Length > 0)
            {
                try
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        int dataLength = BitConverter.ToInt32(bytes, 0);
                        memoryStream.Write(bytes, 4, bytes.Length - 4);

                        var buffer = new byte[dataLength];

                        memoryStream.Position = 0;
                        using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                        {
                            gZipStream.Read(buffer, 0, buffer.Length);
                        }

                        return buffer;
                    }
                }
                catch (Exception ex)
                {
                    throw new OperationFailureException("Decompress", ex, bytes);
                }
            }

            return null;
        }

        /// <summary>
        /// Extracts the zip file.
        /// </summary>
        /// <param name="zipFilePath">The zip file path.</param>
        /// <param name="extractPath">The extract path.</param>
        /// <param name="encoding">The encoding.</param>
        /// <exception cref="OperationFailureException">ExtractZipFile</exception>
        public static void ExtractZipFile(this string zipFilePath, string extractPath, Encoding encoding = null)
        {
            if (!string.IsNullOrWhiteSpace(zipFilePath) && !string.IsNullOrWhiteSpace(extractPath))
            {
                if (encoding == null)
                {
                    encoding = Encoding.UTF8;
                }

                try
                {
                    ZipFile.ExtractToDirectory(zipFilePath, extractPath, encoding);
                }
                catch (Exception ex)
                {
                    throw new OperationFailureException("ExtractZipFile", ex, new { zipFilePath, extractPath });
                }
            }
        }

        /// <summary>
        /// Gets the archive entry by path.
        /// </summary>
        /// <param name="archive">The archive.</param>
        /// <param name="entryPathToExtract">The entry path to extract.
        /// <remarks><example>Payload/xxx.app/Info.plist</example></remarks>
        /// </param>
        /// <returns>ZipArchiveEntry.</returns>
        private static ZipArchiveEntry GetArchiveEntryByPath(this ZipArchive archive, string entryPathToExtract)
        {
            try
            {
                archive.CheckNullObject("archive");
                entryPathToExtract.CheckEmptyString("entryPathToExtract");

                return archive.GetEntry(entryPathToExtract);
            }
            catch (Exception ex)
            {
                throw ex.Handle("GetArchiveEntryByPath", new { entryPathToExtract });
            }
        }

        /// <summary>
        /// Extracts the zip by entry path.
        /// </summary>
        /// <param name="zipFileStreamToOpen">The zip file stream to open.</param>
        /// <param name="entryPathToExtract">The entry path to extract.
        /// <remarks><example>Payload/xxx.app/Info.plist</example></remarks></param>
        /// <param name="destinationDirectoryPath">The destination directory path.</param>
        public static void ExtractZipByEntryPath(this Stream zipFileStreamToOpen, string entryPathToExtract, string destinationDirectoryPath)
        {
            try
            {
                zipFileStreamToOpen.CheckNullObject("zipFileStreamToOpen");
                entryPathToExtract.CheckEmptyString("entryPathToExtract");

                using (var archive = new ZipArchive(zipFileStreamToOpen, ZipArchiveMode.Read))
                {
                    archive.GetEntry(entryPathToExtract).ExtractToFile(destinationDirectoryPath);
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle("ExtractZipByEntryPath", new { entryPathToExtract, destinationDirectoryPath });
            }
        }

        /// <summary>
        /// Extracts the zip by entry path.
        /// </summary>
        /// <param name="zipFileStreamToOpen">The zip file stream to open.</param>
        /// <param name="entryPathToExtract">The entry path to extract.</param>
        /// <returns>System.Byte[].</returns>
        public static byte[] ExtractZipByEntryPath(this Stream zipFileStreamToOpen, string entryPathToExtract)
        {
            try
            {
                zipFileStreamToOpen.CheckNullObject("zipFileStreamToOpen");
                entryPathToExtract.CheckEmptyString("entryPathToExtract");

                using (var archive = new ZipArchive(zipFileStreamToOpen, ZipArchiveMode.Read))
                {
                    var archiveEntry = archive.GetArchiveEntryByPath(entryPathToExtract);
                    if (archiveEntry != null)
                    {
                        using (var stream = archiveEntry.Open())
                        {
                            return stream.ToBytes();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle("ExtractZipByEntryPath", new { entryPathToExtract });
            }

            return null;
        }

        /// <summary>
        /// Zips as bytes.
        /// <remarks>
        /// In parameter items, if you want to add folder, please use path like {directory}/{fileName}.
        /// <example>
        /// Folder1/Folder11/File1.txt
        /// </example>
        /// </remarks>
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="compressionLevel">The compression level.</param>
        /// <returns></returns>
        public static byte[] ZipAsBytes(this Dictionary<string, byte[]> items, CompressionLevel compressionLevel = CompressionLevel.Optimal)
        {
            try
            {
                items.CheckNullObject("items");

                using (var memoryStream = new MemoryStream())
                {
                    using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                    {
                        foreach (var one in items)
                        {
                            var entry = archive.CreateEntry(one.Key);

                            using (var entryStream = entry.Open())
                            {
                                entryStream.Write(one.Value, 0, one.Value.Length);
                                entryStream.Flush();
                            }
                        }
                    }

                    return memoryStream.ToBytes();
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle("ZipAsBytes", new { compressionLevel, items = items.Keys });
            }
        }

        /// <summary>
        /// Zips as bytes.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="compressionLevel">The compression level.</param>
        /// <returns></returns>
        public static byte[] ZipAsBytes(this byte[] bytes, string fileName, CompressionLevel compressionLevel = CompressionLevel.Optimal)
        {
            try
            {
                bytes.CheckNullObject("bytes");
                fileName.CheckEmptyString("fileName");

                var fileToZip = new Dictionary<string, byte[]> { { fileName, bytes } };
                return ZipAsBytes(fileToZip, compressionLevel);
            }
            catch (Exception ex)
            {
                throw ex.Handle("ZipAsBytes", new { compressionLevel, fileName });
            }
        }

        /// <summary>
        /// Zips to path.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="destinationPath">The destination path.</param>
        /// <param name="compressionLevel">The compression level.</param>
        public static void ZipToPath(this Dictionary<string, byte[]> items, string destinationPath, CompressionLevel compressionLevel = CompressionLevel.Optimal)
        {
            try
            {
                destinationPath.CheckNullObject("destinationPath");
                File.WriteAllBytes(destinationPath, items.ZipAsBytes(compressionLevel));
            }
            catch (Exception ex)
            {
                throw ex.Handle("ZipToPath", new { items = items?.Keys, destinationPath });
            }
        }

        /// <summary>
        /// Zips to path.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="destinationPath">The destination path.</param>
        /// <param name="compressionLevel">The compression level.</param>
        public static void ZipToPath(this byte[] bytes, string fileName, string destinationPath, CompressionLevel compressionLevel = CompressionLevel.Optimal)
        {
            try
            {
                destinationPath.CheckNullObject("destinationPath");
                fileName.CheckEmptyString("fileName");

                File.WriteAllBytes(destinationPath, bytes.ZipAsBytes(fileName, compressionLevel));
            }
            catch (Exception ex)
            {
                throw ex.Handle("ZipToPath", new { destinationPath, fileName });
            }
        }

        /// <summary>
        /// Zips as bytes.
        /// </summary>
        /// <param name="anyObject">Any object.</param>
        /// <param name="archiveItems">The archive items.</param>
        /// <returns></returns>
        public static byte[] ZipAsBytes(this object anyObject, params KeyValuePair<string, byte[]>[] archiveItems)
        {
            return ZipAsBytes(archiveItems.ToDictionary());
        }
    }
}
