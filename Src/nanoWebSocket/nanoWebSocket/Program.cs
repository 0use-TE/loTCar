using System.Net;
using System.Net.WebSockets;
using System.Text;

class Program
{
    private static ClientWebSocket _webSocket = new ClientWebSocket();
    private static readonly CancellationTokenSource _cts = new CancellationTokenSource();

    static async Task Main(string[] args)
    {
        string ipAddress = "192.168.31.102";
        string uri = $"ws://{ipAddress}:80";

        try
        {
            // 连接到WebSocket服务器
            Console.WriteLine($"正在连接到 {uri}...");
            await _webSocket.ConnectAsync(new Uri(uri), CancellationToken.None);
            Console.WriteLine("连接成功！");

            // 启动接收消息的任务
            Task receiveTask = ReceiveMessagesAsync();

            // 主循环：读取用户输入并发送消息
            while (!_cts.Token.IsCancellationRequested)
            {
                Console.WriteLine("请输入消息（输入 'exit' 退出）：");
                string message = Console.ReadLine();

                if (message.ToLower() == "exit")
                {
                    _cts.Cancel();
                    break;
                }

                // 发送消息
                byte[] buffer = Encoding.UTF8.GetBytes(message);
                await _webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                Console.WriteLine($"已发送: {message}");
            }

            // 等待接收任务完成
            await receiveTask;

            // 关闭WebSocket连接
            await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "客户端关闭", CancellationToken.None);
            Console.WriteLine("WebSocket连接已关闭");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"错误: {ex.Message}");
        }
    }

    static async Task ReceiveMessagesAsync()
    {
        byte[] buffer = new byte[1024];
        try
        {
            while (_webSocket.State == WebSocketState.Open && !_cts.Token.IsCancellationRequested)
            {
                WebSocketReceiveResult result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), _cts.Token);
                if (result.MessageType == WebSocketMessageType.Text)
                {
                    string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    Console.WriteLine($"收到服务器消息: {message}");
                }
                else if (result.MessageType == WebSocketMessageType.Close)
                {
                    Console.WriteLine("服务器关闭连接");
                    _cts.Cancel();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"接收消息时出错: {ex.Message}");
        }
    }
}