using LowerComputer.Misc;
using LowerComputer.Services;
using nanoFramework.WebServer;
using System.Net;

namespace LowerComputer.Controllers
{
	public class MotorController
	{
		private readonly MotorService _motorService;
		private class MoveCommand
		{
			public int Speed { get; set; } // 前进速度
		}
		public MotorController(MotorService motorService)
		{
			_motorService = motorService;
		}
		[Route("move/forward")]
		[Method("POST")]
		public void Forward(WebServerEventArgs e)
		{
			var moveCommand = (MoveCommand)JsonHelper.ReadJsonBody(e, typeof(MoveCommand));
			_motorService.Forward(moveCommand.Speed);

			e.Context.Response.StatusCode = (int)HttpStatusCode.OK;
			WebServer.OutPutStream(e.Context.Response, "前进");
		}

		[Route("move/stop")]
		[Method("POST")]
		public void Stop(WebServerEventArgs e)
		{
			_motorService.Stop();

			e.Context.Response.StatusCode = (int)HttpStatusCode.OK;
			WebServer.OutPutStream(e.Context.Response, "停止");
		}

		[Route("move/backward")]
		[Method("POST")]
		public void Backward(WebServerEventArgs e)
		{
			var moveCommand = (MoveCommand)JsonHelper.ReadJsonBody(e, typeof(MoveCommand));
			_motorService.Backward(moveCommand.Speed);

			e.Context.Response.StatusCode = (int)HttpStatusCode.OK;
			WebServer.OutPutStream(e.Context.Response, "后退");
		}

		[Route("move/left")]
		[Method("POST")]
		public void TurnLeft(WebServerEventArgs e)
		{
			var moveCommand = (MoveCommand)JsonHelper.ReadJsonBody(e, typeof(MoveCommand));
			_motorService.TurnLeft(moveCommand.Speed);

			e.Context.Response.StatusCode = (int)HttpStatusCode.OK;
			WebServer.OutPutStream(e.Context.Response, "左转");
		}

		[Route("move/right")]
		[Method("POST")]
		public void TurnRight(WebServerEventArgs e)
		{
			var moveCommand = (MoveCommand)JsonHelper.ReadJsonBody(e, typeof(MoveCommand));
			_motorService.TurnRight(moveCommand.Speed);

			e.Context.Response.StatusCode = (int)HttpStatusCode.OK;
			WebServer.OutPutStream(e.Context.Response, "右转");
		}
	}
}
