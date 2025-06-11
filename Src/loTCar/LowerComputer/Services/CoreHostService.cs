using Microsoft.Extensions.Hosting;
using nanoFramework.Networking;
using System;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;

namespace LowerComputer.Services
{
    class CoreHostService : BackgroundService
    {
        private readonly WebServerDIService _webServerDIService;
        public CoreHostService(WebServerDIService webServerDI)
        {
            _webServerDIService = webServerDI;
        }
        protected override void ExecuteAsync(CancellationToken stoppingToken)
        {
            Debug.WriteLine("开始连接Wifi");
            ///先实现局域网控制
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(60_000);
            var wifiResult = WifiNetworkHelper.ConnectDhcp("ouse", "80231314w", requiresDateTime: true, token: cancellationTokenSource.Token);

            if (wifiResult)
            {
                Debug.WriteLine("连接成功"); 
                _webServerDIService.Start();

                //即将塞进OLED模块
                NetworkInterface networkInterface = NetworkInterface.GetAllNetworkInterfaces()[0];
                Debug.WriteLine("ip地址"+networkInterface.IPv4Address);
                Debug.WriteLine("开始监听!");

            }
            else
            {
                Debug.WriteLine("连接失败!");
            }
        }

    }
}
