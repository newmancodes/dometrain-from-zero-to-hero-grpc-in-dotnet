using Grpc.Core.Interceptors;

namespace MvcClient.Interceptors
{
    public class ClientLoggerInterceptor : Interceptor
    {
        private readonly ILogger<ClientLoggerInterceptor> _logger;

        public ClientLoggerInterceptor(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ClientLoggerInterceptor>();
        }

        public override TResponse BlockingUnaryCall<TRequest, TResponse>(
            TRequest request,
            ClientInterceptorContext<TRequest, TResponse> context,
            BlockingUnaryCallContinuation<TRequest, TResponse> continuation)
        {
            try
            {
                _logger.LogInformation($"Starting the client call of type: {context.Method.FullName}, Type: {context.Method.Type}.");
                return continuation(request, context);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error in BlockingUnaryCall");
                throw;
            }
        }
    }
}
