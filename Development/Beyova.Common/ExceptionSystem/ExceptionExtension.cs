using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
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

        #region default event log

        private static EventLog eventLogWriter = CreateEventLog();

        /// <summary>
        /// Creates the event log.
        /// </summary>
        /// <returns>EventLog.</returns>
        private static EventLog CreateEventLog()
        {
            EventLog eventLog = new EventLog();
            eventLog.Source = "Application";

            return eventLog;
        }

        #endregion

        /// <summary>
        /// Formats to string.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="referenceObject">The reference object.</param>
        /// <returns>System.String.</returns>
        public static string FormatToString(this Exception exception, object referenceObject = null)
        {
            StringBuilder stringBuilder = new StringBuilder();

            if (exception != null)
            {
                stringBuilder.AppendLine("-----------------------  Exception  -----------------------");
                stringBuilder.AppendLine("-----------------------  " + DateTime.UtcNow.ToFullDateTimeString());
                stringBuilder.AppendLine("-----------------------  Thread ID: " + Thread.CurrentThread.ManagedThreadId.ToString());

                FormatToString(stringBuilder, exception, 0, referenceObject);

                stringBuilder.AppendLine("---------------------------  End  ---------------------------");
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Formats to XML.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="referenceObject">The reference object.</param>
        /// <param name="treeToList">if set to <c>true</c> [tree to list].</param>
        /// <returns>XElement.</returns>
        public static XElement FormatToXml(this Exception exception, object referenceObject = null, bool treeToList = false)
        {
            if (exception != null)
            {
                var root = node_Exception.CreateXml();
                FormatToXml(root, exception, 0, referenceObject, treeToList);

                return root;
            }

            return null;
        }

        /// <summary>
        /// Formats to HTML.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="referenceObject">The reference object.</param>
        /// <returns>System.String.</returns>
        public static string FormatToHtml(this Exception exception, object referenceObject = null)
        {
            StringBuilder stringBuilder = new StringBuilder();

            if (exception != null)
            {
                FormatToHtml(stringBuilder, exception, referenceObject);
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Writes the event log.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public static void WriteEventLog(this BaseException exception)
        {
            if (exception != null)
            {
                try
                {
                    eventLogWriter.WriteEntry(exception.FormatToString(), EventLogEntryType.Error);
                }
                catch { }
            }
        }

        /// <summary>
        /// Writes the log.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="referenceObject">The reference object.</param>
        /// <param name="eventLogEntryTypeToOverride">The event log entry type to override.</param>
        public static void WriteLog(this Exception exception, EventLog logger, object referenceObject = null, EventLogEntryType? eventLogEntryTypeToOverride = null)
        {
            if (exception != null && logger != null)
            {
                var entryType = eventLogEntryTypeToOverride;

                if (entryType != null)
                {
                    var baseException = exception as BaseException;

                    if (baseException != null)
                    {
                        entryType = baseException.Code.ToEventLogEntryType();
                    }
                }

                logger.WriteEntry(exception.FormatToString(referenceObject), entryType ?? EventLogEntryType.Error);
            }
        }

        /// <summary>
        /// Writes the log.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="referenceObject">The reference object.</param>
        public static void WriteLog(this Exception exception, StreamWriter logger, object referenceObject = null)
        {
            if (exception != null && logger != null)
            {
                logger.WriteLine(exception.FormatToString(referenceObject));
            }
        }

        /// <summary>
        /// Formats to string.
        /// </summary>
        /// <param name="stringBuilder">The string builder.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="level">The level.</param>
        /// <param name="referenceObject">The reference object.</param>
        private static void FormatToString(StringBuilder stringBuilder, Exception exception, int level, object referenceObject = null)
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
                }

                stringBuilder.AppendIndent(level).AppendLineWithFormat("Data Reference: {0}", GenerateDataString(
                    (baseException != null && baseException.DataReference != null) ? baseException.DataReference : referenceObject));

                if (exception.InnerException != null)
                {
                    level++;
                    stringBuilder.AppendIndent(level).AppendLine("--------------------  Inner Exception  --------------------");
                    FormatToString(stringBuilder, exception.InnerException, level, null);
                }
            }
        }

        private const string node_Exception = "Exception";
        private const string node_Type = "Type";
        private const string node_Code = "Code";
        private const string node_MajorCode = "MajorCode";
        private const string node_MinorCode = "MinorCode";
        private const string node_Message = "Message";
        private const string node_Source = "Source";
        private const string node_StackTrace = "StackTrace";
        private const string node_TargetSite = "TargetSite";
        private const string node_Data = "Data";
        private const string attribute_Level = "Level";

        /// <summary>
        /// Formats to XML.
        /// </summary>
        /// <param name="root">The root.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="level">The level.</param>
        /// <param name="referenceObject">The reference object.</param>
        /// <param name="treeAsList">if set to <c>true</c> [tree as list].</param>
        private static void FormatToXml(XElement root, Exception exception, int level, object referenceObject = null, bool treeAsList = false)
        {
            if (root != null && exception != null)
            {
                var thisXml = node_Exception.CreateXml();
                thisXml.SetAttributeValue(attribute_Level, level);

                var baseException = exception as BaseException;
                thisXml.CreateChildNode(node_Type, exception.GetType().FullName);

                if (baseException != null)
                {
                    thisXml.CreateChildNode(node_Code, (int)baseException.Code);
                }

                thisXml.CreateChildNode(node_Message, exception.Message);
                thisXml.CreateChildNode(node_Source, exception.Source);
                thisXml.CreateChildNode(node_TargetSite, exception.TargetSite);
                thisXml.CreateChildNode(node_StackTrace, exception.StackTrace);
                thisXml.CreateChildNode(node_Data, GenerateDataString(
                    (baseException != null && baseException.DataReference != null) ? baseException.DataReference : referenceObject)
                    );

                if (exception.InnerException != null)
                {
                    FormatToXml(treeAsList ? root : thisXml, exception.InnerException, level + 1, null, treeAsList);
                }
            }
        }

        /// <summary>
        /// Formats to HTML.
        /// </summary>
        /// <param name="stringBuilder">The string builder.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="referenceObject">The reference object.</param>
        private static void FormatToHtml(StringBuilder stringBuilder, Exception exception, object referenceObject = null)
        {
            if (stringBuilder != null)
            {
                BaseException baseException = exception as BaseException;

                stringBuilder.AppendLine("<div class='exception'>");
                stringBuilder.AppendLineWithFormat("<div class='row'>Exception Type: {0}</div>", exception.GetType().ToString());

                if (baseException != null)
                {
                    stringBuilder.AppendLineWithFormat("<div class='row'>Exception Code: {0}</div>", baseException.Code.ToString());
                }

                stringBuilder.AppendLineWithFormat("<div class='row'>Exception Message: {0}</div>", exception.Message);
                stringBuilder.AppendLineWithFormat("<div class='row'>Source: {0}</div>", exception.Source);
                stringBuilder.AppendLineWithFormat("<div class='row'>Site: {0}</div>", exception.TargetSite);
                stringBuilder.AppendLineWithFormat("<div class='row'>StackTrace: {0}</div>", exception.StackTrace);

                stringBuilder.AppendLineWithFormat("<div class='row'>Data Reference: {0}</div>", GenerateDataString(
                    (baseException != null && baseException.DataReference != null) ? baseException.DataReference : referenceObject));

                if (exception.InnerException != null)
                {
                    stringBuilder.AppendLine("<div class='innerException'><div class='title'><span />Inner Exception</div>");

                    FormatToHtml(stringBuilder, exception.InnerException, null);
                    stringBuilder.AppendLine("</div>");
                }

                stringBuilder.AppendLine("</div>");
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

        #endregion

        #region Handle exception

        /// <summary>
        /// Handles the exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="operationName">Name of the operation.</param>
        /// <param name="operatorIdentity">The operator identity.</param>
        /// <param name="data">The data.</param>
        /// <returns>BaseServiceException.</returns>
        public static BaseException Handle(this Exception exception, string operationName, string operatorIdentity, object data = null)
        {
            TargetInvocationException targetInvocationException = exception as TargetInvocationException;

            if (targetInvocationException != null)
            {
                return Handle(targetInvocationException.InnerException, operationName, operatorIdentity, data);
            }
            else
            {
                var baseException = exception as BaseException;
                var notImplementException = exception as NotImplementedException;
                var sqlException = exception as SqlException;

                operationName = operationName.SafeToString();
                operatorIdentity = operatorIdentity.SafeToString();

                if (sqlException != null)
                {
                    return new OperationFailureException(operationName, exception, data) as BaseException;
                }
                else if (notImplementException != null)
                {
                    return new UnimplementedException("operationName", notImplementException);
                }
                else if (baseException != null)
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
                                return new UnauthorizedOperationException(operationName, operatorIdentity, baseException.Code.Minor, baseException, data) as BaseException;
                            case ExceptionCode.MajorCode.OperationForbidden:
                                return new OperationForbiddenException(operationName, baseException.Code?.Minor, baseException, operatorIdentity, data) as BaseException;
                            case ExceptionCode.MajorCode.NullOrInvalidValue:
                            case ExceptionCode.MajorCode.DataConflict:
                            case ExceptionCode.MajorCode.NotImplemented:
                            case ExceptionCode.MajorCode.ResourceNotFound:
                            case ExceptionCode.MajorCode.CreditNotAfford:
                                return baseException;
                            default:
                                break;
                        }
                    }
                }

                if (string.IsNullOrWhiteSpace(operationName))
                {
                    return new OperationFailureException(exception, data) as BaseException;
                }
                else
                {
                    return new OperationFailureException(operationName, exception, data) as BaseException;
                }
            }
        }

        /// <summary>
        /// Handles the exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="operationName">Name of the operation.</param>
        /// <param name="data">The data.</param>
        /// <returns>BaseException.</returns>
        public static BaseException Handle(this Exception exception, string operationName, object data = null)
        {
            return Handle(exception, operationName, Framework.OperatorInfo.ObjectToString(), data);
        }

        #endregion

        /// <summary>
        /// To the exception information.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="appIdentifier">The application identifier.</param>
        /// <param name="serverIdentifier">The server identifier.</param>
        /// <param name="level">The level.</param>
        /// <param name="operatorIdentifier">The operator identifier.</param>
        /// <returns>ExceptionInfo.</returns>
        public static ExceptionInfo ToExceptionInfo(this Exception exception, string appIdentifier = null, string serverIdentifier = null, ExceptionInfo.ExceptionCriticality level = ExceptionInfo.ExceptionCriticality.Error, string operatorIdentifier = null)
        {
            return ToExceptionInfo(exception, null, appIdentifier, serverIdentifier);
        }

        /// <summary>
        /// To the exception information.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="key">The key.</param>
        /// <param name="appIdentifier">The application identifier.</param>
        /// <param name="serverIdentifier">The server identifier.</param>
        /// <param name="level">The level.</param>
        /// <param name="operatorIdentifier">The operator identifier.</param>
        /// <returns>ExceptionInfo.</returns>
        private static ExceptionInfo ToExceptionInfo(Exception exception, Guid? key, string appIdentifier = null, string serverIdentifier = null, ExceptionInfo.ExceptionCriticality level = ExceptionInfo.ExceptionCriticality.Error, string operatorIdentifier = null)
        {
            if (exception != null)
            {
                var baseException = exception as BaseException;
                if (key == null)
                {
                    key = baseException != null ? baseException.Key : Guid.NewGuid();
                }

                serverIdentifier = serverIdentifier.SafeToString(EnvironmentCore.ServerName);

                var exceptionInfo = new ExceptionInfo
                {
                    ExceptionType = exception.GetType().GetFullName(),
                    Level = level,
                    ServerIdentifier = serverIdentifier,
                    ServiceIdentifier = appIdentifier,
                    Message = exception.Message,
                    Source = exception.Source,
                    TargetSite = exception.TargetSite.ObjectToString(),
                    UserIdentifier = operatorIdentifier,
                    Code = baseException == null ? new ExceptionCode { Major = ExceptionCode.MajorCode.OperationFailure } : baseException.Code,
                    StackTrace = exception.StackTrace,
                    Data = (baseException != null && baseException.DataReference != null) ? JObject.FromObject(baseException.DataReference) : null,
                    Key = key
                };

                if (exception.InnerException != null)
                {
                    exceptionInfo.InnerException = ToExceptionInfo(exception.InnerException, key, appIdentifier, serverIdentifier);
                }

                return exceptionInfo;
            }

            return null;
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
                        return new HttpOperationException(exceptionInfo.Message, exceptionInfo.Code, exceptionInfo.Data?.Value<HttpOperationException.HttpExceptionReference>(BaseException.dataKey_ReferenceData), exceptionInfo.Data?.Value<string>(BaseException.dataKey_Operator));
                    case "Beyova.ExceptionSystem.SqlStoredProcedureException":
                        return new SqlStoredProcedureException(exceptionInfo.Message, exceptionInfo.Code);
                    default:
                        break;
                }

                switch (exceptionInfo.Code.Major)
                {
                    case ExceptionCode.MajorCode.CreditNotAfford:
                        result = new CreditNotAffordException(exceptionInfo.Message, exceptionInfo.UserIdentifier, exceptionInfo.Code.Minor, ToException(innerException), exceptionInfo.Data);
                        break;
                    case ExceptionCode.MajorCode.DataConflict:
                        result = new DataConflictException(exceptionInfo.Message, exceptionInfo.UserIdentifier, exceptionInfo.Code.Minor, ToException(innerException), exceptionInfo.Data);
                        break;
                    case ExceptionCode.MajorCode.NotImplemented:
                        result = new NotImplementedException(exceptionInfo.Message);
                        break;
                    case ExceptionCode.MajorCode.NullOrInvalidValue:
                        result = new InvalidObjectException(exceptionInfo.Message, exceptionInfo.UserIdentifier, exceptionInfo.Code.Minor, ToException(innerException), exceptionInfo.Data);
                        break;
                    case ExceptionCode.MajorCode.OperationFailure:
                        result = new OperationFailureException(exceptionInfo.Message, exceptionInfo.UserIdentifier, exceptionInfo.Code.Minor, ToException(innerException), exceptionInfo.Data);
                        break;
                    case ExceptionCode.MajorCode.OperationForbidden:
                        result = new OperationForbiddenException(exceptionInfo.Message, exceptionInfo.UserIdentifier, exceptionInfo.Code.Minor, ToException(innerException), exceptionInfo.Data);
                        break;
                    case ExceptionCode.MajorCode.ResourceNotFound:
                        result = new ResourceNotFoundException(exceptionInfo.Message, exceptionInfo.UserIdentifier, exceptionInfo.Code.Minor, ToException(innerException), exceptionInfo.Data);
                        break;
                    case ExceptionCode.MajorCode.ServiceUnavailable:
                        result = new InitializationFailureException(exceptionInfo.Message, exceptionInfo.UserIdentifier, exceptionInfo.Code.Minor, ToException(innerException), exceptionInfo.Data);
                        break;
                    case ExceptionCode.MajorCode.UnauthorizedOperation:
                        result = new UnauthorizedOperationException(exceptionInfo.Message, exceptionInfo.UserIdentifier, exceptionInfo.Code.Minor, ToException(innerException), exceptionInfo.Data);
                        break;
                    case ExceptionCode.MajorCode.HttpBlockError:
                        result = new HttpOperationException(exceptionInfo.Message, exceptionInfo.Code, exceptionInfo.Data?.Value<HttpOperationException.HttpExceptionReference>(BaseException.dataKey_ReferenceData), exceptionInfo.Data?.Value<string>(BaseException.dataKey_Operator));
                        break;
                    default:
                        result = new Exception(exceptionInfo.Message);
                        break;
                }
            }

            return result;
        }
    }
}
