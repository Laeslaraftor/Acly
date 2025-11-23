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
	public class Person1
	{
		public string? Name { get; set; }
	}
    public class Person2
    {
        public string? SpecialName { get; set; }
    }
    public class PersonsConverter : IValueConverter<Person1, Person2>
    {
        public Person2 Convert(Person1 Value)
        {
			return new()
			{
				SpecialName = Value.Name
			};
        }

        public Person1 ConvertBack(Person2 Value)
        {
			return new()
			{
				Name = Value.SpecialName
			};
        }
    }
}
