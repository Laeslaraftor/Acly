using System;
using System.Collections.Generic;

namespace Acly.Data
{
    /// <summary>
    /// Класс пула объектов
    /// </summary>
    /// <typeparam name="T">Тип объекта, находящегося в пуле</typeparam>
    /// <param name="fabric">Фабрика для создания новых экземпляров объектов</param>
    public class ElementsPool<T>(Func<T> fabric) : Disposable
    {
        /// <summary>
        /// Создать новый экземпляр пула объектов
        /// </summary>
        public ElementsPool()
            : this(Activator.CreateInstance<T>)
        {
        }

        private readonly Queue<T> _freeElements = [];
        private readonly List<T> _usedElements = [];
        private readonly Func<T> _fabric = fabric;

        #region Управление

        /// <summary>
        /// Получить объект. Возвращает первый свободный объект. 
        /// Если свободного объекта нет, то будет создан новый экземпляр.
        /// </summary>
        /// <returns>Объект из пула</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public T GetElement()
        {
            if (IsDisposed)
            {
                throw new InvalidOperationException("Невозможно получить объект, так как пул очищен.");
            }
            if (!_freeElements.TryDequeue(out var element))
            {
                element = _fabric();
            }

            _usedElements.Add(element);

            return element;
        }
        /// <summary>
        /// Освободить объект и внести его обратно в пул.
        /// </summary>
        /// <param name="element">Объект, который надо освободить.</param>
        /// <returns>Был ли освобождён объект</returns>
        public bool Free(T element)
        {
            if (IsDisposed)
            {
                throw new InvalidOperationException("Невозможно освободить объект, так как пул очищен.");
            }
            if (_usedElements.Remove(element))
            {
                _usedElements.Remove(element);
                _freeElements.Enqueue(element);

                return true;
            }

            return false;
        }
        /// <summary>
        /// Освободить все объекты и внести их обратно в пул.
        /// </summary>
        /// <returns>Количество освобождённых объектов.</returns>
        public int FreeAll()
        {
            if (IsDisposed)
            {
                throw new InvalidOperationException("Невозможно освободить объекты, так как пул очищен.");
            }

            int count = 0;

            foreach (var element in _usedElements)
            {
                _freeElements.Enqueue(element);
                count++;
            }

            _usedElements.Clear();

            return count;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="isDisposing"><inheritdoc/></param>
        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);

            DisposeAll(_usedElements);
            DisposeAll(_freeElements);

            _usedElements.Clear();
            _freeElements.Clear();
        }

        private static void DisposeAll(IEnumerable<T> enumerable)
        {
            foreach (var element in enumerable)
            {
                if (element is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }
        }

        #endregion
    }
}
