namespace Infrastructure.AI;

public class AiOptions
{
    public const string SectionName = "AI";

    public string Endpoint { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
    public string Model { get; set; } = "gpt-4o-mini";
}
