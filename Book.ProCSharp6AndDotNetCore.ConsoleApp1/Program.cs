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
			//FirstTest();

			ParallelForWithInit();
		}

		private static void ParallelForWithInit()
		{
			Parallel.For<string>(0, 10, () =>
				{
					// Invoked once for each thread
					Log("Init thread");
					return $"t{Thread.CurrentThread.ManagedThreadId}";
				},
				(i, pls, str1) =>
				{
					// Invoked for each member
					Log($"body i {i} str1 {str1}");
					Task.Delay(10).Wait();
					return "$i {i}";
				},
				str1 =>
				{
					// final action on each thread.
					Log($"finally {str1}");
				});
		}

		private static void FirstTest()
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
