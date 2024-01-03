using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using IO = System.IO;

namespace TrickingLibrary.Api.Controllers;

[ApiController]
[Route("api/videos")]
public class VideosController : ControllerBase
{
    private readonly IWebHostEnvironment _env;

    public VideosController(IWebHostEnvironment env)
    {
        _env = env;
    }

    // GET api/videos/{video}
    [HttpGet("{video}")]
    public IActionResult GetVideo(string video)
    {
        const string fallbackMimeType = "video/*";
        var fileExtensionContentTypeProvider = new FileExtensionContentTypeProvider();
        var gotMimeType = fileExtensionContentTypeProvider.TryGetContentType(video, out var mimeType);

        if (!gotMimeType || string.IsNullOrEmpty(mimeType))
            mimeType = fallbackMimeType;

        var savePath = Path.Combine(_env.WebRootPath, "videos", video);
        var fileStream = new FileStream(savePath, FileMode.Open, FileAccess.Read);

        return new FileStreamResult(fileStream, mimeType);
    }

    // POST api/videos
    [HttpPost]
    public async Task<IActionResult> UploadVideo(IFormFile video)
    {
        var workingDirectory = Path.Combine(_env.WebRootPath, "videos");
        var tempFileName = $"temp_{DateTime.Now.Ticks}";
        var extension = Path.GetExtension(video.FileName);
        var saveFileName = $"{tempFileName}{extension}";
        var savePath = Path.Combine(workingDirectory, saveFileName);

        await using var fileStream = new FileStream(savePath, FileMode.Create, FileAccess.Write);
        await video.CopyToAsync(fileStream);

        return Ok(saveFileName);
    }

    // DELETE api/videos/{fileName}
    [HttpDelete("{fileName}")]
    public IActionResult DeleteVideo(string fileName)
    {
        if (!fileName.StartsWith("temp_"))
            return BadRequest();

        var savePath = Path.Combine(_env.WebRootPath, "videos", fileName);

        if (!IO.File.Exists(savePath))
            return NoContent();

        IO.File.Delete(savePath);

        return Ok();
    }
}
