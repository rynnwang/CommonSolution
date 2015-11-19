using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net.Mime;
using ifunction.BinaryStorage;

namespace ifunction
{
    /// <summary>
    /// Class EmailMessage.
    /// </summary>
    public class EmailMessage : IIdentifier, ICloneable
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
        /// Gets or sets the from address for this e-mail message.
        /// </summary>
        /// <returns>A <see cref="T:System.Net.Mail.MailAddress" /> that contains the from address information.</returns>
        public string From
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets from display.
        /// </summary>
        /// <value>
        /// From display.
        /// </value>
        public string FromDisplay
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets to list.
        /// </summary>
        /// <value>
        /// To list.
        /// </value>
        public string ToList
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the CC list.
        /// </summary>
        /// <value>
        /// The CC list.
        /// </value>
        public string CCList
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the BCC list.
        /// </summary>
        /// <value>
        /// The BCC list.
        /// </value>
        public string BCCList
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the subject.
        /// </summary>
        /// <value>
        /// The subject.
        /// </value>
        public string Subject
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the headers.
        /// </summary>
        /// <value>
        /// The headers.
        /// </value>
        public string Headers
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the headers encoding.
        /// </summary>
        /// <value>
        /// The headers encoding.
        /// </value>
        public string HeadersEncoding
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the body.
        /// </summary>
        /// <value>
        /// The body.
        /// </value>
        public string Body
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the body encoding.
        /// </summary>
        /// <value>
        /// The body encoding.
        /// </value>
        public string BodyEncoding
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is body HTML.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is body HTML; otherwise, <c>false</c>.
        /// </value>
        public bool IsBodyHtml
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the sender address.
        /// </summary>
        /// <value>
        /// The sender address.
        /// </value>
        public string SenderAddress
        {
            get;
            set;
        }


        /// <summary>
        /// Gets the attachment collection used to store data attached to this e-mail message.
        /// </summary>
        /// <returns>A writable <see cref="T:System.Net.Mail.AttachmentCollection" />.</returns>
        public List<BinaryStorageObject> Attachments
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the mail priority.
        /// </summary>
        /// <value>
        /// The mail priority.
        /// </value>
        public MailPriority Priority
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the reply to list.
        /// </summary>
        /// <value>
        /// The reply to list.
        /// </value>
        public string ReplyToList
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the source module.
        /// </summary>
        /// <value>
        /// The source module.
        /// </value>
        public string SourceModule
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the source identity.
        /// </summary>
        /// <value>
        /// The source identity.
        /// </value>
        public string SourceIdentity
        {
            get;
            set;
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailMessage" /> class.
        /// </summary>
        public EmailMessage()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailMessage"/> class.
        /// </summary>
        /// <param name="from">A <see cref="T:System.String" /> that contains the address of the sender of the e-mail message.</param>
        /// <param name="to">A <see cref="T:System.String" /> that contains the addresses of the recipients of the e-mail message.</param>
        public EmailMessage(string from, string to)
            : this(from, to, string.Empty, string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailMessage"/> class.
        /// </summary>
        /// <param name="from">A <see cref="T:System.String" /> that contains the address of the sender of the e-mail message.</param>
        /// <param name="to">A <see cref="T:System.String" /> that contains the address of the recipient of the e-mail message.</param>
        /// <param name="subject">A <see cref="T:System.String" /> that contains the subject text.</param>
        /// <param name="body">A <see cref="T:System.String" /> that contains the message body.</param>
        public EmailMessage(string from, string to, string subject, string body)
        {
            this.From = from;
            this.ToList = to;
            this.Subject = subject;
            this.Body = body;
            this.Attachments = new List<BinaryStorageObject>();
        }

        #endregion

        #region Convert

        /// <summary>
        /// Generates the alternate view.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <param name="mimeType">Type of the MIME.</param>
        /// <returns>AlternateView.</returns>
        public static AlternateView GenerateAlternateView(string body, ContentType mimeType)
        {
            var alternate = AlternateView.CreateAlternateViewFromString(body, mimeType);
            return alternate;
        }

        /// <summary>
        /// Fills the alternate view.
        /// </summary>
        /// <param name="mail">The mail.</param>
        /// <param name="mimeType">Type of the MIME.</param>
        public static void FillAlternateView(MailMessage mail, ContentType mimeType)
        {
            if (mail != null && mimeType != null)
            {
                mail.AlternateViews.Add(GenerateAlternateView(mail.Body, mimeType));
            }
        }

        #endregion

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns>System.Object.</returns>
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
