using InventoryApi.Data;
using InventoryApi.DTOs;
using InventoryApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace InventoryApi.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            return await _context.Products
                .AsNoTracking()
                .Include(p => p.Category)
                .Where(p => p.IsActive && p.Category.IsActive)
                .Select(p => new ProductDto(p.Id, p.Name, p.Description, p.Price, p.StockQuantity, p.Category.Name))
                .ToListAsync();
        }

        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            var p = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id && p.IsActive);

            if (p == null) return null;
            return new ProductDto(p.Id, p.Name, p.Description, p.Price, p.StockQuantity, p.Category.Name);
        }

        public async Task<ProductDto> CreateAsync(CreateProductDto dto)
        {
            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                StockQuantity = dto.StockQuantity,
                CategoryId = dto.CategoryId,
                IsActive = true
            };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            
            // Load category name for return
            await _context.Entry(product).Reference(p => p.Category).LoadAsync();
            return new ProductDto(product.Id, product.Name, product.Description, product.Price, product.StockQuantity, product.Category.Name);
        }

        public async Task<bool> UpdateAsync(int id, CreateProductDto dto)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null || !product.IsActive) return false;

            product.Name = dto.Name;
            product.Description = dto.Description;
            product.Price = dto.Price;
            product.StockQuantity = dto.StockQuantity;
            product.CategoryId = dto.CategoryId;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null || !product.IsActive) return false;

            product.IsActive = false; // Soft Delete
            await _context.SaveChangesAsync();
            return true;
        }
    }
}