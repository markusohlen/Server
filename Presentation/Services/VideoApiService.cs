using System.Net.Http.Headers;
using System.Text.Json;

namespace Presentation.Services;

public class VideoApiService(HttpClient httpClient)
{
    public async Task<List<UploadedMediaItem>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        using var response = await httpClient.GetAsync("videos", cancellationToken);
        response.EnsureSuccessStatusCode();

        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        var items = await JsonSerializer.DeserializeAsync<List<UploadedMediaItem>>(stream, cancellationToken: cancellationToken) ?? [];

        return items
            .Select(i =>
            {
                var url = string.IsNullOrWhiteSpace(i.AbsoluteUrl)
                    ? new Uri(httpClient.BaseAddress!, i.Url).ToString()
                    : i.AbsoluteUrl;
                return i with { AbsoluteUrl = url };
            })
            .ToList();
    }

    public async Task<string> UploadAsync(Stream content, string fileName, string? category, CancellationToken cancellationToken = default)
    {
        using var form = new MultipartFormDataContent();

        var streamContent = new StreamContent(content);
        streamContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

        form.Add(streamContent, "file", fileName);

        if (!string.IsNullOrWhiteSpace(category))
        {
            form.Add(new StringContent(category), "category");
        }

        using var response = await httpClient.PostAsync("videos/upload", form, cancellationToken);
        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadAsStringAsync(cancellationToken);

        // Minimal: return raw JSON (caller can parse later if needed)
        return payload;
    }
}

public sealed record UploadedMediaItem(
    string Name,
    string Url,
    string? AbsoluteUrl,
    long SizeBytes,
    DateTime LastModifiedUtc);
