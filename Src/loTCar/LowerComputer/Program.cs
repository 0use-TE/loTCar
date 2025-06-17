using System;
using System.Threading;
using System.Diagnostics;
using Microsoft.Extensions.Hosting;
using LowerComputer.Services;
using Microsoft.Extensions.DependencyInjection;
using nanoFramework.WebServer;
using LowerComputer.Controllers;

//配置主机服务
var host = Host.CreateDefaultBuilder().
    ConfigureServices((context, service) =>
    {
        //运行服务
        service.AddHostedService(typeof(CoreHostService)).
        //添加wifi服务
        AddWifiConnectService("ouse", "80231314w").
		//注入电机服务
		AddSingleton(typeof(MotorService)).
        //Web服务器
       AddWebServerService(80, HttpProtocol.Http, new Type[] {
           typeof(MotorController)});
    })
    .Build();

host.Run();
