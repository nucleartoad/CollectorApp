namespace Models
{
	public class Collection
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public List<Item> Items { get; set; } = new List<Item>{};
	}
}