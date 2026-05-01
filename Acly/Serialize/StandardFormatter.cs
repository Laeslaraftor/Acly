using System.IO;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;

namespace Acly.Serialize
{
    internal sealed class StandardFormatter : IFormatter
    {
        private readonly BinaryFormatter _formatter = new()
        {
            AssemblyFormat = FormatterAssemblyStyle.Simple,
            TypeFormat = FormatterTypeStyle.TypesWhenNeeded,
            FilterLevel = TypeFilterLevel.Full,
        };

        public byte[] Serialize(object objectToSerialize)
        {
            using MemoryStream memory = new();
            _formatter.Serialize(memory, objectToSerialize);

            return memory.ToArray();
        }
        public T Deserialize<T>(byte[] serializedObject)
        {
            using MemoryStream memory = new(serializedObject);
            return (T)_formatter.Deserialize(memory);
        }
    }
}
