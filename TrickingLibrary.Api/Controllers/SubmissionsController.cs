using Microsoft.AspNetCore.Mvc;
using System.Threading.Channels;
using TrickingLibrary.Api.BackgroundServices.Messages;
using TrickingLibrary.Data;
using TrickingLibrary.Models;

namespace TrickingLibrary.Api.Controllers;

[ApiController]
[Route("api/submissions")]
public class SubmissionsController : ControllerBase
{
    private readonly AppDbContext _appDbContext;

    public SubmissionsController(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    // GET api/submissions
    [HttpGet]
    public IEnumerable<Submission> All() =>
        _appDbContext.Submissions.Where(s => s.VideoProcessed).ToList();

    // GET api/submissions/{id}
    [HttpGet("{id}")]
    public Submission? Get(int id) =>
        _appDbContext.Submissions.FirstOrDefault(s => s.Id.Equals(id));

    // POST api/submissions
    [HttpPost]
    public async Task<Submission> Create([FromBody] Submission submission, [FromServices] Channel<EditVideoMessage> channel)
    {
        // TODO - Validate video path.

        submission.VideoProcessed = false;

        _appDbContext.Add(submission);
        await _appDbContext.SaveChangesAsync();

        var input = submission.Video ?? string.Empty;
        var extension = Path.GetExtension(input);
        var output = $"converted_{DateTime.Now.Ticks}{extension}";

        await channel.Writer.WriteAsync(new EditVideoMessage { 
            SubmissionId = submission.Id,
            Input = input,
            Output = output
        });

        return submission;
    }

    // PUT api/submissions
    [HttpPut]
    public async Task<Submission?> Update([FromBody] Submission submission)
    {
        if (submission.Id == 0)
            return null;

        _appDbContext.Add(submission);
        await _appDbContext.SaveChangesAsync();

        return submission;
    }

    // DELETE api/submissions/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var submission = Get(id); 

        if (submission == null)
            return NotFound();

        submission.Deleted = true;
        await _appDbContext.SaveChangesAsync();

        return Ok();
    }
}
