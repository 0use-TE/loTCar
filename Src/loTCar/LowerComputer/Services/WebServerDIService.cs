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
		/// <summary>
		/// 没有被控制器匹配时触发的事件
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void WebServerDIService_CommandReceived(object sender, WebServerEventArgs e)
		{
            e.Context.Response.StatusCode= (int)HttpStatusCode.NotFound;
			WebServer.OutPutStream(e.Context.Response,"找不到对应的路由，请检查路由是否正确！");
		}

		protected override void InvokeRoute(CallbackRoutes route, HttpListenerContext context)
        {
            route.Callback.Invoke(ActivatorUtilities.CreateInstance(_serviceProvider, route.Callback.DeclaringType), new object[] { new WebServerEventArgs(context) });
        }
    }
}
