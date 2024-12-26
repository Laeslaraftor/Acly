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
        /// <param name="Object">Объект для сохранения состояния</param>
        /// <exception cref="ArgumentNullException"></exception>
        public ObjectState(object Object)
        {
            if (Object == null)
            {
                throw new ArgumentNullException(nameof(Object), "Объект не указан");
            }

            Dictionary<PropertyInfo, object> SavedValues = new();
            
            foreach (var Prop in Object.GetType().GetProperties())
            {
                if (Prop.CanRead && Prop.CanWrite)
                {
                    SavedValues.Add(Prop, Prop.GetValue(Object));
                }
            }

            this.Object = Object;
            Values = new(SavedValues);
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
            foreach (var Pair in Values)
            {
                Pair.Key.SetValue(Object, Pair.Value);
            }
        }

        #endregion
    }
}
