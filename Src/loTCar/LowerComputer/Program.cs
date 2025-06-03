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
       AddWebServerService(80, HttpProtocol.Http).
       //注入WebSocket
        AddWebSocketService(new WebSocketServerOptions
        {
            IsStandAlone = false,
            Port = 80,
            MaxClients = 2,
		});
    }).
    Build();

host.Run();
