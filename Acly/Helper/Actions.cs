using System;

namespace Acly
{
	public static partial class Helper
	{
		/// <summary>
		/// Попытаться выполнить <see cref="Action"/>. <paramref name="Action"/> может принимать NULL
		/// </summary>
		/// <typeparam name="T">Тип аргумента</typeparam>
		/// <param name="Action">Действие, которое необходимо попытаться выполнить</param>
		/// <param name="Arg">Аргумент, принимаемый действием</param>
		/// <param name="InvokeException"></param>
		/// <returns>Успешно ли выполнено действие</returns>
		public static bool TryInvoke<T>(this Action<T> Action, T Arg, out Exception? InvokeException)
		{
			try
			{
				Action?.Invoke(Arg);
			}
			catch (Exception Error)
			{
				InvokeException = Error;
				return false;
			}

			InvokeException = null;
			return true;
		}
		/// <summary>
		/// Попытаться выполнить <see cref="Action"/>. <paramref name="Action"/> может принимать NULL
		/// </summary>
		/// <typeparam name="T1">Тип аргумента</typeparam>
		/// <typeparam name="T2">Тип аргумента</typeparam>
		/// <param name="Action">Действие, которое необходимо попытаться выполнить</param>
		/// <param name="Arg1">Аргумент, принимаемый действием</param>
		/// <param name="Arg2">Аргумент, принимаемый действием</param>
		/// <param name="InvokeException"></param>
		/// <returns>Успешно ли выполнено действие</returns>
		public static bool TryInvoke<T1, T2>(this Action<T1, T2> Action, T1 Arg1, T2 Arg2, out Exception? InvokeException)
		{
			try
			{
				Action?.Invoke(Arg1, Arg2);
			}
			catch (Exception Error)
			{
				InvokeException = Error;
				return false;
			}

			InvokeException = null;
			return true;
		}
		/// <summary>
		/// Попытаться выполнить <see cref="Action"/>. <paramref name="Action"/> может принимать NULL
		/// </summary>
		/// <typeparam name="T1">Тип аргумента</typeparam>
		/// <typeparam name="T2">Тип аргумента</typeparam>
		/// <typeparam name="T3">Тип аргумента</typeparam>
		/// <param name="Action">Действие, которое необходимо попытаться выполнить</param>
		/// <param name="Arg1">Аргумент, принимаемый действием</param>
		/// <param name="Arg2">Аргумент, принимаемый действием</param>
		/// <param name="Arg3">Аргумент, принимаемый действием</param>
		/// <param name="InvokeException"></param>
		/// <returns>Успешно ли выполнено действие</returns>
		public static bool TryInvoke<T1, T2, T3>(this Action<T1, T2, T3> Action, T1 Arg1, T2 Arg2, T3 Arg3, out Exception? InvokeException)
		{
			try
			{
				Action?.Invoke(Arg1, Arg2, Arg3);
			}
			catch (Exception Error)
			{
				InvokeException = Error;
				return false;
			}

			InvokeException = null;
			return true;
		}
		/// <summary>
		/// Попытаться выполнить <see cref="Action"/>. <paramref name="Action"/> может принимать NULL
		/// </summary>
		/// <typeparam name="T1">Тип аргумента</typeparam>
		/// <typeparam name="T2">Тип аргумента</typeparam>
		/// <typeparam name="T3">Тип аргумента</typeparam>
		/// <typeparam name="T4">Тип аргумента</typeparam>
		/// <param name="Action">Действие, которое необходимо попытаться выполнить</param>
		/// <param name="Arg1">Аргумент, принимаемый действием</param>
		/// <param name="Arg2">Аргумент, принимаемый действием</param>
		/// <param name="Arg3">Аргумент, принимаемый действием</param>
		/// <param name="Arg4">Аргумент, принимаемый действием</param>
		/// <param name="InvokeException"></param>
		/// <returns>Успешно ли выполнено действие</returns>
		public static bool TryInvoke<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> Action, T1 Arg1, T2 Arg2, T3 Arg3, T4 Arg4, out Exception? InvokeException)
		{
			try
			{
				Action?.Invoke(Arg1, Arg2, Arg3, Arg4);
			}
			catch (Exception Error)
			{
				InvokeException = Error;
				return false;
			}

			InvokeException = null;
			return true;
		}
		/// <summary>
		/// Попытаться выполнить <see cref="Action"/>. <paramref name="Action"/> может принимать NULL
		/// </summary>
		/// <typeparam name="T1">Тип аргумента</typeparam>
		/// <typeparam name="T2">Тип аргумента</typeparam>
		/// <typeparam name="T3">Тип аргумента</typeparam>
		/// <typeparam name="T4">Тип аргумента</typeparam>
		/// <typeparam name="T5">Тип аргумента</typeparam>
		/// <param name="Action">Действие, которое необходимо попытаться выполнить</param>
		/// <param name="Arg1">Аргумент, принимаемый действием</param>
		/// <param name="Arg2">Аргумент, принимаемый действием</param>
		/// <param name="Arg3">Аргумент, принимаемый действием</param>
		/// <param name="Arg4">Аргумент, принимаемый действием</param>
		/// <param name="Arg5">Аргумент, принимаемый действием</param>
		/// <param name="InvokeException"></param>
		/// <returns>Успешно ли выполнено действие</returns>
		public static bool TryInvoke<T1, T2, T3, T4, T5>(this Action<T1, T2, T3, T4, T5> Action, T1 Arg1, T2 Arg2, T3 Arg3, T4 Arg4, T5 Arg5, out Exception? InvokeException)
		{
			try
			{
				Action?.Invoke(Arg1, Arg2, Arg3, Arg4, Arg5);
			}
			catch (Exception Error)
			{
				InvokeException = Error;
				return false;
			}

			InvokeException = null;
			return true;
		}
		/// <summary>
		/// Попытаться выполнить <see cref="Action"/>. <paramref name="Action"/> может принимать NULL
		/// </summary>
		/// <typeparam name="T1">Тип аргумента</typeparam>
		/// <typeparam name="T2">Тип аргумента</typeparam>
		/// <typeparam name="T3">Тип аргумента</typeparam>
		/// <typeparam name="T4">Тип аргумента</typeparam>
		/// <typeparam name="T5">Тип аргумента</typeparam>
		/// <typeparam name="T6">Тип аргумента</typeparam>
		/// <param name="Action">Действие, которое необходимо попытаться выполнить</param>
		/// <param name="Arg1">Аргумент, принимаемый действием</param>
		/// <param name="Arg2">Аргумент, принимаемый действием</param>
		/// <param name="Arg3">Аргумент, принимаемый действием</param>
		/// <param name="Arg4">Аргумент, принимаемый действием</param>
		/// <param name="Arg5">Аргумент, принимаемый действием</param>
		/// <param name="Arg6">Аргумент, принимаемый действием</param>
		/// <param name="InvokeException"></param>
		/// <returns>Успешно ли выполнено действие</returns>
		public static bool TryInvoke<T1, T2, T3, T4, T5, T6>(this Action<T1, T2, T3, T4, T5, T6> Action, T1 Arg1, T2 Arg2, T3 Arg3, T4 Arg4, T5 Arg5, T6 Arg6, out Exception? InvokeException)
		{
			try
			{
				Action?.Invoke(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6);
			}
			catch (Exception Error)
			{
				InvokeException = Error;
				return false;
			}

			InvokeException = null;
			return true;
		}
		/// <summary>
		/// Попытаться выполнить <see cref="Action"/>. <paramref name="Action"/> может принимать NULL
		/// </summary>
		/// <typeparam name="T1">Тип аргумента</typeparam>
		/// <typeparam name="T2">Тип аргумента</typeparam>
		/// <typeparam name="T3">Тип аргумента</typeparam>
		/// <typeparam name="T4">Тип аргумента</typeparam>
		/// <typeparam name="T5">Тип аргумента</typeparam>
		/// <typeparam name="T6">Тип аргумента</typeparam>
		/// <typeparam name="T7">Тип аргумента</typeparam>
		/// <param name="Action">Действие, которое необходимо попытаться выполнить</param>
		/// <param name="Arg1">Аргумент, принимаемый действием</param>
		/// <param name="Arg2">Аргумент, принимаемый действием</param>
		/// <param name="Arg3">Аргумент, принимаемый действием</param>
		/// <param name="Arg4">Аргумент, принимаемый действием</param>
		/// <param name="Arg5">Аргумент, принимаемый действием</param>
		/// <param name="Arg6">Аргумент, принимаемый действием</param>
		/// <param name="Arg7">Аргумент, принимаемый действием</param>
		/// <param name="InvokeException"></param>
		/// <returns>Успешно ли выполнено действие</returns>
		public static bool TryInvoke<T1, T2, T3, T4, T5, T6, T7>(this Action<T1, T2, T3, T4, T5, T6, T7> Action, T1 Arg1, T2 Arg2, T3 Arg3, T4 Arg4, T5 Arg5, T6 Arg6, T7 Arg7, out Exception? InvokeException)
		{
			try
			{
				Action?.Invoke(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7);
			}
			catch (Exception Error)
			{
				InvokeException = Error;
				return false;
			}

			InvokeException = null;
			return true;
		}
		/// <summary>
		/// Попытаться выполнить <see cref="Action"/>. <paramref name="Action"/> может принимать NULL
		/// </summary>
		/// <typeparam name="T1">Тип аргумента</typeparam>
		/// <typeparam name="T2">Тип аргумента</typeparam>
		/// <typeparam name="T3">Тип аргумента</typeparam>
		/// <typeparam name="T4">Тип аргумента</typeparam>
		/// <typeparam name="T5">Тип аргумента</typeparam>
		/// <typeparam name="T6">Тип аргумента</typeparam>
		/// <typeparam name="T7">Тип аргумента</typeparam>
		/// <typeparam name="T8">Тип аргумента</typeparam>
		/// <param name="Action">Действие, которое необходимо попытаться выполнить</param>
		/// <param name="Arg1">Аргумент, принимаемый действием</param>
		/// <param name="Arg2">Аргумент, принимаемый действием</param>
		/// <param name="Arg3">Аргумент, принимаемый действием</param>
		/// <param name="Arg4">Аргумент, принимаемый действием</param>
		/// <param name="Arg5">Аргумент, принимаемый действием</param>
		/// <param name="Arg6">Аргумент, принимаемый действием</param>
		/// <param name="Arg7">Аргумент, принимаемый действием</param>
		/// <param name="Arg8">Аргумент, принимаемый действием</param>
		/// <param name="InvokeException"></param>
		/// <returns>Успешно ли выполнено действие</returns>
		public static bool TryInvoke<T1, T2, T3, T4, T5, T6, T7, T8>(this Action<T1, T2, T3, T4, T5, T6, T7, T8> Action, T1 Arg1, T2 Arg2, T3 Arg3, T4 Arg4, T5 Arg5, T6 Arg6, T7 Arg7, T8 Arg8, out Exception? InvokeException)
		{
			try
			{
				Action?.Invoke(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8);
			}
			catch (Exception Error)
			{
				InvokeException = Error;
				return false;
			}

			InvokeException = null;
			return true;
		}
		/// <summary>
		/// Попытаться выполнить <see cref="Action"/>. <paramref name="Action"/> может принимать NULL
		/// </summary>
		/// <typeparam name="T1">Тип аргумента</typeparam>
		/// <typeparam name="T2">Тип аргумента</typeparam>
		/// <typeparam name="T3">Тип аргумента</typeparam>
		/// <typeparam name="T4">Тип аргумента</typeparam>
		/// <typeparam name="T5">Тип аргумента</typeparam>
		/// <typeparam name="T6">Тип аргумента</typeparam>
		/// <typeparam name="T7">Тип аргумента</typeparam>
		/// <typeparam name="T8">Тип аргумента</typeparam>
		/// <typeparam name="T9">Тип аргумента</typeparam>
		/// <param name="Action">Действие, которое необходимо попытаться выполнить</param>
		/// <param name="Arg1">Аргумент, принимаемый действием</param>
		/// <param name="Arg2">Аргумент, принимаемый действием</param>
		/// <param name="Arg3">Аргумент, принимаемый действием</param>
		/// <param name="Arg4">Аргумент, принимаемый действием</param>
		/// <param name="Arg5">Аргумент, принимаемый действием</param>
		/// <param name="Arg6">Аргумент, принимаемый действием</param>
		/// <param name="Arg7">Аргумент, принимаемый действием</param>
		/// <param name="Arg8">Аргумент, принимаемый действием</param>
		/// <param name="Arg9">Аргумент, принимаемый действием</param>
		/// <param name="InvokeException"></param>
		/// <returns>Успешно ли выполнено действие</returns>
		public static bool TryInvoke<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> Action, T1 Arg1, T2 Arg2, T3 Arg3, T4 Arg4, T5 Arg5, T6 Arg6, T7 Arg7, T8 Arg8, T9 Arg9, out Exception? InvokeException)
		{
			try
			{
				Action?.Invoke(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9);
			}
			catch (Exception Error)
			{
				InvokeException = Error;
				return false;
			}

			InvokeException = null;
			return true;
		}
		/// <summary>
		/// Попытаться выполнить <see cref="Action"/>. <paramref name="Action"/> может принимать NULL
		/// </summary>
		/// <typeparam name="T1">Тип аргумента</typeparam>
		/// <typeparam name="T2">Тип аргумента</typeparam>
		/// <typeparam name="T3">Тип аргумента</typeparam>
		/// <typeparam name="T4">Тип аргумента</typeparam>
		/// <typeparam name="T5">Тип аргумента</typeparam>
		/// <typeparam name="T6">Тип аргумента</typeparam>
		/// <typeparam name="T7">Тип аргумента</typeparam>
		/// <typeparam name="T8">Тип аргумента</typeparam>
		/// <typeparam name="T9">Тип аргумента</typeparam>
		/// <typeparam name="T10">Тип аргумента</typeparam>
		/// <param name="Action">Действие, которое необходимо попытаться выполнить</param>
		/// <param name="Arg1">Аргумент, принимаемый действием</param>
		/// <param name="Arg2">Аргумент, принимаемый действием</param>
		/// <param name="Arg3">Аргумент, принимаемый действием</param>
		/// <param name="Arg4">Аргумент, принимаемый действием</param>
		/// <param name="Arg5">Аргумент, принимаемый действием</param>
		/// <param name="Arg6">Аргумент, принимаемый действием</param>
		/// <param name="Arg7">Аргумент, принимаемый действием</param>
		/// <param name="Arg8">Аргумент, принимаемый действием</param>
		/// <param name="Arg9">Аргумент, принимаемый действием</param>
		/// <param name="Arg10">Аргумент, принимаемый действием</param>
		/// <param name="InvokeException"></param>
		/// <returns>Успешно ли выполнено действие</returns>
		public static bool TryInvoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Action, T1 Arg1, T2 Arg2, T3 Arg3, T4 Arg4, T5 Arg5, T6 Arg6, T7 Arg7, T8 Arg8, T9 Arg9, T10 Arg10, out Exception? InvokeException)
		{
			try
			{
				Action?.Invoke(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10);
			}
			catch (Exception Error)
			{
				InvokeException = Error;
				return false;
			}

			InvokeException = null;
			return true;
		}
		/// <summary>
		/// Попытаться выполнить <see cref="Action"/>. <paramref name="Action"/> может принимать NULL
		/// </summary>
		/// <typeparam name="T1">Тип аргумента</typeparam>
		/// <typeparam name="T2">Тип аргумента</typeparam>
		/// <typeparam name="T3">Тип аргумента</typeparam>
		/// <typeparam name="T4">Тип аргумента</typeparam>
		/// <typeparam name="T5">Тип аргумента</typeparam>
		/// <typeparam name="T6">Тип аргумента</typeparam>
		/// <typeparam name="T7">Тип аргумента</typeparam>
		/// <typeparam name="T8">Тип аргумента</typeparam>
		/// <typeparam name="T9">Тип аргумента</typeparam>
		/// <typeparam name="T10">Тип аргумента</typeparam>
		/// <typeparam name="T11">Тип аргумента</typeparam>
		/// <param name="Action">Действие, которое необходимо попытаться выполнить</param>
		/// <param name="Arg1">Аргумент, принимаемый действием</param>
		/// <param name="Arg2">Аргумент, принимаемый действием</param>
		/// <param name="Arg3">Аргумент, принимаемый действием</param>
		/// <param name="Arg4">Аргумент, принимаемый действием</param>
		/// <param name="Arg5">Аргумент, принимаемый действием</param>
		/// <param name="Arg6">Аргумент, принимаемый действием</param>
		/// <param name="Arg7">Аргумент, принимаемый действием</param>
		/// <param name="Arg8">Аргумент, принимаемый действием</param>
		/// <param name="Arg9">Аргумент, принимаемый действием</param>
		/// <param name="Arg10">Аргумент, принимаемый действием</param>
		/// <param name="Arg11">Аргумент, принимаемый действием</param>
		/// <param name="InvokeException"></param>
		/// <returns>Успешно ли выполнено действие</returns>
		public static bool TryInvoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> Action, T1 Arg1, T2 Arg2, T3 Arg3, T4 Arg4, T5 Arg5, T6 Arg6, T7 Arg7, T8 Arg8, T9 Arg9, T10 Arg10, T11 Arg11, out Exception? InvokeException)
		{
			try
			{
				Action?.Invoke(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10, Arg11);
			}
			catch (Exception Error)
			{
				InvokeException = Error;
				return false;
			}

			InvokeException = null;
			return true;
		}
		/// <summary>
		/// Попытаться выполнить <see cref="Action"/>. <paramref name="Action"/> может принимать NULL
		/// </summary>
		/// <typeparam name="T1">Тип аргумента</typeparam>
		/// <typeparam name="T2">Тип аргумента</typeparam>
		/// <typeparam name="T3">Тип аргумента</typeparam>
		/// <typeparam name="T4">Тип аргумента</typeparam>
		/// <typeparam name="T5">Тип аргумента</typeparam>
		/// <typeparam name="T6">Тип аргумента</typeparam>
		/// <typeparam name="T7">Тип аргумента</typeparam>
		/// <typeparam name="T8">Тип аргумента</typeparam>
		/// <typeparam name="T9">Тип аргумента</typeparam>
		/// <typeparam name="T10">Тип аргумента</typeparam>
		/// <typeparam name="T11">Тип аргумента</typeparam>
		/// <typeparam name="T12">Тип аргумента</typeparam>
		/// <param name="Action">Действие, которое необходимо попытаться выполнить</param>
		/// <param name="Arg1">Аргумент, принимаемый действием</param>
		/// <param name="Arg2">Аргумент, принимаемый действием</param>
		/// <param name="Arg3">Аргумент, принимаемый действием</param>
		/// <param name="Arg4">Аргумент, принимаемый действием</param>
		/// <param name="Arg5">Аргумент, принимаемый действием</param>
		/// <param name="Arg6">Аргумент, принимаемый действием</param>
		/// <param name="Arg7">Аргумент, принимаемый действием</param>
		/// <param name="Arg8">Аргумент, принимаемый действием</param>
		/// <param name="Arg9">Аргумент, принимаемый действием</param>
		/// <param name="Arg10">Аргумент, принимаемый действием</param>
		/// <param name="Arg11">Аргумент, принимаемый действием</param>
		/// <param name="Arg12">Аргумент, принимаемый действием</param>
		/// <param name="InvokeException"></param>
		/// <returns>Успешно ли выполнено действие</returns>
		public static bool TryInvoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> Action, T1 Arg1, T2 Arg2, T3 Arg3, T4 Arg4, T5 Arg5, T6 Arg6, T7 Arg7, T8 Arg8, T9 Arg9, T10 Arg10, T11 Arg11, T12 Arg12, out Exception? InvokeException)
		{
			try
			{
				Action?.Invoke(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10, Arg11, Arg12);
			}
			catch (Exception Error)
			{
				InvokeException = Error;
				return false;
			}

			InvokeException = null;
			return true;
		}
		/// <summary>
		/// Попытаться выполнить <see cref="Action"/>. <paramref name="Action"/> может принимать NULL
		/// </summary>
		/// <typeparam name="T1">Тип аргумента</typeparam>
		/// <typeparam name="T2">Тип аргумента</typeparam>
		/// <typeparam name="T3">Тип аргумента</typeparam>
		/// <typeparam name="T4">Тип аргумента</typeparam>
		/// <typeparam name="T5">Тип аргумента</typeparam>
		/// <typeparam name="T6">Тип аргумента</typeparam>
		/// <typeparam name="T7">Тип аргумента</typeparam>
		/// <typeparam name="T8">Тип аргумента</typeparam>
		/// <typeparam name="T9">Тип аргумента</typeparam>
		/// <typeparam name="T10">Тип аргумента</typeparam>
		/// <typeparam name="T11">Тип аргумента</typeparam>
		/// <typeparam name="T12">Тип аргумента</typeparam>
		/// <typeparam name="T13">Тип аргумента</typeparam>
		/// <param name="Action">Действие, которое необходимо попытаться выполнить</param>
		/// <param name="Arg1">Аргумент, принимаемый действием</param>
		/// <param name="Arg2">Аргумент, принимаемый действием</param>
		/// <param name="Arg3">Аргумент, принимаемый действием</param>
		/// <param name="Arg4">Аргумент, принимаемый действием</param>
		/// <param name="Arg5">Аргумент, принимаемый действием</param>
		/// <param name="Arg6">Аргумент, принимаемый действием</param>
		/// <param name="Arg7">Аргумент, принимаемый действием</param>
		/// <param name="Arg8">Аргумент, принимаемый действием</param>
		/// <param name="Arg9">Аргумент, принимаемый действием</param>
		/// <param name="Arg10">Аргумент, принимаемый действием</param>
		/// <param name="Arg11">Аргумент, принимаемый действием</param>
		/// <param name="Arg12">Аргумент, принимаемый действием</param>
		/// <param name="Arg13">Аргумент, принимаемый действием</param>
		/// <param name="InvokeException"></param>
		/// <returns>Успешно ли выполнено действие</returns>
		public static bool TryInvoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> Action, T1 Arg1, T2 Arg2, T3 Arg3, T4 Arg4, T5 Arg5, T6 Arg6, T7 Arg7, T8 Arg8, T9 Arg9, T10 Arg10, T11 Arg11, T12 Arg12, T13 Arg13, out Exception? InvokeException)
		{
			try
			{
				Action?.Invoke(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10, Arg11, Arg12, Arg13);
			}
			catch (Exception Error)
			{
				InvokeException = Error;
				return false;
			}

			InvokeException = null;
			return true;
		}
		/// <summary>
		/// Попытаться выполнить <see cref="Action"/>. <paramref name="Action"/> может принимать NULL
		/// </summary>
		/// <typeparam name="T1">Тип аргумента</typeparam>
		/// <typeparam name="T2">Тип аргумента</typeparam>
		/// <typeparam name="T3">Тип аргумента</typeparam>
		/// <typeparam name="T4">Тип аргумента</typeparam>
		/// <typeparam name="T5">Тип аргумента</typeparam>
		/// <typeparam name="T6">Тип аргумента</typeparam>
		/// <typeparam name="T7">Тип аргумента</typeparam>
		/// <typeparam name="T8">Тип аргумента</typeparam>
		/// <typeparam name="T9">Тип аргумента</typeparam>
		/// <typeparam name="T10">Тип аргумента</typeparam>
		/// <typeparam name="T11">Тип аргумента</typeparam>
		/// <typeparam name="T12">Тип аргумента</typeparam>
		/// <typeparam name="T13">Тип аргумента</typeparam>
		/// <typeparam name="T14">Тип аргумента</typeparam>
		/// <param name="Action">Действие, которое необходимо попытаться выполнить</param>
		/// <param name="Arg1">Аргумент, принимаемый действием</param>
		/// <param name="Arg2">Аргумент, принимаемый действием</param>
		/// <param name="Arg3">Аргумент, принимаемый действием</param>
		/// <param name="Arg4">Аргумент, принимаемый действием</param>
		/// <param name="Arg5">Аргумент, принимаемый действием</param>
		/// <param name="Arg6">Аргумент, принимаемый действием</param>
		/// <param name="Arg7">Аргумент, принимаемый действием</param>
		/// <param name="Arg8">Аргумент, принимаемый действием</param>
		/// <param name="Arg9">Аргумент, принимаемый действием</param>
		/// <param name="Arg10">Аргумент, принимаемый действием</param>
		/// <param name="Arg11">Аргумент, принимаемый действием</param>
		/// <param name="Arg12">Аргумент, принимаемый действием</param>
		/// <param name="Arg13">Аргумент, принимаемый действием</param>
		/// <param name="Arg14">Аргумент, принимаемый действием</param>
		/// <param name="InvokeException"></param>
		/// <returns>Успешно ли выполнено действие</returns>
		public static bool TryInvoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> Action, T1 Arg1, T2 Arg2, T3 Arg3, T4 Arg4, T5 Arg5, T6 Arg6, T7 Arg7, T8 Arg8, T9 Arg9, T10 Arg10, T11 Arg11, T12 Arg12, T13 Arg13, T14 Arg14, out Exception? InvokeException)
		{
			try
			{
				Action?.Invoke(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10, Arg11, Arg12, Arg13, Arg14);
			}
			catch (Exception Error)
			{
				InvokeException = Error;
				return false;
			}

			InvokeException = null;
			return true;
		}
		/// <summary>
		/// Попытаться выполнить <see cref="Action"/>. <paramref name="Action"/> может принимать NULL
		/// </summary>
		/// <typeparam name="T1">Тип аргумента</typeparam>
		/// <typeparam name="T2">Тип аргумента</typeparam>
		/// <typeparam name="T3">Тип аргумента</typeparam>
		/// <typeparam name="T4">Тип аргумента</typeparam>
		/// <typeparam name="T5">Тип аргумента</typeparam>
		/// <typeparam name="T6">Тип аргумента</typeparam>
		/// <typeparam name="T7">Тип аргумента</typeparam>
		/// <typeparam name="T8">Тип аргумента</typeparam>
		/// <typeparam name="T9">Тип аргумента</typeparam>
		/// <typeparam name="T10">Тип аргумента</typeparam>
		/// <typeparam name="T11">Тип аргумента</typeparam>
		/// <typeparam name="T12">Тип аргумента</typeparam>
		/// <typeparam name="T13">Тип аргумента</typeparam>
		/// <typeparam name="T14">Тип аргумента</typeparam>
		/// <typeparam name="T15">Тип аргумента</typeparam>
		/// <param name="Action">Действие, которое необходимо попытаться выполнить</param>
		/// <param name="Arg1">Аргумент, принимаемый действием</param>
		/// <param name="Arg2">Аргумент, принимаемый действием</param>
		/// <param name="Arg3">Аргумент, принимаемый действием</param>
		/// <param name="Arg4">Аргумент, принимаемый действием</param>
		/// <param name="Arg5">Аргумент, принимаемый действием</param>
		/// <param name="Arg6">Аргумент, принимаемый действием</param>
		/// <param name="Arg7">Аргумент, принимаемый действием</param>
		/// <param name="Arg8">Аргумент, принимаемый действием</param>
		/// <param name="Arg9">Аргумент, принимаемый действием</param>
		/// <param name="Arg10">Аргумент, принимаемый действием</param>
		/// <param name="Arg11">Аргумент, принимаемый действием</param>
		/// <param name="Arg12">Аргумент, принимаемый действием</param>
		/// <param name="Arg13">Аргумент, принимаемый действием</param>
		/// <param name="Arg14">Аргумент, принимаемый действием</param>
		/// <param name="Arg15">Аргумент, принимаемый действием</param>
		/// <param name="InvokeException"></param>
		/// <returns>Успешно ли выполнено действие</returns>
		public static bool TryInvoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> Action, T1 Arg1, T2 Arg2, T3 Arg3, T4 Arg4, T5 Arg5, T6 Arg6, T7 Arg7, T8 Arg8, T9 Arg9, T10 Arg10, T11 Arg11, T12 Arg12, T13 Arg13, T14 Arg14, T15 Arg15, out Exception? InvokeException)
		{
			try
			{
				Action?.Invoke(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10, Arg11, Arg12, Arg13, Arg14, Arg15);
			}
			catch (Exception Error)
			{
				InvokeException = Error;
				return false;
			}

			InvokeException = null;
			return true;
		}
		/// <summary>
		/// Попытаться выполнить <see cref="Action"/>. <paramref name="Action"/> может принимать NULL
		/// </summary>
		/// <typeparam name="T1">Тип аргумента</typeparam>
		/// <typeparam name="T2">Тип аргумента</typeparam>
		/// <typeparam name="T3">Тип аргумента</typeparam>
		/// <typeparam name="T4">Тип аргумента</typeparam>
		/// <typeparam name="T5">Тип аргумента</typeparam>
		/// <typeparam name="T6">Тип аргумента</typeparam>
		/// <typeparam name="T7">Тип аргумента</typeparam>
		/// <typeparam name="T8">Тип аргумента</typeparam>
		/// <typeparam name="T9">Тип аргумента</typeparam>
		/// <typeparam name="T10">Тип аргумента</typeparam>
		/// <typeparam name="T11">Тип аргумента</typeparam>
		/// <typeparam name="T12">Тип аргумента</typeparam>
		/// <typeparam name="T13">Тип аргумента</typeparam>
		/// <typeparam name="T14">Тип аргумента</typeparam>
		/// <typeparam name="T15">Тип аргумента</typeparam>
		/// <typeparam name="T16">Тип аргумента</typeparam>
		/// <param name="Action">Действие, которое необходимо попытаться выполнить</param>
		/// <param name="Arg1">Аргумент, принимаемый действием</param>
		/// <param name="Arg2">Аргумент, принимаемый действием</param>
		/// <param name="Arg3">Аргумент, принимаемый действием</param>
		/// <param name="Arg4">Аргумент, принимаемый действием</param>
		/// <param name="Arg5">Аргумент, принимаемый действием</param>
		/// <param name="Arg6">Аргумент, принимаемый действием</param>
		/// <param name="Arg7">Аргумент, принимаемый действием</param>
		/// <param name="Arg8">Аргумент, принимаемый действием</param>
		/// <param name="Arg9">Аргумент, принимаемый действием</param>
		/// <param name="Arg10">Аргумент, принимаемый действием</param>
		/// <param name="Arg11">Аргумент, принимаемый действием</param>
		/// <param name="Arg12">Аргумент, принимаемый действием</param>
		/// <param name="Arg13">Аргумент, принимаемый действием</param>
		/// <param name="Arg14">Аргумент, принимаемый действием</param>
		/// <param name="Arg15">Аргумент, принимаемый действием</param>
		/// <param name="Arg16">Аргумент, принимаемый действием</param>
		/// <param name="InvokeException"></param>
		/// <returns>Успешно ли выполнено действие</returns>
		public static bool TryInvoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> Action, T1 Arg1, T2 Arg2, T3 Arg3, T4 Arg4, T5 Arg5, T6 Arg6, T7 Arg7, T8 Arg8, T9 Arg9, T10 Arg10, T11 Arg11, T12 Arg12, T13 Arg13, T14 Arg14, T15 Arg15, T16 Arg16, out Exception? InvokeException)
		{
			try
			{
				Action?.Invoke(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10, Arg11, Arg12, Arg13, Arg14, Arg15, Arg16);
			}
			catch (Exception Error)
			{
				InvokeException = Error;
				return false;
			}

			InvokeException = null;
			return true;
		}

        /// <summary>
        /// Попытаться выполнить <see cref="Action"/>. <paramref name="Action"/> может принимать NULL
        /// </summary>
        /// <param name="Action">Действие, которое необходимо попытаться выполнить</param>
        /// <param name="InvokeException">Исключение, которое вызванное выполняемым действием. Если исключения не возникало - NULL</param>
        /// <param name="Arguments">Аргументы делегата</param>
        /// <returns>Успешно ли выполнено действие</returns>
        public static bool TryInvoke<TAction>(this TAction Action, out Exception? InvokeException, params object[] Arguments) where TAction : Delegate?
        {
            try
            {
                Action?.DynamicInvoke(Arguments);
            }
            catch (Exception Error)
            {
                InvokeException = Error;
                return false;
            }

            InvokeException = null;
            return true;
        }
        /// <summary>
        /// Попытаться выполнить <see cref="Action"/>. <paramref name="Action"/> может принимать NULL
        /// </summary>
        /// <param name="Action">Действие, которое необходимо попытаться выполнить</param>
        /// <param name="InvokeException">Исключение, которое вызванное выполняемым действием. Если исключения не возникало - NULL</param>
        /// <returns>Успешно ли выполнено действие</returns>
        public static bool TryInvoke<TAction>(this TAction Action, out Exception? InvokeException) where TAction : Delegate?
        {
            try
            {
                Action?.DynamicInvoke();
            }
            catch (Exception Error)
            {
                InvokeException = Error;
                return false;
            }

            InvokeException = null;
            return true;
        }
        /// <summary>
        /// Попытаться выполнить <see cref="Action"/>. <paramref name="Action"/> может принимать NULL
        /// </summary>
        /// <typeparam name="TAction">Делегат для выполнения</typeparam>
        /// <param name="Action">Действие, которое необходимо попытаться выполнить</param>
        /// <param name="Arg1">Аргумент, принимаемый действием</param>
        /// <param name="InvokeException"></param>
        /// <returns>Успешно ли выполнено действие</returns>
        public static bool TryInvoke<TAction>(this TAction Action, object Arg1, out Exception? InvokeException) where TAction : Delegate?
        {
            return Action.TryInvoke(out InvokeException, Arg1);
        }
        /// <summary>
        /// Попытаться выполнить <see cref="Action"/>. <paramref name="Action"/> может принимать NULL
        /// </summary>
        /// <typeparam name="TAction">Делегат для выполнения</typeparam>
        /// <param name="Action">Действие, которое необходимо попытаться выполнить</param>
        /// <param name="Arg1">Аргумент, принимаемый действием</param>
        /// <param name="Arg2">Аргумент, принимаемый действием</param>
        /// <param name="InvokeException"></param>
        /// <returns>Успешно ли выполнено действие</returns>
        public static bool TryInvoke<TAction>(this TAction Action, object Arg1, object Arg2, out Exception? InvokeException) where TAction : Delegate?
        {
            return Action.TryInvoke(out InvokeException, Arg1, Arg2);
        }
        /// <summary>
        /// Попытаться выполнить <see cref="Action"/>. <paramref name="Action"/> может принимать NULL
        /// </summary>
        /// <typeparam name="TAction">Делегат для выполнения</typeparam>
        /// <param name="Action">Действие, которое необходимо попытаться выполнить</param>
        /// <param name="Arg1">Аргумент, принимаемый действием</param>
        /// <param name="Arg2">Аргумент, принимаемый действием</param>
        /// <param name="Arg3">Аргумент, принимаемый действием</param>
        /// <param name="InvokeException"></param>
        /// <returns>Успешно ли выполнено действие</returns>
        public static bool TryInvoke<TAction>(this TAction Action, object Arg1, object Arg2, object Arg3, out Exception? InvokeException) where TAction : Delegate?
        {
            return Action.TryInvoke(out InvokeException, Arg1, Arg2, Arg3);
        }
        /// <summary>
        /// Попытаться выполнить <see cref="Action"/>. <paramref name="Action"/> может принимать NULL
        /// </summary>
        /// <typeparam name="TAction">Делегат для выполнения</typeparam>
        /// <param name="Action">Действие, которое необходимо попытаться выполнить</param>
        /// <param name="Arg1">Аргумент, принимаемый действием</param>
        /// <param name="Arg2">Аргумент, принимаемый действием</param>
        /// <param name="Arg3">Аргумент, принимаемый действием</param>
        /// <param name="Arg4">Аргумент, принимаемый действием</param>
        /// <param name="InvokeException"></param>
        /// <returns>Успешно ли выполнено действие</returns>
        public static bool TryInvoke<TAction>(this TAction Action, object Arg1, object Arg2, object Arg3, object Arg4, out Exception? InvokeException) where TAction : Delegate?
        {
            return Action.TryInvoke(out InvokeException, Arg1, Arg2, Arg3, Arg4);
        }
        /// <summary>
        /// Попытаться выполнить <see cref="Action"/>. <paramref name="Action"/> может принимать NULL
        /// </summary>
        /// <typeparam name="TAction">Делегат для выполнения</typeparam>
        /// <param name="Action">Действие, которое необходимо попытаться выполнить</param>
        /// <param name="Arg1">Аргумент, принимаемый действием</param>
        /// <param name="Arg2">Аргумент, принимаемый действием</param>
        /// <param name="Arg3">Аргумент, принимаемый действием</param>
        /// <param name="Arg4">Аргумент, принимаемый действием</param>
        /// <param name="Arg5">Аргумент, принимаемый действием</param>
        /// <param name="InvokeException"></param>
        /// <returns>Успешно ли выполнено действие</returns>
        public static bool TryInvoke<TAction>(this TAction Action, object Arg1, object Arg2, object Arg3, object Arg4, object Arg5, out Exception? InvokeException) where TAction : Delegate?
        {
            return Action.TryInvoke(out InvokeException, Arg1, Arg2, Arg3, Arg4, Arg5);
        }
        /// <summary>
        /// Попытаться выполнить <see cref="Action"/>. <paramref name="Action"/> может принимать NULL
        /// </summary>
        /// <typeparam name="TAction">Делегат для выполнения</typeparam>
        /// <param name="Action">Действие, которое необходимо попытаться выполнить</param>
        /// <param name="Arg1">Аргумент, принимаемый действием</param>
        /// <param name="Arg2">Аргумент, принимаемый действием</param>
        /// <param name="Arg3">Аргумент, принимаемый действием</param>
        /// <param name="Arg4">Аргумент, принимаемый действием</param>
        /// <param name="Arg5">Аргумент, принимаемый действием</param>
        /// <param name="Arg6">Аргумент, принимаемый действием</param>
        /// <param name="InvokeException"></param>
        /// <returns>Успешно ли выполнено действие</returns>
        public static bool TryInvoke<TAction>(this TAction Action, object Arg1, object Arg2, object Arg3, object Arg4, object Arg5, object Arg6, out Exception? InvokeException) where TAction : Delegate?
        {
            return Action.TryInvoke(out InvokeException, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6);
        }
        /// <summary>
        /// Попытаться выполнить <see cref="Action"/>. <paramref name="Action"/> может принимать NULL
        /// </summary>
        /// <typeparam name="TAction">Делегат для выполнения</typeparam>
        /// <param name="Action">Действие, которое необходимо попытаться выполнить</param>
        /// <param name="Arg1">Аргумент, принимаемый действием</param>
        /// <param name="Arg2">Аргумент, принимаемый действием</param>
        /// <param name="Arg3">Аргумент, принимаемый действием</param>
        /// <param name="Arg4">Аргумент, принимаемый действием</param>
        /// <param name="Arg5">Аргумент, принимаемый действием</param>
        /// <param name="Arg6">Аргумент, принимаемый действием</param>
        /// <param name="Arg7">Аргумент, принимаемый действием</param>
        /// <param name="InvokeException"></param>
        /// <returns>Успешно ли выполнено действие</returns>
        public static bool TryInvoke<TAction>(this TAction Action, object Arg1, object Arg2, object Arg3, object Arg4, object Arg5, object Arg6, object Arg7, out Exception? InvokeException) where TAction : Delegate?
        {
            return Action.TryInvoke(out InvokeException, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7);
        }
        /// <summary>
        /// Попытаться выполнить <see cref="Action"/>. <paramref name="Action"/> может принимать NULL
        /// </summary>
        /// <typeparam name="TAction">Делегат для выполнения</typeparam>
        /// <param name="Action">Действие, которое необходимо попытаться выполнить</param>
        /// <param name="Arg1">Аргумент, принимаемый действием</param>
        /// <param name="Arg2">Аргумент, принимаемый действием</param>
        /// <param name="Arg3">Аргумент, принимаемый действием</param>
        /// <param name="Arg4">Аргумент, принимаемый действием</param>
        /// <param name="Arg5">Аргумент, принимаемый действием</param>
        /// <param name="Arg6">Аргумент, принимаемый действием</param>
        /// <param name="Arg7">Аргумент, принимаемый действием</param>
        /// <param name="Arg8">Аргумент, принимаемый действием</param>
        /// <param name="InvokeException"></param>
        /// <returns>Успешно ли выполнено действие</returns>
        public static bool TryInvoke<TAction>(this TAction Action, object Arg1, object Arg2, object Arg3, object Arg4, object Arg5, object Arg6, object Arg7, object Arg8, out Exception? InvokeException) where TAction : Delegate?
        {
            return Action.TryInvoke(out InvokeException, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8);
        }
        /// <summary>
        /// Попытаться выполнить <see cref="Action"/>. <paramref name="Action"/> может принимать NULL
        /// </summary>
        /// <typeparam name="TAction">Делегат для выполнения</typeparam>
        /// <param name="Action">Действие, которое необходимо попытаться выполнить</param>
        /// <param name="Arg1">Аргумент, принимаемый действием</param>
        /// <param name="Arg2">Аргумент, принимаемый действием</param>
        /// <param name="Arg3">Аргумент, принимаемый действием</param>
        /// <param name="Arg4">Аргумент, принимаемый действием</param>
        /// <param name="Arg5">Аргумент, принимаемый действием</param>
        /// <param name="Arg6">Аргумент, принимаемый действием</param>
        /// <param name="Arg7">Аргумент, принимаемый действием</param>
        /// <param name="Arg8">Аргумент, принимаемый действием</param>
        /// <param name="Arg9">Аргумент, принимаемый действием</param>
        /// <param name="InvokeException"></param>
        /// <returns>Успешно ли выполнено действие</returns>
        public static bool TryInvoke<TAction>(this TAction Action, object Arg1, object Arg2, object Arg3, object Arg4, object Arg5, object Arg6, object Arg7, object Arg8, object Arg9, out Exception? InvokeException) where TAction : Delegate?
        {
            return Action.TryInvoke(out InvokeException, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9);
        }
        /// <summary>
        /// Попытаться выполнить <see cref="Action"/>. <paramref name="Action"/> может принимать NULL
        /// </summary>
        /// <typeparam name="TAction">Делегат для выполнения</typeparam>
        /// <param name="Action">Действие, которое необходимо попытаться выполнить</param>
        /// <param name="Arg1">Аргумент, принимаемый действием</param>
        /// <param name="Arg2">Аргумент, принимаемый действием</param>
        /// <param name="Arg3">Аргумент, принимаемый действием</param>
        /// <param name="Arg4">Аргумент, принимаемый действием</param>
        /// <param name="Arg5">Аргумент, принимаемый действием</param>
        /// <param name="Arg6">Аргумент, принимаемый действием</param>
        /// <param name="Arg7">Аргумент, принимаемый действием</param>
        /// <param name="Arg8">Аргумент, принимаемый действием</param>
        /// <param name="Arg9">Аргумент, принимаемый действием</param>
        /// <param name="Arg10">Аргумент, принимаемый действием</param>
        /// <param name="InvokeException"></param>
        /// <returns>Успешно ли выполнено действие</returns>
        public static bool TryInvoke<TAction>(this TAction Action, object Arg1, object Arg2, object Arg3, object Arg4, object Arg5, object Arg6, object Arg7, object Arg8, object Arg9, object Arg10, out Exception? InvokeException) where TAction : Delegate?
        {
            return Action.TryInvoke(out InvokeException, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10);
        }
        /// <summary>
        /// Попытаться выполнить <see cref="Action"/>. <paramref name="Action"/> может принимать NULL
        /// </summary>
        /// <typeparam name="TAction">Делегат для выполнения</typeparam>
        /// <param name="Action">Действие, которое необходимо попытаться выполнить</param>
        /// <param name="Arg1">Аргумент, принимаемый действием</param>
        /// <param name="Arg2">Аргумент, принимаемый действием</param>
        /// <param name="Arg3">Аргумент, принимаемый действием</param>
        /// <param name="Arg4">Аргумент, принимаемый действием</param>
        /// <param name="Arg5">Аргумент, принимаемый действием</param>
        /// <param name="Arg6">Аргумент, принимаемый действием</param>
        /// <param name="Arg7">Аргумент, принимаемый действием</param>
        /// <param name="Arg8">Аргумент, принимаемый действием</param>
        /// <param name="Arg9">Аргумент, принимаемый действием</param>
        /// <param name="Arg10">Аргумент, принимаемый действием</param>
        /// <param name="Arg11">Аргумент, принимаемый действием</param>
        /// <param name="InvokeException"></param>
        /// <returns>Успешно ли выполнено действие</returns>
        public static bool TryInvoke<TAction>(this TAction Action, object Arg1, object Arg2, object Arg3, object Arg4, object Arg5, object Arg6, object Arg7, object Arg8, object Arg9, object Arg10, object Arg11, out Exception? InvokeException) where TAction : Delegate?
        {
            return Action.TryInvoke(out InvokeException, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10, Arg11);
        }
        /// <summary>
        /// Попытаться выполнить <see cref="Action"/>. <paramref name="Action"/> может принимать NULL
        /// </summary>
        /// <typeparam name="TAction">Делегат для выполнения</typeparam>
        /// <param name="Action">Действие, которое необходимо попытаться выполнить</param>
        /// <param name="Arg1">Аргумент, принимаемый действием</param>
        /// <param name="Arg2">Аргумент, принимаемый действием</param>
        /// <param name="Arg3">Аргумент, принимаемый действием</param>
        /// <param name="Arg4">Аргумент, принимаемый действием</param>
        /// <param name="Arg5">Аргумент, принимаемый действием</param>
        /// <param name="Arg6">Аргумент, принимаемый действием</param>
        /// <param name="Arg7">Аргумент, принимаемый действием</param>
        /// <param name="Arg8">Аргумент, принимаемый действием</param>
        /// <param name="Arg9">Аргумент, принимаемый действием</param>
        /// <param name="Arg10">Аргумент, принимаемый действием</param>
        /// <param name="Arg11">Аргумент, принимаемый действием</param>
        /// <param name="Arg12">Аргумент, принимаемый действием</param>
        /// <param name="InvokeException"></param>
        /// <returns>Успешно ли выполнено действие</returns>
        public static bool TryInvoke<TAction>(this TAction Action, object Arg1, object Arg2, object Arg3, object Arg4, object Arg5, object Arg6, object Arg7, object Arg8, object Arg9, object Arg10, object Arg11, object Arg12, out Exception? InvokeException) where TAction : Delegate?
        {
            return Action.TryInvoke(out InvokeException, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10, Arg11, Arg12);
        }
        /// <summary>
        /// Попытаться выполнить <see cref="Action"/>. <paramref name="Action"/> может принимать NULL
        /// </summary>
        /// <typeparam name="TAction">Делегат для выполнения</typeparam>
        /// <param name="Action">Действие, которое необходимо попытаться выполнить</param>
        /// <param name="Arg1">Аргумент, принимаемый действием</param>
        /// <param name="Arg2">Аргумент, принимаемый действием</param>
        /// <param name="Arg3">Аргумент, принимаемый действием</param>
        /// <param name="Arg4">Аргумент, принимаемый действием</param>
        /// <param name="Arg5">Аргумент, принимаемый действием</param>
        /// <param name="Arg6">Аргумент, принимаемый действием</param>
        /// <param name="Arg7">Аргумент, принимаемый действием</param>
        /// <param name="Arg8">Аргумент, принимаемый действием</param>
        /// <param name="Arg9">Аргумент, принимаемый действием</param>
        /// <param name="Arg10">Аргумент, принимаемый действием</param>
        /// <param name="Arg11">Аргумент, принимаемый действием</param>
        /// <param name="Arg12">Аргумент, принимаемый действием</param>
        /// <param name="Arg13">Аргумент, принимаемый действием</param>
        /// <param name="InvokeException"></param>
        /// <returns>Успешно ли выполнено действие</returns>
        public static bool TryInvoke<TAction>(this TAction Action, object Arg1, object Arg2, object Arg3, object Arg4, object Arg5, object Arg6, object Arg7, object Arg8, object Arg9, object Arg10, object Arg11, object Arg12, object Arg13, out Exception? InvokeException) where TAction : Delegate?
        {
            return Action.TryInvoke(out InvokeException, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10, Arg11, Arg12, Arg13);
        }
        /// <summary>
        /// Попытаться выполнить <see cref="Action"/>. <paramref name="Action"/> может принимать NULL
        /// </summary>
        /// <typeparam name="TAction">Делегат для выполнения</typeparam>
        /// <param name="Action">Действие, которое необходимо попытаться выполнить</param>
        /// <param name="Arg1">Аргумент, принимаемый действием</param>
        /// <param name="Arg2">Аргумент, принимаемый действием</param>
        /// <param name="Arg3">Аргумент, принимаемый действием</param>
        /// <param name="Arg4">Аргумент, принимаемый действием</param>
        /// <param name="Arg5">Аргумент, принимаемый действием</param>
        /// <param name="Arg6">Аргумент, принимаемый действием</param>
        /// <param name="Arg7">Аргумент, принимаемый действием</param>
        /// <param name="Arg8">Аргумент, принимаемый действием</param>
        /// <param name="Arg9">Аргумент, принимаемый действием</param>
        /// <param name="Arg10">Аргумент, принимаемый действием</param>
        /// <param name="Arg11">Аргумент, принимаемый действием</param>
        /// <param name="Arg12">Аргумент, принимаемый действием</param>
        /// <param name="Arg13">Аргумент, принимаемый действием</param>
        /// <param name="Arg14">Аргумент, принимаемый действием</param>
        /// <param name="InvokeException"></param>
        /// <returns>Успешно ли выполнено действие</returns>
        public static bool TryInvoke<TAction>(this TAction Action, object Arg1, object Arg2, object Arg3, object Arg4, object Arg5, object Arg6, object Arg7, object Arg8, object Arg9, object Arg10, object Arg11, object Arg12, object Arg13, object Arg14, out Exception? InvokeException) where TAction : Delegate?
        {
            return Action.TryInvoke(out InvokeException, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10, Arg11, Arg12, Arg13, Arg14);
        }
        /// <summary>
        /// Попытаться выполнить <see cref="Action"/>. <paramref name="Action"/> может принимать NULL
        /// </summary>
        /// <typeparam name="TAction">Делегат для выполнения</typeparam>
        /// <param name="Action">Действие, которое необходимо попытаться выполнить</param>
        /// <param name="Arg1">Аргумент, принимаемый действием</param>
        /// <param name="Arg2">Аргумент, принимаемый действием</param>
        /// <param name="Arg3">Аргумент, принимаемый действием</param>
        /// <param name="Arg4">Аргумент, принимаемый действием</param>
        /// <param name="Arg5">Аргумент, принимаемый действием</param>
        /// <param name="Arg6">Аргумент, принимаемый действием</param>
        /// <param name="Arg7">Аргумент, принимаемый действием</param>
        /// <param name="Arg8">Аргумент, принимаемый действием</param>
        /// <param name="Arg9">Аргумент, принимаемый действием</param>
        /// <param name="Arg10">Аргумент, принимаемый действием</param>
        /// <param name="Arg11">Аргумент, принимаемый действием</param>
        /// <param name="Arg12">Аргумент, принимаемый действием</param>
        /// <param name="Arg13">Аргумент, принимаемый действием</param>
        /// <param name="Arg14">Аргумент, принимаемый действием</param>
        /// <param name="Arg15">Аргумент, принимаемый действием</param>
        /// <param name="InvokeException"></param>
        /// <returns>Успешно ли выполнено действие</returns>
        public static bool TryInvoke<TAction>(this TAction Action, object Arg1, object Arg2, object Arg3, object Arg4, object Arg5, object Arg6, object Arg7, object Arg8, object Arg9, object Arg10, object Arg11, object Arg12, object Arg13, object Arg14, object Arg15, out Exception? InvokeException) where TAction : Delegate?
        {
            return Action.TryInvoke(out InvokeException, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10, Arg11, Arg12, Arg13, Arg14, Arg15);
        }
        /// <summary>
        /// Попытаться выполнить <see cref="Action"/>. <paramref name="Action"/> может принимать NULL
        /// </summary>
        /// <typeparam name="TAction">Делегат для выполнения</typeparam>
        /// <param name="Action">Действие, которое необходимо попытаться выполнить</param>
        /// <param name="Arg1">Аргумент, принимаемый действием</param>
        /// <param name="Arg2">Аргумент, принимаемый действием</param>
        /// <param name="Arg3">Аргумент, принимаемый действием</param>
        /// <param name="Arg4">Аргумент, принимаемый действием</param>
        /// <param name="Arg5">Аргумент, принимаемый действием</param>
        /// <param name="Arg6">Аргумент, принимаемый действием</param>
        /// <param name="Arg7">Аргумент, принимаемый действием</param>
        /// <param name="Arg8">Аргумент, принимаемый действием</param>
        /// <param name="Arg9">Аргумент, принимаемый действием</param>
        /// <param name="Arg10">Аргумент, принимаемый действием</param>
        /// <param name="Arg11">Аргумент, принимаемый действием</param>
        /// <param name="Arg12">Аргумент, принимаемый действием</param>
        /// <param name="Arg13">Аргумент, принимаемый действием</param>
        /// <param name="Arg14">Аргумент, принимаемый действием</param>
        /// <param name="Arg15">Аргумент, принимаемый действием</param>
        /// <param name="Arg16">Аргумент, принимаемый действием</param>
        /// <param name="InvokeException"></param>
        /// <returns>Успешно ли выполнено действие</returns>
        public static bool TryInvoke<TAction>(this TAction Action, object Arg1, object Arg2, object Arg3, object Arg4, object Arg5, object Arg6, object Arg7, object Arg8, object Arg9, object Arg10, object Arg11, object Arg12, object Arg13, object Arg14, object Arg15, object Arg16, out Exception? InvokeException) where TAction : Delegate?
        {
            return Action.TryInvoke(out InvokeException, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10, Arg11, Arg12, Arg13, Arg14, Arg15, Arg16);
        }



    }
}
