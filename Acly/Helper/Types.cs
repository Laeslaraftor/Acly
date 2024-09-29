using System;
using System.Collections.Generic;
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
        /// Найти метод в типе
        /// </summary>
        /// <param name="Type">Тип для поиска метода</param>
        /// <param name="MethodName">Название искомого метода</param>
        /// <returns>Информация о методе (если найден)</returns>
        public static MethodInfo? FindMethod(this Type Type, string MethodName)
        {
            if (Type == null)
            {
                throw new ArgumentNullException(nameof(Type), "Тип не указан");
            }

            foreach (var Method in Type.GetMethods())
            {
                if (Method.Name == MethodName)
                {
                    return Method;
                }
            }

            return null;
        }
        /// <summary>
        /// Попытаться найти метод в типе
        /// </summary>
        /// <param name="Type">Тип в котором будет осуществлён поиск</param>
        /// <param name="MethodName">Название метода</param>
        /// <param name="Result">Информация о методе (если найден)</param>
        /// <returns>Найден ли метод</returns>
        public static bool TryFindMethod(this Type Type, string MethodName, out MethodInfo? Result)
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
    }
}