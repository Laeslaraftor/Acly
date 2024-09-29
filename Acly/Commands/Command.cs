using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Acly.Commands
{
	/// <summary>
	/// Команда
	/// </summary>
	[Serializable]
	public struct Command : IEquatable<Command>
	{
		/// <summary>
		/// Команда
		/// </summary>
		/// <param name="Value">Строка-команда</param>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="ArgumentException"></exception>
		public Command(string Value)
		{
			if (Value == null)
			{
				throw new ArgumentNullException(nameof(Value), "Команда не указана");
			}
			if (Value.Length == 0)
			{
				throw new ArgumentException("Команда не указана");
			}
			if (Value[0] != '/')
			{
				throw new ArgumentException("Команда должна начинаться с /");
			}

			Name = "";
			Parameters = new(Array.Empty<string>());
			Parse(Value);
		}

		/// <summary>
		/// Название команды
		/// </summary>
		public string Name { get; private set; }
		/// <summary>
		/// Параметры команды
		/// </summary>
		public ReadOnlyCollection<string> Parameters { get; private set; }

		#region Установка

		private void Parse(string Value)
		{
			List<string> Parts = new();
			string CurrentPart = "";
			bool NowText = false;
			bool IgnoreNext = false;
			bool SuperText = false;

			Value = Value[1..];

			foreach (var Symbol in Value)
			{
				if (Symbol != '"' && !NowText && SuperText)
				{
					SuperText = false;
				}
				if (Symbol == '\\' && !IgnoreNext && !SuperText)
				{
					IgnoreNext = true;
					continue;
				}
				else if (Symbol == '"' && !IgnoreNext)
				{
					if (NowText)
					{
						SuperText = false;
					}

					NowText = !NowText;
					continue;
				}
				else if (Symbol == ' ' && !NowText)
				{
					Parts.Add(CurrentPart.Trim());
					CurrentPart = "";
					continue;
				}
				else if (Symbol == '@' && !NowText)
				{
					SuperText = true;
					continue;
				}

				CurrentPart += Symbol;
				IgnoreNext = false;
			}

			if (CurrentPart.Length > 0)
			{
				Parts.Add(CurrentPart);
			}
			if (Parts.Count == 0)
			{
				return;
			}

			Name = Parts[0];
			Parts.RemoveAt(0);
			Parameters = new(Parts);
		}

		#endregion

		#region Операторы

		/// <summary>
		/// Равенство команд
		/// </summary>
		/// <param name="l">Команда 1</param>
		/// <param name="r">Команда 2</param>
		/// <returns>Равны ли команды</returns>
		public static bool operator ==(Command l, Command r)
		{
			if (l == null && r == null)
			{
				return true;
			}
			if (l == null || r == null)
			{
				return false;
			}

			bool Names = l.Name == r.Name;
			bool Params = l.Parameters.Count == r.Parameters.Count;

			if (Params)
			{
				for (int i = 0; i < l.Parameters.Count; i++)
				{
					if (l.Parameters[i] != r.Parameters[i])
					{
						Params = false;
						break;
					}
				}
			}

			return Names && Params;
		}
		/// <summary>
		/// Неравенство команд
		/// </summary>
		/// <param name="l">Команда 1</param>
		/// <param name="r">Команда 2</param>
		/// <returns>Неравны ли команды</returns>
		public static bool operator !=(Command l, Command r)
		{
			return !(l == r);
		}

		#endregion

		#region Управление

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <returns><inheritdoc/></returns>
		public readonly override string ToString()
		{
			string Result = "Команда: " + Name;

			if (Parameters.Count > 0)
			{
				Result += "; Параметры: ";
				string Params = "";

				foreach (var Param in Parameters)
				{
					if (Params.Length != 0)
					{
						Params += ", ";
					}

					Params += Param;
				}

				Result += Params;
			}

			return Result;
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <param name="obj"><inheritdoc/></param>
		/// <returns><inheritdoc/></returns>
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (obj.GetType() != typeof(Command))
			{
				return false;
			}

			return this == ((Command)obj);
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <returns><inheritdoc/></returns>
		public override int GetHashCode()
		{
			HashCode Code = new();
			Code.Add(Name);
			Code.Add(Parameters);

			return Code.ToHashCode();
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <param name="other">Команда</param>
		/// <returns>Равны ли команды</returns>
		public bool Equals(Command other)
		{
			return this == other;
		}

		#endregion

		#region Статика

		/// <summary>
		/// Попытаться создать команду
		/// </summary>
		/// <param name="Value">Строка-команда</param>
		/// <param name="Result">Команда (если не удалось создать, то пусто)</param>
		/// <returns>Удалось ли создать команду</returns>
		public static bool TryParse(string Value, out Command? Result)
		{
			Result = null;

			try
			{
				Result = new(Value);
				return true;
			}
			catch
			{
				return false;
			}
		}

		#endregion
	}
}
