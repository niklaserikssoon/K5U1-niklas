using CloudNativeInventory.Api.DTOS;

namespace CloudNativeInventory.Api.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDTO>> GetAllProductsAsync();
        Task<ProductDTO?> GetProductByIdAsync(Guid id);
        Task<ProductDTO> CreateProductAsync(CreateProductDTO createProductDTO);
        Task<ProductDTO?> UpdateProductAsync(Guid id, CreateProductDTO updateProductDTO);
        Task<bool> DeleteProductAsync(Guid id);
    }
}
