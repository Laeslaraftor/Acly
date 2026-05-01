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
    public class LoadingTasksList : IList<LoadingAsyncTask>, IList<Func<Task<IAsyncTask?>>>, IList<Func<IAsyncTask?>>, IList<Func<Task>>
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
        /// <param name="taskForPerform">Задача для загрузки</param>
        public LoadingTasksList(Func<Task<IAsyncTask?>> taskForPerform)
        {
            if (taskForPerform == null)
            {
                throw new ArgumentNullException(nameof(taskForPerform), TaskNotStated);
            }

            _tasks.Add(new(taskForPerform));
        }
        /// <summary>
        /// Создать список задач для загрузки
        /// </summary>
        /// <param name="tasksForPerform">Список задач для загрузки</param>
        public LoadingTasksList(IEnumerable<Func<Task<IAsyncTask?>>> tasksForPerform)
        {
            if (tasksForPerform == null)
            {
                throw new ArgumentNullException(nameof(tasksForPerform), TasksNotStated);
            }

            foreach (var Task in tasksForPerform)
            {
                _tasks.Add(new(Task));
            }
        }
        /// <summary>
        /// Создать список задач для загрузки
        /// </summary>
        /// <param name="taskForPerform">Задача для загрузки</param>
        public LoadingTasksList(Func<IAsyncTask?> taskForPerform)
        {
            if (taskForPerform == null)
            {
                throw new ArgumentNullException(nameof(taskForPerform), TaskNotStated);
            }

            _tasks.Add(new(taskForPerform));
        }
        /// <summary>
        /// Создать список задач для загрузки
        /// </summary>
        /// <param name="tasksForPerform">Список задач для загрузки</param>
        public LoadingTasksList(IEnumerable<Func<IAsyncTask?>> tasksForPerform)
        {
            if (tasksForPerform == null)
            {
                throw new ArgumentNullException(nameof(tasksForPerform), TasksNotStated);
            }

            foreach (var Task in tasksForPerform)
            {
                _tasks.Add(new(Task));
            }
        }
        /// <summary>
        /// Создать список задач для загрузки
        /// </summary>
        /// <param name="taskForPerform">Задача для загрузки</param>
        public LoadingTasksList(Func<Task> taskForPerform)
        {
            if (taskForPerform == null)
            {
                throw new ArgumentNullException(nameof(taskForPerform), TaskNotStated);
            }

            _tasks.Add(new(taskForPerform));
        }
        /// <summary>
        /// Создать список задач для загрузки
        /// </summary>
        /// <param name="tasksForPerform">Список задач для загрузки</param>
        public LoadingTasksList(IEnumerable<Func<Task>> tasksForPerform)
        {
            if (tasksForPerform == null)
            {
                throw new ArgumentNullException(nameof(tasksForPerform), TasksNotStated);
            }

            foreach (var Task in tasksForPerform)
            {
                _tasks.Add(new(Task));
            }
        }
        /// <summary>
        /// Создать список задач для загрузки
        /// </summary>
        /// <param name="taskForPerform">Задача для загрузки</param>
        public LoadingTasksList(LoadingAsyncTask taskForPerform)
        {
            if (taskForPerform == null)
            {
                throw new ArgumentNullException(nameof(taskForPerform), TaskNotStated);
            }

            _tasks.Add(taskForPerform);
        }
        /// <summary>
        /// Создать список задач для загрузки
        /// </summary>
        /// <param name="tasksForPerform">Список задач для загрузки</param>
        public LoadingTasksList(IEnumerable<LoadingAsyncTask> tasksForPerform)
        {
            if (tasksForPerform == null)
            {
                throw new ArgumentNullException(nameof(tasksForPerform), TasksNotStated);
            }

            foreach (var Task in tasksForPerform)
            {
                _tasks.Add(Task);
            }
        }

        /// <summary>
        /// Количество задач в списке
        /// </summary>
        public int Count => _tasks.Count;
        /// <summary>
        /// Можно ли вносить изменения в список
        /// </summary>
        public bool IsReadOnly { get; private set; }
        /// <summary>
        /// Получить или установить задачу на указанную позицию
        /// </summary>
        /// <param name="index">Позиция в списке</param>
        /// <returns>Задача</returns>
        /// <exception cref="InvalidOperationException">Изменения заблокированы</exception>
        public LoadingAsyncTask this[int index]
        {
            get => _tasks[index];
            set
            {
                if (IsReadOnly)
                {
                    throw new InvalidOperationException(ListHasBeenBanned);
                }

                _tasks[index] = value;
            }
        }

        private List<Func<Task<IAsyncTask?>>> TasksList
        {
            get
            {
                List<Func<Task<IAsyncTask?>>> result = [];

                foreach (var func in _tasks)
                {
                    if (func.FuncAsyncTask == null)
                    {
                        continue;
                    }

                    result.Add(func.FuncAsyncTask);
                }

                return result;
            }
        }
        private List<Func<IAsyncTask?>> AsyncTasksFuncList
        {
            get
            {
                List<Func<IAsyncTask?>> result = [];

                foreach (var func in _tasks)
                {
                    if (func.AsyncTask == null)
                    {
                        continue;
                    }

                    result.Add(func.AsyncTask);
                }

                return result;
            }
        }
        private List<Func<Task>> TasksFuncList
        {
            get
            {
                List<Func<Task>> result = [];

                foreach (var func in _tasks)
                {
                    if (func.Task == null)
                    {
                        continue;
                    }

                    result.Add(func.Task);
                }

                return result;
            }
        }

        Func<Task> IList<Func<Task>>.this[int index]
        {
            get => throw new InvalidOperationException(ActionNotImplemented + "LoadingAsyncTask this[int index]");
            set
            {
                if (IsReadOnly)
                {
                    throw new InvalidOperationException(ListHasBeenBanned);
                }

                _tasks[index] = new(value);
            }
        }
        Func<IAsyncTask?> IList<Func<IAsyncTask?>>.this[int index]
        {
            get => throw new InvalidOperationException(ActionNotImplemented + "LoadingAsyncTask this[int index]");
            set
            {
                if (IsReadOnly)
                {
                    throw new InvalidOperationException(ListHasBeenBanned);
                }

                _tasks[index] = new(value);
            }
        }
        Func<Task<IAsyncTask?>> IList<Func<Task<IAsyncTask?>>>.this[int index]
        {
            get => throw new InvalidOperationException(ActionNotImplemented + "LoadingAsyncTask this[int index]");
            set
            {
                if (IsReadOnly)
                {
                    throw new InvalidOperationException(ListHasBeenBanned);
                }

                _tasks[index] = new(value);
            }
        }

        private readonly List<LoadingAsyncTask> _tasks = [];

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
        /// <param name="index">Позиция</param>
        /// <param name="item">Задача</param>
        /// <exception cref="InvalidOperationException">Изменения заблокированы</exception>
        public void Insert(int index, LoadingAsyncTask item)
        {
            if (IsReadOnly)
            {
                throw new InvalidOperationException(ListHasBeenBanned);
            }

            _tasks.Insert(index, item);
        }
        /// <summary>
        /// Вставить задачу в список
        /// </summary>
        /// <param name="item">Задача</param>
        /// <exception cref="InvalidOperationException">Изменения заблокированы</exception>
        public void Add(LoadingAsyncTask item)
        {
            if (IsReadOnly)
            {
                throw new InvalidOperationException(ListHasBeenBanned);
            }

            _tasks.Add(item);
        }

        /// <summary>
        /// Удалить задачу из списка
        /// </summary>
        /// <param name="item">Задача</param>
        /// <returns>Была ли удалена задача</returns>
        /// <exception cref="InvalidOperationException">Изменения заблокированы</exception>
        public bool Remove(LoadingAsyncTask item)
        {
            if (IsReadOnly)
            {
                throw new InvalidOperationException(ListHasBeenBanned);
            }

            return _tasks.Remove(item);
        }
        /// <summary>
        /// Удалить задачу на указанной позиции
        /// </summary>
        /// <param name="index">Позиция на которой надо удалить задачу</param>
        /// <exception cref="InvalidOperationException">Изменения запрещены</exception>
        public void RemoveAt(int index)
        {
            if (IsReadOnly)
            {
                throw new InvalidOperationException(ListHasBeenBanned);
            }

            _tasks.RemoveAt(index);
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

            _tasks.Clear();
        }

        /// <summary>
        /// Проверить содержится ли указанная задача в списке
        /// </summary>
        /// <param name="item">Задача для проверки</param>
        /// <returns>Содержится ли указанная задача в списке</returns>
        public bool Contains(LoadingAsyncTask item)
        {
            return _tasks.Contains(item);
        }
        /// <summary>
        /// Скопировать список в указанных массив начиная с указанной позиции
        /// </summary>
        /// <param name="Array">Массив к который будет копироваться список</param>
        /// <param name="arrayIndex">Позиция с которой надо начать копирование</param>
        public void CopyTo(LoadingAsyncTask[] Array, int arrayIndex)
        {
            _tasks.CopyTo(Array, arrayIndex);
        }
        /// <summary>
        /// Получить позицию указанной задачи
        /// </summary>
        /// <param name="item">Задача для поиска позиции</param>
        /// <returns>Позиция задачи в списке. Если задачи нет в списке то будет возвращено -1</returns>
        public int IndexOf(LoadingAsyncTask item)
        {
            return _tasks.IndexOf(item);
        }

        /// <summary>
        /// Получить <see cref="IEnumerator{T}"/> коллекции
        /// </summary>
        /// <returns><see cref="IEnumerator{T}"/> коллекции</returns>
        public IEnumerator<LoadingAsyncTask> GetEnumerator()
        {
            return _tasks.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _tasks.GetEnumerator();
        }

        #endregion

        #region IList<Func<Task<IAsyncTask>>>

        /// <summary>
        /// Вставить задачу в список на определённую позицию
        /// </summary>
        /// <param name="index">Позиция</param>
        /// <param name="item">Задача</param>
        /// <exception cref="InvalidOperationException">Изменения заблокированы</exception>
        public void Insert(int index, Func<Task<IAsyncTask?>> item)
        {
            Insert(index, new LoadingAsyncTask(item));
        }
        /// <summary>
        /// Вставить задачу в список
        /// </summary>
        /// <param name="item">Задача</param>
        /// <exception cref="InvalidOperationException">Изменения заблокированы</exception>
        public void Add(Func<Task<IAsyncTask?>> item)
        {
            Add(new LoadingAsyncTask(item));
        }

        /// <summary>
        /// Удалить задачу из списка
        /// </summary>
        /// <param name="item">Задача</param>
        /// <returns>Была ли удалена задача</returns>
        /// <exception cref="InvalidOperationException">Изменения заблокированы</exception>
        public bool Remove(Func<Task<IAsyncTask?>> item)
        {
            if (IsReadOnly)
            {
                throw new InvalidOperationException(ListHasBeenBanned);
            }

            foreach (var func in _tasks)
            {
                if (func.FuncAsyncTask == item)
                {
                    _tasks.Remove(func);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Проверить содержится ли указанная задача в списке
        /// </summary>
        /// <param name="item">Задача для проверки</param>
        /// <returns>Содержится ли указанная задача в списке</returns>
        public bool Contains(Func<Task<IAsyncTask?>> item)
        {
            return IndexOf(item) != -1;
        }
        /// <summary>
        /// Скопировать список в указанных массив начиная с указанной позиции
        /// </summary>
        /// <param name="Array">Массив к который будет копироваться список</param>
        /// <param name="arrayIndex">Позиция с которой надо начать копирование</param>
        public void CopyTo(Func<Task<IAsyncTask?>>[] Array, int arrayIndex)
        {
            TasksList.CopyTo(Array, arrayIndex);
        }
        /// <summary>
        /// Получить позицию указанной задачи
        /// </summary>
        /// <param name="item">Задача для поиска позиции</param>
        /// <returns>Позиция задачи в списке. Если задачи нет в списке то будет возвращено -1</returns>
        public int IndexOf(Func<Task<IAsyncTask?>> item)
        {
            for (int i = 0; i < Count; i++)
            {
                LoadingAsyncTask func = _tasks[i];
                if (func.FuncAsyncTask == item)
                {
                    _tasks.Remove(func);
                    return i;
                }
            }

            return -1;
        }

        IEnumerator<Func<Task<IAsyncTask?>>> IEnumerable<Func<Task<IAsyncTask?>>>.GetEnumerator()
        {
            return TasksList.GetEnumerator();
        }

        #endregion

        #region IList<Func<IAsyncTask>>

        /// <summary>
        /// Вставить задачу в список на определённую позицию
        /// </summary>
        /// <param name="index">Позиция</param>
        /// <param name="item">Задача</param>
        /// <exception cref="InvalidOperationException">Изменения заблокированы</exception>
        public void Insert(int index, Func<IAsyncTask?> item)
        {
            Insert(index, new LoadingAsyncTask(item));
        }
        /// <summary>
        /// Вставить задачу в список
        /// </summary>
        /// <param name="item">Задача</param>
        /// <exception cref="InvalidOperationException">Изменения заблокированы</exception>
        public void Add(Func<IAsyncTask?> item)
        {
            Add(new LoadingAsyncTask(item));
        }

        /// <summary>
        /// Удалить задачу из списка
        /// </summary>
        /// <param name="item">Задача</param>
        /// <returns>Была ли удалена задача</returns>
        /// <exception cref="InvalidOperationException">Изменения заблокированы</exception>
        public bool Remove(Func<IAsyncTask?> item)
        {
            if (IsReadOnly)
            {
                throw new InvalidOperationException(ListHasBeenBanned);
            }

            foreach (var func in _tasks)
            {
                if (func.AsyncTask == item)
                {
                    _tasks.Remove(func);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Проверить содержится ли указанная задача в списке
        /// </summary>
        /// <param name="item">Задача для проверки</param>
        /// <returns>Содержится ли указанная задача в списке</returns>
        public bool Contains(Func<IAsyncTask?> item)
        {
            return IndexOf(item) != -1;
        }
        /// <summary>
        /// Скопировать список в указанных массив начиная с указанной позиции
        /// </summary>
        /// <param name="Array">Массив к который будет копироваться список</param>
        /// <param name="arrayIndex">Позиция с которой надо начать копирование</param>
        public void CopyTo(Func<IAsyncTask?>[] Array, int arrayIndex)
        {
            AsyncTasksFuncList.CopyTo(Array, arrayIndex);
        }
        /// <summary>
        /// Получить позицию указанной задачи
        /// </summary>
        /// <param name="item">Задача для поиска позиции</param>
        /// <returns>Позиция задачи в списке. Если задачи нет в списке то будет возвращено -1</returns>
        public int IndexOf(Func<IAsyncTask?> item)
        {
            for (int i = 0; i < Count; i++)
            {
                LoadingAsyncTask func = _tasks[i];
                if (func.AsyncTask == item)
                {
                    _tasks.Remove(func);
                    return i;
                }
            }

            return -1;
        }

        IEnumerator<Func<IAsyncTask?>> IEnumerable<Func<IAsyncTask?>>.GetEnumerator()
        {
            return AsyncTasksFuncList.GetEnumerator();
        }

        #endregion

        #region IList<Func<Task>>

        /// <summary>
        /// Вставить задачу в список на определённую позицию
        /// </summary>
        /// <param name="index">Позиция</param>
        /// <param name="item">Задача</param>
        /// <exception cref="InvalidOperationException">Изменения заблокированы</exception>
        public void Insert(int index, Func<Task> item)
        {
            Insert(index, new LoadingAsyncTask(item));
        }
        /// <summary>
        /// Вставить задачу в список
        /// </summary>
        /// <param name="item">Задача</param>
        /// <exception cref="InvalidOperationException">Изменения заблокированы</exception>
        public void Add(Func<Task> item)
        {
            Add(new LoadingAsyncTask(item));
        }

        /// <summary>
        /// Удалить задачу из списка
        /// </summary>
        /// <param name="item">Задача</param>
        /// <returns>Была ли удалена задача</returns>
        /// <exception cref="InvalidOperationException">Изменения заблокированы</exception>
        public bool Remove(Func<Task> item)
        {
            if (IsReadOnly)
            {
                throw new InvalidOperationException(ListHasBeenBanned);
            }

            foreach (var func in _tasks)
            {
                if (func.Task == item)
                {
                    _tasks.Remove(func);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Проверить содержится ли указанная задача в списке
        /// </summary>
        /// <param name="item">Задача для проверки</param>
        /// <returns>Содержится ли указанная задача в списке</returns>
        public bool Contains(Func<Task> item)
        {
            return IndexOf(item) != -1;
        }
        /// <summary>
        /// Скопировать список в указанных массив начиная с указанной позиции
        /// </summary>
        /// <param name="Array">Массив к который будет копироваться список</param>
        /// <param name="arrayIndex">Позиция с которой надо начать копирование</param>
        public void CopyTo(Func<Task>[] Array, int arrayIndex)
        {
            TasksFuncList.CopyTo(Array, arrayIndex);
        }
        /// <summary>
        /// Получить позицию указанной задачи
        /// </summary>
        /// <param name="item">Задача для поиска позиции</param>
        /// <returns>Позиция задачи в списке. Если задачи нет в списке то будет возвращено -1</returns>
        public int IndexOf(Func<Task> item)
        {
            for (int i = 0; i < Count; i++)
            {
                LoadingAsyncTask func = _tasks[i];
                if (func.Task == item)
                {
                    _tasks.Remove(func);
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
        /// <param name="tasks">Список задач</param>
        public void AddRange(LoadingTasksList tasks)
        {
            if (tasks == null)
            {
                throw new ArgumentNullException(nameof(tasks), TasksNotStated);
            }

            _tasks.AddRange(tasks._tasks);
        }
        /// <summary>
        /// Добавить список задач
        /// </summary>
        /// <param name="tasks">Список задач</param>
        public void AddRange(IEnumerable<LoadingAsyncTask> tasks)
        {
            if (tasks == null)
            {
                throw new ArgumentNullException(nameof(tasks), TasksNotStated);
            }

            foreach (var task in tasks)
            {
                _tasks.Add(task);
            }
        }
        /// <summary>
        /// Добавить список задач
        /// </summary>
        /// <param name="tasks">Список задач</param>
        public void AddRange(IEnumerable<Func<Task<IAsyncTask?>>> tasks)
        {
            if (tasks == null)
            {
                throw new ArgumentNullException(nameof(tasks), TasksNotStated);
            }

            foreach (var task in tasks)
            {
                Add(task);
            }
        }
        /// <summary>
        /// Добавить список задач
        /// </summary>
        /// <param name="tasks">Список задач</param>
        public void AddRange(IEnumerable<Func<IAsyncTask?>> tasks)
        {
            if (tasks == null)
            {
                throw new ArgumentNullException(nameof(tasks), TasksNotStated);
            }

            foreach (var task in tasks)
            {
                Add(task);
            }
        }
        /// <summary>
        /// Добавить список задач
        /// </summary>
        /// <param name="tasks">Список задач</param>
        public void AddRange(IEnumerable<Func<Task>> tasks)
        {
            if (tasks == null)
            {
                throw new ArgumentNullException(nameof(tasks), TasksNotStated);
            }

            foreach (var task in tasks)
            {
                Add(task);
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