using Microsoft.Extensions.DependencyInjection;
using nanoFramework.WebServer;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;

namespace LowerComputer.Services
{
   public class WebServerDIService : WebServer
    {
        private readonly IServiceProvider _serviceProvider;
        public WebServerDIService(int port, HttpProtocol protocol, Type[] controllers, IServiceProvider serviceProvider) : base(port, protocol, controllers)
        {
            _serviceProvider = serviceProvider;
			CommandReceived += WebServerDIService_CommandReceived;
        }
		private void WebServerDIService_CommandReceived(object sender, WebServerEventArgs e)
		{
			//check the path of the request
			if (e.Context.Request.RawUrl == "/")
			{
				//check if this is a websocket request or a page request 
				if (e.Context.Request.Headers["Upgrade"] == "websocket")
				{
					Debug.WriteLine("connected");
					var _wsServer = (WebSocketService)_serviceProvider.GetService(typeof(WebSocketService));
					//Upgrade to a websocket
					_wsServer.AddWebSocket(e.Context);
				}
				else
				{
					//Return the WebApp
					e.Context.Response.ContentType = "text/html";
				}
			}
			else
			{
				//Send Page not Found
				e.Context.Response.StatusCode = 404;
				WebServer.OutPutStream(e.Context.Response, "Page not Found!");
			}
		}

		protected override void InvokeRoute(CallbackRoutes route, HttpListenerContext context)
        {
            route.Callback.Invoke(ActivatorUtilities.CreateInstance(_serviceProvider, route.Callback.DeclaringType), new object[] { new WebServerEventArgs(context) });
        }
    }
}
