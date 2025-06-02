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
		private readonly WebSocketServerOptions _options;

		public WebSocketService(MotorService motorService, WebSocketServerOptions? options = null): base(options)
		{
			_motorService = motorService ?? throw new ArgumentNullException(nameof(motorService));
			_options = options ?? new WebSocketServerOptions { Port = 80,  IsStandAlone = false };
			MessageReceived += WebSocketService_MessageReceived;
			Debug.WriteLine($"WebSocketService initialized with Port: {_options.Port}, Prefix: {_options.Prefix}");
		}

		private void WebSocketService_MessageReceived(object sender, MessageReceivedEventArgs e)
		{
			var buffer = e.Frame.Buffer;
			var content = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
			Debug.WriteLine($"Received: {content}");
			content = "forward";
			try
			{
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