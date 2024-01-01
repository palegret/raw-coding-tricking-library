using Microsoft.AspNetCore.Mvc;
using TrickingLibrary.Data;
using TrickingLibrary.Models;

namespace TrickingLibrary.Api.Controllers;

[ApiController]
[Route("api/difficulties")]
public class DifficultyController : ControllerBase
{
    private readonly AppDbContext _appDbContext;

    public DifficultyController(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    // GET api/difficulties
    [HttpGet]
    public IEnumerable<Difficulty> All() =>
        _appDbContext.Difficulties.ToList();

    // GET api/difficulties/{id}
    [HttpGet("{id}")]
    public Difficulty? Get(string id) =>
        _appDbContext.Difficulties
            .FirstOrDefault(d => (d.Id ?? string.Empty).Equals(id, StringComparison.InvariantCultureIgnoreCase));

    // GET api/{id}/tricks
    [HttpGet("{id}/tricks")]
    public IEnumerable<Trick?> DifficultyTricks(string id) =>
        _appDbContext.Tricks
            .Where(tc => (tc.Difficulty ?? string.Empty).Equals(id, StringComparison.InvariantCultureIgnoreCase))
            .ToList();

    // POST api/difficulties
    [HttpPost]
    public async Task<Difficulty?> Create([FromBody] Difficulty difficulty)
    {
        if (difficulty == null || string.IsNullOrWhiteSpace(difficulty.Name))
            return null;

        difficulty.Id = difficulty.Name.Replace(" ", "-").ToLowerInvariant();

        _appDbContext.Add(difficulty);
        await _appDbContext.SaveChangesAsync();

        return difficulty;
    }

    // PUT api/difficulties
    [HttpPut]
    public async Task<Difficulty?> Update([FromBody] Difficulty difficulty)
    {
        if (string.IsNullOrWhiteSpace(difficulty.Id))
            return null;

        _appDbContext.Add(difficulty);
        await _appDbContext.SaveChangesAsync();

        return difficulty;
    }

    // DELETE api/difficulties/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var difficulty = Get(id);

        if (difficulty == null)
            return NotFound();

        difficulty.Deleted = true;
        await _appDbContext.SaveChangesAsync();

        return Ok();
    }
}
