using CloudNativeInventory.Api.DTOS;
using CloudNativeInventory.Api.Models;
using CloudNativeInventory.Api.Repository;

namespace CloudNativeInventory.Api.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<ProductDTO> CreateProductAsync(CreateProductDTO dto)
        {
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                StockQuantity = dto.Quantity
            };

            var created = await _repository.CreateProductAsync(product);

            return new ProductDTO
            {
                Id = created.Id,
                Name = created.Name,
                Description = created.Description,
                Price = created.Price,
                StockQuantity = created.StockQuantity
            };
        }

        public Task<bool> DeleteProductAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProductDTO>> GetAllProductsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ProductDTO> GetProductByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<ProductDTO> UpdateProductAsync(Guid id, CreateProductDTO updateProductDTO)
        {
            throw new NotImplementedException();
        }
    }
}
