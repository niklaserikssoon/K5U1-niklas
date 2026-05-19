using CloudNativeInventory.Api.Data;
using CloudNativeInventory.Api.Models;
using CloudNativeInventory.Api.Services;
using Microsoft.EntityFrameworkCore;


namespace CloudNativeInventory.Api.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly InventoryDbContext _context;
        
        public ProductRepository(InventoryDbContext context)
        {
            _context = context;
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public Task<bool> DeleteProductAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetProductByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Product> UpdateProductAsync(Product product)
        {
            throw new NotImplementedException();
        }
    }
}
