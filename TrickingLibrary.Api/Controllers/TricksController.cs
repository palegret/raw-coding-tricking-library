using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TrickingLibrary.Data;
using TrickingLibrary.Models;

namespace TrickingLibrary.Api.Controllers;

[ApiController]
[Route("api/tricks")]
public class TricksController : ControllerBase
{
    private readonly AppDbContext _appDbContext;

    public TricksController(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    // GET api/tricks
    [HttpGet]
    public IEnumerable<Trick> All() =>
        _appDbContext.Tricks.ToList();

    // GET api/tricks/{id}
    [HttpGet("{id}")]
    public Trick? Get(string id) =>
        _appDbContext.Tricks
            .FirstOrDefault(trick => 
                trick != null 
                && !string.IsNullOrWhiteSpace(trick.Id) 
                && trick.Id.Equals(id, StringComparison.InvariantCultureIgnoreCase));

    // GET api/tricks/{trickId}/submissions
    [HttpGet("{trickId}/submissions")]
    public IEnumerable<Submission> TrickSubmissions(string trickId) =>
        _appDbContext.Submissions
            .Where(x => x.TrickId.Equals(trickId, StringComparison.InvariantCultureIgnoreCase))
            .ToList();

    // POST api/tricks
    [HttpPost]
    public async Task<Trick?> Create([FromBody] Trick trick)
    {
        if (trick == null || string.IsNullOrWhiteSpace(trick.Name))
            return null;

        trick.Id = trick.Name.Replace(" ", "-").ToLowerInvariant();

        _appDbContext.Add(trick);
        await _appDbContext.SaveChangesAsync();

        return trick;
    }

    // PUT api/tricks
    [HttpPut]
    public async Task<Trick?> Update([FromBody] Trick trick)
    {
        if (string.IsNullOrWhiteSpace(trick.Id))
            return null;

        _appDbContext.Add(trick);
        await _appDbContext.SaveChangesAsync();

        return trick;
    }

    // DELETE api/tricks/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var trick = _appDbContext.Tricks.FirstOrDefault(x => x.Id.Equals(id));

        if (trick == null)
            return NotFound();

        trick.Deleted = true;
        await _appDbContext.SaveChangesAsync();

        return Ok();
    }
}
