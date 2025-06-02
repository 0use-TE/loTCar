using Microsoft.Extensions.DependencyInjection;
using nanoFramework.WebServer;
using System;
using System.Net.WebSockets.Server;
using System.Text;

namespace LowerComputer.Services
{
	public static class Extensions
	{
		public static void AddWebServerService(this IServiceCollection services, int port, HttpProtocol protocol, Type[] controllers)
		{
		}
		public static void AddWebSocketService(this IServiceCollection services, WebSocketServerOptions? options=null)
		{
			services.AddSingleton(typeof(WebSocketService), sp =>
			{
				var motorService = (MotorService)sp.GetService(typeof(MotorService))
					?? throw new InvalidOperationException("MotorService not registered");
				return new WebSocketService(motorService, options);
			});
		}

	}
}
