using Microsoft.Extensions.DependencyInjection;
using nanoFramework.WebServer;
using System;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Net.WebSockets.Server;
using System.Text;

namespace LowerComputer.Services
{
	public class WebSocketService : WebSocketServer
	{
		private readonly MotorService _motorService;

		public WebSocketService(MotorService motorService, WebSocketServerOptions? options = null) : base(options)
		{
			_motorService = motorService ?? throw new ArgumentNullException(nameof(motorService));
			MessageReceived += WebSocketService_MessageReceived;
		}

		private void WebSocketService_MessageReceived(object sender, MessageReceivedEventArgs e)
		{
			try
			{
				var buffer = e.Frame.Buffer;
				var content = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
				Debug.WriteLine($"Frame Type: {e.Frame.MessageType}, Length: {buffer.Length}");
				if (content == null)
				{
					Debug.WriteLine($"Received buffer length: {buffer.Length}, Raw: {BitConverter.ToString(buffer)}");
					return;
				}
				Debug.WriteLine($"Received: {content}");

				switch (content.ToLower())
				{
					case "forward":
						_motorService.Forward(50);
						break;
					case "backward":
						_motorService.Backward(50);
						break;
					case "left":
						_motorService.TurnLeft(30);
						break;
					case "right":
						_motorService.TurnRight(30);
						break;
					case "stop":
						_motorService.Stop();
						break;
					default:
						Debug.WriteLine($"Unknown command: {content}");
						break;
				}

			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Error processing command: {ex.Message}");
			}
		}

	}
}