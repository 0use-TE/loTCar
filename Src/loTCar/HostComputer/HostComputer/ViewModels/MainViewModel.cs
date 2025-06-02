using ReactiveUI;
using System;
using System.Net.WebSockets;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HostComputer.ViewModels
{
	public class MainViewModel : ViewModelBase,IDisposable
	{
		private readonly ClientWebSocket _webSocket;
		private const string WebSocketUri = "ws://192.168.15.175:80/ws";
		private const int DefaultSpeed = 50; // 固定速度 50%

		public ReactiveCommand<Unit, Unit> ForwardCommand { get; }
		public ReactiveCommand<Unit, Unit> BackwardCommand { get; }
		public ReactiveCommand<Unit, Unit> TurnLeftCommand { get; }
		public ReactiveCommand<Unit, Unit> TurnRightCommand { get; }
		public ReactiveCommand<Unit, Unit> StopCommand { get; }

		public MainViewModel()
		{
			_webSocket = new ClientWebSocket();

			// Initialize WebSocket connection
			Task.Run(async () => await ConnectWebSocketAsync()).GetAwaiter().GetResult();
			// Create ReactiveCommands
			ForwardCommand = ReactiveCommand.CreateFromTask(() => SendCommandAsync("forward"));
			BackwardCommand = ReactiveCommand.CreateFromTask(() => SendCommandAsync("backward"));
			TurnLeftCommand = ReactiveCommand.CreateFromTask(() => SendCommandAsync("left"));
			TurnRightCommand = ReactiveCommand.CreateFromTask(() => SendCommandAsync("right"));
			StopCommand = ReactiveCommand.CreateFromTask(() => SendCommandAsync("stop"));
		}

		private async Task ConnectWebSocketAsync()
		{
			try
			{
				await _webSocket.ConnectAsync(new Uri(WebSocketUri), CancellationToken.None);
				Console.WriteLine("WebSocket connected to nanoFramework device");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"WebSocket connection failed: {ex.Message}");
			}
		}

		private async Task SendCommandAsync(string command)
		{
			if (_webSocket.State != WebSocketState.Open)
			{
				Console.WriteLine("WebSocket is not connected");
				return;
			}

			try
			{
				var message = Encoding.UTF8.GetBytes(command);
				await _webSocket.SendAsync(new ArraySegment<byte>(message), WebSocketMessageType.Text, true, CancellationToken.None);
				Console.WriteLine($"Sent command: {command}");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Failed to send command: {ex.Message}");
			}
		}

		public  void Dispose()
		{
			_webSocket?.Dispose();
			Console.WriteLine("MainViewModel disposed");
		}
	}
}