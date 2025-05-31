using System;
using System.Threading;
using System.Diagnostics;
using Microsoft.Extensions.Hosting;
using LowerComputer.Services;

//配置主机服务
var host = Host.CreateDefaultBuilder().
    ConfigureServices((context, service) =>
    {
        //运行服务
        service.AddHostedService(typeof(CoreHostService));
        //其它依赖

    }).
    Build();

host.Run();
