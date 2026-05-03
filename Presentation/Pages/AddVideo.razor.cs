using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Text.Json;
using Presentation.Services;

namespace Presentation.Pages;

public partial class AddVideo : ComponentBase
{
    [Inject] private NavigationManager Navigation { get; set; } = default!;
    [Inject] private VideoApiService VideoApi { get; set; } = default!;

    private IBrowserFile? SelectedFile { get; set; }
    private string? Category { get; set; }

    private bool IsUploading { get; set; }
    private bool CanUpload => SelectedFile != null && !IsUploading;

    private string? StatusMessage { get; set; }
    private Radzen.AlertStyle StatusSeverity { get; set; } = Radzen.AlertStyle.Info;
    private string? UploadedUrl { get; set; }

    private void OnFileSelected(InputFileChangeEventArgs e)
    {
        SelectedFile = e.File;
        StatusMessage = null;
        UploadedUrl = null;
    }

    private async Task OnUploadClicked()
    {
        if (SelectedFile == null)
        {
            return;
        }

        try
        {
            IsUploading = true;
            StatusSeverity = Radzen.AlertStyle.Info;
            StatusMessage = "Uploading...";
            UploadedUrl = null;

            await using var stream = SelectedFile.OpenReadStream(maxAllowedSize: 1024L * 1024L * 1024L);
            var json = await VideoApi.UploadAsync(stream, SelectedFile.Name, Category);

            var url = TryGetUrlFromJson(json);
            UploadedUrl = url;

            StatusSeverity = Radzen.AlertStyle.Success;
            StatusMessage = url is null ? "Uploaded." : "Uploaded successfully.";
        }
        catch (Exception ex)
        {
            StatusSeverity = Radzen.AlertStyle.Danger;
            StatusMessage = ex.Message;
        }
        finally
        {
            IsUploading = false;
        }
    }

    private static string? TryGetUrlFromJson(string json)
    {
        try
        {
            using var doc = JsonDocument.Parse(json);
            return doc.RootElement.TryGetProperty("url", out var url)
                ? url.GetString()
                : doc.RootElement.TryGetProperty("Url", out var urlPascal)
                    ? urlPascal.GetString()
                    : null;
        }
        catch
        {
            return null;
        }
    }

    private void OnBackClicked()
    {
        Navigation.NavigateTo("/videos");
    }
}
