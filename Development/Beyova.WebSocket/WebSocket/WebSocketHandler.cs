using System;
using System.Collections.Specialized;
using System.Net;
using System.Net.WebSockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Web;

namespace Beyova.WebSocketComponent
{
    /// <summary>
    /// Class WebSocketHandlerBase.
    /// 
    /// </summary>
    public abstract class WebSocketHandlerBase : IHttpHandler
    {
        /// <summary>
        /// Gets a value indicating whether this instance is reusable.
        /// </summary>
        /// <value><c>true</c> if this instance is reusable; otherwise, <c>false</c>.</value>
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public int MaxBufferSize { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebSocketHandlerBase" /> class.
        /// </summary>
        /// <param name="maxBufferSize">Maximum size of the buffer.</param>
        protected WebSocketHandlerBase(int maxBufferSize)
        {
            if (maxBufferSize < 32)
            {
                maxBufferSize = 32;
            }

            this.MaxBufferSize = maxBufferSize;
        }

        /// <summary>
        /// Processes the request.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                context.AcceptWebSocketRequest(async wsContext =>
                {
                    try
                    {
                        byte[] receiveBuffer = new byte[MaxBufferSize];
                        ArraySegment<byte> buffer = new ArraySegment<byte>(receiveBuffer);
                        WebSocket socket = wsContext.WebSocket;
                        string userString;

                        if (socket.State == WebSocketState.Open)
                        {
                            // Announcement when connected
                            var announceString = "EchoWebSocket Connected at: " + DateTime.Now.ToString();
                            ArraySegment<byte> outputBuffer2 = new ArraySegment<byte>(Encoding.UTF8.GetBytes(announceString));
                            await socket.SendAsync(outputBuffer2, WebSocketMessageType.Text, true, CancellationToken.None);
                        }

                        // Stay in loop while websocket is open
                        while (socket.State == WebSocketState.Open)
                        {
                            WebSocketReceiveResult receiveResult = await socket.ReceiveAsync(buffer, CancellationToken.None);

                            if (receiveResult.MessageType == WebSocketMessageType.Close)
                            {
                                // Echo back code and reason strings 
                                await socket.CloseAsync(
                                    receiveResult.CloseStatus.GetValueOrDefault(),
                                    receiveResult.CloseStatusDescription,
                                    CancellationToken.None);
                                return;
                            }

                            int offset = receiveResult.Count;

                            while (receiveResult.EndOfMessage == false)
                            {
                                receiveResult = await socket.ReceiveAsync(new ArraySegment<byte>(receiveBuffer, offset, MaxBufferSize - offset), CancellationToken.None);
                                offset += receiveResult.Count;
                            }

                            if (receiveResult.MessageType == WebSocketMessageType.Text)
                            {
                                string cmdString = Encoding.UTF8.GetString(receiveBuffer, 0, offset);
                                userString = cmdString;
                                userString = "You said: \"" + userString + "\"";

                                ArraySegment<byte> outputBuffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(userString));

                                await socket.SendAsync(outputBuffer, WebSocketMessageType.Text, true, CancellationToken.None);
                            }
                            else if (receiveResult.MessageType == WebSocketMessageType.Binary)
                            {
                                userString = String.Format("binary message received, size={0} bytes", receiveResult.Count);

                                ArraySegment<byte> outputBuffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(userString));

                                await socket.SendAsync(outputBuffer, WebSocketMessageType.Text, true, CancellationToken.None);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Trace.WriteLine(ex);
                    }
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex);
                context.Response.StatusCode = 500;
                context.Response.StatusDescription = ex.Message;
                context.Response.End();
            }
        }
    }
}
