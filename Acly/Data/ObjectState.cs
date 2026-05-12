using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

namespace Acly
{
    /// <summary>
    /// Сохранённое состояние объекта
    /// </summary>
    public readonly struct ObjectState : IEquatable<ObjectState>
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

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="obj"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public override bool Equals(object obj)
        {
            return obj is ObjectState other &&
                   Equals(other);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns><inheritdoc/></returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(Object, Values);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="other"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public bool Equals(ObjectState other)
        {
            return Equals(Object, other.Object) &&
                   Equals(Values, other.Values);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="left"><inheritdoc/></param>
        /// <param name="right"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public static bool operator ==(ObjectState left, ObjectState right)
        {
            return left.Equals(right);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="left"><inheritdoc/></param>
        /// <param name="right"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public static bool operator !=(ObjectState left, ObjectState right)
        {
            return !(left == right);
        }

        #endregion
    }
}
