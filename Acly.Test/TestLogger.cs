namespace Acly.Test
{
	[LoggerImplementation]
	internal class TestLogger : ILogger
	{
		public void Error(string Message)
		{
			Console.WriteLine(Message);
		}

		public void Error(object Object)
		{
			Console.WriteLine(Object);
		}

		public void Message(string Message)
		{
			Console.WriteLine(Message);
		}

		public void Message(object Object)
		{
			Console.WriteLine(Object);
		}

		public void Warning(string Message)
		{
			Console.WriteLine(Message);
		}

		public void Warning(object Object)
		{
			Console.WriteLine(Object);
		}
	}
}
