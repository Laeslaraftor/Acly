using System;
using System.Collections.ObjectModel;

namespace Acly.Commands
{
	/// <summary>
	/// Шаблон команды
	/// </summary>
	[Serializable]
	public class CommandTemplate : IEquatable<Command>
	{
		/// <summary>
		/// Создать шаблон команды без параметров
		/// </summary>
		/// <param name="Name">Название команды</param>
		public CommandTemplate(string Name)
		{
			if (Name == null)
			{
				throw new ArgumentNullException(nameof(Name), "Название команды не указано");
			}

			this.Name = Name;
			Parameters = new(Array.Empty<string>());
		}
		/// <summary>
		/// Создать шаблон команды
		/// </summary>
		/// <param name="Name">Название команды</param>
		/// <param name="Parameters">Параметры команды</param>
		public CommandTemplate(string Name, params string[] Parameters)
		{
			if (Name == null)
			{
				throw new ArgumentNullException(nameof(Name), "Название команды не указано");
			}
			if (Parameters == null)
			{
				throw new ArgumentNullException(nameof(Parameters), "Параметры команды не указаны");
			}

			this.Name = Name;
			this.Parameters = new(Parameters);
		}

		/// <summary>
		/// Название команды
		/// </summary>
		public string Name { get; private set; }
		/// <summary>
		/// Параметры команды
		/// </summary>
		public ReadOnlyCollection<string> Parameters { get; private set; }

		

		#region Управление

		/// <summary>
		/// Получить шаблон команды
		/// </summary>
		/// <returns>Шаблон команды</returns>
		public override string ToString()
		{
			string Result = "/" + Name;

			if (Parameters.Count > 0)
			{
				foreach (var Param in Parameters)
				{
					Result += " [" + Param + "]";
				}
			}

			return Result;
		}
		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <param name="other"><inheritdoc/></param>
		/// <returns><inheritdoc/></returns>
		public bool Equals(Command other)
		{
			if (other == null)
			{
				return false;
			}

			return Name == other.Name;
		}

		#endregion
	}
}
