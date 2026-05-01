using System;
using System.Drawing;

namespace Acly.Data
{
    /// <summary>
    /// Класс, создающий копии объектов
    /// </summary>
    public class ObjectCopyMaker
    {
        /// <summary>
        /// Создать копию объекта
        /// </summary>
        /// <param name="obj">Объект, который надо скопировать</param>
        /// <param name="deepCopy">Копировать значения</param>
        /// <param name="ctorParameters">Параметры конструктора</param>
        /// <returns>Скопированный объект (если получилось)</returns>
        public object CreateCopy(object obj, bool deepCopy, params object[]? ctorParameters)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            return CreateCopy(obj, obj.GetType(), deepCopy, ctorParameters);
        }
        /// <summary>
        /// Скопировать значения объекта в другой объект
        /// </summary>
        /// <param name="source">Объект из которого будут взяты значения</param>
        /// <param name="destination">Объект в который будут вставлены значения</param>
        /// <param name="deepCopy">Копировать значения</param>
        /// <exception cref="ArgumentNullException"></exception>
        public void CopyTo(object source, ref object destination, bool deepCopy)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            Type? resultType = null;
            Type sourceType = source.GetType();
            Type destinationType = destination.GetType();

            if (sourceType.IsAssignableFrom(destinationType))
            {
                resultType = sourceType;
            }
            else if (destinationType.IsAssignableFrom(sourceType))
            {
                resultType = destinationType;
            }

            if (resultType == null)
            {
                throw new ArgumentException("Невозможно вставить значения, так как типы объектов разные");
            }

            CopyTo(source, ref destination, source.GetType(), deepCopy);
        }

        /// <summary>
        /// Проверить можно ли просто вернуть скопированный объект
        /// </summary>
        /// <param name="obj">Скопированный объект</param>
        /// <param name="objectType">Тип скопированного объекта</param>
        /// <returns>Можно ли просто вернуть скопированный объект</returns>
        protected virtual bool ShouldReturnCopiedObject(object obj, Type objectType)
        {
            return objectType == typeof(string)
                || objectType == typeof(Enum)
                || objectType == typeof(Color)
                || objectType.IsStandardStruct();
        }

        /// <summary>
        /// Создать копию объекта
        /// </summary>
        /// <param name="obj">Объект, который надо скопировать</param>
        /// <param name="objectType">Тип скопированного объекта</param>
        /// <param name="deepCopy">Копировать значения</param>
        /// <param name="ctorParameters">Параметры конструктора</param>
        /// <returns>Копия объекта</returns>
        /// <exception cref="ArgumentNullException"></exception>
        protected virtual object CreateCopy(object obj, Type objectType, bool deepCopy, params object[]? ctorParameters)
        {
            if (objectType == null)
            {
                throw new ArgumentNullException(nameof(objectType));
            }
            if (ShouldReturnCopiedObject(obj, objectType))
            {
                return obj;
            }

            object? result = Activator.CreateInstance(objectType, ctorParameters)
                ?? throw new InvalidOperationException("Не удалось создать экземпляр объекта типа " + objectType.Name);

            CopyTo(obj, ref result, deepCopy);

            return result;
        }
        /// <summary>
        /// Скопировать значения объекта в другой объект
        /// </summary>
        /// <param name="source">Объект из которого будут взяты значения</param>
        /// <param name="destination">Объект в который будут вставлены значения</param>
        /// <param name="deepCopy">Копировать значения</param>
        /// <param name="objectsType">Тип объектов</param>
        protected virtual void CopyTo(object source, ref object destination, Type objectsType, bool deepCopy)
        {
            if (objectsType == null)
            {
                throw new ArgumentNullException(nameof(objectsType));
            }
            if (ShouldReturnCopiedObject(source, objectsType))
            {
                destination = source;
                return;
            }

            foreach (var property in objectsType.GetProperties())
            {
                if (property.CanWrite && property.CanRead)
                {
                    object? value = property.GetValue(source);

                    if (deepCopy)
                    {
                        value = value.SoftCopy(deepCopy);
                    }

                    property.SetValue(destination, value, null);
                }
            }
        }

        #region Статика

        /// <summary>
        /// Глобальный экземпляр класса
        /// </summary>
        public static readonly ObjectCopyMaker Instance = new();

        #endregion
    }
}
