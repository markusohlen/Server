using Microsoft.AspNetCore.Components;
using Presentation.Services;

namespace Presentation.Pages;

public partial class Videos : ComponentBase
{
    [Inject] private VideoApiService VideoApi { get; set; } = default!;

    private List<UploadedMediaItem> Items { get; set; } = [];
    private bool IsLoading { get; set; }
    private string? ErrorMessage { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await ReloadAsync();
    }

    private async Task ReloadAsync()
    {
        try
        {
            IsLoading = true;
            ErrorMessage = null;
            Items = await VideoApi.GetAllAsync();
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            Items = [];
        }
        finally
        {
            IsLoading = false;
        }
    }

    private static string FormatBytes(long bytes)
    {
        string[] suffixes = ["B", "KB", "MB", "GB", "TB"]; 
        double size = bytes;
        var order = 0;
        while (size >= 1024 && order < suffixes.Length - 1)
        {
            order++;
            size /= 1024;
        }

        return $"{size:0.##} {suffixes[order]}";
    }
}
