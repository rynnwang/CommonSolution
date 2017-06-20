using System.Text;

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
        private static readonly char[] separators = new char[] { StringConstants.CommaChar, ';' };

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