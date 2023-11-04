using Basics;

namespace FirstGrpc.Tests.Integration
{
    public class FirstServiceTests : IClassFixture<MyFactory<Program>>
    {
        private readonly MyFactory<Program> factory;

        public FirstServiceTests(MyFactory<Program> factory)
        {
            this.factory = factory;
        }

        [Fact]
        public void GetUnaryMessage()
        {
            // Arrange
            var client = factory.CreateGrpcClient();
            
            // Act
            var actualResponse = client.Unary(new Request { Content = "message" });

            // Assert
            var expectedResponse = new Response { Message = "message from server" };
        }
    }
}