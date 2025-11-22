using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters;
using System.IO;

namespace Acly.Serialize
{
    internal class StandardFormatter : IFormatter
    {
        private readonly BinaryFormatter _Formatter = new()
        {
            AssemblyFormat = FormatterAssemblyStyle.Simple,
            TypeFormat = FormatterTypeStyle.TypesWhenNeeded,
            FilterLevel = TypeFilterLevel.Full,
        };

        public byte[] Serialize(object ObjectToSerialize)
        {
            using MemoryStream Memory = new();
            _Formatter.Serialize(Memory, ObjectToSerialize);

            return Memory.ToArray();
        }
        public T Deserialize<T>(byte[] SerializedObject)
        {
            using MemoryStream Memory = new(SerializedObject);
            return (T)_Formatter.Deserialize(Memory);
        }
    }
}
