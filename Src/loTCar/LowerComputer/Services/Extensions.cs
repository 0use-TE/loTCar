using Microsoft.Extensions.DependencyInjection;
using nanoFramework.WebServer;
using System;
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

		public static IServiceCollection AddWifiConnectService(this IServiceCollection services, string ssid, string password)
		{
			return services.AddSingleton(typeof(WifiConnectService), sp =>
			{
				var wifiConnectService = new WifiConnectService(ssid, password);
				return wifiConnectService;
			});
		}
	}
}
