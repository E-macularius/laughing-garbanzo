namespace InventoryManagement.Application.DTOs
{
    public record CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public record CreateCategoryDto
    {
        [Required] public string Name { get; set; }
        public string Description { get; set; }
    }

    public record CategorySummaryDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }
        public int TotalProducts { get; set; }
        public int ActiveProducts { get; set; }
        public decimal AveragePrice { get; set; }
        public decimal TotalInventoryValue { get; set; }
        public PriceRange PriceRange { get; set; }
        public int OutOfStockCount { get; set; }
    }

    public record PriceRange(decimal Min, decimal Max);
}