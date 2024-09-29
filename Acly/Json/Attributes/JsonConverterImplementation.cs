using System;

namespace Acly.JsonData
{
	/// <summary>
	/// Пометить класс как реализация <see cref="IJsonConverter"/>
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class JsonConverterImplementationAttribute : Attribute
	{
	}
}
