using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;
using System.Net.Mime;

namespace AssetManagementStoreApp.Controllers;

[ApiController]
[Route("[controller]")]
public class FileInfoAndUploadsController : ControllerBase
{
    private readonly DataContext _dbContext;

    public FileInfoAndUploadsController(DataContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("GetFileInfo/{fileId}")]
    public async Task<IActionResult> GetFileInfoByIdAsync(int fileId)
    {
        try
        {
            var fileData = await _dbContext.Files.FirstOrDefaultAsync(f => f.FileId == fileId);
            if(fileData == null)
            {
                return NotFound();
            }
            return Ok(fileData); 
        }
        catch(Exception ex)
        {
            throw new Exception(ex.Message);
        }   
    }
    //[RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = int.MaxValue)] 
    //[DisableRequestSizeLimit] 
    [HttpPost("upload")]
    public async Task<FileData> UploadFile(IFormFile file)
    {
        try
        {
            var fileModel = new FileData();
            if (file != null && file.Length > 0)
            {
                // Read the uploaded file into a byte array
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    var fileBytes = memoryStream.ToArray();

                    // Save the file data to the database
                    fileModel.FileName = file.FileName;
                    fileModel.FileContent = fileBytes;
                    fileModel.FileType = file.ContentType;
                    fileModel.dateTime = DateTime.Now;
                    fileModel.IsUploaded = true;
                }
            }
            return fileModel;
        }
        catch(Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    [HttpGet("download/{id}")]
    public async Task<IActionResult> DownloadFile(int id)
    {
        try
        {
            var fileModel = await _dbContext.Files.FindAsync(id);
            if (fileModel != null)
            {
                var contentDisposition = new ContentDisposition
                {
                    FileName = fileModel.FileName,
                    Inline = false
                };
                Response.Headers.Add("Access-Control-Expose-Headers", "Content-Disposition");
                Response.Headers.Add("Content-Disposition", contentDisposition.ToString());
                Response.Headers.Add("File-Name", fileModel.FileName);
                // Return the file data as a downloadable attachment
                return File(fileModel.FileContent, fileModel.FileType, fileModel.FileName);
            }
            else
            {
                return NotFound("File not found.");
            }
        }
        catch(Exception ex)
        {
            throw new Exception(ex.Message);
        }  
    }
    [HttpGet("downloadMultipleFiles/{id}")]
    public IActionResult DownloadMultipleFile(int id)
    {
        try
        {
            var files = _dbContext.Files.Where(a => a.AssetId == id).ToList();
            if(files == null)
            {
                return NotFound();
            }
            // Create a new zip archive
            var archiveStream = new MemoryStream();
            using (var archive = new ZipArchive(archiveStream, ZipArchiveMode.Create, true))
            {
                foreach (var file in files)
                {
                    // Add each file to the archive
                    var entry = archive.CreateEntry(file.FileName);
                    using (var entryStream = entry.Open())
                    {
                        using (var fileStream = new MemoryStream(file.FileContent))
                        {
                            fileStream.CopyTo(entryStream);
                        }
                    }
                }
            }
            // Send the zip archive as the response
            archiveStream.Seek(0, SeekOrigin.Begin);
            return File(archiveStream, "application/zip", "download.zip");
        }
        catch(Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    [HttpPost("EditFile")]
    public async Task<FileData> EditFile(IFormFile file)
    {
        try
        {
            FileData exitingfile = new FileData();
            if (file != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    var fileBytes = memoryStream.ToArray();

                    exitingfile.FileName = file.FileName;
                    exitingfile.FileContent = fileBytes;
                    exitingfile.FileType = file.ContentType;   
                }
            }
            return exitingfile;
        }
        catch(Exception ex)
        {
            throw new Exception(ex.Message);
        }
        
    }
    [HttpDelete("DeleteFile")]
    public async Task<IActionResult> DeleteFile(int id)
    {
        try
        {
            if (_dbContext.Files == null)
            {
                return NotFound();
            }
            var file = await _dbContext.Files.FindAsync(id);
            if (file == null)
            {
                return NotFound();
            }
            _dbContext.Files.Remove(file);
            await _dbContext.SaveChangesAsync();
            return Ok("Deleted Successfully");
        }
        catch(Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }   
}