using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PixelService.Test
{
    public class UnitTests
    {
        [Fact]
        public async Task TestPixelServiceTrack()
        {
            // Arrange
            await using var application = new WebApplicationFactory<Program>();
            using var client = application.CreateClient();

            // Act
            var response = await client.GetAsync("/track");

            // Assert
            response.EnsureSuccessStatusCode(); // Ensure the HTTP response indicates success
        }
    }
}
