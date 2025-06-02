using System;
using System.Threading;
using System.Diagnostics;
using Microsoft.Extensions.Hosting;
using LowerComputer.Services;
using Microsoft.Extensions.DependencyInjection;
using nanoFramework.WebServer;

//配置主机服务
var host = Host.CreateDefaultBuilder().
    ConfigureServices((context, service) =>
    {
        //运行服务
        service.AddHostedService(typeof(CoreHostService));
        //Web服务器
        service.AddSingleton(typeof(WebServerDI),sp =>
        {
            int port = 80;
            HttpProtocol protocol = HttpProtocol.Http;
            Type[] controllers = new Type[] { }; 
            var webServer = new WebServerDI(port, protocol, controllers, sp);
            return webServer;
        });
    }).
    Build();

host.Run();
