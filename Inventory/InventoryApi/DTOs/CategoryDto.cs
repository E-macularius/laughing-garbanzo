namespace InventoryApi.DTOs
{
    public record CategoryDto(
        int Id, 
        string Name, 
        string Description);

    public record CreateCategoryDto(
        string Name, 
        string Description);

    public record CategorySummaryDto(
        int CategoryId, 
        string CategoryName, 
        string CategoryDescription, 
        int TotalProducts, 
        int ActiveProducts, 
        decimal AveragePrice, 
        decimal TotalInventoryValue, 
        PriceRange? PriceRange, 
        int OutOfStockCount);

    public record PriceRange(
        decimal Min, 
        decimal Max);
}