using Grpc.Core;
using Grpc.Core.Interceptors;

namespace FirstGrpc.Interceptors
{
    public class ServerLoggingInterceptor : Interceptor
    {
        private readonly ILogger<ServerLoggingInterceptor> _logger;

        public ServerLoggingInterceptor(ILogger<ServerLoggingInterceptor> logger)
        {
            _logger = logger;
        }

        public override Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
            TRequest request,
            ServerCallContext context,
            UnaryServerMethod<TRequest, TResponse> continuation)
        {
            _logger.LogInformation($"Starting the server call.");
            return continuation(request, context);
        }
    }
}
