using Acly.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Threading.Tasks;

namespace Acly
{
    public static partial class Helper
    {
        /// <summary>
        /// Получить типы имеющие указанный атрибут
        /// </summary>
        /// <typeparam name="T">Атрибут</typeparam>
        /// <returns>Типы с указанным атрибутом</returns>
        public static async Task<IEnumerable<Type>> GetTypesWithAttribute<T>() where T : Attribute
        {
            List<Type> result = [];

            await Task.Run(() =>
            {
                Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

                foreach (var assembly in assemblies)
                {
                    foreach (Type type in assembly.GetTypes())
                    {
                        if (type.GetCustomAttributes(typeof(T), true).Length > 0)
                        {
                            result.Add(type);
                        }
                    }
                }
            });

            return result;
        }
        /// <summary>
        /// Определить реализует ли тип указанный интерфейс
        /// </summary>
        /// <typeparam name="T">Интерфейс</typeparam>
        /// <returns>Реализует ли тип указанный интерфейс</returns>
        /// <exception cref="ArgumentNullException">Ссылка на тип не указывает на экземпляр объекта</exception>
        public static bool IsImplementsInterface<T>(this Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type) + " не указан");
            }

            Type[] interfaces = type.GetInterfaces();
            Type requestInterface = typeof(T);

            foreach (var @interface in interfaces)
            {
                if (@interface == requestInterface)
                {
                    return true;
                }
            }

            return false;
        }
        /// <summary>
        /// Проверить может ли тип принимать null значения
        /// </summary>
        /// <param name="type">Тип, который надо проверить</param>
        /// <returns>Может ли тип принимать null значения</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool IsNullable(this Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return !type.IsValueType || Nullable.GetUnderlyingType(type) != null;
        }

        /// <summary>
        /// Найти поле в типе
        /// </summary>
        /// <param name="type">Тип в котором надо найти поле</param>
        /// <param name="predicate">Условие отбора</param>
        /// <returns>Поле (если найдено)</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static PropertyInfo? FindProperty(this Type type, Predicate<PropertyInfo> predicate)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type), "Тип не указан");
            }
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate), "Условие не указано");
            }

            foreach (var property in type.GetProperties())
            {
                if (predicate(property))
                {
                    return property;
                }
            }

            return null;
        }
        /// <summary>
        /// Найти поле в типе
        /// </summary>
        /// <param name="type">Тип в котором надо найти поле</param>
        /// <param name="propertyName">Название поля</param>
        /// <returns>Поле (если найдено)</returns>
        public static PropertyInfo? FindProperty(this Type type, string propertyName)
        {
            return type.FindProperty(P => P.Name == propertyName);
        }
        /// <summary>
        /// Попытаться найти поле в типе
        /// </summary>
        /// <param name="type">Тип в котором надо попытаться найти поле</param>
        /// <param name="predicate">Условие отбора</param>
        /// <param name="result">Поле (если найдено)</param>
        /// <returns>Найдено ли поле</returns>
        public static bool TryFindProperty(this Type type, Predicate<PropertyInfo> predicate, [NotNullWhen(true)] out PropertyInfo? result)
        {
            result = type.FindProperty(predicate);
            return result != null;
        }
        /// <summary>
        /// Попытаться найти поле в типе
        /// </summary>
        /// <param name="type">Тип в котором надо попытаться найти поле</param>
        /// <param name="propertyName">Название поля</param>
        /// <param name="result">Поле (если найдено)</param>
        /// <returns>Найдено ли поле</returns>
        public static bool TryFindProperty(this Type type, string propertyName, [NotNullWhen(true)] out PropertyInfo? result)
        {
            result = type.FindProperty(propertyName);
            return result != null;
        }

        /// <summary>
        /// Найти метод в типе
        /// </summary>
        /// <param name="type">Тип для поиска метода</param>
        /// <param name="predicate">Условие отбора</param>
        /// <returns>Информация о методе (если найден)</returns>
        public static MethodInfo? FindMethod(this Type type, Predicate<MethodInfo> predicate)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type), "Тип не указан");
            }
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate), "Условие не указано");
            }

            foreach (var method in type.GetMethods())
            {
                if (predicate(method))
                {
                    return method;
                }
            }

            return null;
        }
        /// <summary>
        /// Найти метод в типе
        /// </summary>
        /// <param name="type">Тип для поиска метода</param>
        /// <param name="methodName">Название искомого метода</param>
        /// <returns>Информация о методе (если найден)</returns>
        public static MethodInfo? FindMethod(this Type type, string methodName)
        {
            return type.FindMethod(M => M.Name == methodName);
        }
        /// <summary>
        /// Попытаться найти метод в типе
        /// </summary>
        /// <param name="type">Тип в котором будет осуществлён поиск</param>
        /// <param name="predicate">Условие отбора</param>
        /// <param name="result">Информация о методе (если найден)</param>
        /// <returns>Найден ли метод</returns>
        public static bool TryFindMethod(this Type type, Predicate<MethodInfo> predicate, [NotNullWhen(true)] out MethodInfo? result)
        {
            result = type.FindMethod(predicate);
            return result != null;
        }
        /// <summary>
        /// Попытаться найти метод в типе
        /// </summary>
        /// <param name="type">Тип в котором будет осуществлён поиск</param>
        /// <param name="methodName">Название метода</param>
        /// <param name="result">Информация о методе (если найден)</param>
        /// <returns>Найден ли метод</returns>
        public static bool TryFindMethod(this Type type, string methodName, [NotNullWhen(true)] out MethodInfo? result)
        {
            result = type.FindMethod(methodName);
            return result != null;
        }

        /// <summary>
        /// Вызвать метод
        /// </summary>
        /// <typeparam name="T">Тип выводимых данных</typeparam>
        /// <param name="method">Вызываемый метод</param>
        /// <param name="instance">Экземпляр объекта (для не статичных методов)</param>
        /// <param name="parameters">Параметры метода</param>
        /// <returns>Вывод метода</returns>
        /// <exception cref="ArgumentNullException">Метод не указан</exception>
        public static T Invoke<T>(this MethodInfo method, object? instance, params object[] parameters)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method), "Метод не указан");
            }

            return (T)method.Invoke(instance, parameters);
        }
        /// <summary>
        /// Вызвать метод
        /// </summary>
        /// <param name="method">Вызываемый метод</param>
        /// <param name="Instance">Экземпляр объекта (для не статичных методов)</param>
        /// <param name="parameters">Параметры метода</param>
        /// <exception cref="ArgumentNullException">Метод не указан</exception>
        public static void Invoke(this MethodInfo method, object? Instance, params object[] parameters)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method), "Метод не указан");
            }

            method.Invoke(Instance, parameters);
        }

        /// <summary>
        /// Вызвать статичный метод
        /// </summary>
        /// <typeparam name="T">Тип возвращаемых данных</typeparam>
        /// <param name="type">Тип в котором будет осуществлён поиск метода</param>
        /// <param name="methodName">Название вызываемого метода</param>
        /// <param name="parameters">Параметры метода</param>
        /// <returns>Вывод метода</returns>
        /// <exception cref="ArgumentNullException">Тип не указан</exception>
        /// <exception cref="UndefinedMethodException">Метод не найден</exception>
        public static T CallStatic<T>(this Type type, string methodName, params object[] parameters)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type), "Тип не указан");
            }

            MethodInfo? method = type.FindMethod(methodName);

            return method == null ? throw new UndefinedMethodException(methodName, type) : (T)method.Invoke(null, parameters);
        }
        /// <summary>
        /// Вызвать статичный метод
        /// </summary>
        /// <param name="type">Тип в котором будет осуществлён поиск метода</param>
        /// <param name="methodName">Название вызываемого метода</param>
        /// <param name="parameters">Параметры метода</param>
        /// <exception cref="ArgumentNullException">Тип не указан</exception>
        /// <exception cref="UndefinedMethodException">Метод не найден</exception>
        public static void CallStatic(this Type type, string methodName, params object[] parameters)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type), "Тип не указан");
            }

            MethodInfo? method = type.FindMethod(methodName) ?? throw new UndefinedMethodException(methodName, type);
            method.Invoke(null, parameters);
        }
        /// <summary>
        /// Вызвать метод
        /// </summary>
        /// <typeparam name="T">Тип возвращаемых данных</typeparam>
		/// <param name="instance">Экземпляр объекта в котором будет вызван метод</param>
        /// <param name="type">Тип в котором будет осуществлён поиск метода</param>
        /// <param name="methodName">Название вызываемого метода</param>
        /// <param name="parameters">Параметры метода</param>
        /// <returns>Вывод метода</returns>
        /// <exception cref="ArgumentNullException">Тип не указан</exception>
        /// <exception cref="UndefinedMethodException">Метод не найден</exception>
        public static T Call<T>(this Type type, object instance, string methodName, params object[] parameters)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type), "Тип не указан");
            }

            MethodInfo? method = type.FindMethod(methodName);

            return method == null ? throw new UndefinedMethodException(methodName, type) : (T)method.Invoke(instance, parameters);
        }
        /// <summary>
        /// Вызвать метод
        /// </summary>
        /// <param name="instance">Экземпляр объекта в котором будет вызван метод</param>
        /// <param name="type">Тип в котором будет осуществлён поиск метода</param>
        /// <param name="methodName">Название вызываемого метода</param>
        /// <param name="parameters">Параметры метода</param>
        /// <exception cref="ArgumentNullException">Тип не указан</exception>
        /// <exception cref="UndefinedMethodException">Метод не найден</exception>
        public static void Call(this Type type, object instance, string methodName, params object[] parameters)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type), "Тип не указан");
            }

            MethodInfo? method = type.FindMethod(methodName) ?? throw new UndefinedMethodException(methodName, type);
            method.Invoke(instance, parameters);
        }

        /// <summary>
        /// Проверить является ли тип стандартной структурой
        /// </summary>
        /// <param name="obj">Проверяемый тип</param>
        /// <returns>Является ли тип стандартной структурой</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool IsStandardStruct(this Type obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("Тип не указан", nameof(obj));
            }
            if (!obj.IsValueType || !obj.IsPrimitive)
            {
                return false;
            }

            return obj.Namespace == "System" || obj.Namespace.StartsWith("System.");
        }

        /// <summary>
        /// Получить копию объекта (копируются только публичные доступные для чтения/записи поля)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">Копируемый объект</param>
        /// <param name="deepCopy">Делать ли копию значений</param>
        /// <param name="ctorArgument">Аргументы конструктора</param>
        /// <returns>Копия объекта</returns>
        public static T SoftCopy<T>(this T obj, bool deepCopy = true, params object[]? ctorArgument)
        {
            return obj.SoftCopy(deepCopy, ObjectCopyMaker.Instance, null);
        }
        /// <summary>
        /// Получить копию объекта (копируются только публичные доступные для чтения/записи поля)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">Копируемый объект</param>
        /// <param name="deepCopy">Делать ли копию значений</param>
        /// <param name="ctorArgument">Аргументы конструктора</param>
        /// <param name="copyMaker">Создатель копий</param>
        /// <returns>Копия объекта</returns>
        public static T SoftCopy<T>(this T obj, bool deepCopy, ObjectCopyMaker copyMaker, params object[]? ctorArgument)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }
            if (copyMaker == null)
            {
                throw new ArgumentNullException(nameof(copyMaker));
            }

            return (T)copyMaker.CreateCopy(obj, deepCopy, ctorArgument);
        }
        /// <summary>
        /// Скопировать значения из одного объекта в другой
        /// </summary>
        /// <param name="obj">Копируемый объект</param>
        /// <param name="destination">Объект в который будут вставлены значения</param>
        /// <param name="deepCopy">Делать ли копию значений</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void SoftCopyTo(this object obj, ref object destination, bool deepCopy = true)
        {
            obj.SoftCopyTo(ref destination, ObjectCopyMaker.Instance, deepCopy);
        }
        /// <summary>
        /// Скопировать значения из одного объекта в другой
        /// </summary>
        /// <param name="obj">Копируемый объект</param>
        /// <param name="destination">Объект в который будут вставлены значения</param>
        /// <param name="deepCopy">Делать ли копию значений</param>
        /// <param name="copyMaker">Создатель копий</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void SoftCopyTo(this object obj, ref object destination, ObjectCopyMaker copyMaker, bool deepCopy = true)
        {
            if (copyMaker == null)
            {
                throw new ArgumentNullException(nameof(copyMaker));
            }

            copyMaker.CopyTo(obj, ref destination, deepCopy);
        }

        /// <summary>
        /// Получить Enum из массива байтов
        /// </summary>
        /// <typeparam name="T">Enum тип</typeparam>
        /// <param name="bytes">Массив байтов, представляющий Enum значение</param>
        /// <returns>Enum значение</returns>
        /// <exception cref="NotSupportedException"></exception>
        public static T BytesToEnum<T>(byte[] bytes) where T : Enum
        {
            if (bytes == null)
            {
                throw new ArgumentNullException(nameof(bytes));
            }

            Type underlyingType = Enum.GetUnderlyingType(typeof(T));

            if (underlyingType == typeof(int))
                return (T)(object)BitConverter.ToInt32(bytes, 0);
            else if (underlyingType == typeof(long))
                return (T)(object)BitConverter.ToInt64(bytes, 0);
            else if (underlyingType == typeof(short))
                return (T)(object)BitConverter.ToInt16(bytes, 0);
            else if (underlyingType == typeof(byte))
                return (T)(object)bytes[0];
            else if (underlyingType == typeof(uint))
                return (T)(object)BitConverter.ToUInt32(bytes, 0);
            else if (underlyingType == typeof(ulong))
                return (T)(object)BitConverter.ToUInt64(bytes, 0);
            else if (underlyingType == typeof(ushort))
                return (T)(object)BitConverter.ToUInt16(bytes, 0);
            else if (underlyingType == typeof(sbyte))
                return (T)(object)(sbyte)bytes[0];

            throw new NotSupportedException($"Тип {underlyingType} не поддерживается");
        }

    }
}