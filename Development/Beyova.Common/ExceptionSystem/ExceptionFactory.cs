using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Beyova.ExceptionSystem;

namespace Beyova
{
    /// <summary>
    /// Class ExceptionFactory.
    /// </summary>
    public static class ExceptionFactory
    {
        /// <summary>
        /// Checks empty string.
        /// </summary>
        /// <param name="anyString">Any string.</param>
        /// <param name="objectIdentity">The object identity.</param>
        /// <param name="memberName">Name of the member.</param>
        /// <param name="sourceFilePath">The source file path.</param>
        /// <param name="sourceLineNumber">The source line number.</param>
        /// <exception cref="NullObjectException"></exception>
        public static void CheckEmptyString(this string anyString, string objectIdentity,
            [CallerMemberName] string memberName = null,
            [CallerFilePath] string sourceFilePath = null,
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (string.IsNullOrWhiteSpace(anyString))
            {
                throw new NullObjectException(objectIdentity, new ExceptionScene
                {
                    FilePath = sourceFilePath,
                    LineNumber = sourceLineNumber,
                    MethodName = memberName
                });
            }
        }

        /// <summary>
        /// Checks null object.
        /// </summary>
        /// <param name="anyObject">Any object.</param>
        /// <param name="objectIdentity">The object identity.</param>
        /// <param name="memberName">Name of the member.</param>
        /// <param name="sourceFilePath">The source file path.</param>
        /// <param name="sourceLineNumber">The source line number.</param>
        /// <exception cref="NullObjectException"></exception>
        public static void CheckNullObject(this object anyObject, string objectIdentity,
            [CallerMemberName] string memberName = null,
            [CallerFilePath] string sourceFilePath = null,
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (anyObject == null)
            {
                throw new NullObjectException(objectIdentity, new ExceptionScene
                {
                    FilePath = sourceFilePath,
                    LineNumber = sourceLineNumber,
                    MethodName = memberName
                });
            }
        }

        /// <summary>
        /// Checks the null resource.
        /// </summary>
        /// <param name="anyObject">Any object.</param>
        /// <param name="resourceName">Name of the resource.</param>
        /// <param name="resourceIdentity">The resource identity.</param>
        /// <param name="memberName">Name of the member.</param>
        /// <param name="sourceFilePath">The source file path.</param>
        /// <param name="sourceLineNumber">The source line number.</param>
        /// <exception cref="Beyova.ExceptionSystem.ResourceNotFoundException"></exception>
        /// <exception cref="ExceptionScene"></exception>
        public static void CheckNullResource(this object anyObject, string resourceName, string resourceIdentity,
            [CallerMemberName] string memberName = null,
            [CallerFilePath] string sourceFilePath = null,
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (anyObject == null)
            {
                throw new ResourceNotFoundException(resourceName, resourceIdentity, new ExceptionScene
                {
                    FilePath = sourceFilePath,
                    LineNumber = sourceLineNumber,
                    MethodName = memberName
                });
            }
        }

        /// <summary>
        /// Checks the null or empty collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="objectIdentity">The object identity.</param>
        /// <param name="memberName">Name of the member.</param>
        /// <param name="sourceFilePath">The source file path.</param>
        /// <param name="sourceLineNumber">The source line number.</param>
        /// <exception cref="InvalidObjectException">Null or empty collection</exception>
        /// <exception cref="ExceptionScene"></exception>
        public static void CheckNullOrEmptyCollection<T>(this IEnumerable<T> collection, string objectIdentity,
            [CallerMemberName] string memberName = null,
            [CallerFilePath] string sourceFilePath = null,
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (collection == null || !collection.Any())
            {
                throw new InvalidObjectException(objectIdentity, reason: "EmptyCollection", scene: new ExceptionScene
                {
                    FilePath = sourceFilePath,
                    LineNumber = sourceLineNumber,
                    MethodName = memberName
                });
            }
        }

