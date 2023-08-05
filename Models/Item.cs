namespace Models
{
	public class Item
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public string User { get; set; } = string.Empty;
		public string Value { get; set; }
		public string CollectionId { get; set; }
	}
}

