using nanoFramework.Networking;
using System;
using System.Diagnostics;
using System.Text;
using System.Threading;


namespace LowerComputer.Services
{
	public class WifiConnectService
	{
		private readonly string  wifiSsid;
		private readonly string wifiPassword;
		public WifiConnectService(string ssid, string password)
		{
			wifiSsid = ssid;
			wifiPassword = password;
		}
		public bool ConnectWifi()
		{
			try
			{
				Debug.WriteLine("开始连接Wifi");
				///先实现局域网控制
				CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(60_000);
				var wifiResult = WifiNetworkHelper.ConnectDhcp(wifiSsid, wifiPassword, requiresDateTime: true, token: cancellationTokenSource.Token);
				return wifiResult;
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"连接Wifi时发生异常: {ex.Message}");
				return false;
			}
		}

	}
}
