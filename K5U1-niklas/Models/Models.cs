namespace K5U1_niklas.Models
{
    public class Product
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int StockQuantity { get; set; }
        public decimal Price { get; set; }
    }
}

