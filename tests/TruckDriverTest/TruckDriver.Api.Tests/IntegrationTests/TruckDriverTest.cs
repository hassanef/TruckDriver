using FluentAssertions;
using System.Net;

namespace TruckDriver.Test.IntegrationTests
{
    public class TruckDriverTest: IClassFixture<TestWebApplicationFactory<Program>>
    {
        private readonly TestWebApplicationFactory<Program> _factory;

        public TruckDriverTest(TestWebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task When_location_Is_Correct_Then_Result_Is_OK()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync("/api/v1/TruckDriver/Hamburg");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        [Fact]
        public async Task When_location_Is_InCorrect_Then_Result_Is_NotFound()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync("/api/v1/TruckDriver/Wien");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}




