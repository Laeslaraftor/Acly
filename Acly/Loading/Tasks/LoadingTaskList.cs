using Acly.Performing.Tasks;
using Acly.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Acly.Performing
{
	/// <summary>
	/// Список задач для загрузки
	/// </summary>
	public class LoadingTasksList : IList<LoadingAsyncTask>, IList<Func<Task<IAsyncTask>>>, IList<Func<IAsyncTask>>, IList<Func<Task>>
	{
		/// <summary>
		/// Создать список задач для загрузки
		/// </summary>
		public LoadingTasksList()
		{
		}
		/// <summary>
		/// Создать список задач для загрузки
		/// </summary>
		/// <param name="TaskForPerform">Задача для загрузки</param>
		public LoadingTasksList(Func<Task<IAsyncTask>> TaskForPerform)
		{
			if (TaskForPerform == null)
			{
				throw new ArgumentNullException(nameof(TaskForPerform), TaskNotStated);
			}

			_List.Add(new(TaskForPerform));
		}
		/// <summary>
		/// Создать список задач для загрузки
		/// </summary>
		/// <param name="TasksForPerform">Список задач для загрузки</param>
		public LoadingTasksList(IEnumerable<Func<Task<IAsyncTask>>> TasksForPerform)
		{
			if (TasksForPerform == null)
			{
				throw new ArgumentNullException(nameof(TasksForPerform), TasksNotStated);
			}

			foreach (var Task in TasksForPerform)
			{
				_List.Add(new(Task));
			}
		}
		/// <summary>
		/// Создать список задач для загрузки
		/// </summary>
		/// <param name="TaskForPerform">Задача для загрузки</param>
		public LoadingTasksList(Func<IAsyncTask> TaskForPerform)
		{
			if (TaskForPerform == null)
			{
				throw new ArgumentNullException(nameof(TaskForPerform), TaskNotStated);
			}

			_List.Add(new(TaskForPerform));
		}
		/// <summary>
		/// Создать список задач для загрузки
		/// </summary>
		/// <param name="TasksForPerform">Список задач для загрузки</param>
		public LoadingTasksList(IEnumerable<Func<IAsyncTask>> TasksForPerform)
		{
			if (TasksForPerform == null)
			{
				throw new ArgumentNullException(nameof(TasksForPerform), TasksNotStated);
			}

			foreach (var Task in TasksForPerform)
			{
				_List.Add(new(Task));
			}
		}
		/// <summary>
		/// Создать список задач для загрузки
		/// </summary>
		/// <param name="TaskForPerform">Задача для загрузки</param>
		public LoadingTasksList(Func<Task> TaskForPerform)
		{
			if (TaskForPerform == null)
			{
				throw new ArgumentNullException(nameof(TaskForPerform), TaskNotStated);
			}

			_List.Add(new(TaskForPerform));
		}
		/// <summary>
		/// Создать список задач для загрузки
		/// </summary>
		/// <param name="TasksForPerform">Список задач для загрузки</param>
		public LoadingTasksList(IEnumerable<Func<Task>> TasksForPerform)
		{
			if (TasksForPerform == null)
			{
				throw new ArgumentNullException(nameof(TasksForPerform), TasksNotStated);
			}

			foreach (var Task in TasksForPerform)
			{
				_List.Add(new(Task));
			}
		}
		/// <summary>
		/// Создать список задач для загрузки
		/// </summary>
		/// <param name="TaskForPerform">Задача для загрузки</param>
		public LoadingTasksList(LoadingAsyncTask TaskForPerform)
		{
			if (TaskForPerform == null)
			{
				throw new ArgumentNullException(nameof(TaskForPerform), TaskNotStated);
			}

			_List.Add(TaskForPerform);
		}
		/// <summary>
		/// Создать список задач для загрузки
		/// </summary>
		/// <param name="TasksForPerform">Список задач для загрузки</param>
		public LoadingTasksList(IEnumerable<LoadingAsyncTask> TasksForPerform)
		{
			if (TasksForPerform == null)
			{
				throw new ArgumentNullException(nameof(TasksForPerform), TasksNotStated);
			}

			foreach (var Task in TasksForPerform)
			{
				_List.Add(Task);
			}
		}

		/// <summary>
		/// Количество задач в списке
		/// </summary>
		public int Count => _List.Count;
		/// <summary>
		/// Можно ли вносить изменения в список
		/// </summary>
		public bool IsReadOnly { get; private set; }
		/// <summary>
		/// Получить или установить задачу на указанную позицию
		/// </summary>
		/// <param name="Index">Позиция в списке</param>
		/// <returns>Задача</returns>
		/// <exception cref="InvalidOperationException">Изменения заблокированы</exception>
		public LoadingAsyncTask this[int Index]
		{
			get => _List[Index];
			set
			{
				if (IsReadOnly)
				{
					throw new InvalidOperationException(ListHasBeenBanned);
				}

				_List[Index] = value;
			}
		}

		private readonly List<LoadingAsyncTask> _List = new();
		private List<Func<Task<IAsyncTask>>> TasksList
		{
			get
			{
				List<Func<Task<IAsyncTask>>> Result = new();

				foreach (var Func in _List)
				{
					if (Func.FuncAsyncTask == null)
					{
						continue;
					}

					Result.Add(Func.FuncAsyncTask);
				}

				return Result;
			}
		}
		private List<Func<IAsyncTask>> AsyncTasksFuncList
		{
			get
			{
				List<Func<IAsyncTask>> Result = new();

				foreach (var Func in _List)
				{
					if (Func.AsyncTask == null)
					{
						continue;
					}

					Result.Add(Func.AsyncTask);
				}

				return Result;
			}
		}
		private List<Func<Task>> TasksFuncList
		{
			get
			{
				List<Func<Task>> Result = new();

				foreach (var Func in _List)
				{
					if (Func.Task == null)
					{
						continue;
					}

					Result.Add(Func.Task);
				}

				return Result;
			}
		}

		Func<Task> IList<Func<Task>>.this[int Index]
		{
			get => throw new InvalidOperationException(ActionNotImplemented + "LoadingAsyncTask this[int Index]");
			set
			{
				if (IsReadOnly)
				{
					throw new InvalidOperationException(ListHasBeenBanned);
				}

				_List[Index] = new(value);
			}
		}
		Func<IAsyncTask> IList<Func<IAsyncTask>>.this[int Index]
		{
			get => throw new InvalidOperationException(ActionNotImplemented + "LoadingAsyncTask this[int Index]");
			set
			{
				if (IsReadOnly)
				{
					throw new InvalidOperationException(ListHasBeenBanned);
				}

				_List[Index] = new(value);
			}
		}
		Func<Task<IAsyncTask>> IList<Func<Task<IAsyncTask>>>.this[int Index]
		{
			get => throw new InvalidOperationException(ActionNotImplemented + "LoadingAsyncTask this[int Index]");
			set
			{
				if (IsReadOnly)
				{
					throw new InvalidOperationException(ListHasBeenBanned);
				}

				_List[Index] = new(value);
			}
		}

		#region Управление

		/// <summary>
		/// Заблокировать изменения в списке
		/// </summary>
		/// <exception cref="InvalidOperationException">Изменения уже заблокированы</exception>
		public void MakeReadOnly()
		{
			if (IsReadOnly)
			{
				throw new InvalidOperationException(AlreadyBanned);
			}

			IsReadOnly = true;
		}

		#endregion

		#region IList<LoadingAsyncTask>

		/// <summary>
		/// Вставить задачу в список на определённую позицию
		/// </summary>
		/// <param name="Index">Позиция</param>
		/// <param name="Item">Задача</param>
		/// <exception cref="InvalidOperationException">Изменения заблокированы</exception>
		public void Insert(int Index, LoadingAsyncTask Item)
		{
			if (IsReadOnly)
			{
				throw new InvalidOperationException(ListHasBeenBanned);
			}

			_List.Insert(Index, Item);
		}
		/// <summary>
		/// Вставить задачу в список
		/// </summary>
		/// <param name="Item">Задача</param>
		/// <exception cref="InvalidOperationException">Изменения заблокированы</exception>
		public void Add(LoadingAsyncTask Item)
		{
			if (IsReadOnly)
			{
				throw new InvalidOperationException(ListHasBeenBanned);
			}

			_List.Add(Item);
		}

		/// <summary>
		/// Удалить задачу из списка
		/// </summary>
		/// <param name="Item">Задача</param>
		/// <returns>Была ли удалена задача</returns>
		/// <exception cref="InvalidOperationException">Изменения заблокированы</exception>
		public bool Remove(LoadingAsyncTask Item)
		{
			if (IsReadOnly)
			{
				throw new InvalidOperationException(ListHasBeenBanned);
			}

			return _List.Remove(Item);
		}
		/// <summary>
		/// Удалить задачу на указанной позиции
		/// </summary>
		/// <param name="Index">Позиция на которой надо удалить задачу</param>
		/// <exception cref="InvalidOperationException">Изменения запрещены</exception>
		public void RemoveAt(int Index)
		{
			if (IsReadOnly)
			{
				throw new InvalidOperationException(ListHasBeenBanned);
			}

			_List.RemoveAt(Index);
		}

		/// <summary>
		/// Очистить список задач
		/// </summary>
		/// <exception cref="InvalidOperationException">Изменения заблокированы</exception>
		public void Clear()
		{
			if (IsReadOnly)
			{
				throw new InvalidOperationException(ListHasBeenBanned);
			}

			_List.Clear();
		}

		/// <summary>
		/// Проверить содержится ли указанная задача в списке
		/// </summary>
		/// <param name="Item">Задача для проверки</param>
		/// <returns>Содержится ли указанная задача в списке</returns>
		public bool Contains(LoadingAsyncTask Item)
		{
			return _List.Contains(Item);
		}
		/// <summary>
		/// Скопировать список в указанных массив начиная с указанной позиции
		/// </summary>
		/// <param name="Array">Массив к который будет копироваться список</param>
		/// <param name="ArrayIndex">Позиция с которой надо начать копирование</param>
		public void CopyTo(LoadingAsyncTask[] Array, int ArrayIndex)
		{
			_List.CopyTo(Array, ArrayIndex);
		}
		/// <summary>
		/// Получить позицию указанной задачи
		/// </summary>
		/// <param name="Item">Задача для поиска позиции</param>
		/// <returns>Позиция задачи в списке. Если задачи нет в списке то будет возвращено -1</returns>
		public int IndexOf(LoadingAsyncTask Item)
		{
			return _List.IndexOf(Item);
		}

		/// <summary>
		/// Получить <see cref="IEnumerator{T}"/> коллекции
		/// </summary>
		/// <returns><see cref="IEnumerator{T}"/> коллекции</returns>
		public IEnumerator<LoadingAsyncTask> GetEnumerator()
		{
			return _List.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _List.GetEnumerator();
		}

		#endregion

		#region IList<Func<Task<IAsyncTask>>>

		/// <summary>
		/// Вставить задачу в список на определённую позицию
		/// </summary>
		/// <param name="Index">Позиция</param>
		/// <param name="Item">Задача</param>
		/// <exception cref="InvalidOperationException">Изменения заблокированы</exception>
		public void Insert(int Index, Func<Task<IAsyncTask>> Item)
		{
			Insert(Index, new LoadingAsyncTask(Item));
		}
		/// <summary>
		/// Вставить задачу в список
		/// </summary>
		/// <param name="Item">Задача</param>
		/// <exception cref="InvalidOperationException">Изменения заблокированы</exception>
		public void Add(Func<Task<IAsyncTask>> Item)
		{
			Add(new LoadingAsyncTask(Item));
		}

		/// <summary>
		/// Удалить задачу из списка
		/// </summary>
		/// <param name="Item">Задача</param>
		/// <returns>Была ли удалена задача</returns>
		/// <exception cref="InvalidOperationException">Изменения заблокированы</exception>
		public bool Remove(Func<Task<IAsyncTask>> Item)
		{
			if (IsReadOnly)
			{
				throw new InvalidOperationException(ListHasBeenBanned);
			}

			foreach (var Func in _List)
			{
				if (Func.FuncAsyncTask == Item)
				{
					_List.Remove(Func);
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Проверить содержится ли указанная задача в списке
		/// </summary>
		/// <param name="Item">Задача для проверки</param>
		/// <returns>Содержится ли указанная задача в списке</returns>
		public bool Contains(Func<Task<IAsyncTask>> Item)
		{
			return IndexOf(Item) != -1;
		}
		/// <summary>
		/// Скопировать список в указанных массив начиная с указанной позиции
		/// </summary>
		/// <param name="Array">Массив к который будет копироваться список</param>
		/// <param name="ArrayIndex">Позиция с которой надо начать копирование</param>
		public void CopyTo(Func<Task<IAsyncTask>>[] Array, int ArrayIndex)
		{
			TasksList.CopyTo(Array, ArrayIndex);
		}
		/// <summary>
		/// Получить позицию указанной задачи
		/// </summary>
		/// <param name="Item">Задача для поиска позиции</param>
		/// <returns>Позиция задачи в списке. Если задачи нет в списке то будет возвращено -1</returns>
		public int IndexOf(Func<Task<IAsyncTask>> Item)
		{
			for (int i = 0; i < Count; i++)
			{
				LoadingAsyncTask Func = _List[i];
				if (Func.FuncAsyncTask == Item)
				{
					_List.Remove(Func);
					return i;
				}
			}

			return -1;
		}

		IEnumerator<Func<Task<IAsyncTask>>> IEnumerable<Func<Task<IAsyncTask>>>.GetEnumerator()
		{
			return TasksList.GetEnumerator();
		}

		#endregion

		#region IList<Func<IAsyncTask>>

		/// <summary>
		/// Вставить задачу в список на определённую позицию
		/// </summary>
		/// <param name="Index">Позиция</param>
		/// <param name="Item">Задача</param>
		/// <exception cref="InvalidOperationException">Изменения заблокированы</exception>
		public void Insert(int Index, Func<IAsyncTask> Item)
		{
			Insert(Index, new LoadingAsyncTask(Item));
		}
		/// <summary>
		/// Вставить задачу в список
		/// </summary>
		/// <param name="Item">Задача</param>
		/// <exception cref="InvalidOperationException">Изменения заблокированы</exception>
		public void Add(Func<IAsyncTask> Item)
		{
			Add(new LoadingAsyncTask(Item));
		}

		/// <summary>
		/// Удалить задачу из списка
		/// </summary>
		/// <param name="Item">Задача</param>
		/// <returns>Была ли удалена задача</returns>
		/// <exception cref="InvalidOperationException">Изменения заблокированы</exception>
		public bool Remove(Func<IAsyncTask> Item)
		{
			if (IsReadOnly)
			{
				throw new InvalidOperationException(ListHasBeenBanned);
			}

			foreach (var Func in _List)
			{
				if (Func.AsyncTask == Item)
				{
					_List.Remove(Func);
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Проверить содержится ли указанная задача в списке
		/// </summary>
		/// <param name="Item">Задача для проверки</param>
		/// <returns>Содержится ли указанная задача в списке</returns>
		public bool Contains(Func<IAsyncTask> Item)
		{
			return IndexOf(Item) != -1;
		}
		/// <summary>
		/// Скопировать список в указанных массив начиная с указанной позиции
		/// </summary>
		/// <param name="Array">Массив к который будет копироваться список</param>
		/// <param name="ArrayIndex">Позиция с которой надо начать копирование</param>
		public void CopyTo(Func<IAsyncTask>[] Array, int ArrayIndex)
		{
			AsyncTasksFuncList.CopyTo(Array, ArrayIndex);
		}
		/// <summary>
		/// Получить позицию указанной задачи
		/// </summary>
		/// <param name="Item">Задача для поиска позиции</param>
		/// <returns>Позиция задачи в списке. Если задачи нет в списке то будет возвращено -1</returns>
		public int IndexOf(Func<IAsyncTask> Item)
		{
			for (int i = 0; i < Count; i++)
			{
				LoadingAsyncTask Func = _List[i];
				if (Func.AsyncTask == Item)
				{
					_List.Remove(Func);
					return i;
				}
			}

			return -1;
		}

		IEnumerator<Func<IAsyncTask>> IEnumerable<Func<IAsyncTask>>.GetEnumerator()
		{
			return AsyncTasksFuncList.GetEnumerator();
		}

		#endregion

		#region IList<Func<Task>>

		/// <summary>
		/// Вставить задачу в список на определённую позицию
		/// </summary>
		/// <param name="Index">Позиция</param>
		/// <param name="Item">Задача</param>
		/// <exception cref="InvalidOperationException">Изменения заблокированы</exception>
		public void Insert(int Index, Func<Task> Item)
		{
			Insert(Index, new LoadingAsyncTask(Item));
		}
		/// <summary>
		/// Вставить задачу в список
		/// </summary>
		/// <param name="Item">Задача</param>
		/// <exception cref="InvalidOperationException">Изменения заблокированы</exception>
		public void Add(Func<Task> Item)
		{
			Add(new LoadingAsyncTask(Item));
		}

		/// <summary>
		/// Удалить задачу из списка
		/// </summary>
		/// <param name="Item">Задача</param>
		/// <returns>Была ли удалена задача</returns>
		/// <exception cref="InvalidOperationException">Изменения заблокированы</exception>
		public bool Remove(Func<Task> Item)
		{
			if (IsReadOnly)
			{
				throw new InvalidOperationException(ListHasBeenBanned);
			}

			foreach (var Func in _List)
			{
				if (Func.Task == Item)
				{
					_List.Remove(Func);
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Проверить содержится ли указанная задача в списке
		/// </summary>
		/// <param name="Item">Задача для проверки</param>
		/// <returns>Содержится ли указанная задача в списке</returns>
		public bool Contains(Func<Task> Item)
		{
			return IndexOf(Item) != -1;
		}
		/// <summary>
		/// Скопировать список в указанных массив начиная с указанной позиции
		/// </summary>
		/// <param name="Array">Массив к который будет копироваться список</param>
		/// <param name="ArrayIndex">Позиция с которой надо начать копирование</param>
		public void CopyTo(Func<Task>[] Array, int ArrayIndex)
		{
			TasksFuncList.CopyTo(Array, ArrayIndex);
		}
		/// <summary>
		/// Получить позицию указанной задачи
		/// </summary>
		/// <param name="Item">Задача для поиска позиции</param>
		/// <returns>Позиция задачи в списке. Если задачи нет в списке то будет возвращено -1</returns>
		public int IndexOf(Func<Task> Item)
		{
			for (int i = 0; i < Count; i++)
			{
				LoadingAsyncTask Func = _List[i];
				if (Func.Task == Item)
				{
					_List.Remove(Func);
					return i;
				}
			}

			return -1;
		}

		IEnumerator<Func<Task>> IEnumerable<Func<Task>>.GetEnumerator()
		{
			return TasksFuncList.GetEnumerator();
		}

		#endregion

		#region Общее

		/// <summary>
		/// Добавить список задач
		/// </summary>
		/// <param name="Tasks">Список задач</param>
		public void AddRange(LoadingTasksList Tasks)
		{
			if (Tasks == null)
			{
				throw new ArgumentNullException(nameof(Tasks), TasksNotStated);
			}

			_List.AddRange(Tasks._List);
		}
		/// <summary>
		/// Добавить список задач
		/// </summary>
		/// <param name="Tasks">Список задач</param>
		public void AddRange(IEnumerable<LoadingAsyncTask> Tasks)
		{
			if (Tasks == null)
			{
				throw new ArgumentNullException(nameof(Tasks), TasksNotStated);
			}

			foreach (var Task in Tasks)
			{
				_List.Add(Task);
			}
		}
		/// <summary>
		/// Добавить список задач
		/// </summary>
		/// <param name="Tasks">Список задач</param>
		public void AddRange(IEnumerable<Func<Task<IAsyncTask>>> Tasks)
		{
			if (Tasks == null)
			{
				throw new ArgumentNullException(nameof(Tasks), TasksNotStated);
			}

			foreach (var Task in Tasks)
			{
				Add(Task);
			}
		}
		/// <summary>
		/// Добавить список задач
		/// </summary>
		/// <param name="Tasks">Список задач</param>
		public void AddRange(IEnumerable<Func<IAsyncTask>> Tasks)
		{
			if (Tasks == null)
			{
				throw new ArgumentNullException(nameof(Tasks), TasksNotStated);
			}

			foreach (var Task in Tasks)
			{
				Add(Task);
			}
		}
		/// <summary>
		/// Добавить список задач
		/// </summary>
		/// <param name="Tasks">Список задач</param>
		public void AddRange(IEnumerable<Func<Task>> Tasks)
		{
			if (Tasks == null)
			{
				throw new ArgumentNullException(nameof(Tasks), TasksNotStated);
			}

			foreach (var Task in Tasks)
			{
				Add(Task);
			}
		}

		#endregion

		#region Константы

		private const string ListHasBeenBanned = "Невозможно изменить список задач, так как он был заблокирован";
		private const string AlreadyBanned = "Список уже заблокирован";

		private const string TaskNotStated = "Задача для выполнения не указана";
		private const string TasksNotStated = "Задачи для выполнения не указаны";

		private const string ActionNotImplemented = "Это действие недоступно используй: ";

		#endregion
	}
}