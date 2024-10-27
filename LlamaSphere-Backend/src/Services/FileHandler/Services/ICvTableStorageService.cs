using FileHandler.DTOs;

namespace FileHandler.Services;

public interface ICvTableStorageService
{
    Task UpdateTableStorageContent(ParsedCv parsedCv);
}