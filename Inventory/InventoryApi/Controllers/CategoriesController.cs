using Microsoft.AspNetCore.Mvc;
using InventoryApi.Services;
using InventoryApi.DTOs;

namespace InventoryApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController(ICategoryService service) : ControllerBase
    {
        private readonly ICategoryService _service = service;

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryDto dto) =>
            Ok(await _service.CreateAsync(dto));

        [HttpGet("{id}/summary")]
        public async Task<IActionResult> GetSummary(int id)
        {
            var result = await _service.GetSummaryAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }
    }
}