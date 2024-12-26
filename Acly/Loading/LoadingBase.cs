using Acly.Performing.Tasks;
using Acly.Requests;
using Acly.Tasks;
using System;

namespace Acly.Performing
{
	/// <summary>
	/// Базовый класс загрузки. Для создания своей загрузки необходимо унаследовать этот класс и перезаписать свойство <see cref="TasksForPerform"/>
	/// </summary>
	public abstract class LoadingBase : ILoading
	{
		/// <summary>
		/// <inheritdoc/>
		/// </summary>
        public event LoadingEvent? DescriptionChanged;
        /// <summary>
        /// Вызывается сразу после окончания выполнения задач
        /// </summary>
        public event LoadingEvent? Completed;
		/// <summary>
		/// Вызывается при обновлении прогресса выполнения
		/// </summary>
		public event LoadingProgressEvent? ProgressUpdated;
		/// <summary>
		/// Вызывается если при выполнении задач произошла какая-то ошибка
		/// </summary>
		public event LoadingFailEvent? Failed;

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
        public virtual string? Description
		{
			get => _Description;
			set
			{
				if (value == _Description)
				{
					return;
				}

                _Description = value;
				DescriptionChanged?.Invoke(this);
            }
        }

        /// <summary>
        /// Последовательность задач, которая будет выполняться во время загрузки.
        /// </summary>
        protected abstract LoadingTasksList TasksForPerform { get; }

        private LoadingAsyncTasksController? _Controller;
		private bool _Started;
		private string? _Description;

		#region Управление

		/// <summary>
		/// Начать загрузку
		/// </summary>
		/// <returns>Задача загрузки</returns>
		public IAsyncTask Start()
		{
			if (_Started)
			{
				throw new InvalidOperationException("Загрузка уже была начата");
			}

			_Started = true;
			_Controller = new(TasksForPerform);

			_Controller.Completed += OnLoadingCompleted;
			_Controller.ProgressUpdated += OnLoadingProgressUpdated;
			_Controller.Failed += OnLoadingProgressFailed;
            _Controller.DescriptionChanged += OnLoadingDescriptionChanged;

			return new AclyAsyncTask(_Controller);
		}

        /// <summary>
        /// Получить какое-либо значение
        /// </summary>
        /// <typeparam name="T">Тип получаемого значения</typeparam>
        /// <param name="Name">Название значения</param>
        /// <returns>Значение</returns>
        public virtual T? GetValue<T>(string Name)
		{
			return default;
		}

		#endregion

		#region Загрузка

		/// <summary>
		/// Прервать загрузку с ошибкой
		/// </summary>
		/// <param name="Response">Сообщение</param>
		protected void BreakWithError(Response Response)
		{
			_Controller?.BreakWithError(Response);
		}
		/// <summary>
		/// Прервать загрузку с ошибкой
		/// </summary>
		/// <param name="Error">Возникшее исключение</param>
		protected void BreakWithError(Exception Error)
		{
			BreakWithError(new Response(Error));
		}
		/// <summary>
		/// Прервать загрузку с ошибкой
		/// </summary>
		/// <param name="Code">Код ошибки</param>
		/// <param name="Text">Сообщение ошибки</param>
		protected void BreakWithError(string Code, string Text)
		{
			BreakWithError(new Response(Code, Text));
		}
        /// <summary>
        /// Прервать загрузку с ошибкой
        /// </summary>
        /// <param name="Error">Возникшее исключение</param>
		/// <param name="Format">Формат сообщения об ошибке. {0} - сообщение ошибки, {1} - стек вызовов</param>
        protected void BreakWithError(Exception Error, string Format)
        {
            BreakWithError(new Response(Error, Format));
        }

        #endregion

        #region События

        private void OnLoadingCompleted()
		{
			RemoveEvents();
			Description = null;
			Completed?.Invoke(this);
		}
		private void OnLoadingProgressUpdated(float Percent)
		{
			ProgressUpdated?.Invoke(this, Percent);
		}
		private void OnLoadingProgressFailed(IAsyncTaskError Response)
		{
			RemoveEvents();
			Failed?.Invoke(this, Response);
		}
        private void OnLoadingDescriptionChanged()
        {
			Description = _Controller?.Description;
        }

        private void RemoveEvents()
		{
			if (_Controller == null)
			{
				return;
			}

			_Controller.Completed -= OnLoadingCompleted;
			_Controller.ProgressUpdated -= OnLoadingProgressUpdated;
			_Controller.Failed -= OnLoadingProgressFailed;
            _Controller.DescriptionChanged -= OnLoadingDescriptionChanged;
        }

		#endregion
	}
}
