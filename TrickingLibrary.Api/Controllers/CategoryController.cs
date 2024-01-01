using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrickingLibrary.Data;
using TrickingLibrary.Models;

namespace TrickingLibrary.Api.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoryController : ControllerBase
{
    private readonly AppDbContext _appDbContext;

    public CategoryController(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    // GET api/categories
    [HttpGet]
    public IEnumerable<Category> All() =>
        _appDbContext.Categories.ToList();

    // GET api/categories/{id}
    [HttpGet("{id}")]
    public Category? Get(string id) =>
        _appDbContext.Categories
            .FirstOrDefault(c => (c.Id ?? string.Empty).Equals(id, StringComparison.InvariantCultureIgnoreCase));

    // GET api/{id}/tricks
    [HttpGet("{id}/tricks")]
    public IEnumerable<Trick?> CategoryTricks(string id) =>
        _appDbContext.TrickCategories
            .Include(tc => tc.Trick)
            .Where(tc => (tc.CategoryId ?? string.Empty).Equals(id, StringComparison.InvariantCultureIgnoreCase))
            .Select(tc => tc.Trick)
            .ToList();

    // POST api/categories
    [HttpPost]
    public async Task<Category?> Create([FromBody] Category category)
    {
        if (category == null || string.IsNullOrWhiteSpace(category.Name))
            return null;

        category.Id = category.Name.Replace(" ", "-").ToLowerInvariant();

        _appDbContext.Add(category);
        await _appDbContext.SaveChangesAsync();

        return category;
    }

    // PUT api/categories
    [HttpPut]
    public async Task<Category?> Update([FromBody] Category category)
    {
        if (string.IsNullOrWhiteSpace(category.Id))
            return null;

        _appDbContext.Add(category);
        await _appDbContext.SaveChangesAsync();

        return category;
    }

    // DELETE api/categories/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var category = Get(id);

        if (category == null)
            return NotFound();

        category.Deleted = true;
        await _appDbContext.SaveChangesAsync();

        return Ok();
    }
}
