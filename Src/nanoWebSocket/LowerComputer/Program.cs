using System;
using System.Diagnostics;
using System.Net;
using System.Net.WebSockets.Server;
using System.Net.WebSockets.WebSocketFrame;
using System.Threading;
using nanoFramework.Networking;
using nanoFramework.Runtime.Native;
using nanoFramework.WebServer;
public class Program
{
    private static WebSocketServer _wsServer;

    public static void Main()
    {
        // Wi-Fi 配置
        const string ssid = "wangqiang";
        const string password = "wang884496";
        CancellationTokenSource cs = new(120000);
        var success = WifiNetworkHelper.ScanAndConnectDhcp(ssid, password, requiresDateTime: true, token: cs.Token);
        Thread.Sleep(3000);
        if (success)
        {

            // 初始化 WebSocket 服务器
            _wsServer = new WebSocketServer(new WebSocketServerOptions
            {
                Port = 80,
                MaxClients = 2,
                IsStandAlone = false,
                ServerName = "NFTestServer",
            });
            // 绑定消息接收事件
            _wsServer.MessageReceived += (sender, e) =>
            {
                if (e.Frame.MessageType == WebSocketMessageType.Text)
                {
                    try
                    {
                        var buffer = e.Frame.Buffer;
                        string message = System.Text.Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                        Debug.WriteLine($"收到客户端消息: {message}");
                        _wsServer.BroadCast(System.Text.Encoding.UTF8.GetBytes($"服务器回复: {message}"));
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"消息解析错误: {ex.Message}");
                    }
                }
                else
                {
                    Debug.WriteLine($"收到非文本消息，长度: {e.Frame.MessageLength}");
                }
            };

            // 初始化 Web 服务器
            WebServer webServer = new WebServer(80, HttpProtocol.Http);
            webServer.CommandReceived += (obj, e) =>
            {
                if (e.Context.Request.RawUrl == "/")
                {
                    if (e.Context.Request.Headers["Upgrade"] == "websocket")
                    {
                        Debug.WriteLine("收到 WebSocket 握手请求");
                        _wsServer.AddWebSocket(e.Context);
                    }
                    else
                    {
                        // 返回测试页面
                        string html = @"<!DOCTYPE html>
    <html>
    <body>
       <h1>WebSocket 测试</h1>
       <input id='msg' type='text' placeholder='输入消息' />
       <button onclick='sendMsg()'>发送</button>
       <div id='output'></div>
       <script>
           var ws = new WebSocket('ws://' + location.hostname + ':80');
           ws.onopen = function() { console.log('WebSocket 连接成功'); };
           ws.onmessage = function(evt) {
               document.getElementById('output').innerHTML += '<p>' + evt.data + '</p>';
           };
           ws.onerror = function(err) { console.log('WebSocket 错误: ', err); };
           ws.onclose = function() { console.log('WebSocket 连接关闭'); };
           function sendMsg() {
               var msg = document.getElementById('msg').value;
               ws.send(msg);
               document.getElementById('msg').value = '';
           }
       </script>
    </body>
    </html>";
                        e.Context.Response.ContentType = "text/html";
                        e.Context.Response.ContentLength64 = html.Length;
                        WebServer.OutPutStream(e.Context.Response, html);
                    }
                }
                else
                {
                    e.Context.Response.StatusCode = 404;
                    WebServer.OutPutStream(e.Context.Response, "Page not Found!");
                }
            };

            // 启动服务
            _wsServer.Start();
            webServer.Start();
            Debug.WriteLine("WebSocket 和 WebServer 已启动");
            Debug.WriteLine($"服务器名称: {_wsServer.ServerName}");
            Debug.WriteLine($"访问: http://{IPAddress.GetDefaultLocalAddress()}");
            Thread.Sleep(Timeout.InfiniteTimeSpan);
        }
        else
        {
            Debug.WriteLine($"Wi-Fi 连接失败: {WifiNetworkHelper.Status}");
        }
    }
}
