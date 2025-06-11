using LowerComputer.Services;
using nanoFramework.WebServer;
using System;
using System.Diagnostics;
using System.Net;
using System.Text;

namespace LowerComputer.Controllers
{
	public class MotorController
	{
		private readonly MotorService _motorService;
		public MotorController(MotorService motorService)
		{
			_motorService = motorService;
		}

		[Route("forword")]
		[Method("POST")]
		public void Forward(WebServerEventArgs e)
		{
			Debug.WriteLine("前进");
			var request= e.Context.Request;
			var result= request.ReadBody();
			// 将 byte[] 转换为字符串
			string body = Encoding.UTF8.GetString(result, 0, result.Length);
			Debug.WriteLine($"Request body: {body}");


			//Get the speed from the request body
			_motorService.Forward(50); // 前进
			e.Context.Response.StatusCode= (int)HttpStatusCode.OK;
			WebServer.OutPutStream(e.Context.Response, "Sucessful");
		}

		[Route("forword")]
		[Method("POST")]
		public void Stop(WebServerEventArgs e)
		{
			_motorService.Stop(); // 停止
			e.Context.Response.StatusCode = (int)HttpStatusCode.OK;
			WebServer.OutPutStream(e.Context.Response, "Sucessful");
		}

	}
}
