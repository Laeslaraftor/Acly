namespace Acly.Test
{
	public class Example
	{
		public delegate void NameChange(string Name);

		public event NameChange? NameChanged;

		/// <summary>
		/// Подписаться на событие
		/// </summary>
		public void Subscribe()
		{
			NameChanged += OnNameChanged;
		}

		private void OnNameChanged(string Name)
		{
			throw new NotImplementedException();
		}
	}
}
