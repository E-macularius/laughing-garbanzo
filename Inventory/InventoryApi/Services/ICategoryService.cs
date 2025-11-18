using InventoryApi.DTOs;

namespace InventoryApi.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllAsync();
        Task<CategoryDto> CreateAsync(CreateCategoryDto dto);
        Task<CategorySummaryDto?> GetSummaryAsync(int id);
    }
}