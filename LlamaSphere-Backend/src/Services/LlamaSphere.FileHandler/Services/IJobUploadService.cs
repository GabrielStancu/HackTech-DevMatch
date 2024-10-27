namespace LlamaSphere.API.Services;

public interface IJobUploadService
{
    Task<string> UploadFileAsync(IFormFile file);
}