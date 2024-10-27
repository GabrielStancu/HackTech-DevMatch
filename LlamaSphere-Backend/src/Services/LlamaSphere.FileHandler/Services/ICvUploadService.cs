namespace LlamaSphere.API.Services;

public interface ICvUploadService
{
    Task<string> UploadFileAsync(IFormFile file);
}