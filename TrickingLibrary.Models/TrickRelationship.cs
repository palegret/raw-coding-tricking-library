namespace TrickingLibrary.Models;

public class TrickRelationship
{
    public required string PrerequisiteId { get; set; }
    public Trick? Prerequisite { get; set; }
    public required string ProgressionId { get; set; }
    public Trick? Progression { get; set; }
}
