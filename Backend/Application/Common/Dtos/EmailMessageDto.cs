namespace Application.Common.Dtos;

public class EmailMessageDto
{
    public string ToEmail { get; set; }
    public string ToName { get; set; }
    public string Subject { get; set; }
    public string From { get; set; }
    public string Content { get; set; }
}