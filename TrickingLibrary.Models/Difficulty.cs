namespace TrickingLibrary.Models;

public class Difficulty : BaseModel<string>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public IList<Trick> Tricks { get; set; }

    public Difficulty()
    {
        Name = string.Empty;
        Description = string.Empty;
        Tricks = new List<Trick>();
    }
}
