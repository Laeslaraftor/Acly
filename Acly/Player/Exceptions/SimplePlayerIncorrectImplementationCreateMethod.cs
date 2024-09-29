using System;

namespace Acly.Player
{
	/// <summary>
	/// Исключение, вызывающееся при неправильном методе получения реализации <see cref="ISimplePlayer"/>
	/// </summary>
	public sealed class SimplePlayerIncorrectImplementationCreateMethod : Exception
	{
		/// <summary>
		/// Вызвать исключение о том что метод получения экземпляра реализации имеет какие-то параметры
		/// </summary>
		/// <param name="Type">Тип объекта с реализацией</param>
		/// <param name="Method">Метод получения экземпляра реализации</param>
		public SimplePlayerIncorrectImplementationCreateMethod(Type Type, string Method)
			: base(string.Format(_MessageParameters, Type, Method))
		{
		}
		/// <summary>
		/// Вызвать исключение о том что метод получения экземпляра реализации ничего не возвращает
		/// </summary>
		/// <param name="Type">Тип объекта с реализацией</param>
		/// <param name="Method">Метод получения экземпляра реализации</param>
		/// <param name="MustReturn">Необходимый тип данных</param>
		public SimplePlayerIncorrectImplementationCreateMethod(Type Type, string Method, string MustReturn)
			: base(string.Format(_MessageNoReturn, Type, Method, MustReturn))
		{
		}
		/// <summary>
		/// Вызвать исключение о том что метод получения экземпляра реализации возвращает не тот тип данных
		/// </summary>
		/// <param name="Type">Тип объекта с реализацией</param>
		/// <param name="Method">Метод получения экземпляра реализации</param>
		/// <param name="Returning">Возвращаемый тип данных</param>
		/// <param name="MustReturn">Необходимый тип данных</param>
		public SimplePlayerIncorrectImplementationCreateMethod(Type Type, string Method, string Returning, string MustReturn)
			: base(string.Format(_MessageIncorrectReturn, Type, Method, Returning, MustReturn))
		{
		}

		private const string _MessageParameters = "Для типа {0} был отмечен метод {1}, как метод создания экземпляра, но он имеет параметры." +
			" Такой метод не может иметь каких-либо параметров";
		private const string _MessageNoReturn = "Для типа {0} был отмечен метод {1}, как метод создания экземпляра, но он ничего не возвращает." +
			" Такой метод должен возвращать {2}";
		private const string _MessageIncorrectReturn = "Для типа {0} был отмечен метод {1}, как метод создания экземпляра, но он возвращает {2}." +
			" Такой метод должен возвращать {3}";
	}
}
