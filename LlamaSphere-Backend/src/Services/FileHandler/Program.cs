using Core.Extensions;
using FileHandler.Configurations;
using FileHandler.Entities;
using FileHandler.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddConfiguration<ApiServiceConfiguration>("ParsingService");
        services.AddConfiguration<TableStorageConfiguration>("TableStorage");
        services.AddConfiguration<EmailConfiguration>("Email");

        services.AddScoped<IFileParserService, FileParserService>();
        services.AddHttpClient("fileParserClient", (sp, client)=>
        {
            var configuration = sp.GetService<ApiServiceConfiguration>();
            client.BaseAddress = new Uri(configuration.BaseUrl);
        });

        services.AddKeyedScoped<IAlertEmailSender, AlertAlertEmailSender>("alert");
        services.AddKeyedScoped<ITableStorageClient<CvEntity>, CvsTableStorageClient>("cvs");
        services.AddKeyedScoped<ITableStorageClient<JobEntity>, JobsTableStorageClient>("jobs");
        services.AddScoped<ICvTableStorageService, CvsTableStorageService>();
        services.AddScoped<IJobsTableStorageService, JobsTableStorageService>();
    })
    .Build();

host.Run();