using nanoFramework.Json;
using nanoFramework.WebServer;
using System;
using System.Diagnostics;
using System.Net;
using System.Text;

namespace LowerComputer.Misc
{
	public class JsonHelper
	{
		private static readonly JsonSerializerOptions _options = new JsonSerializerOptions
		{
			PropertyNameCaseInsensitive = true
		};

		public static object ReadJsonBody(WebServerEventArgs webServerEventArgs,Type targetType)
		{
		return	ReadJsonBody(webServerEventArgs.Context.Request, targetType);
		}
		public static object ReadJsonBody(HttpListenerRequest request, Type targetType)
		{
			try
			{
				var bodyBytes = request.ReadBody();
				string bodyText = Encoding.UTF8.GetString(bodyBytes, 0, bodyBytes.Length);

				var obj = JsonConvert.DeserializeObject(bodyText, targetType, _options);

				if (obj == null)
				{
					throw new InvalidOperationException("反序列化失败：返回 null");
				}

				return obj;
			}
			catch (Exception ex)
			{
				Debug.WriteLine("反序列化失败：" + ex.Message);
				throw new InvalidOperationException("请求体格式错误或类型不匹配");
			}
		}
	}
}
