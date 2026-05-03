using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("videos")]
[ApiController]
public class VideoController : ControllerBase
{
    private static string GetUploadsRoot() => Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "media");

    [HttpGet]
    public ActionResult<List<object>> GetAll()
    {
        var uploadsRoot = GetUploadsRoot();
        if (!Directory.Exists(uploadsRoot))
        {
            return Ok(new List<object>());
        }

        var baseUrl = $"{Request.Scheme}://{Request.Host}";

        var files = Directory
            .EnumerateFiles(uploadsRoot)
            .Select(path =>
            {
                var fi = new FileInfo(path);
                var relativeUrl = $"/uploads/media/{fi.Name}";
                return new
                {
                    Name = fi.Name,
                    Url = relativeUrl,
                    AbsoluteUrl = baseUrl + relativeUrl,
                    SizeBytes = fi.Length,
                    LastModifiedUtc = fi.LastWriteTimeUtc
                };
            })
            .OrderByDescending(x => x.LastModifiedUtc)
            .Cast<object>()
            .ToList();

        return Ok(files);
    }

    [HttpPost("upload")]
    [RequestSizeLimit(1024L * 1024L * 1024L)]
    public async Task<ActionResult<object>> Upload([FromForm] IFormFile file, [FromForm] string? category)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest(new { Message = "No file uploaded." });
        }

        var uploadsRoot = GetUploadsRoot();
        Directory.CreateDirectory(uploadsRoot);

        var safeExt = Path.GetExtension(file.FileName);
        var fileName = $"{Guid.NewGuid():N}{safeExt}";
        var fullPath = Path.Combine(uploadsRoot, fileName);

        await using (var stream = System.IO.File.Create(fullPath))
        {
            await file.CopyToAsync(stream);
        }

        var relativeUrl = $"/uploads/media/{fileName}";
        return Ok(new
        {
            Url = relativeUrl,
            FileName = file.FileName,
            Category = category
        });
    }
}
