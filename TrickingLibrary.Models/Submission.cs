namespace TrickingLibrary.Models;

public class Submission : BaseModel<int>
{
    public required string TrickId { get; set; }
    public string Video { get; set; }
    public bool VideoProcessed { get; set; }
    public string Description { get; set; }

    public Submission()
    {
        Video = string.Empty;
        VideoProcessed = false;
        Description = string.Empty;
    }
}
