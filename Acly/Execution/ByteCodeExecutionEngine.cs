using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Acly.Execution
{
    /// <summary>
    /// Базовый класс исполнителя байт-кода
    /// </summary>
    /// <typeparam name="T">Тип байт-кода</typeparam>
    public abstract class ByteCodeExecutionEngine<T> : Disposable
        where T : Enum
    {
        /// <summary>
        /// Создать новый экземпляр исполнителя байт-кода
        /// </summary>
        protected ByteCodeExecutionEngine()
        {
            OpCodeSize = Marshal.SizeOf<T>();
            _opcodeBuffer = new byte[OpCodeSize];
        }

        /// <summary>
        /// Исполняется ли код
        /// </summary>
        public bool IsStarted
        {
            get => field;
            private set
            {
                if (field != value)
                {
                    OnPropertyChanging(nameof(IsStarted));
                    field = value;
                    OnPropertyChanged(nameof(IsStarted));
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

        private readonly byte[] _opcodeBuffer;

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
                foreach (var opCode in ReadOpCodes())
                {
                    if (!IsStarted)
                    {
                        return;
                    }

                    try
                    {
                        await Execute(opCode);
                    }
                    catch (Exception error)
                    {
                        if (!HandleExecutionException(error, true))
                        {
                            break;
                        }
                    }
                }
            }
            catch (Exception error)
            {
                HandleExecutionException(error, false);
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
        /// Выполнить оператор
        /// </summary>
        /// <param name="opCode">Оператор, который необходимо выполнить</param>
        protected abstract Task Execute(T opCode);

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="isDisposing"><inheritdoc/></param>
        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);

            if (IsStarted)
            {
                Stop();
            }
        }
        /// <summary>
        /// Обработать исключение выполнения кода
        /// </summary>
        /// <param name="error">Исключение, которое выданное во время выполнения кода</param>
        /// <param name="isOpCodeExecution">Исключение выдано при выполнении оператора</param>
        /// <returns>Продолжить ли исполнение кода</returns>
        protected virtual bool HandleExecutionException(Exception error, bool isOpCodeExecution)
        {
            return false;
        }

        /// <summary>
        /// Прочитать следующий оператор
        /// </summary>
        /// <returns>Оператор</returns>
        protected T ReadOpCode()
        {
            Array.Clear(_opcodeBuffer, 0, _opcodeBuffer.Length);

            for (int i = 0; i < OpCodeSize; i++)
            {
                _opcodeBuffer[i] = (byte)CodeStream.ReadByte();
            }

            return Helper.BytesToEnum<T>(_opcodeBuffer);
        }

        private IEnumerable<T> ReadOpCodes()
        {
            var stream = CodeStream;

            while (stream.Length > stream.Position)
            {
                yield return ReadOpCode();
            }
        }

        #endregion
    }
}
