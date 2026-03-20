namespace Infrastructure.AI;

public class AiOptions
{
    public const string SectionName = "AI";

    public string Provider { get; set; } = "Gemini";
    public string Endpoint { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
    public string Model { get; set; } = "gemini-2.0-flash";
}
