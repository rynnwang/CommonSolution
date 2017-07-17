using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Beyova
{
    /// <summary>
    /// Class EncodingOrSecurityExtension.
    /// </summary>
    public static class EncodingOrSecurityExtension
    {
        #region Html encode

        /// <summary>
        /// To the URL path encoded text. It is used for URL Query String or when passing a URL as parameter of anther URL. (Space to '%20', but not encode special character like '/', '=', '?' and '&amp;'. Any character after first '?' would not be encoded)
        /// </summary>
        /// <param name="originalText">The original text.</param>
        /// <returns>System.String.</returns>
        public static string ToUrlPathEncodedText(this string originalText)
        {
            if (originalText != null)
            {
                originalText = HttpUtility.UrlPathEncode(originalText);
            }

            return originalText;
        }

        /// <summary>
        /// To the URL encoded text. It is used for URL Path. (Space to '+', and encode any special character, including '/', '=', '?' and '&amp;')
        /// </summary>
        /// <param name="originalText">The original text.</param>
        /// <returns>System.String.</returns>
        public static string ToUrlEncodedText(this string originalText)
        {
            if (originalText != null)
            {
                originalText = HttpUtility.UrlEncode(originalText, Encoding.UTF8);
            }

            return originalText;
        }

        /// <summary>
        /// To the URL decoded text.
        /// </summary>
        /// <param name="originalText">The original text.</param>
        /// <returns>System.String.</returns>
        public static string ToUrlDecodedText(this string originalText)
        {
            if (originalText != null)
            {
                originalText = HttpUtility.UrlDecode(originalText, Encoding.UTF8);
            }

            return originalText;
        }

        /// <summary>
        /// To the HTML encoded text.
        /// </summary>
        /// <param name="originalText">The original text.</param>
        /// <returns>System.String.</returns>
        public static string ToHtmlEncodedText(this string originalText)
        {
            if (originalText != null)
            {
                originalText = HttpUtility.HtmlEncode(originalText);
            }

            return originalText;
        }

        /// <summary>
        /// To the HTML decoded text.
        /// </summary>
        /// <param name="originalText">The original text.</param>
        /// <returns>System.String.</returns>
        public static string ToHtmlDecodedText(this string originalText)
        {
            if (originalText != null)
            {
                originalText = HttpUtility.HtmlDecode(originalText);
            }

            return originalText;
        }

        #endregion Html encode

        #region Unicode

        /// <summary>
        /// Encodes the string to unicode.
        /// </summary>
        /// <param name="sourceText">The SRC text.</param>
        /// <returns>System.String.</returns>
        public static string EncodeStringToUnicode(this string sourceText)
        {
            return HttpUtility.UrlEncode(sourceText);
        }

        /// <summary>
        /// Decodes the unicode to string.
        /// </summary>
        /// <param name="sourceText">The source text.</param>
        /// <returns>System.String.</returns>
        public static string DecodeUnicodeToString(this string sourceText)
        {
            return HttpUtility.UrlDecode(sourceText);
        }

        #endregion Unicode

        #region Base64

        /// <summary>
        /// Encodes to base64.
        /// </summary>
        /// <param name="stringObject">The source.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>System.String.</returns>
        public static string EncodeBase64(this string stringObject, Encoding encoding = null)
        {
            try
            {
                byte[] bytes = (encoding ?? Encoding.UTF8).GetBytes(stringObject);
                return Convert.ToBase64String(bytes);
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Encodes the base64.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns>System.String.</returns>
        public static string EncodeBase64(this byte[] bytes)
        {
            try
            {
                return bytes == null ? null : Convert.ToBase64String(bytes);
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Decodes the base64.
        /// </summary>
        /// <param name="stringObject">The string object.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>System.String.</returns>
        public static string DecodeBase64(this string stringObject, Encoding encoding = null)
        {
            try
            {
                byte[] bytes = Convert.FromBase64String(stringObject);
                return (encoding ?? Encoding.UTF8).GetString(bytes);
            }
            catch
            {
                return string.Empty;
            }
        }

        #endregion Base64

        #region Base62

        private static string Base62CodingSpace = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

        /// <summary>
        /// Convert a byte array
        /// </summary>
        /// <param name="original">Byte array</param>
        /// <returns>Base62 string</returns>
        public static string ToBase62(this byte[] original)
        {
            StringBuilder sb = new StringBuilder();

            BitStream stream = new BitStream(original);         // Set up the BitStream
            byte[] read = new byte[1];                          // Only read 6-bit at a time
            while (true)
            {
                read[0] = 0;
                int length = stream.Read(read, 0, 6);           // Try to read 6 bits
                if (length == 6)                                // Not reaching the end
                {
                    if ((int)(read[0] >> 3) == 0x1f)            // First 5-bit is 11111
                    {
                        sb.Append(Base62CodingSpace[61]);
                        stream.Seek(-1, SeekOrigin.Current);    // Leave the 6th bit to next group
                    }
                    else if ((int)(read[0] >> 3) == 0x1e)       // First 5-bit is 11110
                    {
                        sb.Append(Base62CodingSpace[60]);
                        stream.Seek(-1, SeekOrigin.Current);
                    }
                    else                                        // Encode 6-bit
                    {
                        sb.Append(Base62CodingSpace[(int)(read[0] >> 2)]);
                    }
                }
                else
                {
                    // Padding 0s to make the last bits to 6 bit
                    sb.Append(Base62CodingSpace[(int)(read[0] >> (int)(8 - length))]);
                    break;
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Convert a Base62 string to byte array
        /// </summary>
        /// <param name="base62">Base62 string</param>
        /// <returns>Byte array</returns>
        public static byte[] FromBase62(this string base62)
        {
            // Character count
            int count = 0;

            // Set up the BitStream
            BitStream stream = new BitStream(base62.Length * 6 / 8);

            foreach (char c in base62)
            {
                // Look up coding table
                int index = Base62CodingSpace.IndexOf(c);

                // If end is reached
                if (count == base62.Length - 1)
                {
                    // Check if the ending is good
                    int mod = (int)(stream.Position % 8);
                    stream.Write(new byte[] { (byte)(index << (mod)) }, 0, 8 - mod);
                }
                else
                {
                    // If 60 or 61 then only write 5 bits to the stream, otherwise 6 bits.
                    if (index == 60)
                    {
                        stream.Write(new byte[] { 0xf0 }, 0, 5);
                    }
                    else if (index == 61)
                    {
                        stream.Write(new byte[] { 0xf8 }, 0, 5);
                    }
                    else
                    {
                        stream.Write(new byte[] { (byte)index }, 2, 6);
                    }
                }
                count++;
            }

            // Dump out the bytes
            byte[] result = new byte[stream.Position / 8];
            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(result, 0, result.Length * 8);
            return result;
        }

        #endregion Base62

        #region MD5

        /// <summary>
        /// To the m d5 bytes.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns>System.Byte[].</returns>
        public static byte[] ToMD5Bytes(this byte[] bytes)
        {
            if (bytes != null)
            {
                try
                {
                    MD5CryptoServiceProvider md5Provider = new MD5CryptoServiceProvider();
                    return md5Provider.ComputeHash(bytes);
                }
                catch (Exception ex)
                {
                    throw ex.Handle();
                }
            }

            return null;
        }

        /// <summary>
        /// To the m d5 bytes.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="resetPosition">The reset position.</param>
        /// <returns>System.Byte[].</returns>
        public static byte[] ToMD5Bytes(this Stream stream, long? resetPosition = null)
        {
            if (stream != null)
            {
                try
                {
                    MD5CryptoServiceProvider md5Provider = new MD5CryptoServiceProvider();
                    var hash = md5Provider.ComputeHash(stream);

                    if (stream.CanSeek && resetPosition.HasValue)
                    {
                        stream.Position = resetPosition.Value;
                    }
                }
                catch (Exception ex)
                {
                    throw ex.Handle();
                }
            }

            return null;
        }

        /// <summary>
        /// To the m d5 string.
        /// </summary>
        /// <param name="stringObject">The string object.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>System.String.</returns>
        public static string ToMD5String(this string stringObject, Encoding encoding = null)
        {
            try
            {
                byte[] data = (encoding ?? Encoding.UTF8).GetBytes(stringObject);
                return ToMD5String(data);
            }
            catch (Exception ex)
            {
                throw ex.Handle(stringObject);
            }
        }

        /// <summary>
        /// To the m d5 base64 string.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns>System.String.</returns>
        public static string ToBase64Md5(this byte[] bytes)
        {
            return ToMD5Bytes(bytes)?.EncodeBase64();
        }

        /// <summary>
        /// To the m d5 base64 string.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="resetPosition">The reset position.</param>
        /// <returns>System.String.</returns>
        public static string ToBase64Md5(this Stream stream, long? resetPosition = null)
        {
            return ToMD5Bytes(stream, resetPosition)?.EncodeBase64();
        }

        /// <summary>
        /// To the m d5 string.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns>System.String.</returns>
        public static string ToMD5String(this byte[] bytes)
        {
            try
            {
                return ToMD5Bytes(bytes).ToHex();
            }
            catch (Exception ex)
            {
                throw ex.Handle();
            }
        }

        /// <summary>
        /// To the m d5 string.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="resetPosition">The reset position.</param>
        /// <returns>System.String.</returns>
        public static string ToMD5String(this Stream stream, long? resetPosition = null)
        {
            try
            {
                return ToMD5Bytes(stream, resetPosition).ToHex();
            }
            catch (Exception ex)
            {
                throw ex.Handle();
            }
        }

        #endregion MD5

        #region SHA1

        /// <summary>
        /// Encrypts to SHA1.
        /// </summary>
        /// <param name="stringObject">The string object.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>System.String.</returns>
        public static string ToSHA1(this string stringObject, Encoding encoding = null)
        {
            try
            {
                return ToSHA1((encoding ?? Encoding.UTF8).GetBytes(stringObject));
            }
            catch (Exception ex)
            {
                throw ex.Handle(stringObject);
            }
        }

        /// <summary>
        /// Encrypts to SH a1.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>System.String.</returns>
        public static string ToSHA1(this byte[] data)
        {
            try
            {
                using (SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider())
                {
                    var hashByte = sha1.ComputeHash(data);
                    string result = BitConverter.ToString(hashByte);
                    return result.Replace("-", "").ToUpperInvariant();
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(data);
            }
        }

        #endregion SHA1

        #region 3DES

        /// <summary>
        /// Generates the triple DES key.
        /// </summary>
        /// <returns>System.Byte[].</returns>
        public static byte[] GenerateTripleDESKey()
        {
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] tripleDesKey = new byte[tripleDesKeyLength];
                rng.GetBytes(tripleDesKey);

                for (var i = 0; i < tripleDesKey.Length; ++i)
                {
                    int keyByte = tripleDesKey[i] & 0xFE;
                    var parity = 0;
                    for (int b = keyByte; b != 0; b >>= 1)
                        parity ^= b & 1;
                    tripleDesKey[i] = (byte)(keyByte | (parity == 0 ? 1 : 0));
                }

                return tripleDesKey;
            }
        }

        /// <summary>
        /// Generates the triple DES key as string.
        /// </summary>
        /// <returns>System.String.</returns>
        public static string GenerateTripleDESKeyAsString()
        {
            return GenerateTripleDESKey().EncodeBase64();
        }

        /// <summary>
        /// Encrypt 3DES.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="encryptKey">The encrypt key.</param>
        /// <param name="cipherMode">The cipher mode.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>System.String.</returns>
        public static string EncryptTripleDES(this string content, string encryptKey, CipherMode cipherMode = CipherMode.ECB, Encoding encoding = null)
        {
            if (!string.IsNullOrWhiteSpace(content) && !string.IsNullOrWhiteSpace(encryptKey))
            {
                return Convert.ToBase64String(EncryptTripleDES((encoding ?? Encoding.UTF8).GetBytes(content), encryptKey, cipherMode));
            }

            return null;
        }

        /// <summary>
        /// Encrypts the triple DES.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="encryptKey">The encrypt key.</param>
        /// <param name="cipherMode">The cipher mode.</param>
        /// <returns>System.Byte[].</returns>
        public static byte[] EncryptTripleDES(this byte[] content, byte[] encryptKey, CipherMode cipherMode = CipherMode.ECB)
        {
            if (content != null && encryptKey.HasItem())
            {
                TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();

                DES.Key = encryptKey;
                DES.Mode = cipherMode;

                ICryptoTransform DESEncrypt = DES.CreateEncryptor();

                return DESEncrypt.TransformFinalBlock(content, 0, content.Length);
            }

            return null;
        }

        /// <summary>
        /// Encrypts the triple DES.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="encryptKey">The encrypt key.</param>
        /// <param name="cipherMode">The cipher mode.</param>
        /// <returns>System.Byte[].</returns>
        public static byte[] EncryptTripleDES(this byte[] content, string encryptKey, CipherMode cipherMode = CipherMode.ECB)
        {
            return EncryptTripleDES(content, ASCIIEncoding.ASCII.GetBytes(encryptKey), cipherMode);
        }

        /// <summary>
        /// Decrypt3s the DES.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="encryptKey">The encrypt key.</param>
        /// <param name="cipherMode">The cipher mode.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>System.String.</returns>
        public static string DecryptTripleDES(this string content, string encryptKey, CipherMode cipherMode = CipherMode.ECB, Encoding encoding = null)
        {
            if (!string.IsNullOrWhiteSpace(content) && !string.IsNullOrWhiteSpace(encryptKey))
            {
                try
                {
                    byte[] buffer = Convert.FromBase64String(content);
                    return (encoding ?? Encoding.UTF8).GetString(DecryptTripleDES(buffer, ASCIIEncoding.ASCII.GetBytes(encryptKey), cipherMode));
                }
                catch (Exception ex)
                {
                    throw ex.Handle(new { content, encryptKey, cipherMode });
                }
            }

            return null;
        }

        /// <summary>
        /// Decrypt3s the DES.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="encryptKey">The encrypt key.</param>
        /// <param name="cipherMode">The cipher mode.</param>
        /// <returns>System.Byte[].</returns>
        public static byte[] DecryptTripleDES(this byte[] content, byte[] encryptKey, CipherMode cipherMode = CipherMode.ECB)
        {
            if (content.HasItem() && encryptKey.HasItem())
            {
                try
                {
                    using (TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider())
                    {
                        DES.Key = encryptKey;
                        DES.Mode = cipherMode;
                        DES.Padding = PaddingMode.PKCS7;

                        ICryptoTransform DESDecrypt = DES.CreateDecryptor();

                        return DESDecrypt.TransformFinalBlock(content, 0, content.Length);
                    }
                }
                catch (Exception ex)
                {
                    throw ex.Handle(new { cipherMode = cipherMode.EnumToString() });
                }
            }

            return null;
        }

        #endregion 3DES

        #region RSA

        /// <summary>
        /// Encrypts within RSA. Note: Under 1024 bit key, 117 bytes can be encrypted at most. Under 2048 bit key, 245 bytes can be encrypted at most.
        /// </summary>
        /// <param name="dataToEncrypt">The data to encrypt.</param>
        /// <param name="publicKey">The public key.</param>
        /// <param name="dwKeySize">Size of the dw key.</param>
        /// <returns>System.String.</returns>
        public static byte[] RsaEncrypt(this byte[] dataToEncrypt, string publicKey, int dwKeySize = 2048)
        {
            try
            {
                using (var rsa = new RSACryptoServiceProvider(dwKeySize))
                {
                    rsa.ImportCspBlob(Convert.FromBase64String(publicKey));

                    //OAEP padding is only available on Microsoft Windows XP or later.
                    return rsa.Encrypt(dataToEncrypt, true);
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { publicKey, dwKeySize });
            }
        }

        /// <summary>
        /// Decrypts within RSA.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="privateKey">The private key.</param>
        /// <param name="dwKeySize">Size of the dw key.</param>
        /// <returns>System.Byte[].</returns>
        public static byte[] RsaDecrypt(this string content, string privateKey, int dwKeySize = 2048)
        {
            try
            {
                var dataToDecrypt = Convert.FromBase64String(content);
                return RsaDecrypt(dataToDecrypt, privateKey, dwKeySize);
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { content, privateKey, dwKeySize });
            }
        }

        /// <summary>
        /// RSAs the decrypt.
        /// </summary>
        /// <param name="dataToDecrypt">The data to decrypt.</param>
        /// <param name="privateKey">The private key.</param>
        /// <param name="dwKeySize">Size of the dw key.</param>
        /// <returns>System.Byte[].</returns>
        public static byte[] RsaDecrypt(this byte[] dataToDecrypt, string privateKey, int dwKeySize = 2048)
        {
            try
            {
                using (var rsa = new RSACryptoServiceProvider(dwKeySize))
                {
                    rsa.ImportCspBlob(Convert.FromBase64String(privateKey));

                    return rsa.Decrypt(dataToDecrypt, true);
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle();
            }
        }

        /// <summary>
        /// Creates the RSA keys.
        /// </summary>
        /// <param name="anyObject">Any object.</param>
        /// <param name="publicKey">The public key.</param>
        /// <param name="privateKey">The private key.</param>
        /// <param name="dwKeySize">Size of the dw key.</param>
        public static void CreateRsaKeys(this object anyObject, out string publicKey, out string privateKey, int dwKeySize = 2048)
        {
            try
            {
                using (var rsa = new RSACryptoServiceProvider(dwKeySize))
                {
                    privateKey = rsa.ExportCspBlob(true).EncodeBase64();
                    publicKey = rsa.ExportCspBlob(false).EncodeBase64();
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle();
            }
        }

        /// <summary>
        /// Creates the RSA keys.
        /// </summary>
        /// <param name="dwKeySize">Size of the dw key.</param>
        /// <returns>Beyova.RsaKeys.</returns>
        public static RsaKeys CreateRsaKeys(int dwKeySize = 2048)
        {
            try
            {
                using (var rsa = new RSACryptoServiceProvider(dwKeySize))
                {
                    return new RsaKeys
                    {
                        PrivateKey = rsa.ExportCspBlob(true).EncodeBase64(),
                        PublicKey = rsa.ExportCspBlob(false).EncodeBase64()
                    };
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle();
            }
        }

        #endregion RSA

        #region DES

        /// <summary>
        /// Generates the random DES key.
        /// </summary>
        /// <param name="anyObject">Any object.</param>
        /// <returns>The <see cref="byte" /> array for DES key.</returns>
        private static byte[] GenerateRandomDESKey(this object anyObject)
        {
            return anyObject.CreateRandomHex(desKeyLength);
        }

        /// <summary>
        /// Encrypts the DES.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="encryptKey">The encrypt key.</param>
        /// <returns>System.String.</returns>
        public static string EncryptDES(this string content, string encryptKey)
        {
            string result = null;

            if (!string.IsNullOrWhiteSpace(content) && !string.IsNullOrWhiteSpace(encryptKey))
            {
                try
                {
                    using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
                    {
                        byte[] inputByteArray = Encoding.UTF8.GetBytes(content);
                        des.Key = ASCIIEncoding.ASCII.GetBytes(encryptKey);
                        des.IV = ASCIIEncoding.ASCII.GetBytes(encryptKey);
                        MemoryStream ms = new MemoryStream();
                        using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(inputByteArray, 0, inputByteArray.Length);
                            cs.FlushFinalBlock();
                            cs.Close();
                        }
                        string str = Convert.ToBase64String(ms.ToArray());
                        ms.Close();
                        result = str;
                    }
                }
                catch (Exception ex)
                {
                    throw ex.Handle(new { content, encryptKey });
                }
            }

            return result;
        }

        /// <summary>
        /// Decrypts the DES.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="encryptKey">The encrypt key.</param>
        /// <returns>System.String.</returns>
        public static string DecryptDES(this string content, string encryptKey)
        {
            string result = null;

            if (!string.IsNullOrWhiteSpace(content) && !string.IsNullOrWhiteSpace(encryptKey))
            {
                try
                {
                    byte[] inputByteArray = Convert.FromBase64String(content);

                    using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
                    {
                        des.Key = ASCIIEncoding.ASCII.GetBytes(encryptKey);
                        des.IV = ASCIIEncoding.ASCII.GetBytes(encryptKey);
                        MemoryStream ms = new MemoryStream();
                        using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(inputByteArray, 0, inputByteArray.Length);
                            cs.FlushFinalBlock();
                            cs.Close();
                        }
                        string str = Encoding.UTF8.GetString(ms.ToArray());
                        ms.Close();
                        result = str;
                    }
                }
                catch (Exception ex)
                {
                    throw ex.Handle(new { content, encryptKey });
                }
            }

            return result;
        }

        #endregion DES

        #region My encryption

        /// <summary>
        /// The description key length
        /// </summary>
        private const int desKeyLength = 48;

        /// <summary>
        /// The triple description key length
        /// </summary>
        private const int tripleDesKeyLength = 24;

        /// <summary>
        /// Encrypts the r3 DES.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns>System.Byte[].</returns>
        internal static byte[] EncryptR3DES(this byte[] content)
        {
            if (content.HasItem())
            {
                try
                {
                    byte[] keyBytes = GenerateTripleDESKey();
                    using (TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider())
                    {
                        DES.Key = keyBytes;
                        DES.Mode = CipherMode.ECB;
                        DES.Padding = PaddingMode.PKCS7;

                        ICryptoTransform DESEncrypt = DES.CreateEncryptor();

                        var buffer = DESEncrypt.TransformFinalBlock(content, 0, content.Length);
                        List<byte> data = new List<byte>(keyBytes);
                        data.AddRange(buffer);
                        return data.ToArray();
                    }
                }
                catch (Exception ex)
                {
                    throw ex.Handle();
                }
            }

            return null;
        }

        /// <summary>
        /// Encrypts the r3 DES.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>System.String.</returns>
        internal static string EncryptR3DES(this string content, Encoding encoding = null)
        {
            string result = content;

            if (!string.IsNullOrWhiteSpace(content))
            {
                try
                {
                    return Convert.ToBase64String(EncryptR3DES((encoding ?? Encoding.UTF8).GetBytes(content)));
                }
                catch (Exception ex)
                {
                    throw ex.Handle(new { content });
                }
            }

            return result;
        }

        /// <summary>
        /// Decrypts the r3 DES.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns>System.Byte[].</returns>
        internal static byte[] DecryptR3DES(this byte[] content)
        {
            if (content.HasItem())
            {
                try
                {
                    List<byte> bytes = new List<byte>(content);
                    using (TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider())
                    {
                        DES.Key = bytes.GetRange(0, tripleDesKeyLength).ToArray();
                        var buffer = bytes.GetRange(tripleDesKeyLength, bytes.Count - tripleDesKeyLength).ToArray();
                        DES.Mode = CipherMode.ECB;
                        DES.Padding = PaddingMode.PKCS7;

                        ICryptoTransform DESDecrypt = DES.CreateDecryptor();
                        buffer = DESDecrypt.TransformFinalBlock(buffer, 0, buffer.Length);
                        return buffer;
                    }
                }
                catch (Exception ex)
                {
                    throw ex.Handle(content);
                }
            }

            return null;
        }

        /// <summary>
        /// Decrypt within 3DES.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>System.String.</returns>
        internal static string DecryptR3DES(this string content, Encoding encoding = null)
        {
            string result = content;

            if (!string.IsNullOrWhiteSpace(content))
            {
                try
                {
                    byte[] buffer = Convert.FromBase64String(content);
                    List<byte> bytes = new List<byte>(buffer);
                    using (TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider())
                    {
                        DES.Key = bytes.GetRange(0, tripleDesKeyLength).ToArray();
                        buffer = bytes.GetRange(tripleDesKeyLength, bytes.Count - tripleDesKeyLength).ToArray();
                        DES.Mode = CipherMode.ECB;
                        DES.Padding = PaddingMode.PKCS7;

                        ICryptoTransform DESDecrypt = DES.CreateDecryptor();
                        buffer = DESDecrypt.TransformFinalBlock(buffer, 0, buffer.Length);
                        result = (encoding ?? Encoding.UTF8).GetString(buffer);
                    }
                }
                catch (Exception ex)
                {
                    throw ex.Handle(content);
                }
            }

            return result;
        }

        #endregion My encryption

        #region ToHMACSHA1

        /// <summary>
        /// To the HMAC SHA1.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="secretKey">The secret key.</param>
        /// <returns>System.String.</returns>
        public static string ToHMACSHA1(this string message, byte[] secretKey)
        {
            using (var hmac = new HMACSHA1(secretKey))
            {
                return Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(message)));
            }
        }

        /// <summary>
        /// To the HMAC SHA1.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="secretKey">The secret key.</param>
        /// <returns>System.String.</returns>
        public static string ToHMACSHA1(this string message, string secretKey)
        {
            using (var hmac = new HMACSHA1(Encoding.UTF8.GetBytes(secretKey)))
            {
                return Convert.ToBase64String(
                   hmac.ComputeHash(Encoding.UTF8.GetBytes(message)));
            }
        }

        #endregion ToHMACSHA1
    }
}