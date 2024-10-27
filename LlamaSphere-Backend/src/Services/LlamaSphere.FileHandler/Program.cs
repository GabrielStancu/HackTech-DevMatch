using LlamaSphere.API.Business.TableStorage;
using LlamaSphere.API.Configuration;
using LlamaSphere.API.Entities;
using LlamaSphere.API.Services;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("https://0.0.0.0:7037", "http://0.0.0.0:5045");

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

builder.Services.Configure<TableStorageConfiguration>(builder.Configuration.GetSection("FileTableStorage"));
builder.Services.Configure<BlobStorageConfiguration>(builder.Configuration.GetSection("FileBlobStorage"));
builder.Services.Configure<ApiServiceConfiguration>(builder.Configuration.GetSection("ReasoningApi"));
builder.Services.Configure<ResponseEmailConfiguration>(builder.Configuration.GetSection("ResponseEmail"));
builder.Services.Configure<MatchingPerformanceConfiguration>(builder.Configuration.GetSection("MatchingPerformance"));
builder.Services.AddKeyedScoped(typeof(ITableStorageClient<CvEntity>), "cv", typeof(CvsTableStorageClient));
builder.Services.AddKeyedScoped(typeof(ITableStorageClient<JobEntity>), "job", typeof(JobsTableStorageClient));
builder.Services.AddScoped<ICvUploadService, CvUploadService>();
builder.Services.AddScoped<IJobUploadService, JobUploadService>();
builder.Services.AddScoped<IJobMatchingCvsService, JobMatchingCvsService>();
builder.Services.AddScoped<ICvMatchingJobsService, CvMatchingJobsService>();
builder.Services.AddScoped<IResponseEmailSender, ResponseEmailSender>();
builder.Services.AddHttpClient("reasoning", (sp, client) =>
{
    var configuration = sp.GetService<IOptions<ApiServiceConfiguration>>();
    client.BaseAddress = new Uri(configuration.Value.BaseUrl);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.MapControllers();

app.Run();
