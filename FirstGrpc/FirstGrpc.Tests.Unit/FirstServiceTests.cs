using Basics;
using FirstGrpc.Services;
using FirstGrpc.Tests.Unit.Helpers;
using FluentAssertions;

namespace FirstGrpc.Tests.Unit
{
    public class FirstServiceTests
    {
        private readonly IFirstService sut;

        public FirstServiceTests()
        {
            sut = new FirstService();
        }

        [Fact]
        public async Task Unary_ShouldReturn_An_Object()
        {
            // Arrange
            var request = new Request
            {
                Content = "message"
            };

            var callContext = TestServerCallContext.Create();

            // Act
            var response = await sut.Unary(request, callContext);

            // Assert
            var expectedResponse = new Response
            {
                Message = "message from server"
            };
            response.Should().BeEquivalentTo(expectedResponse);
        }
    }
}