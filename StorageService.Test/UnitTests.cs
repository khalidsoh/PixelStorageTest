using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Text;

namespace StorageService.Test
{
    public class UnitTests
    {
        [Fact]
        public async Task TestStorageService()
        {
            // Arrange
            await using var application = new WebApplicationFactory<Program>();
            using var client = application.CreateClient();

            var requestData = new
            {
                Referrer = "https://example.com",
                UserAgent = "TestUserAgent",
                IP = "192.168.1.1"
            };

            var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("/store", content);

            // Assert
            response.EnsureSuccessStatusCode(); // Ensure the HTTP response indicates success
        }
    }
}
