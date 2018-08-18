using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;
using DotNetty.Codecs.Http;
using DotNetty.Codecs.Http.WebSockets;
using DotNetty.Common.Utilities;
using DotNetty.Transport.Channels;

using static DotNetty.Codecs.Http.HttpVersion;
using static DotNetty.Codecs.Http.HttpResponseStatus;
using NettyServer;

namespace WebSockets.Server
{

    /**
     * @author lilinfeng
     * @version 1.0
     * @date 2014年2月14日
     */
    public class WebSocketServerHandler : SimpleChannelInboundHandler<object>
    {

        public static bool IsSsl = false;
        const string WebsocketPath = "/websocket";

        #region 新增
        // 连接， 管道从不活跃状态  转到  活跃状态 触发 udp没有
        channel_listener.channelActive channelActive
        {
            set;
            get;
        }
        // 断开连接，管道从活跃状态  转到  不活跃状态 触发 udp没有
        channel_listener.channelInactive channelInactive
        {
            set;
            get;
        }

        //接收新消息 tcp,webtcp 用
        channel_listener.channelRead channelRead
        {
            set;
            get;
        }

        public WebSocketServerHandler(channel_listener.channelActive _mychannelActive,
            channel_listener.channelInactive _mychannelInactive,
             channel_listener.channelRead _mychannelRead,
             bool _isssl=false
            )
        {
            channelActive = _mychannelActive;
            channelInactive = _mychannelInactive;
            channelRead = _mychannelRead;
            IsSsl = _isssl;
        }


        public override void ChannelActive(IChannelHandlerContext ctx)
        {
            channelActive(ctx);
            base.ChannelActive(ctx);
        }

        // 管道从活跃状态  转到  不活跃状态 触发
        public override void ChannelInactive(IChannelHandlerContext ctx)
        {
            channelInactive(ctx);
            base.ChannelInactive(ctx);
        }
        #endregion
        WebSocketServerHandshaker handshaker;

        protected override void ChannelRead0(IChannelHandlerContext ctx, object msg)
        {
            if (msg is IFullHttpRequest request)
            {
                this.HandleHttpRequest(ctx, request);
            }
            else if (msg is WebSocketFrame frame)
            {
                this.HandleWebSocketFrame(ctx, frame);
            }
        }

        public override void ChannelReadComplete(IChannelHandlerContext context) => context.Flush();

        void HandleHttpRequest(IChannelHandlerContext ctx, IFullHttpRequest req)
        {
            // Handle a bad request.
            if (!req.Result.IsSuccess)
            {
                SendHttpResponse(ctx, req, new DefaultFullHttpResponse(Http11, BadRequest));
                return;
            }

            // Allow only GET methods.
            if (!Equals(req.Method, HttpMethod.Get))
            {
                SendHttpResponse(ctx, req, new DefaultFullHttpResponse(Http11, Forbidden));
                return;
            }

            // Send the demo page and favicon.ico
            if ("/".Equals(req.Uri))
            {
                IByteBuffer content = WebSocketServerBenchmarkPage.GetContent(GetWebSocketLocation(req));
                var res = new DefaultFullHttpResponse(Http11, OK, content);

                res.Headers.Set(HttpHeaderNames.ContentType, "text/html; charset=UTF-8");
                HttpUtil.SetContentLength(res, content.ReadableBytes);

                SendHttpResponse(ctx, req, res);
                return;
            }
            if ("/favicon.ico".Equals(req.Uri))
            {
                var res = new DefaultFullHttpResponse(Http11, NotFound);
                SendHttpResponse(ctx, req, res);
                return;
            }

            // Handshake
            var wsFactory = new WebSocketServerHandshakerFactory(
                GetWebSocketLocation(req), null, true, 5 * 1024 * 1024);
            this.handshaker = wsFactory.NewHandshaker(req);
            if (this.handshaker == null)
            {
                WebSocketServerHandshakerFactory.SendUnsupportedVersionResponse(ctx.Channel);
            }
            else
            {
                this.handshaker.HandshakeAsync(ctx.Channel, req);
            }
        }

        void HandleWebSocketFrame(IChannelHandlerContext ctx, WebSocketFrame frame)
        {
            // Check for closing frame
            if (frame is CloseWebSocketFrame)
            {
                this.handshaker.CloseAsync(ctx.Channel, (CloseWebSocketFrame)frame.Retain());
                return;
            }

            if (frame is PingWebSocketFrame)
            {
                ctx.WriteAsync(new PongWebSocketFrame((IByteBuffer)frame.Content.Retain()));
                return;
            }

            if (frame is TextWebSocketFrame)//\文本消息\
            {
                // Echo the frame
                ctx.WriteAsync(frame.Retain());
                return;
            }

            if (frame is BinaryWebSocketFrame) //支持二进制消息
            {

                IByteBuffer byteBuf = frame.Content;
                // Echo the frame
                //ctx.WriteAsync(frame.Retain());
                #region 新增2
                if (frame.Content.HasArray)
                {
                    byte[] msg = frame.Content.Array;
                    channelRead(ctx, msg);
                }
                #endregion
            }
        }

        static void SendHttpResponse(IChannelHandlerContext ctx, IFullHttpRequest req, IFullHttpResponse res)
        {
            // Generate an error page if response getStatus code is not OK (200).
            if (res.Status.Code != 200)
            {
                IByteBuffer buf = Unpooled.CopiedBuffer(Encoding.UTF8.GetBytes(res.Status.ToString()));
                res.Content.WriteBytes(buf);
                buf.Release();
                HttpUtil.SetContentLength(res, res.Content.ReadableBytes);
            }

            // Send the response and close the connection if necessary.
            Task task = ctx.Channel.WriteAndFlushAsync(res);
            if (!HttpUtil.IsKeepAlive(req) || res.Status.Code != 200)
            {
                task.ContinueWith((t, c) => ((IChannelHandlerContext)c).CloseAsync(),
                    ctx, TaskContinuationOptions.ExecuteSynchronously);
            }
        }

        public override void ExceptionCaught(IChannelHandlerContext ctx, Exception e)
        {
            Console.WriteLine($"{nameof(WebSocketServerHandler)} {0}", e);
            ctx.CloseAsync();
        }

        static string GetWebSocketLocation(IFullHttpRequest req)
        {
            bool result = req.Headers.TryGet(HttpHeaderNames.Host, out ICharSequence value);
            Debug.Assert(result, "Host header does not exist.");
            string location = value.ToString() + WebsocketPath;

            if (IsSsl)
            {
                return "wss://" + location;
            }
            else
            {
                return "ws://" + location;
            }
        }
    }
}
