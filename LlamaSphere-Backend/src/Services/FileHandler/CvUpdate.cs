using DocumentFormat.OpenXml.Packaging;
using FileHandler.Models;
using FileHandler.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FileHandler
{
    public class CvUpdate
    {
        private readonly IFileParserService _fileParserService;
        private readonly ICvTableStorageService _cvTableStorageService;
        private readonly IAlertEmailSender _alertEmailSender;
        private readonly ILogger<CvUpdate> _logger;

        public CvUpdate(IFileParserService fileParserService,
            ICvTableStorageService cvTableStorageService,
            [FromKeyedServices("alert")] IAlertEmailSender alertEmailSender,
            ILogger<CvUpdate> logger)
        {
            _fileParserService = fileParserService;
            _cvTableStorageService = cvTableStorageService;
            _alertEmailSender = alertEmailSender;
            _logger = logger;
        }

        [Function(nameof(CvUpdate))]
        public async Task Run([BlobTrigger("cvs/{file}", Connection = "StorageAccount:ConnectionString")] Stream stream, string file)
        {
            try
            {
                _logger.LogInformation("Parsing file {File}", file);

                var fileContent = ExtractFileContentAsync(stream);
                var id = file.Substring(0, file.IndexOf("_", StringComparison.OrdinalIgnoreCase));
                var parsedCv = await _fileParserService.ParseCvFileAsync(fileContent, id);

                await _cvTableStorageService.UpdateTableStorageContent(parsedCv);
                var emailModel = new NewCvEmailModel
                {
                    Name = $"{parsedCv.StructuredCv.FirstName} {parsedCv.StructuredCv.LastName}"
                };
                await _alertEmailSender.SendEmailAlertAsync(emailModel);
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
