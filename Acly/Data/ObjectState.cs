using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

namespace Acly
{
    /// <summary>
    /// Сохранённое состояние объекта
    /// </summary>
    public class ObjectState
    {
        /// <summary>
        /// Создать сохранённое состояние объекта
        /// </summary>
        /// <param name="obj">Объект для сохранения состояния</param>
        /// <exception cref="ArgumentNullException"></exception>
        public ObjectState(object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj), "Объект не указан");
            }

            Dictionary<PropertyInfo, object> savedValues = [];

            foreach (var property in obj.GetType().GetProperties())
            {
                if (property.CanRead && property.CanWrite)
                {
                    savedValues.Add(property, property.GetValue(obj));
                }
            }

            Object = obj;
            Values = new(savedValues);
        }

        /// <summary>
        /// Объект сохранённого состояния
        /// </summary>
        public object Object { get; }
        /// <summary>
        /// Сохранённые значения
        /// </summary>
        public ReadOnlyDictionary<PropertyInfo, object> Values { get; }

        #region Управление

        /// <summary>
        /// Восстановить состояние
        /// </summary>
        public void Restore()
        {
            foreach (var info in Values)
            {
                info.Key.SetValue(Object, info.Value);
            }
        }

        #endregion
    }
}
