using Microsoft.Extensions.DependencyInjection;
using nanoFramework.WebServer;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;

namespace LowerComputer.Services
{
    class WebServerDIService : WebServer
    {
        private readonly IServiceProvider _serviceProvider;
        public WebServerDIService(int port, HttpProtocol protocol, Type[] controllers, IServiceProvider serviceProvider) : base(port, protocol, controllers)
        {
            _serviceProvider = serviceProvider;
			CommandReceived += WebServerDIService_CommandReceived;
        }

		private void WebServerDIService_CommandReceived(object sender, WebServerEventArgs e)
		{
			var url = e.Context.Request.RawUrl.ToLower();
			Debug.WriteLine($"Received HTTP request: {url}");

			 if (url == "/ws")
			{
				// WebSocket 升级请求由 WebSocketService 自动处理
				var webServer= (WebSocketService)_serviceProvider.GetService(typeof(WebSocketService));
				webServer.AddWebSocket(e.Context);
				Debug.WriteLine("WebSocket upgrade request received");
			}
			else
			{
				// 其他请求返回 404
				OutputHttpCode(e.Context.Response,HttpStatusCode.NotFound);
				Debug.WriteLine($"Unknown HTTP request: {url}");
			}
		}

		protected override void InvokeRoute(CallbackRoutes route, HttpListenerContext context)
        {
            route.Callback.Invoke(ActivatorUtilities.CreateInstance(_serviceProvider, route.Callback.DeclaringType), new object[] { new WebServerEventArgs(context) });
        }
    }
}
