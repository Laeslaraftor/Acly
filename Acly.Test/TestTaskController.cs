using Acly.Tasks;
using System.Threading.Tasks;

namespace Acly.Test
{
    public class TestTaskController : AsyncTaskControllerBase
    {
        protected override async Task StartTask()
        {
            for (float i = 0; i < 1; i += 0.1f)
            {
                SendProgress(i);
                await Task.Delay(1000);
            }

            throw new NotImplementedException();
        }

        public static AclyAsyncTask Begin()
        {
            return new AclyAsyncTask(new TestTaskController());
        }
        public static async Task BeginAsync()
        {
            var task = Begin();
            task.Completed += OnTaskCompleted;
            task.ProgressUpdated += OnProgressUpdated;
            task.Failed += OnTaskFailed;

            void OnProgressUpdated(float progress)
            {
                Console.WriteLine(progress);
            }
            void OnTaskCompleted()
            {
                Console.WriteLine("task completed");
            }
            void OnTaskFailed(IAsyncTaskError error)
            {
                Console.WriteLine(error);
            }

            while (!task.IsCompleted)
            {
                await Task.Delay(50);
            }
        }
    }
}
