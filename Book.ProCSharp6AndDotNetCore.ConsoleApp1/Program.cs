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

			//ParallelForWithInit();

			//TasksUsingThreadPool();

			ContinuationWithTasks();
		}

		private static void ContinuationWithTasks()
		{
			Task t1 = new Task(DoOnFirst);
			Task t2 = t1.ContinueWith(DoOnSecond);
			Task t3 = t1.ContinueWith(DoOnSecond);
			Task t4 = t2.ContinueWith(DoOnSecond);
			Task t5 = t1.ContinueWith(DoOnError, TaskContinuationOptions.OnlyOnFaulted);
			t1.Start();

			Task.WaitAll(t1, t2, t3, t4, t5);
		}

		private static void DoOnError(Task arg1, object arg2)
		{
			WriteLine($"Error occured: {Task.CurrentId}");
		}

		private static void DoOnFirst()
		{
			WriteLine($"doing some task {Task.CurrentId}");
			Task.Delay(1000).Wait();
		}

		private static void DoOnSecond(Task t)
		{
			WriteLine($"task {t.Id} finished");
			WriteLine($"this task id {Task.CurrentId}");
			WriteLine("do some cleanup");
			Task.Delay(1000).Wait();
		}

		private static void TasksUsingThreadPool()
		{
			var tf = new TaskFactory();
			Task t1 = tf.StartNew(TaskMethod, "using a task factory");
			Task t2 = Task.Factory.StartNew(TaskMethod, "factory via a task");
			var t3 = new Task(TaskMethod, "using a task ctor and STart");
			t3.Start();
			Task t4 = Task.Run(() => TaskMethod("using the Run method"));

			Task.WaitAll(t1, t2, t3, t4);
		}


		private static readonly object s_logLock = new object();

		public static void TaskMethod(object o)
		{
			Log2(o?.ToString());
		}

		private static void Log2(string title)
		{
			lock (s_logLock)
			{
				WriteLine(title);
				WriteLine($"Task id: {Task.CurrentId?.ToString() ?? "no task"}, " +
					$"thread: {Thread.CurrentThread.ManagedThreadId}");
#if (!DNXCORE)
				WriteLine($"is pooled thread: {Thread.CurrentThread.IsThreadPoolThread}");
#endif
				WriteLine($"is background thread: {Thread.CurrentThread.IsBackground}");
				WriteLine();
			}
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
