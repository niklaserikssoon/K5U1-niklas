namespace CloudNativeInventory.Api.DTOS
{
    public class ProductDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int stockQuantity { get; set; }
    }
}
