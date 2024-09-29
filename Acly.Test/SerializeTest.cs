using Acly.Serialize;

namespace Acly.Test
{
	[Serializable]
	public class SerializeTest
	{
		public Action? Action { get; set; }
		public static string FilePath => Path.Combine(Environment.CurrentDirectory, "test.dat");

		public void Do()
		{
			Action?.Invoke();
		}
		public void Save()
		{
			this.Serialize(FilePath);
		}
		public static SerializeTest? Restore()
		{
			if (!File.Exists(FilePath))
			{
				return null;
			}

			using Stream FileStream = File.OpenRead(FilePath);
			return FileStream.Deserialize<SerializeTest>();
		}
	}
}
