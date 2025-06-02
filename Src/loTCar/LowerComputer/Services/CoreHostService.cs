using Microsoft.Extensions.Hosting;
using nanoFramework.Networking;
using System;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Threading;

namespace LowerComputer.Services
{
    class CoreHostService : BackgroundService
    {
        private readonly WebServerDI _webServerDI;
        public CoreHostService(WebServerDI webServerDI)
        {
            _webServerDI = webServerDI;
        }
        protected override void ExecuteAsync(CancellationToken stoppingToken)
        {
            Debug.WriteLine("开始连接Wifi");
            ///先实现局域网控制
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(60_000);
            var wifiResult = WifiNetworkHelper.ConnectDhcp("wangqiang", "wang884496", requiresDateTime: true, token: cancellationTokenSource.Token);
            if (wifiResult)
            {
                Debug.WriteLine("连接成功");
                _webServerDI.CommandReceived += _webServerDI_CommandReceived;
                _webServerDI.Start();
                NetworkInterface networkInterface = NetworkInterface.GetAllNetworkInterfaces()[0];
                Debug.WriteLine(networkInterface.IPv4Address);
                Debug.WriteLine("开始监听!");
            }
            else
            {
                Debug.WriteLine("连接失败!");
            }
        }
        private void _webServerDI_CommandReceived(object obj, nanoFramework.WebServer.WebServerEventArgs e)
        {
            Debug.WriteLine("Recive data");
        }
    }
}
