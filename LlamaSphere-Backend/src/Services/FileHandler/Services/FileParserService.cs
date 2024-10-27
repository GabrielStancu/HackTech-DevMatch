using System.Net.Http.Json;
using System.Text.Json.Nodes;
using FileHandler.DTOs;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace FileHandler.Services;

public class FileParserService : IFileParserService
{
    private readonly HttpClient _client;

    public FileParserService(IHttpClientFactory factory)
    {
        _client = factory.CreateClient("fileParserClient");
    }

    public async Task<ParsedCv> ParseCvFileAsync(string content, string id)
    {
        var parseCvRequest = new ParseCvRequest
        {
            CvText = $"id:{id} {content}"
        };
        var result = await _client.PostAsJsonAsync("structure-cv", parseCvRequest);
        var resultContent = await result.Content.ReadAsStringAsync();
        var parsedCv = JsonSerializer.Deserialize<ParsedCv>(resultContent);
        
        return parsedCv;
    }

    public async Task<ParsedJob> ParseJobFileAsync(string content, string id)
    {
        var parseJobRequest = new ParseJobRequest
        {
            JobText = $"id:{id} {content}"
        };
        var result = await _client.PostAsJsonAsync("structure-job", parseJobRequest);
        var resultContent = await result.Content.ReadAsStringAsync();
        var parsedJob = JsonSerializer.Deserialize<ParsedJob>(resultContent);

        return parsedJob;
    }
}
