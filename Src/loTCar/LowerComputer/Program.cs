using System;
using System.Threading;
using System.Diagnostics;
using Microsoft.Extensions.Hosting;
using LowerComputer.Services;
using Microsoft.Extensions.DependencyInjection;
using nanoFramework.WebServer;
using System.Net.WebSockets.Server;

//配置主机服务
var host = Host.CreateDefaultBuilder().
    ConfigureServices((context, service) =>
    {
        //运行服务
        service.AddHostedService(typeof(CoreHostService)).
        //注入电机服务
        AddSingleton(typeof(MotorService)).
        //Web服务器
        AddSingleton(typeof(WebServerDIService), sp =>
        {
            int port = 80;
            HttpProtocol protocol = HttpProtocol.Http;
            Type[] controllers = new Type[] { };
            var webServer = new WebServerDIService(port, protocol, controllers, sp);
            return webServer;
        }).
        AddWebSocketService();
    }).
    Build();

host.Run();
