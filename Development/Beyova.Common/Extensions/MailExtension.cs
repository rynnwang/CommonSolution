using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using Beyova.BinaryStorage;

namespace Beyova
{
    /// <summary>
    /// Extension class for mail.
    /// </summary>
    public static class MailExtension
    {
        /// <summary>
        /// The separators
        /// </summary>
        private static readonly char[] separators = new char[] { ',', ';' };

        #region Extensions email translation

        /// <summary>
        /// To the mail message.
        /// </summary>
        /// <param name="emailMessageObject">The email message object.</param>
        /// <returns>MailMessage.</returns>
        public static MailMessage ToMailMessage(this EmailMessage emailMessageObject)
        {
            try
            {
                emailMessageObject.CheckNullObject("emailMessageObject");

                var result = new MailMessage
                {
                    From = new MailAddress(emailMessageObject.From),
                    Subject = emailMessageObject.Subject,
                    IsBodyHtml = emailMessageObject.IsBodyHtml,
                    BodyEncoding = GetMailEncoding(emailMessageObject.BodyEncoding),
                    HeadersEncoding = GetMailEncoding(emailMessageObject.HeadersEncoding),
                    Body = emailMessageObject.Body,
                    Priority = emailMessageObject.Priority
                };

                result.FillHeaders(emailMessageObject.Headers);
                emailMessageObject.FillMailAddressCollection(result.To, emailMessageObject.ToList);
                emailMessageObject.FillMailAddressCollection(result.CC, emailMessageObject.CCList);
                emailMessageObject.FillMailAddressCollection(result.Bcc, emailMessageObject.BCCList);

                if (!string.IsNullOrWhiteSpace(emailMessageObject.ReplyToList))
                {
                    foreach (var one in emailMessageObject.ReplyToList.Split(separators, StringSplitOptions.RemoveEmptyEntries))
                    {
                        result.ReplyToList.Add(new MailAddress(one));
                    }
                }
                result.Sender = new MailAddress(emailMessageObject.SenderAddress);

                if (emailMessageObject.Attachments != null && emailMessageObject.Attachments.Count > 0)
                {
                    foreach (var one in emailMessageObject.Attachments)
                    {
                        result.Attachments.Add(one.ToAttachment());
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex.Handle("ToMailMessage", emailMessageObject);
            }
        }

        /// <summary>
        /// To the email message.
        /// </summary>
        /// <param name="mailMessageObject">The mail message object.</param>
        /// <returns>EmailMessage.</returns>
        public static EmailMessage ToEmailMessage(this MailMessage mailMessageObject)
        {
            EmailMessage result = null;

            if (mailMessageObject != null)
            {
                result = new EmailMessage
                {
                    ToList = mailMessageObject.To.ToString(),
                    CCList = mailMessageObject.CC.ToString(),
                    BCCList = mailMessageObject.Bcc.ToString(),
                    Subject = mailMessageObject.Subject,
                    SenderAddress = mailMessageObject.Sender.ToString(),
                    From = mailMessageObject.From.ToString(),
                    BodyEncoding = mailMessageObject.BodyEncoding.WebName,
                    IsBodyHtml = mailMessageObject.IsBodyHtml,
                    Body = mailMessageObject.Body,
                    HeadersEncoding = mailMessageObject.HeadersEncoding.WebName,
                    Headers = mailMessageObject.Headers.ToKeyValueString(true),
                    Attachments = new List<BinaryStorageObject>(),
                    Priority = mailMessageObject.Priority
                };

                List<string> replyToList = new List<string>();
                foreach (var one in mailMessageObject.ReplyToList)
                {
                    replyToList.Add(one.ToString());
                }
                result.ReplyToList = replyToList.Join(",");

                if (mailMessageObject.Attachments.Count > 0)
                {
                    foreach (var one in mailMessageObject.Attachments)
                    {
                        result.Attachments.Add(one.ToBinary());
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// To the binary.
        /// </summary>
        /// <param name="attachmentObject">The attachment object.</param>
        /// <returns>BinaryStorageObject.</returns>
        public static BinaryStorageObject ToBinary(this Attachment attachmentObject)
        {
            BinaryStorageObject result = null;

            if (attachmentObject != null)
            {
                result = new BinaryStorageObject
                {
                    DataInBytes = attachmentObject.ContentStream.ToBytes(),
                    Name = attachmentObject.Name
                };
            }

            return result;
        }

        /// <summary>
        /// To the attachment.
        /// </summary>
        /// <param name="binaryObject">The binary object.</param>
        /// <returns>Attachment.</returns>
        public static Attachment ToAttachment(this BinaryStorageObject binaryObject)
        {
            Attachment result = null;

            if (binaryObject != null && binaryObject.DataInBytes.Length > 0)
            {
                var stream = binaryObject.DataInBytes.ToStream();
                result = new Attachment(stream, binaryObject.Name)
                {
                    ContentType = new ContentType(binaryObject.Mime.SafeToString("application/octstream"))
                };
            }

            return result;
        }

        /// <summary>
        /// To the mail addresses.
        /// </summary>
        /// <param name="emailMessageObject">The email message object.</param>
        /// <param name="addressString">The address string.</param>
        /// <returns>MailAddressCollection.</returns>
        public static MailAddressCollection ToMailAddresses(this EmailMessage emailMessageObject, string addressString)
        {
            var result = new MailAddressCollection();
            FillMailAddressCollection(emailMessageObject, result, addressString);
            return result;
        }

        /// <summary>
        /// Fills the mail address collection.
        /// </summary>
        /// <param name="emailMessageObject">The email message object.</param>
        /// <param name="mailAddressCollection">The mail address collection.</param>
        /// <param name="addressString">The address string.</param>
        public static void FillMailAddressCollection(this EmailMessage emailMessageObject, MailAddressCollection mailAddressCollection, string addressString)
        {
            if (mailAddressCollection != null)
            {
                try
                {
                    foreach (var one in addressString.SafeToString().Split(separators, StringSplitOptions.RemoveEmptyEntries))
                    {
                        mailAddressCollection.Add(new MailAddress(one));
                    }
                }
                catch (Exception ex)
                {
                    throw ex.Handle("FillMailAddressCollection", addressString);
                }
            }
        }

        /// <summary>
        /// To the mail addresses.
        /// </summary>
        /// <param name="mailMessageObject">The mail message object.</param>
        /// <param name="mailAddressCollection">The mail address collection.</param>
        /// <returns>System.String.</returns>
        public static string ToMailAddresses(this MailMessage mailMessageObject, MailAddressCollection mailAddressCollection)
        {
            return (mailAddressCollection != null) ? mailAddressCollection.ToString() : string.Empty;
        }

        /// <summary>
        /// Fills the headers.
        /// </summary>
        /// <param name="mailMessageObject">The mail message object.</param>
        /// <param name="headerString">The header string.</param>
        public static void FillHeaders(this MailMessage mailMessageObject, string headerString)
        {
            if (mailMessageObject != null)
            {
                mailMessageObject.Headers.FillValuesByKeyValueString(headerString);
            }
        }

        #endregion

        /// <summary>
        /// Gets the encoding.
        /// If failed to get, return UTF-8
        /// </summary>
        /// <param name="webName">Name of the web.</param>
        /// <returns>Encoding.</returns>
        public static Encoding GetMailEncoding(string webName)
        {
            Encoding result = null;

            try
            {
                result = Encoding.GetEncoding(webName);
            }
            catch { }

            return result ?? Encoding.UTF8;
        }

        /// <summary>
        /// Gets the name of the email.
        /// </summary>
        /// <param name="anyEmail">Any email.</param>
        /// <returns></returns>
        public static string GetEmailName(this string anyEmail)
        {
            if (anyEmail.IsEmailAddress())
            {
                string name = anyEmail.Substring(0, anyEmail.IndexOf('@'));
                name = name.Replace(new char[] { '.', '-', '_' }, ' ');

                return name;
            }
            else
            {
                return anyEmail;
            }
        }
    }
}
