namespace TrickingLibrary.Api.Forms;

public class TrickForm
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Difficulty { get; set; }
    public IEnumerable<string>? Categories { get; set; }
}
