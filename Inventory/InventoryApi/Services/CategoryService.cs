using InventoryApi.Data;
using InventoryApi.DTOs;
using InventoryApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace InventoryApi.Services
{
    public class CategoryService(AppDbContext context) : ICategoryService
    {
        private readonly AppDbContext _context = context;

        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            return await _context.Categories
                .AsNoTracking()
                .Where(c => c.IsActive)
                .Select(c => new CategoryDto(c.Id, c.Name, c.Description))
                .ToListAsync();
        }

        public async Task<CategoryDto> CreateAsync(CreateCategoryDto dto)
        {
            var category = new Category { Name = dto.Name, Description = dto.Description };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return new CategoryDto(category.Id, category.Name, category.Description);
        }

        public async Task<CategorySummaryDto?> GetSummaryAsync(int id)
        {
            // Efficient Aggregation Query
            var summary = await _context.Categories
                .AsNoTracking()
                .Where(c => c.Id == id)
                .Select(c => new CategorySummaryDto
                (
                    c.Id,
                    c.Name,
                    c.Description,
                    c.Products.Count,
                    c.Products.Count(p => p.IsActive),
                    c.Products.Any() ? c.Products.Average(p => p.Price) : 0,
                    c.Products.Sum(p => p.Price * p.StockQuantity),
                    new PriceRange(
                        c.Products.Any() ? c.Products.Min(p => p.Price) : 0,
                        c.Products.Any() ? c.Products.Max(p => p.Price) : 0
                    ),
                    c.Products.Count(p => p.StockQuantity == 0)
                ))
                .FirstOrDefaultAsync();

            return summary;
        }
    }
}