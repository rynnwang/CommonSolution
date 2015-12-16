using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using Beyova.ExceptionSystem;

namespace Beyova
{
    /// <summary>
    /// Class of sender for mails
    /// </summary>
    public class MailSender : IIdentifier
    {
        #region Properties

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        public Guid? Key
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the SMTP address.
        /// </summary>
        /// <value>
        /// The SMTP address.
        /// </value>
        public string SmtpAddress
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public string UserName
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the full mail address.
        /// </summary>
        /// <value>
        /// The full mail address.
        /// </value>
        public string FullMailAddress
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public string Password
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the sender identity.
        /// </summary>
        /// <value>
        /// The sender identity.
        /// </value>
        public string SenderIdentity
        {
            get;
            protected set;
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MailSender"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="smtpAddress">The SMTP address.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <param name="fullMailAddress">The full mail address.</param>
        public MailSender(Guid key, string smtpAddress, string userName, string password, string fullMailAddress)
        {
            this.Key = key;
            this.SmtpAddress = smtpAddress;
            this.UserName = userName;
            this.Password = password;
            this.FullMailAddress = fullMailAddress;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MailSender"/> class.
        /// </summary>
        /// <param name="smtpAddress">The SMTP address.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <param name="fullMailAddress">The full mail address.</param>
        public MailSender(string smtpAddress, string userName, string password, string fullMailAddress)
            : this(Guid.NewGuid(), smtpAddress, userName, password, fullMailAddress)
        {
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Sends the mail.
        /// </summary>
        /// <param name="mailMessage">The mail message.</param>
        /// <param name="fromDisplay">From display.</param>
        /// <exception cref="OperationFailureException">SendMail</exception>
        public virtual void SendMail(MailMessage mailMessage, string fromDisplay)
        {
            mailMessage.CheckNullObject("mailMessage");

            try
            {
                var smtpClient = new SmtpClient
                {
                    Credentials = new NetworkCredential(this.FullMailAddress, this.Password)
                };

                this.SenderIdentity = this.FullMailAddress;
                if (this.SmtpAddress.Contains(":"))
                {
                    string[] parts = this.SmtpAddress.Split(':');
                    smtpClient.Host = parts[0];
                    smtpClient.Port = parts[1].ObjectToInt32();
                }
                else
                {
                    smtpClient.Host = this.SmtpAddress;
                }
                mailMessage.From = new MailAddress(this.FullMailAddress);
                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                throw new OperationFailureException("SendMail", ex, mailMessage);
            }
        }

        /// <summary>
        /// Sends the mail.
        /// </summary>
        /// <param name="subject">The subject.</param>
        /// <param name="body">The body.</param>
        /// <param name="isHtml">if set to <c>true</c> [is HTML].</param>
        /// <param name="encoding">The encoding.</param>
        /// <param name="toList">To list.</param>
        /// <param name="ccList">The cc list.</param>
        /// <param name="bccList">The BCC list.</param>
        /// <param name="attachments">The attachments.</param>
        /// <param name="mailPriority">The mail priority.</param>
        public void SendMail(string subject, string body, bool isHtml, Encoding encoding, MailAddress[] toList, MailAddress[] ccList, MailAddress[] bccList, List<Attachment> attachments, MailPriority mailPriority)
        {
            MailMessage mailMessage = GenerateMailMessage(subject, body, isHtml, encoding, toList, ccList, bccList, attachments, mailPriority);
            SendMail(mailMessage, null);
        }

        #endregion

        #region GenerateMailMessage

        /// <summary>
        /// Generates the mail message.
        /// </summary>
        /// <param name="subject">The subject.</param>
        /// <param name="body">The body.</param>
        /// <param name="isHtml">if set to <c>true</c> [is HTML].</param>
        /// <param name="encoding">The encoding.</param>
        /// <param name="toList">To list.</param>
        /// <param name="ccList">The cc list.</param>
        /// <param name="bccList">The BCC list.</param>
        /// <param name="attachments">The attachments.</param>
        /// <param name="mailPriority">The mail priority.</param>
        /// <returns>MailMessage.</returns>
        /// <exception cref="InvalidObjectException">toList</exception>
        public static MailMessage GenerateMailMessage(string subject, string body, bool isHtml, Encoding encoding, MailAddress[] toList, MailAddress[] ccList, MailAddress[] bccList, List<Attachment> attachments, MailPriority mailPriority = MailPriority.Normal)
        {
            toList.CheckNullObject("toList");

            if (toList.Length == 0)
            {
                throw new InvalidObjectException("toList");
            }

            var mailMessage = new MailMessage();
            mailMessage.Subject = mailMessage.SafeToString(subject);
            mailMessage.Body = mailMessage.SafeToString(body);
            mailMessage.BodyEncoding = encoding ?? Encoding.UTF8;
            mailMessage.IsBodyHtml = isHtml;

            foreach (MailAddress to in toList)
            {
                mailMessage.To.Add(to);
            }

            if (ccList != null && ccList.Length > 0)
            {
                foreach (MailAddress cc in ccList.Where(cc => cc != null))
                {
                    mailMessage.CC.Add(cc);
                }
            }

            if (bccList != null && bccList.Length > 0)
            {
                foreach (var bcc in bccList.Where(bcc => bcc != null))
                {
                    mailMessage.Bcc.Add(bcc);
                }
            }

            if (attachments != null && attachments.Count > 0)
            {
                foreach (var attachment in attachments)
                {
                    mailMessage.Attachments.Add(attachment);
                }
            }

            mailMessage.Priority = mailPriority;

            return mailMessage;
        }


        /// <summary>
        /// Generates the mail message.
        /// </summary>
        /// <param name="subject">The subject.</param>
        /// <param name="body">The body.</param>
        /// <param name="toList">To list.</param>
        /// <param name="ccList">The cc list.</param>
        /// <param name="bccList">The BCC list.</param>
        /// <param name="attachments">The attachments.</param>
        /// <param name="mailPriority">The mail priority.</param>
        /// <returns></returns>
        public static MailMessage GenerateMailMessage(string subject, string body, MailAddress[] toList, MailAddress[] ccList, MailAddress[] bccList, List<Attachment> attachments, MailPriority mailPriority)
        {
            return GenerateMailMessage(subject, body, true, Encoding.UTF8, toList, ccList, bccList, attachments, mailPriority);
        }

        /// <summary>
        /// Generates the mail message.
        /// </summary>
        /// <param name="subject">The subject.</param>
        /// <param name="body">The body.</param>
        /// <param name="isHtml">if set to <c>true</c> [is HTML].</param>
        /// <param name="encoding">The encoding.</param>
        /// <param name="toList">To list.</param>
        /// <param name="ccList">The cc list.</param>
        /// <returns></returns>
        public static MailMessage GenerateMailMessage(string subject, string body, bool isHtml, Encoding encoding, MailAddress[] toList, MailAddress[] ccList = null)
        {
            return GenerateMailMessage(subject, body, isHtml, encoding, toList, ccList, null, null, MailPriority.Normal);
        }

        /// <summary>
        /// Generates the mail message.
        /// </summary>
        /// <param name="subject">The subject.</param>
        /// <param name="body">The body.</param>
        /// <param name="toList">To list.</param>
        /// <param name="ccList">The cc list.</param>
        /// <returns>MailMessage.</returns>
        public static MailMessage GenerateMailMessage(string subject, string body, MailAddress[] toList, MailAddress[] ccList = null)
        {
            return GenerateMailMessage(subject, body, true, Encoding.UTF8, toList, ccList, null, null, MailPriority.Normal);
        }

        #endregion

        /// <summary>
        /// Sets the password.
        /// </summary>
        /// <param name="password">The password.</param>
        public void SetPassword(string password)
        {
            this.Password = password;
        }
    }
}
