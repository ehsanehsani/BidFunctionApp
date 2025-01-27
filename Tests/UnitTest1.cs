using BidFunctionApp.Handlers;
using BidFunctionApp.Models;
using BidFunctionApp.Requests;
using Moq;
using RabbitMQ.Client;
using FluentAssertions;

namespace BidFunctionAppTests
{
    public class Tests
    {
        private Mock<IConnectionFactory> mockFactoryService;
        private Mock<IConnection> mockConnection;
        private Mock<IChannel> mockChannel;

        private ProcessBidHandler bidHandler;

        [SetUp]
        public void Setup()
        {
            mockFactoryService = new Mock<IConnectionFactory>();
            mockConnection = new Mock<IConnection>();
            mockChannel = new Mock<IChannel>();

            mockFactoryService.Setup(x => x.CreateConnectionAsync(It.IsAny<CancellationToken>())).ReturnsAsync(mockConnection.Object);
            mockConnection.Setup(x => x.CreateChannelAsync(It.IsAny<CreateChannelOptions>(), It.IsAny<CancellationToken>())).ReturnsAsync(mockChannel.Object);

            bidHandler = new ProcessBidHandler(mockFactoryService.Object);
        }

        [Test]
        public async Task RefereceId_should_be_generated()
        {
            var bidRequest = new ProcessBidRequest(new Bid()
            {
                Id = 1,
            });

            var result = await bidHandler.Handle(bidRequest,new CancellationToken());

            result.Should().NotBeNull();

            Assert.That(Guid.TryParse(result, out var _), Is.True);
        }
    }
}