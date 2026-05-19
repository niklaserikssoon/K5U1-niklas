using System.ComponentModel.DataAnnotations;

namespace CloudNativeInventory.Api.DTOS
{
    public class CreateProductDTO
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; } = 0;
    }
}
