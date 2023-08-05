namespace Models.Dtos {
    public class UpdateCollectionDto {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Item> Items { get; set; }
    }
}