        /// <summary>
        /// Creates the operation exception.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="minor">The minor.</param>
        /// <param name="hint">The hint.</param>
        /// <param name="memberName">Name of the member.</param>
        /// <param name="sourceFilePath">The source file path.</param>
        /// <param name="sourceLineNumber">The source line number.</param>
        /// <returns>OperationFailureException.</returns>
        /// <exception cref="Beyova.ExceptionSystem.OperationFailureException"></exception>
        /// <exception cref="ExceptionScene"></exception>
        public static OperationFailureException CreateOperationException(object data = null, string minor = null, FriendlyHint hint = null,
            [CallerMemberName] string memberName = null,
            [CallerFilePath] string sourceFilePath = null,
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            return new OperationFailureException(data: data, minor: minor, hint: hint, scene: new ExceptionScene
            {
                FilePath = sourceFilePath,
                LineNumber = sourceLineNumber,
                MethodName = memberName
            });
        }

        /// <summary>
        /// Creates the invalid object exception.
        /// </summary>
        /// <param name="objectIdentifier">The object identifier.</param>
        /// <param name="data">The data.</param>
        /// <param name="reason">The reason.</param>
        /// <param name="hint">The hint.</param>
        /// <param name="memberName">Name of the member.</param>
        /// <param name="sourceFilePath">The source file path.</param>
        /// <param name="sourceLineNumber">The source line number.</param>
        /// <returns>InvalidObjectException.</returns>
        public static InvalidObjectException CreateInvalidObjectException(string objectIdentifier, object data = null, string reason = null, FriendlyHint hint = null,
            [CallerMemberName] string memberName = null,
            [CallerFilePath] string sourceFilePath = null,
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            return new InvalidObjectException(objectIdentifier, data: data, reason: reason, hint: hint, scene: new ExceptionScene
            {
                FilePath = sourceFilePath,
                LineNumber = sourceLineNumber,
                MethodName = memberName
            });
        }

        /// <summary>
        /// Creates the operation forbidden exception.
        /// </summary>
        /// <param name="reason">The reason.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="data">The data.</param>
        /// <param name="hint">The hint.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="memberName">Name of the member.</param>
        /// <param name="sourceFilePath">The source file path.</param>
        /// <param name="sourceLineNumber">The source line number.</param>
        /// <returns>OperationForbiddenException.</returns>
        public static OperationForbiddenException CreateOperationForbiddenException(string reason, Exception innerException = null, object data = null, FriendlyHint hint = null,
            [CallerMemberName] string actionName = null,
            [CallerMemberName] string memberName = null,
            [CallerFilePath] string sourceFilePath = null,
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            return new OperationForbiddenException(actionName, reason, innerException, data, hint, new ExceptionScene
            {
                FilePath = sourceFilePath,
                LineNumber = sourceLineNumber,
                MethodName = memberName
            });
        }

        /// <summary>
        /// Creates the unauthorized token exception.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="hint">The hint.</param>
        /// <param name="memberName">Name of the member.</param>
        /// <param name="sourceFilePath">The source file path.</param>
        /// <param name="sourceLineNumber">The source line number.</param>
        /// <returns></returns>
        public static UnauthorizedTokenException CreateUnauthorizedTokenException(string token, FriendlyHint hint = null,
            [CallerMemberName] string memberName = null,
            [CallerFilePath] string sourceFilePath = null,
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            return new UnauthorizedTokenException(data: token, hint: hint, scene: new ExceptionScene
            {
                FilePath = sourceFilePath,
                LineNumber = sourceLineNumber,
                MethodName = memberName
            });
        }

        /// <summary>
        /// Creates the unsupported exception.
        /// </summary>
        /// <param name="objectIdentifier">The object identifier.</param>
        /// <param name="data">The data.</param>
        /// <param name="reason">The reason.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="hint">The hint.</param>
        /// <param name="memberName">Name of the member.</param>
        /// <param name="sourceFilePath">The source file path.</param>
        /// <param name="sourceLineNumber">The source line number.</param>
        /// <returns>UnsupportedException.</returns>
        public static UnsupportedException CreateUnsupportedException(string objectIdentifier, object data = null, string reason = null, Exception innerException = null, FriendlyHint hint = null,
        [CallerMemberName] string memberName = null,
        [CallerFilePath] string sourceFilePath = null,
        [CallerLineNumber] int sourceLineNumber = 0)
        {
            return new UnsupportedException(objectIdentifier, data: data, innerException: innerException, reason: reason, hint: hint, scene: new ExceptionScene
            {
                FilePath = sourceFilePath,
                LineNumber = sourceLineNumber,
                MethodName = memberName
            });
        }
    }
}
