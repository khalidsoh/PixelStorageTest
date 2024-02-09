using Newtonsoft.Json;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add configuration
builder.Configuration.AddJsonFile("appsettings.json", optional: false);

// Add services
builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.

//app.UseHttpsRedirection();


// Configure the endpoint
app.MapGet("/track", async context =>
{
    var referrer = context.Request.Headers["Referer"].ToString();
    var userAgent = context.Request.Headers["User-Agent"].ToString();
    var ipAddress = context.Connection.RemoteIpAddress?.ToString();

    // Send data to Storage Service
    var storageServiceUrl = builder.Configuration["StorageServiceUrl"];
    await SendDataToStorageService(storageServiceUrl, referrer, userAgent, ipAddress);

    // Return 1x1 transparent GIF
    var transparentGifBytes = Convert.FromBase64String("R0lGODlhAQABAIAAAP///wAAACH5BAEAAAAALAAAAAABAAEAAAICRAEAOw==");
    await context.Response.Body.WriteAsync(transparentGifBytes);
});

app.Run();

async Task SendDataToStorageService(string storageServiceUrl, string referrer, string userAgent, string ipAddress)
{
    try
    {
        using var client = new HttpClient();
        var content = new StringContent(JsonConvert.SerializeObject(new
        {
            Referrer = referrer,
            UserAgent = userAgent,
            IP = ipAddress
        }), Encoding.UTF8, "application/json");

        await client.PostAsync($"{storageServiceUrl}/store", content);
    }
    catch (Exception ex)
    {
        // Log the exception
        Console.WriteLine($"Error sending data to Storage Service: {ex.Message}");
    }
}
public partial class Program { }
