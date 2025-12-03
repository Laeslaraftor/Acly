using Acly.Execution;
using System.Diagnostics;

namespace Acly.Tests.Execution
{
    public class ExecutionEngine(Stream stream) : ByteCodeExecutionEngine<OpCode>()
    {
        protected override Stream CodeStream { get; } = stream;

        private readonly Dictionary<string, object> _values = [];
        private readonly object[] _stack = new object[2];

        protected override async Task Execute(OpCode OpCode)
        {
            if (OpCode == OpCode.ReadNextValue)
            {
                string? name = CodeData.Read(CodeStream) as string;

                if (name == null)
                {
                    throw new InvalidProgramException($"Required string, given: {name}");
                }

                object value = CodeData.Read(CodeStream);

                _values.TryAdd(name, value);
            }
            else if (OpCode == OpCode.GetValue)
            {
                string? name = CodeData.Read(CodeStream) as string;

                if (name == null)
                {
                    throw new InvalidProgramException($"Required string, given: {name}");
                }
                if (!_values.TryGetValue(name, out var value))
                {
                    throw new InvalidProgramException($"Can not find value with name \"{name}\"");
                }

                AddToStack(value);
            }
            else if (OpCode == OpCode.Equals)
            {
                bool value = _stack[0]?.Equals(_stack[1]) == true;
                AddToStack(value);
            }
            else if (OpCode == OpCode.SkipIfTrue)
            {
                if (_stack[0] is not bool value || !value)
                {
                    return;
                }

                object nextValue = CodeData.Read(CodeStream);

                if (nextValue is not int bytesCount)
                {
                    throw new InvalidProgramException($"Unexpected value: {nextValue}");
                }

                CodeStream.Position += bytesCount;
            }

            await Task.CompletedTask;
        }

        protected override bool HandleExecutionException(Exception Error, bool IsOpCodeExecution)
        {
            Debug.WriteLine(Error);
            return base.HandleExecutionException(Error, IsOpCodeExecution);
        }

        private void AddToStack(object value)
        {
            _stack[1] = _stack[0];
            _stack[0] = value;
        }
    }
}
