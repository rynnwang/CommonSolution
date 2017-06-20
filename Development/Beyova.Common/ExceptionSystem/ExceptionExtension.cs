using System;
using System.Data.SqlClient;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Web;
using Beyova.ExceptionSystem;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Beyova
{
    /// <summary>
    /// Class ExceptionExtension.
    /// </summary>
    public static class ExceptionExtension
    {
        private const char indentChar = '-';

        /// <summary>
        /// To the HTTP operation exception.
        /// </summary>
        /// <param name="webException">The web exception.</param>
        /// <param name="httpWebRequest">The HTTP web request.</param>
        /// <param name="cookies">The cookies.</param>
        /// <param name="headers">The headers.</param>
        /// <returns></returns>
        public static HttpOperationException ToHttpOperationException(this WebException webException, HttpWebRequest httpWebRequest, out CookieCollection cookies, out WebHeaderCollection headers)
        {
            HttpOperationException result = null;
            cookies = null;
            headers = null;

            if (webException != null)
            {
                var webResponse = (HttpWebResponse)webException.Response;
                headers = webResponse.Headers;
                var destinationMachine = headers?.Get(HttpConstants.HttpHeader.SERVERNAME);
                cookies = webResponse.Cookies;

                var responseText = webResponse.ReadAsText();

                result = new HttpOperationException(httpWebRequest.RequestUri.ToString(),
                    httpWebRequest.Method,
                    webException.Message,
                    httpWebRequest.ContentLength,
                    responseText,
                    webResponse.StatusCode,
                    webException.Status, destinationMachine);
            }

            return result;
        }

        /// <summary>
        /// Converts the exception to base exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="data">The data.</param>
        /// <param name="memberName">Name of the member.</param>
        /// <param name="sourceFilePath">The source file path.</param>
        /// <param name="sourceLineNumber">The source line number.</param>
        /// <returns>BaseException.</returns>
        public static BaseException ConvertExceptionToBaseException(this Exception exception, object data = null, [CallerMemberName] string memberName = null,
            [CallerFilePath] string sourceFilePath = null,
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            return exception == null ? null : ((exception as BaseException) ?? exception.Handle(new ExceptionScene
            {
                FilePath = sourceFilePath,
                LineNumber = sourceLineNumber,
                MethodName = memberName
            }, data));
        }

        /// <summary>
        /// Roots the exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns>Exception.</returns>
        public static Exception RootException(this Exception exception)
        {
            if (exception != null)
            {
                return exception.InnerException == null ? exception : exception.InnerException.RootException() as Exception;
            }

            return null;
        }

        /// <summary>
        /// Formats to string.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns>System.String.</returns>
        public static string FormatToString(this Exception exception)
        {
            StringBuilder stringBuilder = new StringBuilder();

            if (exception != null)
            {
                stringBuilder.AppendLine("-----------------------  Exception  -----------------------");
                stringBuilder.AppendLine("-----------------------  " + DateTime.UtcNow.ToFullDateTimeString());
                stringBuilder.AppendLine("-----------------------  Thread ID: " + Thread.CurrentThread.ManagedThreadId.ToString());

                FormatToString(stringBuilder, exception, 0);

                stringBuilder.AppendLine("---------------------------  End  ---------------------------");
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Formats to string.
        /// </summary>
        /// <param name="stringBuilder">The string builder.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="level">The level.</param>
        private static void FormatToString(StringBuilder stringBuilder, Exception exception, int level)
        {
            if (stringBuilder != null && exception != null)
            {
                BaseException baseException = exception as BaseException;

                stringBuilder.AppendIndent(level).AppendLineWithFormat("Exception Type: {0}", exception.GetType().ToString());

                if (baseException != null)
                {
                    stringBuilder.AppendIndent(level).AppendLineWithFormat("Exception Code: {0}({1})", baseException.Code.ToString(), (int)baseException.Code);
                }

                SqlException sqlException = exception as SqlException;
                if (sqlException != null)
                {
                    int i = 0;

                    stringBuilder.AppendIndent(level).AppendLineWithFormat("SQL error. Count = {0}", sqlException.Errors.Count);

                    foreach (SqlError sqlError in sqlException.Errors)
                    {
                        i++;
                        int tempLevel = level + 1;
                        stringBuilder.AppendIndent(tempLevel).AppendLineWithFormat("---------- Error #{0} ----------", i);
                        stringBuilder.AppendIndent(tempLevel).AppendLineWithFormat("->Class: {0}", sqlError.Class);
                        stringBuilder.AppendIndent(tempLevel).AppendLineWithFormat("->Number: {0}", sqlError.Number);
                        stringBuilder.AppendIndent(tempLevel).AppendLineWithFormat("->Server: {0}", sqlError.Server);
                        stringBuilder.AppendIndent(tempLevel).AppendLineWithFormat("->Source: {0}", sqlError.Source);
                        stringBuilder.AppendIndent(tempLevel).AppendLineWithFormat("->Procedure: {0}", sqlError.Procedure);
                        stringBuilder.AppendIndent(tempLevel).AppendLineWithFormat("->LineNumber: {0}", sqlError.LineNumber);
                        stringBuilder.AppendIndent(tempLevel).AppendLineWithFormat("->State: {0}", sqlError.State);
                        stringBuilder.AppendIndent(tempLevel).AppendLineWithFormat("->Message: {0}", sqlError.Message);
                    }
                }

                stringBuilder.AppendIndent(level).AppendLineWithFormat("Exception Message: {0}", exception.Message);
                stringBuilder.AppendIndent(level).AppendLineWithFormat("Source: {0}", exception.Source);
                stringBuilder.AppendIndent(level).AppendLineWithFormat("Site: {0}", exception.TargetSite);
                stringBuilder.AppendIndent(level).AppendLineWithFormat("StackTrace: {0}", exception.StackTrace);

                if (baseException != null)
                {
                    stringBuilder.AppendIndent(level).AppendLineWithFormat("Exception Code: {0}({1})", baseException.Code.ToString(), (int)baseException.Code);
                    stringBuilder.AppendIndent(level).AppendLineWithFormat("Operator Credential: {0}", baseException.OperatorCredential.ToJson());
                    stringBuilder.AppendIndent(level).AppendLineWithFormat("Scene: {0}", baseException.Scene.ToJson());
                    stringBuilder.AppendIndent(level).AppendLineWithFormat("Hint: {0}", baseException.Hint.ToJson());
                }

                stringBuilder.AppendIndent(level).AppendLineWithFormat("Data Reference: {0}", GenerateDataString(baseException?.ReferenceData?.ToJson()));

                if (exception.InnerException != null)
                {
                    level++;
                    stringBuilder.AppendIndent(level).AppendLine("--------------------  Inner Exception  --------------------");
                    FormatToString(stringBuilder, exception.InnerException, level);
                }
            }
        }

        /// <summary>
        /// Appends the indent.
        /// Returns  the same <see cref="StringBuilder"/> instance to make sure it supports chain based actions.
        /// </summary>
        /// <param name="stringBuilder">The string builder.</param>
        /// <param name="level">The level.</param>
        /// <returns>System.Text.StringBuilder.</returns>
        private static StringBuilder AppendIndent(this StringBuilder stringBuilder, int level)
        {
            if (stringBuilder != null)
            {
                stringBuilder.AppendIndent(indentChar, level * 2);
                stringBuilder.Append(" ");
            }

            return stringBuilder;
        }

        #region GenerateDataString

        /// <summary>
        /// Generates the data string.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>System.String.</returns>
        private static string GenerateDataString(HttpContext obj)
        {
            return GenerateDataString(obj == null ? null : obj.Request);
        }

        /// <summary>
        /// Generates the data string.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>System.String.</returns>
        private static string GenerateDataString(HttpListenerContext obj)
        {
            return GenerateDataString(obj == null ? null : obj.Request);
        }

        /// <summary>
        /// Generates the data string.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>System.String.</returns>
        private static string GenerateDataString(object obj)
        {
            string result = "<null>";

            if (obj != null)
            {
                result = JsonConvert.SerializeObject(obj, SerializationExtension.SafeJsonSerializationSettings);
            }

            return result;
        }

        #endregion GenerateDataString

        #region Handle exception

        /// <summary>
        /// Handles the specified exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="data">The data.</param>
        /// <param name="hint">The hint.</param>
        /// <param name="operationName">Name of the operation.</param>
        /// <param name="sourceFilePath">The source file path.</param>
        /// <param name="sourceLineNumber">The source line number.</param>
        /// <returns>Beyova.ExceptionSystem.BaseException.</returns>
        public static BaseException Handle(this Exception exception, object data = null, FriendlyHint hint = null,
                    [CallerMemberName] string operationName = null,
                    [CallerFilePath] string sourceFilePath = null,
                    [CallerLineNumber] int sourceLineNumber = 0)
        {
            return Handle(exception, new ExceptionScene
            {
                MethodName = operationName,
                FilePath = sourceFilePath,
                LineNumber = sourceLineNumber
            }, data, hint);
        }

        /// <summary>
        /// Handles the exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="scene">The scene.</param>
        /// <param name="data">The data.</param>
        /// <param name="hint">The hint.</param>
        /// <returns>BaseServiceException.</returns>
        public static BaseException Handle(this Exception exception, ExceptionScene scene, object data = null, FriendlyHint hint = null)
        {
            TargetInvocationException targetInvocationException = exception as TargetInvocationException;
            var xmlException = exception as System.Xml.XmlException;

            if (targetInvocationException != null)
            {
                return targetInvocationException.InnerException.Handle(scene, data, hint);
            }
            else if (xmlException != null)
            {
                return new InvalidObjectException("Xml", exception, data: data, hint: hint ?? new FriendlyHint { Message = xmlException.Message });
            }
            else
            {
                var baseException = exception as BaseException;
                var operationName = scene?.MethodName;

                if (baseException != null)
                {
                    if (string.IsNullOrWhiteSpace(operationName))
                    {
                        return baseException;
                    }
                    else
                    {
                        switch (baseException.Code.Major)
                        {
                            case ExceptionCode.MajorCode.UnauthorizedOperation:
                                return new UnauthorizedOperationException(baseException, baseException.Code.Minor, data, scene: scene) as BaseException;

                            case ExceptionCode.MajorCode.OperationForbidden:
                                return new OperationForbiddenException(operationName, baseException.Code?.Minor, baseException, data, scene: scene) as BaseException;

                            case ExceptionCode.MajorCode.NullOrInvalidValue:
                            case ExceptionCode.MajorCode.DataConflict:
                            case ExceptionCode.MajorCode.NotImplemented:
                            case ExceptionCode.MajorCode.ResourceNotFound:
                            case ExceptionCode.MajorCode.CreditNotAfford:
                            case ExceptionCode.MajorCode.ServiceUnavailable:
                                return baseException;

                            default:
                                break;
                        }
                    }
                }
                else
                {
                    var sqlException = exception as SqlException;

                    if (sqlException != null)
                    {
                        return new OperationFailureException(exception, data, hint: hint, scene: scene) as BaseException;
                    }
                    else
                    {
                        var notImplementException = exception as NotImplementedException;

                        if (notImplementException != null)
                        {
                            return new UnimplementedException(operationName, notImplementException);
                        }
                    }
                }

                return new OperationFailureException(exception, data, scene: scene) as BaseException;
            }
        }

        #endregion Handle exception

        /// <summary>
        /// To the exception information.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="level">The level.</param>
        /// <param name="operatorIdentifier">The operator identifier.</param>
        /// <returns>ExceptionInfo.</returns>
        public static ExceptionInfo ToExceptionInfo(this Exception exception, ExceptionInfo.ExceptionCriticality level = ExceptionInfo.ExceptionCriticality.Error, string operatorIdentifier = null)
        {
            return ToExceptionInfo(exception, null);
        }

        /// <summary>
        /// To the exception information.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="key">The key.</param>
        /// <param name="level">The level.</param>
        /// <returns>ExceptionInfo.</returns>
        private static ExceptionInfo ToExceptionInfo(Exception exception, Guid? key, ExceptionInfo.ExceptionCriticality level = ExceptionInfo.ExceptionCriticality.Error)
        {
            if (exception != null)
            {
                var baseException = exception as BaseException;
                if (key == null)
                {
                    key = baseException != null ? baseException.Key : Guid.NewGuid();
                }

                var operatorIdentifier = ContextHelper.CurrentCredential?.Key?.ToString();

                var exceptionInfo = new ExceptionInfo
                {
                    ExceptionType = exception.GetType()?.GetFullName(),
                    Level = level,
                    ServerIdentifier = EnvironmentCore.ServerName,
                    ServiceIdentifier = EnvironmentCore.ProjectName,
                    ServerHost = string.Format("{0} {1}", EnvironmentCore.LocalMachineHostName, EnvironmentCore.LocalMachineIpAddress),
                    Message = exception.Message,
                    Source = exception.Source,
                    TargetSite = exception.TargetSite.SafeToString(),
                    Code = baseException == null ? new ExceptionCode { Major = ExceptionCode.MajorCode.OperationFailure } : baseException.Code,
                    StackTrace = exception.StackTrace,
                    Data = baseException?.ReferenceData,
                    Key = key,
                    Scene = baseException?.Scene,
                    OperatorCredential = baseException?.OperatorCredential ?? (ContextHelper.CurrentCredential)
                };

                if (exception.InnerException != null)
                {
                    exceptionInfo.InnerException = ToExceptionInfo(exception.InnerException, key);
                }

                return exceptionInfo;
            }

            return null;
        }

        /// <summary>
        /// To the simple exception information.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns>Beyova.ExceptionSystem.ExceptionInfo.</returns>
        public static ExceptionInfo ToSimpleExceptionInfo(this BaseException exception)
        {
            return exception == null ? null : new ExceptionInfo
            {
                ExceptionType = exception.GetType()?.GetFullName(),
                Level = ExceptionInfo.ExceptionCriticality.Error,
                Message = exception.Hint != null ? exception.Hint.Message : exception.Message,
                Code = exception.Hint != null ? new ExceptionCode
                {
                    Major = exception.Code.Major,
                    Minor = exception.Hint.HintCode
                } : exception.Code,
                Data = (exception.Hint != null && exception.Hint.CauseObjects != null) ? JToken.FromObject(exception.Hint.CauseObjects) : exception.ReferenceData
            };
        }

        /// <summary>
        /// To the exception.
        /// </summary>
        /// <param name="exceptionInfo">The exception information.</param>
        /// <returns>System.Exception.</returns>
        public static Exception ToException(this ExceptionInfo exceptionInfo)
        {
            Exception result = null;

            if (exceptionInfo != null)
            {
                var innerException = exceptionInfo.InnerException;

                switch (exceptionInfo.ExceptionType)
                {
                    case "Beyova.ExceptionSystem.HttpOperationException":
                        return new HttpOperationException(exceptionInfo.Key ?? Guid.NewGuid(), exceptionInfo.CreatedStamp ?? DateTime.UtcNow, exceptionInfo.Message, exceptionInfo.Scene, exceptionInfo.Code, ToException(innerException), exceptionInfo.OperatorCredential, exceptionInfo.Data, exceptionInfo.Hint);

                    case "Beyova.ExceptionSystem.SqlStoredProcedureException":
                        return new SqlStoredProcedureException(exceptionInfo.Message, exceptionInfo.Code);

                    default:
                        break;
                }

                switch (exceptionInfo.Code.Major)
                {
                    case ExceptionCode.MajorCode.CreditNotAfford:
                        result = new CreditNotAffordException(exceptionInfo.Key ?? Guid.NewGuid(), exceptionInfo.CreatedStamp ?? DateTime.UtcNow, exceptionInfo.Message, exceptionInfo.Scene, exceptionInfo.Code, ToException(innerException), exceptionInfo.OperatorCredential, exceptionInfo.Data, exceptionInfo.Hint);
                        break;

                    case ExceptionCode.MajorCode.DataConflict:
                        result = new DataConflictException(exceptionInfo.Key ?? Guid.NewGuid(), exceptionInfo.CreatedStamp ?? DateTime.UtcNow, exceptionInfo.Message, exceptionInfo.Scene, exceptionInfo.Code, ToException(innerException), exceptionInfo.OperatorCredential, exceptionInfo.Data, exceptionInfo.Hint);
                        break;

                    case ExceptionCode.MajorCode.NotImplemented:
                        result = new UnimplementedException(exceptionInfo.Key ?? Guid.NewGuid(), exceptionInfo.CreatedStamp ?? DateTime.UtcNow, exceptionInfo.Message, exceptionInfo.Scene, exceptionInfo.Code, ToException(innerException), exceptionInfo.OperatorCredential, exceptionInfo.Data, exceptionInfo.Hint);
                        break;

                    case ExceptionCode.MajorCode.NullOrInvalidValue:
                        result = new InvalidObjectException(exceptionInfo.Key ?? Guid.NewGuid(), exceptionInfo.CreatedStamp ?? DateTime.UtcNow, exceptionInfo.Message, exceptionInfo.Scene, exceptionInfo.Code, ToException(innerException), exceptionInfo.OperatorCredential, exceptionInfo.Data, exceptionInfo.Hint);
                        break;

                    case ExceptionCode.MajorCode.OperationFailure:
                        result = new OperationFailureException(exceptionInfo.Key ?? Guid.NewGuid(), exceptionInfo.CreatedStamp ?? DateTime.UtcNow, exceptionInfo.Message, exceptionInfo.Scene, exceptionInfo.Code, ToException(innerException), exceptionInfo.OperatorCredential, exceptionInfo.Data, exceptionInfo.Hint);
                        break;

                    case ExceptionCode.MajorCode.OperationForbidden:
                        result = new OperationForbiddenException(exceptionInfo.Key ?? Guid.NewGuid(), exceptionInfo.CreatedStamp ?? DateTime.UtcNow, exceptionInfo.Message, exceptionInfo.Scene, exceptionInfo.Code, ToException(innerException), exceptionInfo.OperatorCredential, exceptionInfo.Data, exceptionInfo.Hint);
                        break;

                    case ExceptionCode.MajorCode.ResourceNotFound:
                        result = new ResourceNotFoundException(exceptionInfo.Key ?? Guid.NewGuid(), exceptionInfo.CreatedStamp ?? DateTime.UtcNow, exceptionInfo.Message, exceptionInfo.Scene, exceptionInfo.Code, ToException(innerException), exceptionInfo.OperatorCredential, exceptionInfo.Data, exceptionInfo.Hint);
                        break;

                    case ExceptionCode.MajorCode.ServiceUnavailable:
                        result = new ServiceUnavailableException(exceptionInfo.Key ?? Guid.NewGuid(), exceptionInfo.CreatedStamp ?? DateTime.UtcNow, exceptionInfo.Message, exceptionInfo.Scene, exceptionInfo.Code, ToException(innerException), exceptionInfo.OperatorCredential, exceptionInfo.Data, exceptionInfo.Hint);
                        break;

                    case ExceptionCode.MajorCode.UnauthorizedOperation:
                        result = new UnauthorizedOperationException(exceptionInfo.Key ?? Guid.NewGuid(), exceptionInfo.CreatedStamp ?? DateTime.UtcNow, exceptionInfo.Message, exceptionInfo.Scene, exceptionInfo.Code, ToException(innerException), exceptionInfo.OperatorCredential, exceptionInfo.Data, exceptionInfo.Hint);
                        break;

                    case ExceptionCode.MajorCode.HttpBlockError:
                        result = new HttpOperationException(exceptionInfo.Key ?? Guid.NewGuid(), exceptionInfo.CreatedStamp ?? DateTime.UtcNow, exceptionInfo.Message, exceptionInfo.Scene, exceptionInfo.Code, ToException(innerException), exceptionInfo.OperatorCredential, exceptionInfo.Data, exceptionInfo.Hint);
                        break;

                    case ExceptionCode.MajorCode.Unsupported:
                        result = new UnsupportedException(exceptionInfo.Key ?? Guid.NewGuid(), exceptionInfo.CreatedStamp ?? DateTime.UtcNow, exceptionInfo.Message, exceptionInfo.Scene, exceptionInfo.Code, ToException(innerException), exceptionInfo.OperatorCredential, exceptionInfo.Data, exceptionInfo.Hint);
                        break;

                    default:
                        result = new Exception(exceptionInfo.Message);
                        break;
                }
            }

            return result;
        }

        /// <summary>
        /// Converts to.
        /// </summary>
        /// <param name="sqlException">The SQL exception.</param>
        /// <returns>Beyova.ExceptionSystem.BaseException.</returns>
        public static BaseException ConvertTo(SqlStoredProcedureException sqlException)
        {
            BaseException result = null;

            if (sqlException != null)
            {
                switch (sqlException.Code.Major)
                {
                    case ExceptionCode.MajorCode.NullOrInvalidValue:
                        result = new InvalidObjectException(sqlException, reason: sqlException.Code.Minor);
                        break;

                    case ExceptionCode.MajorCode.UnauthorizedOperation:
                        result = new UnauthorizedOperationException(sqlException, reason: sqlException.Code.Minor);
                        break;

                    case ExceptionCode.MajorCode.OperationForbidden:
                        result = new OperationForbiddenException(sqlException.Code.Minor, sqlException);
                        break;

                    case ExceptionCode.MajorCode.DataConflict:
                        result = new DataConflictException(sqlException.Code.Minor, innerException: sqlException);
                        break;

                    default:
                        result = sqlException;
                        break;
                }
            }

            return result;
        }

        #region Http To Exception Scene

        /// <summary>
        /// To the exception scene.
        /// </summary>
        /// <param name="httpRequest">The HTTP request.</param>
        /// <param name="controllerOrServiceName">Name of the controller or service.</param>
        /// <returns>Beyova.ExceptionSystem.ExceptionScene.</returns>
        public static ExceptionScene ToExceptionScene(this HttpRequest httpRequest, string controllerOrServiceName = null)
        {
            return httpRequest == null ? null : new ExceptionScene
            {
                MethodName = string.Format("{0}: {1}", httpRequest.HttpMethod, httpRequest.RawUrl),
                FilePath = controllerOrServiceName
            };
        }

        /// <summary>
        /// To the exception scene.
        /// </summary>
        /// <param name="httpRequest">The HTTP request.</param>
        /// <param name="controllerOrServiceName">Name of the controller or service.</param>
        /// <returns>Beyova.ExceptionSystem.ExceptionScene.</returns>
        public static ExceptionScene ToExceptionScene(this HttpRequestBase httpRequest, string controllerOrServiceName = null)
        {
            return httpRequest == null ? null : new ExceptionScene
            {
                MethodName = string.Format("{0}: {1}", httpRequest.HttpMethod, httpRequest.RawUrl),
                FilePath = controllerOrServiceName
            };
        }

        #endregion Http To Exception Scene
    }
}