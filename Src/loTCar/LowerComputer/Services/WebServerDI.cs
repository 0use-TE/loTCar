using nanoFramework.WebServer;
using System;
using System.Net;

namespace LowerComputer.Services
{
    class WebServerDI : WebServer
    {
        public WebServerDI(int port, HttpProtocol protocol) : base(port, protocol)
        {
        }

        public WebServerDI(int port, HttpProtocol protocol, Type[] controllers) : base(port, protocol, controllers)
        {
        }
        protected override void InvokeRoute(CallbackRoutes route, HttpListenerContext context)
        {
            base.InvokeRoute(route, context);
        }
    }
}
