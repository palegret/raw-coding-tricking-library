namespace TrickingLibrary.Api.BackgroundServices.Messages
{
    public class EditVideoMessage
    {
        public int SubmissionId { get; set; }
        public string? Input { get; set; }
        public string? Output { get; set; }
    }
}
