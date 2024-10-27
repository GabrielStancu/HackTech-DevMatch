using FileHandler.DTOs;

namespace FileHandler.Services;

public interface IFileParserService
{
    Task<ParsedCv> ParseCvFileAsync(string content, string id);
    Task<ParsedJob> ParseJobFileAsync(string content, string id);
}