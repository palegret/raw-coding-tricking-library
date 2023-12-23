using Microsoft.AspNetCore.Mvc;
using TrickingLibrary.Api.Models;

namespace TrickingLibrary.Api.Controllers;

[ApiController]
[Route("api/tricks")]
public class TricksController : ControllerBase
{
    private readonly TrickyStore _trickyStore;

    public TricksController(TrickyStore trickyStore)
    {
        _trickyStore = trickyStore;
    }

    // GET api/tricks
    [HttpGet]
    public IActionResult All() => Ok(_trickyStore.All);

    // GET api/tricks/{id}
    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var trick = _trickyStore.All.FirstOrDefault(x => x.Id.Equals(id));

        if (trick == null)
            return NotFound();

        return Ok(trick);
    }

    // GET api/tricks/{id}/submissions/{submissionId}
    [HttpGet("{id}")]
    public IActionResult GetSubmissions(int id)
    {
        var trick = _trickyStore.All.FirstOrDefault(x => x.Id.Equals(id));

        if (trick == null)
            return NotFound();

        return Ok(trick);
    }

    // POST api/tricks
    [HttpPost]
    public IActionResult Create([FromBody] Trick trick)
    {
        _trickyStore.Add(trick);
        return Ok(trick);
    }

    // PUT api/tricks
    [HttpPut]
    public IActionResult Update([FromBody] Trick trick)
    {
        throw new NotImplementedException();
    }

    // DELETE api/tricks/{id}
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        throw new NotImplementedException();
    }
}
