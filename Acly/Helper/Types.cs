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
			List<Type> Result = new();

			await Task.Run(() =>
			{
				Assembly[] Assemblies = AppDomain.CurrentDomain.GetAssemblies();

				foreach (var Assembly in Assemblies)
				{
					foreach (Type Type in Assembly.GetTypes())
					{
						if (Type.GetCustomAttributes(typeof(T), true).Length > 0)
						{
							Result.Add(Type);
						}
					}
				}
			});

			return Result;
		}
		/// <summary>
		/// Определить реализует ли тип указанный интерфейс
		/// </summary>
		/// <typeparam name="T">Интерфейс</typeparam>
		/// <returns>Реализует ли тип указанный интерфейс</returns>
		/// <exception cref="ArgumentNullException">Ссылка на тип не указывает на экземпляр объекта</exception>
		public static bool IsImplementsInterface<T>(this Type Type)
		{
			if (Type == null)
			{
				throw new ArgumentNullException(nameof(Type) + " не указан");
			}

			Type[] Interfaces = Type.GetInterfaces();
			Type RequestInterface = typeof(T);

			foreach (var Interface in Interfaces)
			{
				if (Interface == RequestInterface)
				{
					return true;
				}
			}

			return false;
		}

        /// <summary>
        /// Найти поле в типе
        /// </summary>
        /// <param name="Type">Тип в котором надо найти поле</param>
        /// <param name="Condition">Условие отбора</param>
        /// <returns>Поле (если найдено)</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static PropertyInfo? FindProperty(this Type Type, Predicate<PropertyInfo> Condition)
        {
            if (Type == null)
            {
                throw new ArgumentNullException(nameof(Type), "Тип не указан");
            }
            if (Condition == null)
            {
                throw new ArgumentNullException(nameof(Condition), "Условие не указано");
            }

            foreach (var Property in Type.GetProperties())
            {
                if (Condition(Property))
                {
                    return Property;
                }
            }

            return null;
        }
        /// <summary>
        /// Найти поле в типе
        /// </summary>
        /// <param name="Type">Тип в котором надо найти поле</param>
        /// <param name="PropertyName">Название поля</param>
        /// <returns>Поле (если найдено)</returns>
        public static PropertyInfo? FindProperty(this Type Type, string PropertyName)
        {
            return Type.FindProperty(P => P.Name == PropertyName);
        }
        /// <summary>
        /// Попытаться найти поле в типе
        /// </summary>
        /// <param name="Type">Тип в котором надо попытаться найти поле</param>
        /// <param name="Condition">Условие отбора</param>
        /// <param name="Result">Поле (если найдено)</param>
        /// <returns>Найдено ли поле</returns>
        public static bool TryFindProperty(this Type Type, Predicate<PropertyInfo> Condition, [NotNullWhen(true)] out PropertyInfo? Result)
        {
            Result = Type.FindProperty(Condition);
            return Result != null;
        }
        /// <summary>
        /// Попытаться найти поле в типе
        /// </summary>
        /// <param name="Type">Тип в котором надо попытаться найти поле</param>
        /// <param name="PropertyName">Название поля</param>
        /// <param name="Result">Поле (если найдено)</param>
        /// <returns>Найдено ли поле</returns>
        public static bool TryFindProperty(this Type Type, string PropertyName, [NotNullWhen(true)] out PropertyInfo? Result)
        {
            Result = Type.FindProperty(PropertyName);
            return Result != null;
        }

        /// <summary>
        /// Найти метод в типе
        /// </summary>
        /// <param name="Type">Тип для поиска метода</param>
        /// <param name="Condition">Условие отбора</param>
        /// <returns>Информация о методе (если найден)</returns>
        public static MethodInfo? FindMethod(this Type Type, Predicate<MethodInfo> Condition)
        {
            if (Type == null)
            {
                throw new ArgumentNullException(nameof(Type), "Тип не указан");
            }
            if (Condition == null)
            {
                throw new ArgumentNullException(nameof(Condition), "Условие не указано");
            }

            foreach (var Method in Type.GetMethods())
            {
                if (Condition(Method))
                {
                    return Method;
                }
            }

            return null;
        }
        /// <summary>
        /// Найти метод в типе
        /// </summary>
        /// <param name="Type">Тип для поиска метода</param>
        /// <param name="MethodName">Название искомого метода</param>
        /// <returns>Информация о методе (если найден)</returns>
        public static MethodInfo? FindMethod(this Type Type, string MethodName)
        {
            return Type.FindMethod(M => M.Name == MethodName);
        }
        /// <summary>
        /// Попытаться найти метод в типе
        /// </summary>
        /// <param name="Type">Тип в котором будет осуществлён поиск</param>
        /// <param name="Condition">Условие отбора</param>
        /// <param name="Result">Информация о методе (если найден)</param>
        /// <returns>Найден ли метод</returns>
        public static bool TryFindMethod(this Type Type, Predicate<MethodInfo> Condition, [NotNullWhen(true)] out MethodInfo? Result)
        {
            Result = Type.FindMethod(Condition);
            return Result != null;
        }
        /// <summary>
        /// Попытаться найти метод в типе
        /// </summary>
        /// <param name="Type">Тип в котором будет осуществлён поиск</param>
        /// <param name="MethodName">Название метода</param>
        /// <param name="Result">Информация о методе (если найден)</param>
        /// <returns>Найден ли метод</returns>
        public static bool TryFindMethod(this Type Type, string MethodName, [NotNullWhen(true)] out MethodInfo? Result)
        {
            Result = Type.FindMethod(MethodName);
            return Result != null;
        }

        /// <summary>
        /// Вызвать метод
        /// </summary>
        /// <typeparam name="T">Тип выводимых данных</typeparam>
        /// <param name="Method">Вызываемый метод</param>
        /// <param name="Instance">Экземпляр объекта (для не статичных методов)</param>
        /// <param name="Parameters">Параметры метода</param>
        /// <returns>Вывод метода</returns>
        /// <exception cref="ArgumentNullException">Метод не указан</exception>
        public static T Invoke<T>(this MethodInfo Method, object? Instance, params object[] Parameters)
        {
            if (Method == null)
            {
                throw new ArgumentNullException(nameof(Method), "Метод не указан");
            }

            return (T)Method.Invoke(Instance, Parameters);
        }
        /// <summary>
        /// Вызвать метод
        /// </summary>
        /// <param name="Method">Вызываемый метод</param>
        /// <param name="Instance">Экземпляр объекта (для не статичных методов)</param>
        /// <param name="Parameters">Параметры метода</param>
        /// <exception cref="ArgumentNullException">Метод не указан</exception>
        public static void Invoke(this MethodInfo Method, object? Instance, params object[] Parameters)
        {
            if (Method == null)
            {
                throw new ArgumentNullException(nameof(Method), "Метод не указан");
            }

            Method.Invoke(Instance, Parameters);
        }

        /// <summary>
        /// Вызвать статичный метод
        /// </summary>
        /// <typeparam name="T">Тип возвращаемых данных</typeparam>
        /// <param name="Type">Тип в котором будет осуществлён поиск метода</param>
        /// <param name="MethodName">Название вызываемого метода</param>
        /// <param name="Parameters">Параметры метода</param>
        /// <returns>Вывод метода</returns>
        /// <exception cref="ArgumentNullException">Тип не указан</exception>
        /// <exception cref="UndefinedMethodException">Метод не найден</exception>
        public static T CallStatic<T>(this Type Type, string MethodName, params object[] Parameters)
		{
            if (Type == null)
            {
                throw new ArgumentNullException(nameof(Type), "Тип не указан");
            }

            MethodInfo? Method = Type.FindMethod(MethodName);

            if (Method == null)
			{
				throw new UndefinedMethodException(MethodName, Type);
			}

			return (T)Method.Invoke(null, Parameters);
        }
        /// <summary>
        /// Вызвать статичный метод
        /// </summary>
        /// <param name="Type">Тип в котором будет осуществлён поиск метода</param>
        /// <param name="MethodName">Название вызываемого метода</param>
        /// <param name="Parameters">Параметры метода</param>
        /// <exception cref="ArgumentNullException">Тип не указан</exception>
        /// <exception cref="UndefinedMethodException">Метод не найден</exception>
        public static void CallStatic(this Type Type, string MethodName, params object[] Parameters)
        {
            if (Type == null)
            {
                throw new ArgumentNullException(nameof(Type), "Тип не указан");
            }

            MethodInfo? Method = Type.FindMethod(MethodName);

            if (Method == null)
            {
                throw new UndefinedMethodException(MethodName, Type);
            }

            Method.Invoke(null, Parameters);
        }
        /// <summary>
        /// Вызвать метод
        /// </summary>
        /// <typeparam name="T">Тип возвращаемых данных</typeparam>
		/// <param name="Instance">Экземпляр объекта в котором будет вызван метод</param>
        /// <param name="Type">Тип в котором будет осуществлён поиск метода</param>
        /// <param name="MethodName">Название вызываемого метода</param>
        /// <param name="Parameters">Параметры метода</param>
        /// <returns>Вывод метода</returns>
        /// <exception cref="ArgumentNullException">Тип не указан</exception>
        /// <exception cref="UndefinedMethodException">Метод не найден</exception>
        public static T Call<T>(this Type Type, object Instance, string MethodName, params object[] Parameters)
        {
            if (Type == null)
            {
                throw new ArgumentNullException(nameof(Type), "Тип не указан");
            }

            MethodInfo? Method = Type.FindMethod(MethodName);

            if (Method == null)
            {
                throw new UndefinedMethodException(MethodName, Type);
            }

            return (T)Method.Invoke(Instance, Parameters);
        }
        /// <summary>
        /// Вызвать метод
        /// </summary>
        /// <param name="Instance">Экземпляр объекта в котором будет вызван метод</param>
        /// <param name="Type">Тип в котором будет осуществлён поиск метода</param>
        /// <param name="MethodName">Название вызываемого метода</param>
        /// <param name="Parameters">Параметры метода</param>
        /// <exception cref="ArgumentNullException">Тип не указан</exception>
        /// <exception cref="UndefinedMethodException">Метод не найден</exception>
        public static void Call(this Type Type, object Instance, string MethodName, params object[] Parameters)
        {
            if (Type == null)
            {
                throw new ArgumentNullException(nameof(Type), "Тип не указан");
            }

            MethodInfo? Method = Type.FindMethod(MethodName);

            if (Method == null)
            {
                throw new UndefinedMethodException(MethodName, Type);
            }

            Method.Invoke(Instance, Parameters);
        }

        /// <summary>
        /// Проверить является ли тип стандартной структурой
        /// </summary>
        /// <param name="ObjType">Проверяемый тип</param>
        /// <returns>Является ли тип стандартной структурой</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool IsStandardStruct(this Type ObjType)
        {
            if (ObjType == null)
            {
                throw new ArgumentNullException("Тип не указан", nameof(ObjType));
            }
            if (!ObjType.IsValueType || !ObjType.IsPrimitive)
            {
                return false;
            }

            return ObjType.Namespace == "System" || ObjType.Namespace.StartsWith("System.");
        }

        /// <summary>
        /// Получить копию объекта (копируются только публичные доступные для чтения/записи поля)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Object">Копируемый объект</param>
        /// <param name="DeepCopy">Делать ли копию значений</param>
        /// <param name="CtorArgument">Аргументы конструктора</param>
        /// <returns>Копия объекта</returns>
        public static T SoftCopy<T>(this T Object, bool DeepCopy = true, params object[]? CtorArgument)
        {
            return Object.SoftCopy(DeepCopy, ObjectCopyMaker.Instance, null);
        }
        /// <summary>
        /// Получить копию объекта (копируются только публичные доступные для чтения/записи поля)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Object">Копируемый объект</param>
        /// <param name="DeepCopy">Делать ли копию значений</param>
        /// <param name="CtorArgument">Аргументы конструктора</param>
        /// <param name="CopyMaker">Создатель копий</param>
        /// <returns>Копия объекта</returns>
        public static T SoftCopy<T>(this T Object, bool DeepCopy, ObjectCopyMaker CopyMaker, params object[]? CtorArgument)
        {
            if (Object == null)
            {
                throw new ArgumentNullException(nameof(Object));
            }
            if (CopyMaker == null)
            {
                throw new ArgumentNullException(nameof(CopyMaker));
            }

            return (T)CopyMaker.CreateCopy(Object, DeepCopy, CtorArgument);
        }
        /// <summary>
        /// Скопировать значения из одного объекта в другой
        /// </summary>
        /// <param name="Object">Копируемый объект</param>
        /// <param name="Destination">Объект в который будут вставлены значения</param>
        /// <param name="DeepCopy">Делать ли копию значений</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void SoftCopyTo(this object Object, ref object Destination, bool DeepCopy = true)
        {
            Object.SoftCopyTo(ref Destination, ObjectCopyMaker.Instance, DeepCopy);
        }
        /// <summary>
        /// Скопировать значения из одного объекта в другой
        /// </summary>
        /// <param name="Object">Копируемый объект</param>
        /// <param name="Destination">Объект в который будут вставлены значения</param>
        /// <param name="DeepCopy">Делать ли копию значений</param>
        /// <param name="CopyMaker">Создатель копий</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void SoftCopyTo(this object Object, ref object Destination, ObjectCopyMaker CopyMaker, bool DeepCopy = true)
        {
            if (CopyMaker == null)
            {
                throw new ArgumentNullException(nameof(CopyMaker));
            }

            CopyMaker.CopyTo(Object, ref Destination, DeepCopy);
        }
    }
}