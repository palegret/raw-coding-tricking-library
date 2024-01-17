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
    [HttpGet("{videoResourceLink}")]
    public IActionResult GetVideoResource(string videoResourceLink)
    {
        if (string.IsNullOrWhiteSpace(videoResourceLink))
            return BadRequest();

        if (!_videoHelper.ThumbnailFileExists(videoResourceLink) 
            && !_videoHelper.ConvertedVideoFileExists(videoResourceLink))
            return NotFound();

        var videoSavePath = _videoHelper.VideoSavePath(videoResourceLink);
        var fileStream = new FileStream(videoSavePath, FileMode.Open, FileAccess.Read);
        var mimeType = _videoHelper.GetVideoResourceMimeType(videoResourceLink);

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
