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
				Parallel.For(10, 40, (int i, ParallelLoopState pls) =>
				{
					Log($"S {i}");

					if (i > 12)
					{
						pls.Break();
						Log($"Break now... {i}");
					}

					// This waits for the background task to end
					Task.Delay(10).Wait();
					// This dose NOT wait for the background task to finish.
					//await Task.Delay(10);
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
