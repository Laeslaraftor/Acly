using System;

namespace Acly
{
	/// <summary>
	/// Пометить объект как реализация <see cref="ILogger"/>
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class LoggerImplementation : Attribute
	{
	}
}
