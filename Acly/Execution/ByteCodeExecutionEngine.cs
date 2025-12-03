using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Acly.Execution
{
    /// <summary>
    /// Базовый класс исполнителя байт-кода
    /// </summary>
    /// <typeparam name="T">Тип байт-кода</typeparam>
    public abstract class ByteCodeExecutionEngine<T> : IDisposable, INotifyPropertyChanged
        where T : Enum
    {
        /// <summary>
        /// Создать новый экземпляр исполнителя байт-кода
        /// </summary>
        protected ByteCodeExecutionEngine()
        {
            OpCodeSize = Marshal.SizeOf<T>();
            _OpcodeBuffer = new byte[OpCodeSize];
        }
        /// <summary>
        /// Очистка
        /// </summary>
        ~ByteCodeExecutionEngine()
        {
            Dispose(false);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Исполняется ли код
        /// </summary>
        public bool IsStarted
        {
            get => _IsStarted;
            private set
            {
                if (_IsStarted != value)
                {
                    _IsStarted = value;
                    InvokePropertyChanged(nameof(IsStarted));
                }
            }
        }

        /// <summary>
        /// Размер оператора
        /// </summary>
        protected int OpCodeSize { get; }
        /// <summary>
        /// Поток кода
        /// </summary>
        protected abstract Stream CodeStream { get; }

        private readonly byte[] _OpcodeBuffer;
        private bool _IsStarted;

        #region Управление

        /// <summary>
        /// Начать выполнение кода
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public async void Start()
        {
            if (IsStarted)
            {
                throw new InvalidOperationException("Выполнение кода уже начато");
            }

            IsStarted = true;
            CodeStream.Position = 0;

            try
            {
                foreach (var OpCode in ReadOpCodes())
                {
                    if (!IsStarted)
                    {
                        return;
                    }

                    try
                    {
                        await Execute(OpCode);
                    }
                    catch (Exception Error)
                    {
                        if (!HandleExecutionException(Error, true))
                        {
                            break;
                        }
                    }
                }
            }
            catch (Exception Error)
            {
                HandleExecutionException(Error, false);
            }

            Stop();
        }
        /// <summary>
        /// Остановить выполнение кода
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public void Stop()
        {
            if (!IsStarted)
            {
                throw new InvalidOperationException("Невозможно остановить выполнение кода, так как он и так не выполняется.");
            }

            IsStarted = false;
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Выполнить оператор
        /// </summary>
        /// <param name="OpCode">Оператор, который необходимо выполнить</param>
        protected abstract Task Execute(T OpCode);

        /// <summary>
        /// Очистить объект
        /// </summary>
        /// <param name="IsDisposing">Очистка вручную</param>
        protected virtual void Dispose(bool IsDisposing)
        {
            if (IsStarted)
            {
                Stop();
            }
        }
        /// <summary>
        /// Обработать исключение выполнения кода
        /// </summary>
        /// <param name="Error">Исключение, которое выданное во время выполнения кода</param>
        /// <param name="IsOpCodeExecution">Исключение выдано при выполнении оператора</param>
        /// <returns>Продолжить ли исполнение кода</returns>
        protected virtual bool HandleExecutionException(Exception Error, bool IsOpCodeExecution)
        {
            return false;
        }

        /// <summary>
        /// Прочитать следующий оператор
        /// </summary>
        /// <returns>Оператор</returns>
        protected T ReadOpCode()
        {
            Array.Clear(_OpcodeBuffer, 0, _OpcodeBuffer.Length);

            for (int i = 0; i < OpCodeSize; i++)
            {
                _OpcodeBuffer[i] = (byte)CodeStream.ReadByte();
            }

            return Helper.BytesToEnum<T>(_OpcodeBuffer);
        }

        private IEnumerable<T> ReadOpCodes()
        {
            var Stream = CodeStream;

            while (Stream.Length > Stream.Position)
            {
                yield return ReadOpCode();
            }
        }

        #endregion

        #region События

        /// <summary>
        /// Вызвать событие изменения поля
        /// </summary>
        /// <param name="PropertyName">Название изменённое поля</param>
        protected virtual void InvokePropertyChanged(string PropertyName)
        {
            PropertyChanged?.Invoke(this, new(PropertyName));
        }

        #endregion
    }
}
