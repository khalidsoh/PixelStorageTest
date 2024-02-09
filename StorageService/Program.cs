using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// Add configuration
builder.Configuration.AddJsonFile("appsettings.json", optional: false);

// Add services to the container.

var app = builder.Build();

app.UseRouting();
// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.Map("/store", StoreHandler);

app.Run();

void StoreHandler(IApplicationBuilder app)
{
    app.UseEndpoints(endpoints =>
    {
        endpoints.MapPost("/store", async context =>
        {
            // Extract data from the request
            string data = await new StreamReader(context.Request.Body).ReadToEndAsync();

            // Store data in the append-only file
            await StoreInFileAsync(data);
        });
    });
}


async Task StoreInFileAsync(string data)
{
    // Get the file path from appsettings.json or environment variables
    string filePath = builder.Configuration.GetSection("FilePath").Value + "/tmp/visits.log";

    // Parse the incoming JSON data
    dynamic jsonData = JsonConvert.DeserializeObject(data);

    // Extract values from JSON data (referrer, user-agent, IP)
    string referrer = jsonData.Referrer ?? "null";
    string userAgent = jsonData.UserAgent ?? "null";
    string ipAddress = jsonData.IP;

    // Create or append to the file with the desired format
    await File.AppendAllTextAsync(filePath, $"{DateTime.UtcNow:o} | {referrer} | {userAgent} | {ipAddress}\n");
}
public partial class Program { }