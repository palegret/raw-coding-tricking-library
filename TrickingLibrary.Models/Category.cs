namespace TrickingLibrary.Models;

public class Category : BaseModel<string>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public IList<Trick> Tricks { get; set; }

    public Category()
    {
        Name = string.Empty; 
        Description = string.Empty;
        Tricks = new List<Trick>();
    }
}
