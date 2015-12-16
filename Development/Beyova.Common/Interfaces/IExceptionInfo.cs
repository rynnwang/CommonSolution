
namespace Beyova.ExceptionSystem
{
    /// <summary>
    /// Interface IExceptionInfo
    /// </summary>
    public interface IExceptionInfo
    {
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        string Message { get; set; }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>The code.</value>
        ExceptionCode Code { get; set; }
    }
}
