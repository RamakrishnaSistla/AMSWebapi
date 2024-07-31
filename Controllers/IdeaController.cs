using Microsoft.AspNetCore.Mvc;

namespace AssetManagementStoreApp.Controllers;

[ApiController]
[Route("[controller]")]
public class IdeaController : ControllerBase
{
    private readonly DataContext _dbContext;

    public IdeaController(DataContext dbContext)
    {
        _dbContext = dbContext;
    }
    [HttpGet("GetIdeas")]
    public async Task<List<Idea>> GetIdeasAsync()
    {
        return await _dbContext.Ideas.ToListAsync();
    }
    
}