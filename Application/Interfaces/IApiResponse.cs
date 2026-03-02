namespace Application.Models;

public interface IApiResponse
{
    bool Success { get; }
    string? Message { get; }
}
