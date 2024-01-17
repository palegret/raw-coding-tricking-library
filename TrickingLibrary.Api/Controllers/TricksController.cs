using Microsoft.AspNetCore.Mvc;
using TrickingLibrary.Data;
using TrickingLibrary.Models;
using TrickingLibrary.Api.Forms;
using TrickingLibrary.Api.ViewModels;
using Microsoft.EntityFrameworkCore;

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
    public IEnumerable<object> All() =>
        _appDbContext.Tricks.Select(TrickViewModel.Default).ToList();

    // GET api/tricks/{id}
    [HttpGet("{id}")]
    public object? Get(string id) =>
        _appDbContext.Tricks
            .Where(t => (t.Id ?? string.Empty).Equals(id, StringComparison.InvariantCultureIgnoreCase))
            .Select(TrickViewModel.Default)
            .FirstOrDefault();

    // GET api/tricks/{trickId}/submissions
    [HttpGet("{trickId}/submissions")]
    public IEnumerable<Submission> TrickSubmissions(string trickId) =>
        _appDbContext.Submissions
            .Include(s => s.Video)
            .Where(s => s.TrickId.Equals(trickId, StringComparison.InvariantCultureIgnoreCase))
            .ToList();

    // POST api/tricks
    [HttpPost]
    public async Task<object?> Create([FromBody] TrickForm trickForm)
    {
        if (trickForm == null || string.IsNullOrWhiteSpace(trickForm.Name))
            return null;

        trickForm.Id = trickForm.Name.Replace(" ", "-").ToLowerInvariant();

        var trickCategories = trickForm.Categories?.Select(category => new TrickCategory {
            CategoryId = category,
            TrickId = trickForm.Id
        }).ToList() ?? new List<TrickCategory>();

        var trick = new Trick {
            Id = trickForm.Id,
            Name = trickForm.Name,
            Description = trickForm.Description,
            Difficulty = trickForm.Difficulty,
            TrickCategories = trickCategories
        };

        _appDbContext.Add(trick);
        await _appDbContext.SaveChangesAsync();

        var trickViewModel = GetTrickViewModel(trick);

        return trickViewModel;
    }

    // PUT api/tricks
    [HttpPut]
    public async Task<object?> Update([FromBody] Trick trick)
    {
        if (string.IsNullOrWhiteSpace(trick.Id))
            return null;

        _appDbContext.Add(trick);
        await _appDbContext.SaveChangesAsync();

        var trickViewModel = GetTrickViewModel(trick);

        return trickViewModel;
    }

    // DELETE api/tricks/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var trick = _appDbContext.Tricks
            .FirstOrDefault(t => (t.Id ?? "").Equals(id, StringComparison.InvariantCultureIgnoreCase));

        if (trick == null)
            return NotFound();

        trick.Deleted = true;
        await _appDbContext.SaveChangesAsync();

        return Ok();
    }

    private static object? GetTrickViewModel(Trick trick)
    {
        if (trick == null)
            return null;

        var trickViewModelExpression = TrickViewModel.Default.Compile();
        var trickViewModel = TrickViewModel.Default.Compile().Invoke(trick);

        return trickViewModel;
    }
}
