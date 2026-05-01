using System;
using System.Diagnostics.CodeAnalysis;

namespace Acly
{
    public static partial class Helper
    {
        /// <summary>
        /// Попытаться выполнить <see cref="Action"/>. <paramref name="action"/> может принимать NULL
        /// </summary>
        /// <typeparam name="T">Тип аргумента</typeparam>
        /// <param name="action">Действие, которое необходимо попытаться выполнить</param>
        /// <param name="arg">Аргумент, принимаемый действием</param>
        /// <param name="invokeException"></param>
        /// <returns>Успешно ли выполнено действие</returns>
        public static bool TryInvoke<T>(this Action<T>? action, T arg, [NotNullWhen(false)] out Exception? invokeException)
        {
            try
            {
                action?.Invoke(arg);
            }
            catch (Exception error)
            {
                invokeException = error;
                return false;
            }

            invokeException = null;
            return true;
        }
        /// <summary>
        /// Попытаться выполнить <see cref="Action"/>. <paramref name="action"/> может принимать NULL
        /// </summary>
        /// <typeparam name="T1">Тип аргумента</typeparam>
        /// <typeparam name="T2">Тип аргумента</typeparam>
        /// <param name="action">Действие, которое необходимо попытаться выполнить</param>
        /// <param name="arg1">Аргумент, принимаемый действием</param>
        /// <param name="arg2">Аргумент, принимаемый действием</param>
        /// <param name="invokeException"></param>
        /// <returns>Успешно ли выполнено действие</returns>
        public static bool TryInvoke<T1, T2>(this Action<T1, T2>? action, T1 arg1, T2 arg2, [NotNullWhen(false)] out Exception? invokeException)
        {
            try
            {
                action?.Invoke(arg1, arg2);
            }
            catch (Exception error)
            {
                invokeException = error;
                return false;
            }

            invokeException = null;
            return true;
        }
        /// <summary>
        /// Попытаться выполнить <see cref="Action"/>. <paramref name="action"/> может принимать NULL
        /// </summary>
        /// <typeparam name="T1">Тип аргумента</typeparam>
        /// <typeparam name="T2">Тип аргумента</typeparam>
        /// <typeparam name="T3">Тип аргумента</typeparam>
        /// <param name="action">Действие, которое необходимо попытаться выполнить</param>
        /// <param name="arg1">Аргумент, принимаемый действием</param>
        /// <param name="arg2">Аргумент, принимаемый действием</param>
        /// <param name="arg3">Аргумент, принимаемый действием</param>
        /// <param name="invokeException"></param>
        /// <returns>Успешно ли выполнено действие</returns>
        public static bool TryInvoke<T1, T2, T3>(this Action<T1, T2, T3>? action, T1 arg1, T2 arg2, T3 arg3, [NotNullWhen(false)] out Exception? invokeException)
        {
            try
            {
                action?.Invoke(arg1, arg2, arg3);
            }
            catch (Exception error)
            {
                invokeException = error;
                return false;
            }

            invokeException = null;
            return true;
        }
        /// <summary>
        /// Попытаться выполнить <see cref="Action"/>. <paramref name="action"/> может принимать NULL
        /// </summary>
        /// <typeparam name="T1">Тип аргумента</typeparam>
        /// <typeparam name="T2">Тип аргумента</typeparam>
        /// <typeparam name="T3">Тип аргумента</typeparam>
        /// <typeparam name="T4">Тип аргумента</typeparam>
        /// <param name="action">Действие, которое необходимо попытаться выполнить</param>
        /// <param name="arg1">Аргумент, принимаемый действием</param>
        /// <param name="arg2">Аргумент, принимаемый действием</param>
        /// <param name="arg3">Аргумент, принимаемый действием</param>
        /// <param name="arg4">Аргумент, принимаемый действием</param>
        /// <param name="invokeException"></param>
        /// <returns>Успешно ли выполнено действие</returns>
        public static bool TryInvoke<T1, T2, T3, T4>(this Action<T1, T2, T3, T4>? action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, [NotNullWhen(false)] out Exception? invokeException)
        {
            try
            {
                action?.Invoke(arg1, arg2, arg3, arg4);
            }
            catch (Exception error)
            {
                invokeException = error;
                return false;
            }

            invokeException = null;
            return true;
        }
        /// <summary>
        /// Попытаться выполнить <see cref="Action"/>. <paramref name="action"/> может принимать NULL
        /// </summary>
        /// <typeparam name="T1">Тип аргумента</typeparam>
        /// <typeparam name="T2">Тип аргумента</typeparam>
        /// <typeparam name="T3">Тип аргумента</typeparam>
        /// <typeparam name="T4">Тип аргумента</typeparam>
        /// <typeparam name="T5">Тип аргумента</typeparam>
        /// <param name="action">Действие, которое необходимо попытаться выполнить</param>
        /// <param name="arg1">Аргумент, принимаемый действием</param>
        /// <param name="arg2">Аргумент, принимаемый действием</param>
        /// <param name="arg3">Аргумент, принимаемый действием</param>
        /// <param name="arg4">Аргумент, принимаемый действием</param>
        /// <param name="arg5">Аргумент, принимаемый действием</param>
        /// <param name="invokeException"></param>
        /// <returns>Успешно ли выполнено действие</returns>
        public static bool TryInvoke<T1, T2, T3, T4, T5>(this Action<T1, T2, T3, T4, T5>? action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, [NotNullWhen(false)] out Exception? invokeException)
        {
            try
            {
                action?.Invoke(arg1, arg2, arg3, arg4, arg5);
            }
            catch (Exception error)
            {
                invokeException = error;
                return false;
            }

            invokeException = null;
            return true;
        }
        /// <summary>
        /// Попытаться выполнить <see cref="Action"/>. <paramref name="action"/> может принимать NULL
        /// </summary>
        /// <typeparam name="T1">Тип аргумента</typeparam>
        /// <typeparam name="T2">Тип аргумента</typeparam>
        /// <typeparam name="T3">Тип аргумента</typeparam>
        /// <typeparam name="T4">Тип аргумента</typeparam>
        /// <typeparam name="T5">Тип аргумента</typeparam>
        /// <typeparam name="T6">Тип аргумента</typeparam>
        /// <param name="action">Действие, которое необходимо попытаться выполнить</param>
        /// <param name="arg1">Аргумент, принимаемый действием</param>
        /// <param name="arg2">Аргумент, принимаемый действием</param>
        /// <param name="arg3">Аргумент, принимаемый действием</param>
        /// <param name="arg4">Аргумент, принимаемый действием</param>
        /// <param name="arg5">Аргумент, принимаемый действием</param>
        /// <param name="arg6">Аргумент, принимаемый действием</param>
        /// <param name="invokeException"></param>
        /// <returns>Успешно ли выполнено действие</returns>
        public static bool TryInvoke<T1, T2, T3, T4, T5, T6>(this Action<T1, T2, T3, T4, T5, T6>? action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, [NotNullWhen(false)] out Exception? invokeException)
        {
            try
            {
                action?.Invoke(arg1, arg2, arg3, arg4, arg5, arg6);
            }
            catch (Exception error)
            {
                invokeException = error;
                return false;
            }

            invokeException = null;
            return true;
        }
        /// <summary>
        /// Попытаться выполнить <see cref="Action"/>. <paramref name="action"/> может принимать NULL
        /// </summary>
        /// <typeparam name="T1">Тип аргумента</typeparam>
        /// <typeparam name="T2">Тип аргумента</typeparam>
        /// <typeparam name="T3">Тип аргумента</typeparam>
        /// <typeparam name="T4">Тип аргумента</typeparam>
        /// <typeparam name="T5">Тип аргумента</typeparam>
        /// <typeparam name="T6">Тип аргумента</typeparam>
        /// <typeparam name="T7">Тип аргумента</typeparam>
        /// <param name="action">Действие, которое необходимо попытаться выполнить</param>
        /// <param name="arg1">Аргумент, принимаемый действием</param>
        /// <param name="arg2">Аргумент, принимаемый действием</param>
        /// <param name="arg3">Аргумент, принимаемый действием</param>
        /// <param name="arg4">Аргумент, принимаемый действием</param>
        /// <param name="arg5">Аргумент, принимаемый действием</param>
        /// <param name="arg6">Аргумент, принимаемый действием</param>
        /// <param name="arg7">Аргумент, принимаемый действием</param>
        /// <param name="invokeException"></param>
        /// <returns>Успешно ли выполнено действие</returns>
        public static bool TryInvoke<T1, T2, T3, T4, T5, T6, T7>(this Action<T1, T2, T3, T4, T5, T6, T7>? action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, [NotNullWhen(false)] out Exception? invokeException)
        {
            try
            {
                action?.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
            }
            catch (Exception error)
            {
                invokeException = error;
                return false;
            }

            invokeException = null;
            return true;
        }
        /// <summary>
        /// Попытаться выполнить <see cref="Action"/>. <paramref name="action"/> может принимать NULL
        /// </summary>
        /// <typeparam name="T1">Тип аргумента</typeparam>
        /// <typeparam name="T2">Тип аргумента</typeparam>
        /// <typeparam name="T3">Тип аргумента</typeparam>
        /// <typeparam name="T4">Тип аргумента</typeparam>
        /// <typeparam name="T5">Тип аргумента</typeparam>
        /// <typeparam name="T6">Тип аргумента</typeparam>
        /// <typeparam name="T7">Тип аргумента</typeparam>
        /// <typeparam name="T8">Тип аргумента</typeparam>
        /// <param name="action">Действие, которое необходимо попытаться выполнить</param>
        /// <param name="arg1">Аргумент, принимаемый действием</param>
        /// <param name="arg2">Аргумент, принимаемый действием</param>
        /// <param name="arg3">Аргумент, принимаемый действием</param>
        /// <param name="arg4">Аргумент, принимаемый действием</param>
        /// <param name="arg5">Аргумент, принимаемый действием</param>
        /// <param name="arg6">Аргумент, принимаемый действием</param>
        /// <param name="arg7">Аргумент, принимаемый действием</param>
        /// <param name="arg8">Аргумент, принимаемый действием</param>
        /// <param name="invokeException"></param>
        /// <returns>Успешно ли выполнено действие</returns>
        public static bool TryInvoke<T1, T2, T3, T4, T5, T6, T7, T8>(this Action<T1, T2, T3, T4, T5, T6, T7, T8>? action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, [NotNullWhen(false)] out Exception? invokeException)
        {
            try
            {
                action?.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
            }
            catch (Exception error)
            {
                invokeException = error;
                return false;
            }

            invokeException = null;
            return true;
        }
        /// <summary>
        /// Попытаться выполнить <see cref="Action"/>. <paramref name="action"/> может принимать NULL
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
        /// <param name="action">Действие, которое необходимо попытаться выполнить</param>
        /// <param name="arg1">Аргумент, принимаемый действием</param>
        /// <param name="arg2">Аргумент, принимаемый действием</param>
        /// <param name="arg3">Аргумент, принимаемый действием</param>
        /// <param name="arg4">Аргумент, принимаемый действием</param>
        /// <param name="arg5">Аргумент, принимаемый действием</param>
        /// <param name="arg6">Аргумент, принимаемый действием</param>
        /// <param name="arg7">Аргумент, принимаемый действием</param>
        /// <param name="arg8">Аргумент, принимаемый действием</param>
        /// <param name="arg9">Аргумент, принимаемый действием</param>
        /// <param name="invokeException"></param>
        /// <returns>Успешно ли выполнено действие</returns>
        public static bool TryInvoke<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>? action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, [NotNullWhen(false)] out Exception? invokeException)
        {
            try
            {
                action?.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
            }
            catch (Exception error)
            {
                invokeException = error;
                return false;
            }

            invokeException = null;
            return true;
        }
        /// <summary>
        /// Попытаться выполнить <see cref="Action"/>. <paramref name="action"/> может принимать NULL
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
        /// <param name="action">Действие, которое необходимо попытаться выполнить</param>
        /// <param name="arg1">Аргумент, принимаемый действием</param>
        /// <param name="arg2">Аргумент, принимаемый действием</param>
        /// <param name="arg3">Аргумент, принимаемый действием</param>
        /// <param name="arg4">Аргумент, принимаемый действием</param>
        /// <param name="arg5">Аргумент, принимаемый действием</param>
        /// <param name="arg6">Аргумент, принимаемый действием</param>
        /// <param name="arg7">Аргумент, принимаемый действием</param>
        /// <param name="arg8">Аргумент, принимаемый действием</param>
        /// <param name="arg9">Аргумент, принимаемый действием</param>
        /// <param name="arg10">Аргумент, принимаемый действием</param>
        /// <param name="invokeException"></param>
        /// <returns>Успешно ли выполнено действие</returns>
        public static bool TryInvoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>? action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, [NotNullWhen(false)] out Exception? invokeException)
        {
            try
            {
                action?.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
            }
            catch (Exception error)
            {
                invokeException = error;
                return false;
            }

            invokeException = null;
            return true;
        }
        /// <summary>
        /// Попытаться выполнить <see cref="Action"/>. <paramref name="action"/> может принимать NULL
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
        /// <param name="action">Действие, которое необходимо попытаться выполнить</param>
        /// <param name="arg1">Аргумент, принимаемый действием</param>
        /// <param name="arg2">Аргумент, принимаемый действием</param>
        /// <param name="arg3">Аргумент, принимаемый действием</param>
        /// <param name="arg4">Аргумент, принимаемый действием</param>
        /// <param name="arg5">Аргумент, принимаемый действием</param>
        /// <param name="arg6">Аргумент, принимаемый действием</param>
        /// <param name="arg7">Аргумент, принимаемый действием</param>
        /// <param name="arg8">Аргумент, принимаемый действием</param>
        /// <param name="arg9">Аргумент, принимаемый действием</param>
        /// <param name="arg10">Аргумент, принимаемый действием</param>
        /// <param name="arg11">Аргумент, принимаемый действием</param>
        /// <param name="invokeException"></param>
        /// <returns>Успешно ли выполнено действие</returns>
        public static bool TryInvoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>? action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, [NotNullWhen(false)] out Exception? invokeException)
        {
            try
            {
                action?.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
            }
            catch (Exception error)
            {
                invokeException = error;
                return false;
            }

            invokeException = null;
            return true;
        }
        /// <summary>
        /// Попытаться выполнить <see cref="Action"/>. <paramref name="action"/> может принимать NULL
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
        /// <param name="action">Действие, которое необходимо попытаться выполнить</param>
        /// <param name="arg1">Аргумент, принимаемый действием</param>
        /// <param name="arg2">Аргумент, принимаемый действием</param>
        /// <param name="arg3">Аргумент, принимаемый действием</param>
        /// <param name="arg4">Аргумент, принимаемый действием</param>
        /// <param name="arg5">Аргумент, принимаемый действием</param>
        /// <param name="arg6">Аргумент, принимаемый действием</param>
        /// <param name="arg7">Аргумент, принимаемый действием</param>
        /// <param name="arg8">Аргумент, принимаемый действием</param>
        /// <param name="arg9">Аргумент, принимаемый действием</param>
        /// <param name="arg10">Аргумент, принимаемый действием</param>
        /// <param name="arg11">Аргумент, принимаемый действием</param>
        /// <param name="arg12">Аргумент, принимаемый действием</param>
        /// <param name="invokeException"></param>
        /// <returns>Успешно ли выполнено действие</returns>
        public static bool TryInvoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>? action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, [NotNullWhen(false)] out Exception? invokeException)
        {
            try
            {
                action?.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
            }
            catch (Exception error)
            {
                invokeException = error;
                return false;
            }

            invokeException = null;
            return true;
        }
        /// <summary>
        /// Попытаться выполнить <see cref="Action"/>. <paramref name="action"/> может принимать NULL
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
        /// <param name="action">Действие, которое необходимо попытаться выполнить</param>
        /// <param name="arg1">Аргумент, принимаемый действием</param>
        /// <param name="arg2">Аргумент, принимаемый действием</param>
        /// <param name="arg3">Аргумент, принимаемый действием</param>
        /// <param name="arg4">Аргумент, принимаемый действием</param>
        /// <param name="arg5">Аргумент, принимаемый действием</param>
        /// <param name="arg6">Аргумент, принимаемый действием</param>
        /// <param name="arg7">Аргумент, принимаемый действием</param>
        /// <param name="arg8">Аргумент, принимаемый действием</param>
        /// <param name="arg9">Аргумент, принимаемый действием</param>
        /// <param name="arg10">Аргумент, принимаемый действием</param>
        /// <param name="arg11">Аргумент, принимаемый действием</param>
        /// <param name="arg12">Аргумент, принимаемый действием</param>
        /// <param name="arg13">Аргумент, принимаемый действием</param>
        /// <param name="invokeException"></param>
        /// <returns>Успешно ли выполнено действие</returns>
        public static bool TryInvoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>? action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, [NotNullWhen(false)] out Exception? invokeException)
        {
            try
            {
                action?.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
            }
            catch (Exception error)
            {
                invokeException = error;
                return false;
            }

            invokeException = null;
            return true;
        }
        /// <summary>
        /// Попытаться выполнить <see cref="Action"/>. <paramref name="action"/> может принимать NULL
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
        /// <param name="action">Действие, которое необходимо попытаться выполнить</param>
        /// <param name="arg1">Аргумент, принимаемый действием</param>
        /// <param name="arg2">Аргумент, принимаемый действием</param>
        /// <param name="arg3">Аргумент, принимаемый действием</param>
        /// <param name="arg4">Аргумент, принимаемый действием</param>
        /// <param name="arg5">Аргумент, принимаемый действием</param>
        /// <param name="arg6">Аргумент, принимаемый действием</param>
        /// <param name="arg7">Аргумент, принимаемый действием</param>
        /// <param name="arg8">Аргумент, принимаемый действием</param>
        /// <param name="arg9">Аргумент, принимаемый действием</param>
        /// <param name="arg10">Аргумент, принимаемый действием</param>
        /// <param name="arg11">Аргумент, принимаемый действием</param>
        /// <param name="arg12">Аргумент, принимаемый действием</param>
        /// <param name="arg13">Аргумент, принимаемый действием</param>
        /// <param name="arg14">Аргумент, принимаемый действием</param>
        /// <param name="invokeException"></param>
        /// <returns>Успешно ли выполнено действие</returns>
        public static bool TryInvoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>? action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, [NotNullWhen(false)] out Exception? invokeException)
        {
            try
            {
                action?.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
            }
            catch (Exception error)
            {
                invokeException = error;
                return false;
            }

            invokeException = null;
            return true;
        }
        /// <summary>
        /// Попытаться выполнить <see cref="Action"/>. <paramref name="action"/> может принимать NULL
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
        /// <param name="action">Действие, которое необходимо попытаться выполнить</param>
        /// <param name="arg1">Аргумент, принимаемый действием</param>
        /// <param name="arg2">Аргумент, принимаемый действием</param>
        /// <param name="arg3">Аргумент, принимаемый действием</param>
        /// <param name="arg4">Аргумент, принимаемый действием</param>
        /// <param name="arg5">Аргумент, принимаемый действием</param>
        /// <param name="arg6">Аргумент, принимаемый действием</param>
        /// <param name="arg7">Аргумент, принимаемый действием</param>
        /// <param name="arg8">Аргумент, принимаемый действием</param>
        /// <param name="arg9">Аргумент, принимаемый действием</param>
        /// <param name="arg10">Аргумент, принимаемый действием</param>
        /// <param name="arg11">Аргумент, принимаемый действием</param>
        /// <param name="arg12">Аргумент, принимаемый действием</param>
        /// <param name="arg13">Аргумент, принимаемый действием</param>
        /// <param name="arg14">Аргумент, принимаемый действием</param>
        /// <param name="arg15">Аргумент, принимаемый действием</param>
        /// <param name="invokeException"></param>
        /// <returns>Успешно ли выполнено действие</returns>
        public static bool TryInvoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>? action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, [NotNullWhen(false)] out Exception? invokeException)
        {
            try
            {
                action?.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
            }
            catch (Exception error)
            {
                invokeException = error;
                return false;
            }

            invokeException = null;
            return true;
        }
        /// <summary>
        /// Попытаться выполнить <see cref="Action"/>. <paramref name="action"/> может принимать NULL
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
        /// <param name="action">Действие, которое необходимо попытаться выполнить</param>
        /// <param name="arg1">Аргумент, принимаемый действием</param>
        /// <param name="arg2">Аргумент, принимаемый действием</param>
        /// <param name="arg3">Аргумент, принимаемый действием</param>
        /// <param name="arg4">Аргумент, принимаемый действием</param>
        /// <param name="arg5">Аргумент, принимаемый действием</param>
        /// <param name="arg6">Аргумент, принимаемый действием</param>
        /// <param name="arg7">Аргумент, принимаемый действием</param>
        /// <param name="arg8">Аргумент, принимаемый действием</param>
        /// <param name="arg9">Аргумент, принимаемый действием</param>
        /// <param name="arg10">Аргумент, принимаемый действием</param>
        /// <param name="arg11">Аргумент, принимаемый действием</param>
        /// <param name="arg12">Аргумент, принимаемый действием</param>
        /// <param name="arg13">Аргумент, принимаемый действием</param>
        /// <param name="arg14">Аргумент, принимаемый действием</param>
        /// <param name="arg15">Аргумент, принимаемый действием</param>
        /// <param name="arg16">Аргумент, принимаемый действием</param>
        /// <param name="invokeException"></param>
        /// <returns>Успешно ли выполнено действие</returns>
        public static bool TryInvoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>? action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, [NotNullWhen(false)] out Exception? invokeException)
        {
            try
            {
                action?.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
            }
            catch (Exception error)
            {
                invokeException = error;
                return false;
            }

            invokeException = null;
            return true;
        }

        /// <summary>
        /// Попытаться выполнить <see cref="Action"/>. <paramref name="action"/> может принимать NULL
        /// </summary>
        /// <param name="action">Действие, которое необходимо попытаться выполнить</param>
        /// <param name="invokeException">Исключение, которое вызванное выполняемым действием. Если исключения не возникало - NULL</param>
        /// <param name="arguments">Аргументы делегата</param>
        /// <returns>Успешно ли выполнено действие</returns>
        public static bool TryInvoke<TAction>(this TAction action, [NotNullWhen(false)] out Exception? invokeException, params object[] arguments) where TAction : Delegate?
        {
            try
            {
                action?.DynamicInvoke(arguments);
            }
            catch (Exception error)
            {
                invokeException = error;
                return false;
            }

            invokeException = null;
            return true;
        }
        /// <summary>
        /// Попытаться выполнить <see cref="Action"/>. <paramref name="action"/> может принимать NULL
        /// </summary>
        /// <param name="action">Действие, которое необходимо попытаться выполнить</param>
        /// <param name="invokeException">Исключение, которое вызванное выполняемым действием. Если исключения не возникало - NULL</param>
        /// <returns>Успешно ли выполнено действие</returns>
        public static bool TryInvoke<TAction>(this TAction action, [NotNullWhen(false)] out Exception? invokeException) where TAction : Delegate?
        {
            try
            {
                action?.DynamicInvoke();
            }
            catch (Exception error)
            {
                invokeException = error;
                return false;
            }

            invokeException = null;
            return true;
        }
        /// <summary>
        /// Попытаться выполнить <see cref="Action"/>. <paramref name="action"/> может принимать NULL
        /// </summary>
        /// <typeparam name="TAction">Делегат для выполнения</typeparam>
        /// <param name="action">Действие, которое необходимо попытаться выполнить</param>
        /// <param name="arg1">Аргумент, принимаемый действием</param>
        /// <param name="invokeException"></param>
        /// <returns>Успешно ли выполнено действие</returns>
        public static bool TryInvoke<TAction>(this TAction action, object arg1, [NotNullWhen(false)] out Exception? invokeException) where TAction : Delegate?
        {
            return action.TryInvoke(out invokeException, arg1);
        }
        /// <summary>
        /// Попытаться выполнить <see cref="Action"/>. <paramref name="action"/> может принимать NULL
        /// </summary>
        /// <typeparam name="TAction">Делегат для выполнения</typeparam>
        /// <param name="action">Действие, которое необходимо попытаться выполнить</param>
        /// <param name="arg1">Аргумент, принимаемый действием</param>
        /// <param name="arg2">Аргумент, принимаемый действием</param>
        /// <param name="invokeException"></param>
        /// <returns>Успешно ли выполнено действие</returns>
        public static bool TryInvoke<TAction>(this TAction action, object arg1, object arg2, [NotNullWhen(false)] out Exception? invokeException) where TAction : Delegate?
        {
            return action.TryInvoke(out invokeException, arg1, arg2);
        }
        /// <summary>
        /// Попытаться выполнить <see cref="Action"/>. <paramref name="action"/> может принимать NULL
        /// </summary>
        /// <typeparam name="TAction">Делегат для выполнения</typeparam>
        /// <param name="action">Действие, которое необходимо попытаться выполнить</param>
        /// <param name="arg1">Аргумент, принимаемый действием</param>
        /// <param name="arg2">Аргумент, принимаемый действием</param>
        /// <param name="arg3">Аргумент, принимаемый действием</param>
        /// <param name="invokeException"></param>
        /// <returns>Успешно ли выполнено действие</returns>
        public static bool TryInvoke<TAction>(this TAction action, object arg1, object arg2, object arg3, [NotNullWhen(false)] out Exception? invokeException) where TAction : Delegate?
        {
            return action.TryInvoke(out invokeException, arg1, arg2, arg3);
        }
        /// <summary>
        /// Попытаться выполнить <see cref="Action"/>. <paramref name="action"/> может принимать NULL
        /// </summary>
        /// <typeparam name="TAction">Делегат для выполнения</typeparam>
        /// <param name="action">Действие, которое необходимо попытаться выполнить</param>
        /// <param name="arg1">Аргумент, принимаемый действием</param>
        /// <param name="arg2">Аргумент, принимаемый действием</param>
        /// <param name="arg3">Аргумент, принимаемый действием</param>
        /// <param name="arg4">Аргумент, принимаемый действием</param>
        /// <param name="invokeException"></param>
        /// <returns>Успешно ли выполнено действие</returns>
        public static bool TryInvoke<TAction>(this TAction action, object arg1, object arg2, object arg3, object arg4, [NotNullWhen(false)] out Exception? invokeException) where TAction : Delegate?
        {
            return action.TryInvoke(out invokeException, arg1, arg2, arg3, arg4);
        }
        /// <summary>
        /// Попытаться выполнить <see cref="Action"/>. <paramref name="action"/> может принимать NULL
        /// </summary>
        /// <typeparam name="TAction">Делегат для выполнения</typeparam>
        /// <param name="action">Действие, которое необходимо попытаться выполнить</param>
        /// <param name="arg1">Аргумент, принимаемый действием</param>
        /// <param name="arg2">Аргумент, принимаемый действием</param>
        /// <param name="arg3">Аргумент, принимаемый действием</param>
        /// <param name="arg4">Аргумент, принимаемый действием</param>
        /// <param name="arg5">Аргумент, принимаемый действием</param>
        /// <param name="invokeException"></param>
        /// <returns>Успешно ли выполнено действие</returns>
        public static bool TryInvoke<TAction>(this TAction action, object arg1, object arg2, object arg3, object arg4, object arg5, [NotNullWhen(false)] out Exception? invokeException) where TAction : Delegate?
        {
            return action.TryInvoke(out invokeException, arg1, arg2, arg3, arg4, arg5);
        }
        /// <summary>
        /// Попытаться выполнить <see cref="Action"/>. <paramref name="action"/> может принимать NULL
        /// </summary>
        /// <typeparam name="TAction">Делегат для выполнения</typeparam>
        /// <param name="action">Действие, которое необходимо попытаться выполнить</param>
        /// <param name="arg1">Аргумент, принимаемый действием</param>
        /// <param name="arg2">Аргумент, принимаемый действием</param>
        /// <param name="arg3">Аргумент, принимаемый действием</param>
        /// <param name="arg4">Аргумент, принимаемый действием</param>
        /// <param name="arg5">Аргумент, принимаемый действием</param>
        /// <param name="arg6">Аргумент, принимаемый действием</param>
        /// <param name="invokeException"></param>
        /// <returns>Успешно ли выполнено действие</returns>
        public static bool TryInvoke<TAction>(this TAction action, object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, [NotNullWhen(false)] out Exception? invokeException) where TAction : Delegate?
        {
            return action.TryInvoke(out invokeException, arg1, arg2, arg3, arg4, arg5, arg6);
        }
        /// <summary>
        /// Попытаться выполнить <see cref="Action"/>. <paramref name="action"/> может принимать NULL
        /// </summary>
        /// <typeparam name="TAction">Делегат для выполнения</typeparam>
        /// <param name="action">Действие, которое необходимо попытаться выполнить</param>
        /// <param name="arg1">Аргумент, принимаемый действием</param>
        /// <param name="arg2">Аргумент, принимаемый действием</param>
        /// <param name="arg3">Аргумент, принимаемый действием</param>
        /// <param name="arg4">Аргумент, принимаемый действием</param>
        /// <param name="arg5">Аргумент, принимаемый действием</param>
        /// <param name="arg6">Аргумент, принимаемый действием</param>
        /// <param name="arg7">Аргумент, принимаемый действием</param>
        /// <param name="invokeException"></param>
        /// <returns>Успешно ли выполнено действие</returns>
        public static bool TryInvoke<TAction>(this TAction action, object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object arg7, [NotNullWhen(false)] out Exception? invokeException) where TAction : Delegate?
        {
            return action.TryInvoke(out invokeException, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }
        /// <summary>
        /// Попытаться выполнить <see cref="Action"/>. <paramref name="action"/> может принимать NULL
        /// </summary>
        /// <typeparam name="TAction">Делегат для выполнения</typeparam>
        /// <param name="action">Действие, которое необходимо попытаться выполнить</param>
        /// <param name="arg1">Аргумент, принимаемый действием</param>
        /// <param name="arg2">Аргумент, принимаемый действием</param>
        /// <param name="arg3">Аргумент, принимаемый действием</param>
        /// <param name="arg4">Аргумент, принимаемый действием</param>
        /// <param name="arg5">Аргумент, принимаемый действием</param>
        /// <param name="arg6">Аргумент, принимаемый действием</param>
        /// <param name="arg7">Аргумент, принимаемый действием</param>
        /// <param name="arg8">Аргумент, принимаемый действием</param>
        /// <param name="invokeException"></param>
        /// <returns>Успешно ли выполнено действие</returns>
        public static bool TryInvoke<TAction>(this TAction action, object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object arg7, object arg8, [NotNullWhen(false)] out Exception? invokeException) where TAction : Delegate?
        {
            return action.TryInvoke(out invokeException, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        }
        /// <summary>
        /// Попытаться выполнить <see cref="Action"/>. <paramref name="action"/> может принимать NULL
        /// </summary>
        /// <typeparam name="TAction">Делегат для выполнения</typeparam>
        /// <param name="action">Действие, которое необходимо попытаться выполнить</param>
        /// <param name="arg1">Аргумент, принимаемый действием</param>
        /// <param name="arg2">Аргумент, принимаемый действием</param>
        /// <param name="arg3">Аргумент, принимаемый действием</param>
        /// <param name="arg4">Аргумент, принимаемый действием</param>
        /// <param name="arg5">Аргумент, принимаемый действием</param>
        /// <param name="arg6">Аргумент, принимаемый действием</param>
        /// <param name="arg7">Аргумент, принимаемый действием</param>
        /// <param name="arg8">Аргумент, принимаемый действием</param>
        /// <param name="arg9">Аргумент, принимаемый действием</param>
        /// <param name="invokeException"></param>
        /// <returns>Успешно ли выполнено действие</returns>
        public static bool TryInvoke<TAction>(this TAction action, object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object arg7, object arg8, object arg9, [NotNullWhen(false)] out Exception? invokeException) where TAction : Delegate?
        {
            return action.TryInvoke(out invokeException, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        }
        /// <summary>
        /// Попытаться выполнить <see cref="Action"/>. <paramref name="action"/> может принимать NULL
        /// </summary>
        /// <typeparam name="TAction">Делегат для выполнения</typeparam>
        /// <param name="action">Действие, которое необходимо попытаться выполнить</param>
        /// <param name="arg1">Аргумент, принимаемый действием</param>
        /// <param name="arg2">Аргумент, принимаемый действием</param>
        /// <param name="arg3">Аргумент, принимаемый действием</param>
        /// <param name="arg4">Аргумент, принимаемый действием</param>
        /// <param name="arg5">Аргумент, принимаемый действием</param>
        /// <param name="arg6">Аргумент, принимаемый действием</param>
        /// <param name="arg7">Аргумент, принимаемый действием</param>
        /// <param name="arg8">Аргумент, принимаемый действием</param>
        /// <param name="arg9">Аргумент, принимаемый действием</param>
        /// <param name="arg10">Аргумент, принимаемый действием</param>
        /// <param name="invokeException"></param>
        /// <returns>Успешно ли выполнено действие</returns>
        public static bool TryInvoke<TAction>(this TAction action, object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object arg7, object arg8, object arg9, object arg10, [NotNullWhen(false)] out Exception? invokeException) where TAction : Delegate?
        {
            return action.TryInvoke(out invokeException, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
        }
        /// <summary>
        /// Попытаться выполнить <see cref="Action"/>. <paramref name="action"/> может принимать NULL
        /// </summary>
        /// <typeparam name="TAction">Делегат для выполнения</typeparam>
        /// <param name="action">Действие, которое необходимо попытаться выполнить</param>
        /// <param name="arg1">Аргумент, принимаемый действием</param>
        /// <param name="arg2">Аргумент, принимаемый действием</param>
        /// <param name="arg3">Аргумент, принимаемый действием</param>
        /// <param name="arg4">Аргумент, принимаемый действием</param>
        /// <param name="arg5">Аргумент, принимаемый действием</param>
        /// <param name="arg6">Аргумент, принимаемый действием</param>
        /// <param name="arg7">Аргумент, принимаемый действием</param>
        /// <param name="arg8">Аргумент, принимаемый действием</param>
        /// <param name="arg9">Аргумент, принимаемый действием</param>
        /// <param name="arg10">Аргумент, принимаемый действием</param>
        /// <param name="arg11">Аргумент, принимаемый действием</param>
        /// <param name="invokeException"></param>
        /// <returns>Успешно ли выполнено действие</returns>
        public static bool TryInvoke<TAction>(this TAction action, object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object arg7, object arg8, object arg9, object arg10, object arg11, [NotNullWhen(false)] out Exception? invokeException) where TAction : Delegate?
        {
            return action.TryInvoke(out invokeException, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
        }
        /// <summary>
        /// Попытаться выполнить <see cref="Action"/>. <paramref name="action"/> может принимать NULL
        /// </summary>
        /// <typeparam name="TAction">Делегат для выполнения</typeparam>
        /// <param name="action">Действие, которое необходимо попытаться выполнить</param>
        /// <param name="arg1">Аргумент, принимаемый действием</param>
        /// <param name="arg2">Аргумент, принимаемый действием</param>
        /// <param name="arg3">Аргумент, принимаемый действием</param>
        /// <param name="arg4">Аргумент, принимаемый действием</param>
        /// <param name="arg5">Аргумент, принимаемый действием</param>
        /// <param name="arg6">Аргумент, принимаемый действием</param>
        /// <param name="arg7">Аргумент, принимаемый действием</param>
        /// <param name="arg8">Аргумент, принимаемый действием</param>
        /// <param name="arg9">Аргумент, принимаемый действием</param>
        /// <param name="arg10">Аргумент, принимаемый действием</param>
        /// <param name="arg11">Аргумент, принимаемый действием</param>
        /// <param name="arg12">Аргумент, принимаемый действием</param>
        /// <param name="invokeException"></param>
        /// <returns>Успешно ли выполнено действие</returns>
        public static bool TryInvoke<TAction>(this TAction action, object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object arg7, object arg8, object arg9, object arg10, object arg11, object arg12, [NotNullWhen(false)] out Exception? invokeException) where TAction : Delegate?
        {
            return action.TryInvoke(out invokeException, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
        }
        /// <summary>
        /// Попытаться выполнить <see cref="Action"/>. <paramref name="action"/> может принимать NULL
        /// </summary>
        /// <typeparam name="TAction">Делегат для выполнения</typeparam>
        /// <param name="action">Действие, которое необходимо попытаться выполнить</param>
        /// <param name="arg1">Аргумент, принимаемый действием</param>
        /// <param name="arg2">Аргумент, принимаемый действием</param>
        /// <param name="arg3">Аргумент, принимаемый действием</param>
        /// <param name="arg4">Аргумент, принимаемый действием</param>
        /// <param name="arg5">Аргумент, принимаемый действием</param>
        /// <param name="arg6">Аргумент, принимаемый действием</param>
        /// <param name="arg7">Аргумент, принимаемый действием</param>
        /// <param name="arg8">Аргумент, принимаемый действием</param>
        /// <param name="arg9">Аргумент, принимаемый действием</param>
        /// <param name="arg10">Аргумент, принимаемый действием</param>
        /// <param name="arg11">Аргумент, принимаемый действием</param>
        /// <param name="arg12">Аргумент, принимаемый действием</param>
        /// <param name="arg13">Аргумент, принимаемый действием</param>
        /// <param name="invokeException"></param>
        /// <returns>Успешно ли выполнено действие</returns>
        public static bool TryInvoke<TAction>(this TAction action, object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object arg7, object arg8, object arg9, object arg10, object arg11, object arg12, object arg13, [NotNullWhen(false)] out Exception? invokeException) where TAction : Delegate?
        {
            return action.TryInvoke(out invokeException, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
        }
        /// <summary>
        /// Попытаться выполнить <see cref="Action"/>. <paramref name="action"/> может принимать NULL
        /// </summary>
        /// <typeparam name="TAction">Делегат для выполнения</typeparam>
        /// <param name="action">Действие, которое необходимо попытаться выполнить</param>
        /// <param name="arg1">Аргумент, принимаемый действием</param>
        /// <param name="arg2">Аргумент, принимаемый действием</param>
        /// <param name="arg3">Аргумент, принимаемый действием</param>
        /// <param name="arg4">Аргумент, принимаемый действием</param>
        /// <param name="arg5">Аргумент, принимаемый действием</param>
        /// <param name="arg6">Аргумент, принимаемый действием</param>
        /// <param name="arg7">Аргумент, принимаемый действием</param>
        /// <param name="arg8">Аргумент, принимаемый действием</param>
        /// <param name="arg9">Аргумент, принимаемый действием</param>
        /// <param name="arg10">Аргумент, принимаемый действием</param>
        /// <param name="arg11">Аргумент, принимаемый действием</param>
        /// <param name="arg12">Аргумент, принимаемый действием</param>
        /// <param name="arg13">Аргумент, принимаемый действием</param>
        /// <param name="arg14">Аргумент, принимаемый действием</param>
        /// <param name="invokeException"></param>
        /// <returns>Успешно ли выполнено действие</returns>
        public static bool TryInvoke<TAction>(this TAction action, object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object arg7, object arg8, object arg9, object arg10, object arg11, object arg12, object arg13, object arg14, [NotNullWhen(false)] out Exception? invokeException) where TAction : Delegate?
        {
            return action.TryInvoke(out invokeException, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
        }
        /// <summary>
        /// Попытаться выполнить <see cref="Action"/>. <paramref name="action"/> может принимать NULL
        /// </summary>
        /// <typeparam name="TAction">Делегат для выполнения</typeparam>
        /// <param name="action">Действие, которое необходимо попытаться выполнить</param>
        /// <param name="arg1">Аргумент, принимаемый действием</param>
        /// <param name="arg2">Аргумент, принимаемый действием</param>
        /// <param name="arg3">Аргумент, принимаемый действием</param>
        /// <param name="arg4">Аргумент, принимаемый действием</param>
        /// <param name="arg5">Аргумент, принимаемый действием</param>
        /// <param name="arg6">Аргумент, принимаемый действием</param>
        /// <param name="arg7">Аргумент, принимаемый действием</param>
        /// <param name="arg8">Аргумент, принимаемый действием</param>
        /// <param name="arg9">Аргумент, принимаемый действием</param>
        /// <param name="arg10">Аргумент, принимаемый действием</param>
        /// <param name="arg11">Аргумент, принимаемый действием</param>
        /// <param name="arg12">Аргумент, принимаемый действием</param>
        /// <param name="arg13">Аргумент, принимаемый действием</param>
        /// <param name="arg14">Аргумент, принимаемый действием</param>
        /// <param name="arg15">Аргумент, принимаемый действием</param>
        /// <param name="invokeException"></param>
        /// <returns>Успешно ли выполнено действие</returns>
        public static bool TryInvoke<TAction>(this TAction action, object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object arg7, object arg8, object arg9, object arg10, object arg11, object arg12, object arg13, object arg14, object arg15, [NotNullWhen(false)] out Exception? invokeException) where TAction : Delegate?
        {
            return action.TryInvoke(out invokeException, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
        }
        /// <summary>
        /// Попытаться выполнить <see cref="Action"/>. <paramref name="action"/> может принимать NULL
        /// </summary>
        /// <typeparam name="TAction">Делегат для выполнения</typeparam>
        /// <param name="action">Действие, которое необходимо попытаться выполнить</param>
        /// <param name="arg1">Аргумент, принимаемый действием</param>
        /// <param name="arg2">Аргумент, принимаемый действием</param>
        /// <param name="arg3">Аргумент, принимаемый действием</param>
        /// <param name="arg4">Аргумент, принимаемый действием</param>
        /// <param name="arg5">Аргумент, принимаемый действием</param>
        /// <param name="arg6">Аргумент, принимаемый действием</param>
        /// <param name="arg7">Аргумент, принимаемый действием</param>
        /// <param name="arg8">Аргумент, принимаемый действием</param>
        /// <param name="arg9">Аргумент, принимаемый действием</param>
        /// <param name="arg10">Аргумент, принимаемый действием</param>
        /// <param name="arg11">Аргумент, принимаемый действием</param>
        /// <param name="arg12">Аргумент, принимаемый действием</param>
        /// <param name="arg13">Аргумент, принимаемый действием</param>
        /// <param name="arg14">Аргумент, принимаемый действием</param>
        /// <param name="arg15">Аргумент, принимаемый действием</param>
        /// <param name="arg16">Аргумент, принимаемый действием</param>
        /// <param name="invokeException"></param>
        /// <returns>Успешно ли выполнено действие</returns>
        public static bool TryInvoke<TAction>(this TAction action, object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object arg7, object arg8, object arg9, object arg10, object arg11, object arg12, object arg13, object arg14, object arg15, object arg16, [NotNullWhen(false)] out Exception? invokeException) where TAction : Delegate?
        {
            return action.TryInvoke(out invokeException, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
        }
    }
}
