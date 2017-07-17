using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Beyova.Web
{
    /// <summary>
    /// 
    /// </summary>
    public static class WebUiBuilder
    {
        /// <summary>
        /// Creates the CheckBox.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="isChecked">if set to <c>true</c> [is checked].</param>
        /// <param name="classNames">The class names.</param>
        /// <param name="labelName">Name of the label.</param>
        /// <param name="styles">The styles.</param>
        /// <param name="domId">The DOM identifier.</param>
        /// <param name="otherAttributes">The other attributes.</param>
        /// <returns></returns>
        public static string CreateCheckBox(string value, bool isChecked, string classNames = null, string labelName = null, string styles = null, string domId = null, Dictionary<string, string> otherAttributes = null)
        {
            StringBuilder builder = new StringBuilder(256);

            if (string.IsNullOrWhiteSpace(labelName))
            {
                builder.Append("<input type=\"checkbox\" ");
            }
            else
            {
                builder.Append("<label><input type=\"checkbox\" ");
            }

            if (isChecked)
            {
                builder.Append("checked=\"checked\" ");
            }

            builder.Append("value=\"");
            builder.Append(value.SafeToString());
            builder.Append("\" ");

            if (!string.IsNullOrWhiteSpace(classNames))
            {
                builder.Append("class=\"");
                builder.Append(classNames);
                builder.Append("\" ");
            }

            if (!string.IsNullOrWhiteSpace(domId))
            {
                builder.Append("id=\"");
                builder.Append(domId);
                builder.Append("\" ");
            }

            if (!string.IsNullOrWhiteSpace(styles))
            {
                builder.Append("style=\"");
                builder.Append(styles);
                builder.Append("\" ");
            }

            if (otherAttributes.HasItem())
            {
                foreach (var one in otherAttributes)
                {
                    builder.Append(one.Key);
                    builder.Append("=\"");
                    builder.Append(one.Value.SafeToString());
                    builder.Append("\" ");
                }
            }

            builder.Append("/>");
            if (!string.IsNullOrWhiteSpace(labelName))
            {
                builder.Append(labelName);
                builder.Append("</label>");
            }

            return builder.ToString();
        }

        /// <summary>
        /// Creates the CheckBox.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="value">The value.</param>
        /// <param name="isChecked">if set to <c>true</c> [is checked].</param>
        /// <param name="classNames">The class names.</param>
        /// <param name="labelName">Name of the label.</param>
        /// <param name="styles">The styles.</param>
        /// <param name="domId">The DOM identifier.</param>
        /// <param name="otherAttributes">The other attributes.</param>
        /// <returns></returns>
        public static IHtmlString CreateCheckBox<TModel>(this HtmlHelper<TModel> htmlHelper,
            string value, bool isChecked, string classNames = null, string labelName = null, string styles = null, string domId = null, Dictionary<string, string> otherAttributes = null)
        {
            return htmlHelper?.Raw(CreateCheckBox(value, isChecked, classNames, labelName, styles, domId, otherAttributes));
        }
    }
}
