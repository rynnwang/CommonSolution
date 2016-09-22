using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Beyova.ExceptionSystem;
using Beyova.Gravity;
using Newtonsoft.Json;

namespace Beyova
{
    /// <summary>
    /// Class EncodingOrSecurityExtension.
    /// </summary>
    public static class EncodingOrSecurityExtension
    {
        #region Html encode

        /// <summary>
        /// To the URL path encoded text. It is used for URL Query String or when passing a URL as parameter of antoher URL. (Space to '%20', but not encode special charactor like '/', '=', '?' and '&amp;'. Any charactor after first '?' would not be encoded)
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
        /// To the URL encoded text. It is used for URL Path. (Space to '+', and encode any special charactor, including '/', '=', '?' and '&amp;')
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

        #endregion

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

        #endregion

        #region Base64

        /// <summary>
        /// Encodes the base64 from string.
        /// </summary>
        /// <param name="stringObject">The source.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>System.String.</returns>
        public static string ToBase64(this string stringObject, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            try
            {
                byte[] bytes = encoding.GetBytes(stringObject);
                return Convert.ToBase64String(bytes);
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Encodes the base64 from bytes.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns>System.String.</returns>
        public static string ToBase64(this byte[] bytes)
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
        /// Decodes the base64 to string.
        /// </summary>
        /// <param name="stringObject">The string object.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>System.String.</returns>
        public static string DecodeBase64ToString(this string stringObject, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            try
            {
                byte[] bytes = Convert.FromBase64String(stringObject);
                return encoding.GetString(bytes);
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Decodes the base64 to bytes.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns>System.Byte[][].</returns>
        public static byte[] DecodeBase64ToBytes(string result)
        {
            try
            {
                byte[] bytes = Convert.FromBase64String(result);
                return bytes;
            }
            catch
            {
                return null;
            }
        }

        #endregion

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
            return ToMD5Bytes(bytes)?.ToBase64();
        }

        /// <summary>
        /// To the m d5 base64 string.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="resetPosition">The reset position.</param>
        /// <returns>System.String.</returns>
        public static string ToBase64Md5(this Stream stream, long? resetPosition = null)
        {
            return ToMD5Bytes(stream, resetPosition)?.ToBase64();
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

        #endregion

        #region SHA1

        /// <summary>
        /// Encrypts to SHA1.
        /// </summary>
        /// <param name="stringObject">The string object.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="OperationFailureException">ToSHA1</exception>
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


        #endregion

        #region 3DES

        /// <summary>
        /// Generates the random3 DES key.
        /// </summary>
        /// <param name="anyObject">Any object.</param>
        /// <returns>System.Byte[].</returns>
        public static byte[] GenerateRandom3DESKey(this object anyObject)
        {
            return anyObject.CreateRandomHex(tripleDesKeyLength);
        }

        /// <summary>
        /// Encrypt3s the DES.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="encryptKey">The encrypt key.</param>
        /// <returns>System.String.</returns>
        public static string Encrypt3DES(this string content, string encryptKey)
        {
            return Encrypt3DES(content, encryptKey, CipherMode.ECB, Encoding.ASCII);
        }

        /// <summary>
        /// Encrypt 3DES.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="encryptKey">The encrypt key.</param>
        /// <param name="cipherMode">The cipher mode.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>System.String.</returns>
        public static string Encrypt3DES(this string content, string encryptKey, CipherMode cipherMode, Encoding encoding)
        {
            string result = null;

            if (!string.IsNullOrWhiteSpace(content) && !string.IsNullOrWhiteSpace(encryptKey) && encoding != null)
            {
                TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();

                DES.Key = ASCIIEncoding.ASCII.GetBytes(encryptKey);
                DES.Mode = cipherMode;

                ICryptoTransform DESEncrypt = DES.CreateEncryptor();

                byte[] Buffer = encoding.GetBytes(content);
                result = Convert.ToBase64String(DESEncrypt.TransformFinalBlock(Buffer, 0, Buffer.Length));
            }

            return result;
        }

        /// <summary>
        /// Decrypt3s the DES.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="encryptKey">The encrypt key.</param>
        /// <returns>System.String.</returns>
        public static string Decrypt3DES(string content, string encryptKey)
        {
            return Decrypt3DES(content, encryptKey, CipherMode.ECB, Encoding.ASCII);
        }

        /// <summary>
        /// Decrypt3s the DES.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="encryptKey">The encrypt key.</param>
        /// <param name="cipherMode">The cipher mode.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>System.String.</returns>
        public static string Decrypt3DES(string content, string encryptKey, CipherMode cipherMode, Encoding encoding)
        {
            string result = null;

            if (!string.IsNullOrWhiteSpace(content) && !string.IsNullOrWhiteSpace(encryptKey) && encoding != null)
            {
                TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();

                DES.Key = ASCIIEncoding.ASCII.GetBytes(encryptKey);
                DES.Mode = cipherMode;
                DES.Padding = PaddingMode.PKCS7;

                ICryptoTransform DESDecrypt = DES.CreateDecryptor();

                try
                {
                    byte[] Buffer = Convert.FromBase64String(content);
                    result = encoding.GetString(DESDecrypt.TransformFinalBlock(Buffer, 0, Buffer.Length));
                }
                catch { }
            }

            return result;
        }

        #endregion

        #region RSA

        /// <summary>
        /// Encrypts within RSA.
        /// </summary>
        /// <param name="dataToEncrypt">The data to encrypt.</param>
        /// <param name="publicKey">The public key.</param>
        /// <param name="dwKeySize">Size of the dw key.</param>
        /// <returns>System.String.</returns>
        public static byte[] RsaEncrypt(this byte[] dataToEncrypt, string publicKey, int dwKeySize = 2048)
        {
            try
            {
                var rsa = new RSACryptoServiceProvider(dwKeySize);
                rsa.ImportCspBlob(Convert.FromBase64String(publicKey));

                //OAEP padding is only available on Microsoft Windows XP or later.  
                return rsa.Encrypt(dataToEncrypt, true);
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
                var rsa = new RSACryptoServiceProvider(dwKeySize);
                rsa.ImportCspBlob(Convert.FromBase64String(privateKey));

                return rsa.Decrypt(dataToDecrypt, true);
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
                var rsa = new RSACryptoServiceProvider(dwKeySize);

                privateKey = rsa.ExportCspBlob(true).ToBase64();
                publicKey = rsa.ExportCspBlob(false).ToBase64();
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

        #endregion

        #region My encryption

        /// <summary>
        /// The description key length
        /// </summary>
        const int desKeyLength = 48;

        /// <summary>
        /// The triple description key length
        /// </summary>
        const int tripleDesKeyLength = 24;

        /// <summary>
        /// Encrypt within 3DES.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="OperationFailureException">R3DEncrypt3DES</exception>
        internal static string R3DEncrypt3DES(this string content, Encoding encoding = null)
        {
            string result = content;

            if (!string.IsNullOrWhiteSpace(content))
            {
                try
                {
                    byte[] keyBytes = content.GenerateRandom3DESKey();
                    TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();

                    DES.Key = keyBytes;
                    DES.Mode = CipherMode.ECB;
                    DES.Padding = PaddingMode.PKCS7;

                    ICryptoTransform DESEncrypt = DES.CreateEncryptor();

                    byte[] buffer = (encoding ?? Encoding.UTF8).GetBytes(content);
                    buffer = DESEncrypt.TransformFinalBlock(buffer, 0, buffer.Length);
                    List<byte> data = new List<byte>(keyBytes);
                    data.AddRange(buffer);
                    result = Convert.ToBase64String(data.ToArray());
                }
                catch (Exception ex)
                {
                    throw ex.Handle(new { content });
                }
            }

            return result;
        }

        /// <summary>
        /// Decrypt within 3DES.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>System.String.</returns>
        internal static string R3DDecrypt3DES(this string content, Encoding encoding = null)
        {
            string result = content;

            if (!string.IsNullOrWhiteSpace(content))
            {
                try
                {
                    byte[] buffer = Convert.FromBase64String(content);
                    List<byte> bytes = new List<byte>(buffer);
                    TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();

                    DES.Key = bytes.GetRange(0, tripleDesKeyLength).ToArray();
                    buffer = bytes.GetRange(tripleDesKeyLength, bytes.Count - tripleDesKeyLength).ToArray();
                    DES.Mode = CipherMode.ECB;
                    DES.Padding = PaddingMode.PKCS7;

                    ICryptoTransform DESDecrypt = DES.CreateDecryptor();
                    buffer = DESDecrypt.TransformFinalBlock(buffer, 0, buffer.Length);
                    result = (encoding ?? Encoding.UTF8).GetString(buffer);
                }
                catch (Exception ex)
                {
                    throw ex.Handle(content);
                }
            }

            return result;
        }

        #endregion

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

        #endregion

        #region Secure Communication

        /// <summary>
        /// Secures the synchronize.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="data">The data.</param>
        /// <param name="rsaPublicKey">The RSA public key.</param>
        /// <returns>System.String.</returns>
        internal static string SecureHttpInvoke<T>(this Uri uri, T data, string rsaPublicKey)
        {
            if (uri != null && data != null && !string.IsNullOrWhiteSpace(rsaPublicKey))
            {
                try
                {
                    var httpRequest = uri.CreateHttpWebRequest(HttpConstants.HttpMethod.Post);
                    httpRequest.SafeSetHttpHeader(HttpConstants.HttpHeader.SECUREKEY, rsaPublicKey);

                    var rsaProvider = new RSACryptoServiceProvider(2048);
                    var responsePublicKey = rsaProvider.ExportCspBlob(false).ToBase64();

                    var package = new SecureMessageObject<T>
                    {
                        PublicKey = responsePublicKey,
                        Data = data
                    };

                    var postBodyBytes = RsaEncrypt(Encoding.UTF8.GetBytes(package.ToJson()), rsaPublicKey);
                    httpRequest.FillData(HttpConstants.HttpMethod.Post, postBodyBytes);

                    var responseBytes = httpRequest.ReadResponseAsBytes();
                    return Encoding.UTF8.GetString(rsaProvider.Decrypt(responseBytes, true));
                }
                catch (Exception ex)
                {
                    throw ex.Handle(new { uri, rsaPublicKey });
                }
            }

            return null;
        }

        /// <summary>
        /// Processes the secure HTTP invoke.
        /// </summary>
        /// <typeparam name="TInput">The type of the input.</typeparam>
        /// <typeparam name="TOuput">The type of the ouput.</typeparam>
        /// <param name="httpContext">The HTTP context.</param>
        /// <param name="processFunc">The process function.</param>
        /// <param name="getPrivateKeyByPublicKey">The get private key by public key.</param>
        /// <returns>Beyova.ExceptionSystem.BaseException.</returns>
        internal static BaseException ProcessSecureHttpInvoke<TInput, TOuput>(this HttpContext httpContext, Func<TInput, TOuput> processFunc, Func<string, string> getPrivateKeyByPublicKey)
        {
            if (httpContext != null)
            {
                try
                {
                    var rsaPublicKey = httpContext.Request.TryGetHeader(HttpConstants.HttpHeader.SECUREKEY);
                    string rsaPrivateKey = null;
                    if (!string.IsNullOrWhiteSpace(rsaPublicKey) && getPrivateKeyByPublicKey != null)
                    {
                        rsaPrivateKey = getPrivateKeyByPublicKey(rsaPublicKey);
                    }

                    if (!string.IsNullOrWhiteSpace(rsaPrivateKey))
                    {
                        var inputPackage = httpContext.Request.GetSecureCommunicationPackage<TInput>(rsaPrivateKey);
                        inputPackage.CheckNullObject("inputPackage");
                        inputPackage.PublicKey.CheckNullObject("inputPackage.PublicKey");
                        inputPackage.Data.CheckNullObject("inputPackage.Data");

                        var ouput = processFunc(inputPackage.Data);
                        httpContext.Response.ResponseSecureCommunicationPackage<TOuput>(ouput, inputPackage.PublicKey);
                    }
                }
                catch (Exception ex)
                {
                    return ex.Handle();
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the secure communication package.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="httpRequest">The HTTP request.</param>
        /// <param name="rsaPrivateKey">The RSA private key.</param>
        /// <returns>SecureSynchronizePackage.</returns>
        internal static SecureMessageObject<T> GetSecureCommunicationPackage<T>(this HttpRequest httpRequest, string rsaPrivateKey)
        {
            if (httpRequest != null)
            {
                try
                {
                    var requestData = httpRequest.GetPostData();
                    var jsonString = Encoding.UTF8.GetString(RsaDecrypt(requestData, rsaPrivateKey));
                    return JsonConvert.DeserializeObject<SecureMessageObject<T>>(jsonString);
                }
                catch (Exception ex)
                {
                    throw ex.Handle(data: new { rsaPrivateKey });
                }
            }

            return null;
        }

        /// <summary>
        /// Responses the secure communication package.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="response">The response.</param>
        /// <param name="responseObject">The response object.</param>
        /// <param name="rsaPublicKey">The RSA public key.</param>
        internal static void ResponseSecureCommunicationPackage<T>(this HttpResponse response, T responseObject, string rsaPublicKey)
        {
            if (response != null && responseObject != null)
            {
                try
                {
                    var responseBodyBytes = RsaEncrypt(Encoding.UTF8.GetBytes(responseObject.ToJson()), rsaPublicKey);
                    response.OutputStream.Write(responseBodyBytes, 0, responseBodyBytes.Length);
                }
                catch (Exception ex)
                {
                    throw ex.Handle(data: new { responseObject, rsaPublicKey });
                }
            }
        }

        #endregion
    }
}
