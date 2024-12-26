using Acly.Performing;
using Acly.Tasks;
using System.ComponentModel;

namespace Acly.Test
{
	public class TestLoading : LoadingBase
	{
		public TestLoading() { }
		public TestLoading(bool Looped)
		{
			_Looped = Looped;
		}

		protected override LoadingTasksList TasksForPerform
		{
			get
			{
				if (_Tasks == null)
				{
					_Tasks = GetTasks(10);
				}

				return _Tasks;
			}
		}
		private LoadingTasksList? _Tasks;
		private bool _Looped = true;

		private LoadingTasksList GetTasks(int Amount)
		{
			LoadingTasksList Result = new();

			for (int i = 0; i < Amount; i++)
			{
				Result.Add([Description("Проверка описание {0}")] () =>
				{
					int Time = Helper.Random.Next(10, 1000);

					if (Time > 900 && _Looped)
					{
						Log.Message("Мощ");
						return new TestLoading(false).Start();
					}

					return new AclyAsyncTask([Description("Золька {0}")] async () =>
					{
						await Task.Delay(Time);
					});
				});
			}

			return Result;
		}

		private async Task Delay()
		{
			await Task.Delay(50);
		}
	}
}
