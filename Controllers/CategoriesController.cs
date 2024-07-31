using Microsoft.AspNetCore.Mvc;

namespace AssetManagementStoreApp.Controllers;

[ApiController]
[Route("[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly DataContext _dbContext;

    public CategoriesController(DataContext dbContext)
    {
        _dbContext = dbContext;
    }
    [HttpGet("GetCategories")]
    public async Task<List<Category>> GetCategoriesAsync()
    {
        return await _dbContext.Categories.ToListAsync();
    }
}