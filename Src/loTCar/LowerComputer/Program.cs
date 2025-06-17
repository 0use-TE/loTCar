using System;
using System.Threading;
using System.Diagnostics;
using Microsoft.Extensions.Hosting;
using LowerComputer.Services;
using Microsoft.Extensions.DependencyInjection;
using nanoFramework.WebServer;
using LowerComputer.Controllers;

//������������
var host = Host.CreateDefaultBuilder().
    ConfigureServices((context, service) =>
    {
        //���з���
        service.AddHostedService(typeof(CoreHostService)).
        //���wifi����
        AddWifiConnectService("ouse", "80231314w").
		//ע��������
		AddSingleton(typeof(MotorService)).
        //Web������
       AddWebServerService(80, HttpProtocol.Http, new Type[] {
           typeof(MotorController)});
    })
    .Build();

host.Run();
