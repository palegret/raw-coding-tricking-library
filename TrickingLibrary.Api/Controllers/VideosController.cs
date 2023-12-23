using Microsoft.AspNetCore.Mvc;

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

    // POST api/videos
    [HttpPost]
    public async Task<IActionResult> UploadVideo(IFormFile video)
    {
        var extension = Path.GetExtension(video.FileName);
        var fileName = $"{Path.GetRandomFileName()}{extension}";
        var savePath = Path.Combine(_env.WebRootPath, "videos", fileName);

        await using var fileStream = new FileStream(savePath, FileMode.Create, FileAccess.Write);
        await video.CopyToAsync(fileStream);

        return Ok();
    }
}
