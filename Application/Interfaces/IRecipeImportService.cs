using Application.Models;

namespace Application.Interfaces;

public interface IRecipeImportService
{
    Task<ImportedRecipeData?> ImportFromUrlAsync(string url);
}
