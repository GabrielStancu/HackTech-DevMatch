using DocumentFormat.OpenXml.Packaging;
using FileHandler.Models;
using FileHandler.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FileHandler
{
    public class JobUpdate
    {
        private readonly IFileParserService _fileParserService;
        private readonly IJobsTableStorageService _jobsTableStorageService;
        private readonly IAlertEmailSender _alertEmailSender;
        private readonly ILogger<JobUpdate> _logger;

        public JobUpdate(IFileParserService fileParserService,
            IJobsTableStorageService jobsTableStorageService,
            [FromKeyedServices("alert")] IAlertEmailSender alertEmailSender,
            ILogger<JobUpdate> logger)
        {
            _fileParserService = fileParserService;
            _jobsTableStorageService = jobsTableStorageService;
            _alertEmailSender = alertEmailSender;
            _logger = logger;
        }

        [Function(nameof(JobUpdate))]
        public async Task Run([BlobTrigger("jobs/{file}", Connection = "StorageAccount:ConnectionString")] Stream stream, string file)
        {
            try
            {
                _logger.LogInformation("Parsing file {File}", file);

                var fileContent = ExtractFileContentAsync(stream);
                string id = file.Substring(0, file.IndexOf("_", StringComparison.OrdinalIgnoreCase));
                var parsedJob = await _fileParserService.ParseJobFileAsync(fileContent, id);

                await _jobsTableStorageService.UpdateTableStorageContent(parsedJob);
                await _alertEmailSender.SendEmailAlertAsync(new NewJobEmailModel
                {
                    Name = parsedJob.StructuredJob.JobTitle
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error processing blob. Exception: {ex.Message}");
            }
        }

        private static string ExtractFileContentAsync(Stream stream)
        {
            using var wordDoc = WordprocessingDocument.Open(stream, false);
            var body = wordDoc.MainDocumentPart?.Document.Body;
            var content = body?.InnerText.Replace("\"", "'") ?? string.Empty;

            return content;
        }
    }
}
