using Microsoft.Extensions.DependencyInjection;
using nanoFramework.WebServer;
using System;
using System.Net.WebSockets.Server;
using System.Text;

namespace LowerComputer.Services
{
	public static class Extensions
	{
		public static IServiceCollection AddWebServerService(this IServiceCollection services, int port, HttpProtocol protocol, Type[]? controllers=null)
		{
			return services.AddSingleton(typeof(WebServerDIService), sp =>
			{
				if(controllers==null)
					controllers = new Type[0];
				var webServerDiService=new WebServerDIService(port, protocol, controllers,sp);
				return webServerDiService;
			});
		}

		public static  IServiceCollection AddWebSocketService(this IServiceCollection services, WebSocketServerOptions? options=null)
		{
			return services.AddSingleton(typeof(WebSocketService), sp =>
			{
				var motorService = (MotorService)sp.GetService(typeof(MotorService))
					?? throw new InvalidOperationException("MotorService not registered");
				return new WebSocketService(motorService, options);
			});
		}


	}
}
