using System;
using System.Threading;
using System.Threading.Tasks;

using static System.Console;

namespace Book.ProCSharp6AndDotNetCore.ConsoleApp1
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var result =
				Parallel.For(0, 10, i =>
				{
					Log($"S {i}");
					Task.Delay(10).Wait();
					Log($"E {i}");
				});

			Console.WriteLine($"Is Completed: {result.IsCompleted}");
		}

		public static void Log(string prefix)
		{
			WriteLine($"{prefix}, task: {Task.CurrentId}, " +
				$"thread: {Thread.CurrentThread.ManagedThreadId}");
		}
	}
}
