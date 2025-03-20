using System;
using System.Data;
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
        /// <param name="Object">Объект, который надо скопировать</param>
        /// <param name="DeepCopy">Копировать значения</param>
        /// <param name="CtorParameters">Параметры конструктора</param>
        /// <returns>Скопированный объект (если получилось)</returns>
        public object CreateCopy(object Object, bool DeepCopy, params object[]? CtorParameters)
        {
            if (Object == null)
            {
                throw new ArgumentNullException(nameof(Object));
            }

            return CreateCopy(Object, Object.GetType(), DeepCopy, CtorParameters);
        }
        /// <summary>
        /// Скопировать значения объекта в другой объект
        /// </summary>
        /// <param name="Source">Объект из которого будут взяты значения</param>
        /// <param name="Destination">Объект в который будут вставлены значения</param>
        /// <param name="DeepCopy">Копировать значения</param>
        /// <exception cref="ArgumentNullException"></exception>
        public void CopyTo(object Source, ref object Destination, bool DeepCopy)
        {
            if (Source == null)
            {
                throw new ArgumentNullException(nameof(Source));
            }
            if (Destination == null)
            {
                throw new ArgumentNullException(nameof(Destination));
            }

            Type? ResultType = null;
            Type SourceType = Source.GetType();
            Type DestinationType = Destination.GetType();

            if (SourceType.IsAssignableFrom(DestinationType))
            {
                ResultType = SourceType;
            }
            else if (DestinationType.IsAssignableFrom(SourceType))
            {
                ResultType = DestinationType;
            }

            if (ResultType == null)
            {
                throw new ArgumentException("Невозможно вставить значения, так как типы объектов разные");
            }

            CopyTo(Source, ref Destination, Source.GetType(), DeepCopy);
        }

        /// <summary>
        /// Проверить можно ли просто вернуть скопированный объект
        /// </summary>
        /// <param name="Object">Скопированный объект</param>
        /// <param name="ObjectType">Тип скопированного объекта</param>
        /// <returns>Можно ли просто вернуть скопированный объект</returns>
        protected virtual bool ShouldReturnCopiedObject(object Object, Type ObjectType)
        {
            return ObjectType == typeof(string) 
                || ObjectType == typeof(Enum) 
                || ObjectType == typeof(Color) 
                || ObjectType.IsStandardStruct();
        }

        /// <summary>
        /// Создать копию объекта
        /// </summary>
        /// <param name="Object">Объект, который надо скопировать</param>
        /// <param name="ObjectType">Тип скопированного объекта</param>
        /// <param name="DeepCopy">Копировать значения</param>
        /// <param name="CtorParameters">Параметры конструктора</param>
        /// <returns>Копия объекта</returns>
        /// <exception cref="ArgumentNullException"></exception>
        protected virtual object CreateCopy(object Object, Type ObjectType, bool DeepCopy, params object[]? CtorParameters)
        {
            if (ObjectType == null)
            {
                throw new ArgumentNullException(nameof(ObjectType));
            }
            if (ShouldReturnCopiedObject(Object, ObjectType))
            {
                return Object;
            }

            object? Result = Activator.CreateInstance(ObjectType, CtorParameters);

            if (Result == null)
            {
                throw new InvalidOperationException("Не удалось создать экземпляр объекта типа " +  ObjectType.Name);
            }

            CopyTo(Object, ref Result, DeepCopy);

            return Result;
        }
        /// <summary>
        /// Скопировать значения объекта в другой объект
        /// </summary>
        /// <param name="Source">Объект из которого будут взяты значения</param>
        /// <param name="Destination">Объект в который будут вставлены значения</param>
        /// <param name="DeepCopy">Копировать значения</param>
        /// <param name="ObjectsType">Тип объектов</param>
        protected virtual void CopyTo(object Source, ref object Destination, Type ObjectsType, bool DeepCopy)
        {
            if (ObjectsType == null)
            {
                throw new ArgumentNullException(nameof(ObjectsType));
            }
            if (ShouldReturnCopiedObject(Source, ObjectsType))
            {
                Destination = Source;
                return;
            }

            foreach (var Property in ObjectsType.GetProperties())
            {
                if (Property.CanWrite && Property.CanRead)
                {
                    object? Value = Property.GetValue(Source);

                    if (DeepCopy)
                    {
                        Value = Value.SoftCopy(DeepCopy);
                    }

                    Property.SetValue(Destination, Value, null);
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
