using System;
using System.ComponentModel;

namespace Acly.Requests
{
	/// <summary>
	/// Время ожидания запроса
	/// </summary>
	public struct Timeout : IEquatable<Timeout>, IEquatable<TimeSpan>, IEquatable<int>
	{
		/// <summary>
		/// Время ожидания запроса
		/// </summary>
		/// <param name="Milliseconds">Время в миллисекундах</param>
		public Timeout(int Milliseconds)
		{
			Time = TimeSpan.FromMilliseconds(Milliseconds);
			this.Milliseconds = Milliseconds;
		}
		/// <summary>
		/// Время ожидания запроса
		/// </summary>
		/// <param name="Seconds">Время в секундах</param>
		public Timeout(double Seconds)
		{
			Time = TimeSpan.FromSeconds(Seconds);
			Milliseconds = Convert.ToInt32(Seconds * 1000);
		}
		/// <summary>
		/// Время ожидания запроса
		/// </summary>
		/// <param name="Time">Время</param>
		public Timeout(TimeSpan Time)
		{
			this.Time = Time;
			Milliseconds = Convert.ToInt32(Time.TotalMilliseconds);
		}

		/// <summary>
		/// Время ожидания
		/// </summary>
		public TimeSpan Time { get; private set; }
		/// <summary>
		/// Время ожидания в миллисекундах
		/// </summary>
		public int Milliseconds { get; private set; }

		#region Операторы

#pragma warning disable CS1591
		public static bool operator ==(Timeout L, Timeout R)
		{
			return L.Milliseconds == R.Milliseconds;
		}
		public static bool operator !=(Timeout L, Timeout R)
		{
			return L.Milliseconds != R.Milliseconds;
		}

		public static bool operator ==(Timeout L, int R)
		{
			return L.Milliseconds == R;
		}
		public static bool operator !=(Timeout L, int R)
		{
			return L.Milliseconds != R;
		}

		public static bool operator ==(int L, Timeout R)
		{
			return L == R.Milliseconds;
		}
		public static bool operator !=(int L, Timeout R)
		{
			return L != R.Milliseconds;
		}

		public static bool operator ==(Timeout L, TimeSpan R)
		{
			return L.Time == R;
		}
		public static bool operator !=(Timeout L, TimeSpan R)
		{
			return L.Time != R;
		}

		public static bool operator ==(TimeSpan L, Timeout R)
		{
			return L == R.Time;
		}
		public static bool operator !=(TimeSpan L, Timeout R)
		{
			return L != R.Time;
		}
#pragma warning restore CS1591

		#endregion

		#region Дополнительно

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <param name="obj"><inheritdoc/></param>
		/// <returns><inheritdoc/></returns>
		public readonly override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}

			Type Type = obj.GetType();

			if (Type == typeof(Timeout))
			{
				return Milliseconds == ((Timeout)obj).Milliseconds;
			}
			else if (Type == typeof(TimeSpan))
			{
				return Time == (TimeSpan)obj;
			}
			else if (Type == typeof(int))
			{
				return Milliseconds == (int)obj;
			}

			return false;
		}
		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <param name="other"><inheritdoc/></param>
		/// <returns><inheritdoc/></returns>
		public readonly bool Equals(Timeout other)
		{
			return Milliseconds == other.Milliseconds;
		}
		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <param name="other"><inheritdoc/></param>
		/// <returns><inheritdoc/></returns>
		public readonly bool Equals(TimeSpan other)
		{
			return Time == other;
		}
		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <param name="other"><inheritdoc/></param>
		/// <returns><inheritdoc/></returns>
		public readonly bool Equals(int other)
		{
			return Milliseconds == other;
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <returns><inheritdoc/></returns>
		public override int GetHashCode()
		{
			HashCode Code = new();
			Code.Add(Milliseconds);
			Code.Add(Time);

			return Code.ToHashCode();
		}

		#endregion

		/// <summary>
		/// Бесконечное время ожидания
		/// </summary>
		public static readonly Timeout Infinity = new(-1);
	}
}
