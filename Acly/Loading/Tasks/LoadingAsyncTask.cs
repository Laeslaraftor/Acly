using Acly.Tasks;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;

namespace Acly.Performing.Tasks
{
    /// <summary>
    /// Класс для хранения и выполнения асинхронной задачи
    /// </summary>
    public class LoadingAsyncTask
    {
        /// <summary>
        /// Создать экземпляр класса для хранения и выполнения асинхронной задачи
        /// </summary>
        /// <param name="asyncTask">Асинхронная задача для хранения и выполнения</param>
        /// <exception cref="ArgumentNullException">Асинхронная задача не указана</exception>
        public LoadingAsyncTask(Func<Task<IAsyncTask?>> asyncTask)
        {
            FuncAsyncTask = asyncTask ?? throw new ArgumentNullException(nameof(asyncTask), "Асинхронная задача не указана");
        }
        /// <summary>
        /// Создать экземпляр класса для хранения и выполнения асинхронной задачи
        /// </summary>
        /// <param name="asyncTask">Асинхронная задача для хранения и выполнения</param>
        /// <exception cref="ArgumentNullException">Асинхронная задача не указана</exception>
        public LoadingAsyncTask(Func<IAsyncTask?> asyncTask)
        {
            this.AsyncTask = asyncTask ?? throw new ArgumentNullException(nameof(asyncTask), "Асинхронная задача не указана");
        }
        /// <summary>
        /// Создать экземпляр класса для хранения и выполнения асинхронной задачи
        /// </summary>
        /// <param name="asyncTask">Асинхронная задача для хранения и выполнения</param>
        /// <exception cref="ArgumentNullException">Асинхронная задача не указана</exception>
        public LoadingAsyncTask(Func<Task> asyncTask)
        {
            Task = asyncTask ?? throw new ArgumentNullException(nameof(asyncTask), "Асинхронная задача не указана");
        }

        /// <summary>
        /// Задача для хранения и выполнения
        /// </summary>
        public Func<Task<IAsyncTask?>>? FuncAsyncTask { get; private set; }
        /// <summary>
        /// Задача для хранения и выполнения
        /// </summary>
#pragma warning disable CA1721
        public Func<IAsyncTask?>? AsyncTask { get; private set; }
#pragma warning restore CA1721
        /// <summary>
        /// Задача для хранения и выполнения
        /// </summary>
        public Func<Task>? Task { get; private set; }
        /// <summary>
        /// Описание задачи
        /// </summary>
        public string? Description
        {
            get
            {
                if (AsyncTask != null)
                {
                    return GetDescription(AsyncTask);
                }
                if (FuncAsyncTask != null)
                {
                    return GetDescription(FuncAsyncTask);
                }
                if (Task != null)
                {
                    return GetDescription(Task);
                }

                return null;
            }
        }

        /// <summary>
        /// Попытаться выполнить асинхронную задачу на хранении
        /// </summary>
        /// <returns>Ошибка при выполнении асинхронной задачи. Если никакой ошибки не возникало - NULL</returns>
        public async Task<IAsyncTask?> GetAsyncTask()
        {
            if (AsyncTask != null)
            {
                return AsyncTask.Invoke();
            }
            if (FuncAsyncTask != null)
            {
                return await FuncAsyncTask.Invoke();
            }

#pragma warning disable CS8604
            return new AclyAsyncTask(Task);
#pragma warning restore CS8604
        }

        private static string? GetDescription(Delegate @delegate)
        {
            return @delegate.GetMethodInfo().GetCustomAttribute<DescriptionAttribute>()?.Description;
        }
    }
}
