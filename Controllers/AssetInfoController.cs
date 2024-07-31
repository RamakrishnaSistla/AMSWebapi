using Microsoft.AspNetCore.Mvc;

namespace AssetManagementStoreApp.Controllers;

[ApiController]
[Route("[controller]")]
public class AssetInfoController : ControllerBase
{
    private readonly DataContext _dbContext;

    public AssetInfoController(DataContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("GetAssets")]
    public async Task<List<AssetInfo>> GetAssetsAsync()
    {
        try
        {
            List<AssetInfo> assetInfos = await _dbContext.Assets.ToListAsync();
            List<Category> categories = await _dbContext.Categories.ToListAsync();
            foreach (var item in assetInfos)
            {
                item.categoryName = await _dbContext.Categories.Where(x => x.CategoryId == item.CategoryId).Select(y => y.CategoryName).FirstOrDefaultAsync();
            }
            return assetInfos;
        }
        catch(Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    [HttpGet("GetAssetInfo/{assetId}")]
    public async Task<IActionResult> GetAssetInfoByIdAsync(int assetId)
    {
        try
        {
            List<FileData> filesList = new List<FileData>();
            var objAssest = await _dbContext.Assets.FirstOrDefaultAsync(a => a.AssetId == assetId);
            if (objAssest == null)
            {
                return NotFound();
            }
            var files = await _dbContext.Files.ToListAsync();
            foreach (var file in files)
            {
                if (file.AssetId == assetId)
                {
                    filesList.Add(file);
                }
            }
            objAssest.Files = filesList;
            objAssest.categoryName = await _dbContext.Categories.Where(x => x.CategoryId == objAssest.CategoryId).Select(y => y.CategoryName).FirstOrDefaultAsync();
            objAssest.IdeaName = await _dbContext.Ideas.Where(x => x.Id == objAssest.IdeaId).Select(y => y.Name).FirstOrDefaultAsync();
            return Ok(objAssest);
        }
        catch(Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    [HttpPost("CreateAndEditAsset")]
    public async Task<Tuple<string>> CreateAndEditAsset(AssetInfo asset)
    {
        try
        {
            // check if the entity exists in the database
            var entity = await _dbContext.Assets.FindAsync(asset.AssetId);
            if (entity == null)
            {
                if (asset.CategoryId == 3)
                {
                    if (asset.ObjIdea != null)
                    {
                        _dbContext.Ideas.Add(asset.ObjIdea);
                        await _dbContext.SaveChangesAsync();
                        asset.IdeaId = asset.ObjIdea.Id;
                    }
                }
                if (asset.CategoryId == 2)
                {
                    asset.IdeaId = asset.ObjIdea.Id;
                }
                asset.UploadedOn = DateTime.Now;
                _dbContext.Assets.Add(asset);
                string message = asset.CategoryId == 1 ? "Accelerator " : asset.CategoryId == 2 ? "POC " : "Idea ";
                await _dbContext.SaveChangesAsync();

                var newAsset = await _dbContext.Assets.FindAsync(asset.AssetId);
                if(asset.Files != null)
                {
                    foreach (var file in asset.Files)
                    {
                        file.AssetId = newAsset.AssetId;
                        _dbContext.Files.Add(file);
                    }
                } 
                await _dbContext.SaveChangesAsync();
                return Tuple.Create(message + "created successfully");
            }
            else
            {
                // entity already exists, update it
                entity.TechnologyUsed = asset.TechnologyUsed;
                entity.UploadedBy = asset.UploadedBy;
                entity.UploadedOn = DateTime.Now;
                entity.Comments = asset.Comments;
                entity.Description = asset.Description;
                entity.Files = asset.Files;

                foreach (var file in entity.Files)
                {
                    var fileExitorNot = _dbContext.Files.FindAsync(file.FileId);
                    if(fileExitorNot.Result == null)
                    {
                        file.AssetId = asset.AssetId;
                         _dbContext.Files.Add(file);
                    }
                    if(file.IsReplaced == true)
                    {
                        var exitingFile = _dbContext.Files.FindAsync(file.FileId);
                        if (exitingFile.Result != null)
                        {
                            exitingFile.Result.FileName = file.FileName;
                            exitingFile.Result.FileContent = file.FileContent;
                            exitingFile.Result.FileType = file.FileType;
                        }
                    } 
                    await _dbContext.SaveChangesAsync(); 
                }
                entity.CategoryId = asset.CategoryId;
                entity.IdeaId = asset.CategoryId == 2 ? asset.IdeaId : entity.IdeaId;
                if (asset.CategoryId == 3)
                {
                    var idea = _dbContext.Ideas.Where(x => x.Id == entity.IdeaId).FirstOrDefault();
                    if (idea != null)
                    {
                        idea.Description = asset.Description;
                        idea.Name = asset.IdeaName;
                    }
                    // _dbContext.Ideas.Update(idea);
                }
                await _dbContext.SaveChangesAsync();
                return Tuple.Create("Updated successfully");
            }
        }
        catch(Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    [HttpDelete("DeleteAsset")]
    public async Task<Tuple<string>> DeleteAssetAsync(int assetId)
    {
        try
        {
            var assetToDelete = await _dbContext.Assets.FindAsync(assetId);
            var files = await _dbContext.Files.ToListAsync();
            if (files != null && assetToDelete != null)
            {
                foreach (var file in files)
                {
                    if (file.AssetId == assetId)
                    {
                        _dbContext.Files.Remove(file);
                    }
                }
                if (assetToDelete.CategoryId == 3)
                {
                    var idea = await _dbContext.Ideas.FindAsync(assetToDelete.IdeaId);
                    if (idea != null)
                    {
                        _dbContext.Ideas.Remove(idea);
                        _dbContext.Assets.Remove(assetToDelete);
                    }
                }
                else
                {
                    _dbContext.Assets.Remove(assetToDelete);
                }

                await _dbContext.SaveChangesAsync();
            }
            return Tuple.Create("Deleted successfully");
        }
        catch(Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}