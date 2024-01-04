using Microsoft.AspNetCore.Mvc;
using TrickingLibrary.Api.Helpers;

namespace TrickingLibrary.Api.Controllers;

[ApiController]
[Route("api/videos")]
public class VideosController : ControllerBase
{
    private readonly VideoHelper _videoHelper;

    public VideosController(VideoHelper videoHelper)
    {
        _videoHelper = videoHelper;
    }

    // GET api/videos/{video}
    [HttpGet("{video}")]
    public IActionResult GetVideo(string video)
    {
        var videoSavePath = _videoHelper.VideoSavePath(video);

        if (string.IsNullOrWhiteSpace(videoSavePath))
            return BadRequest();

        var fileStream = new FileStream(videoSavePath, FileMode.Open, FileAccess.Read);
        var mimeType = _videoHelper.GetVideoMimeType(video);

        return new FileStreamResult(fileStream, mimeType);
    }

    // POST api/videos
    [HttpPost]
    public async Task<IActionResult> UploadVideo(IFormFile video)
    {
        var temporaryFileName = await _videoHelper.SaveTemporaryVideo(video);

        if (string.IsNullOrWhiteSpace(temporaryFileName))
            return BadRequest();

        return Ok(temporaryFileName);
    }

    // DELETE api/videos/{fileName}
    [HttpDelete("{fileName}")]
    public IActionResult DeleteVideo(string fileName)
    {
        if (!_videoHelper.IsTemporaryFile(fileName))
            return BadRequest();

        if (!_videoHelper.TemporaryVideoFileExists(fileName))
            return NoContent();

        _videoHelper.DeleteTemporaryVideo(fileName);

        return Ok();
    }
}
