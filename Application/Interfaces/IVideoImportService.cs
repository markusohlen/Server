namespace Application.Interfaces;

public interface IVideoImportService
{
    Task ImportByCategoryAsync(string category);
}
