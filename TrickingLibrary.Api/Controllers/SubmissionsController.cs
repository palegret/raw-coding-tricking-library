using Microsoft.AspNetCore.Mvc;
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
        _appDbContext.Submissions.ToList();

    // GET api/submissions/{id}
    [HttpGet("{id}")]
    public Submission? Get(int id) =>
        _appDbContext.Submissions.FirstOrDefault(s => s.Id.Equals(id));

    // POST api/submissions
    [HttpPost]
    public async Task<Submission> Create([FromBody] Submission submission)
    {
        _appDbContext.Add(submission);
        await _appDbContext.SaveChangesAsync();

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
