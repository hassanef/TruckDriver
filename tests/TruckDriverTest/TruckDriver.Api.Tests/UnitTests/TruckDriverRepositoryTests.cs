using FluentAssertions;
using Microsoft.Azure.Cosmos;
using Moq;
using TruckDriver.Test.TestDoubles;
using TruckDriver.Domain.Models;
using TruckDriver.Infrastructure.Repositories;

public class TruckDriverRepositoryTests
{
    private readonly Mock<Container> _mockContainer;
    private readonly Mock<FeedIterator<Driver>> _mockIterator;

    public TruckDriverRepositoryTests()
    {
        _mockContainer = new Mock<Container>();
        _mockIterator = new Mock<FeedIterator<Driver>>();
    }

    [Fact]
    public async Task When_Location_Is_Correct_Then_Result_Is_List_Of_Drivers()
    {
        var expectedLocation = "Hamburg";
        var expectedDrivers = CreateDrivers(expectedLocation);

        var mockFeedResponse = new MockFeedResponse<Driver>(expectedDrivers);
        _mockIterator.Setup(i => i.ReadNextAsync(default))
                    .ReturnsAsync(mockFeedResponse);

        _mockIterator.SetupSequence(i => i.HasMoreResults)
          .Returns(true)
            .Returns(false);

        _mockContainer.Setup(c => c.GetItemQueryIterator<Driver>(
                It.IsAny<QueryDefinition>(),
                It.IsAny<string>(),
                It.IsAny<QueryRequestOptions>()))
            .Returns(_mockIterator.Object);

        var repository = new TruckDriverRepository(_mockContainer.Object);
        var actualDrivers = await repository.GetAsync(expectedLocation);

        actualDrivers.Should().NotBeNull();
        actualDrivers.Should().NotBeEmpty();
        actualDrivers.Should().HaveCount(expectedDrivers.Count);
        actualDrivers.Should().BeEquivalentTo(expectedDrivers);
    }
    [Fact]
    public async Task When_Location_Is_Wrong_Then_Result_Is_Empty()
    {
        var invalidLocation = "Wien";
        var expectedDrivers = new List<Driver>();

        var mockFeedResponse = new MockFeedResponse<Driver>(expectedDrivers);
        _mockIterator.Setup(i => i.ReadNextAsync(default))
                    .ReturnsAsync(mockFeedResponse);

        _mockContainer.Setup(c => c.GetItemQueryIterator<Driver>(
                It.IsAny<QueryDefinition>(),
                It.IsAny<string>(),
                It.IsAny<QueryRequestOptions>()))
            .Returns(_mockIterator.Object);

        var repository = new TruckDriverRepository(_mockContainer.Object);
        var actualDrivers = await repository.GetAsync(invalidLocation);

        actualDrivers.Should().BeEmpty();
        actualDrivers.Should().NotBeNull();
        actualDrivers.Should().HaveCount(expectedDrivers.Count);
        actualDrivers.Should().BeEquivalentTo(expectedDrivers);
    }
    private static List<Driver> CreateDrivers(string expectedLocation)
    {
        var expectedDrivers = new List<Driver>
       {
           new("1", "Steve", "Kahn", expectedLocation),
           new("2", "Hani", "Cool", expectedLocation)
       };

        return expectedDrivers;
    }
}



