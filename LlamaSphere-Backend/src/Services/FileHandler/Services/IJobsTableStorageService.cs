using FileHandler.DTOs;

namespace FileHandler.Services;

public interface IJobsTableStorageService
{
    Task UpdateTableStorageContent(ParsedJob parsedJob);
}