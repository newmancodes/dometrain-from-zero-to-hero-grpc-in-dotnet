using Grpc.Core;

namespace FirstGrpc.Tests.Unit.Helpers;

public class TestServerCallContext : ServerCallContext
{
    private readonly Metadata requestHeaders;
    private readonly CancellationToken cancellationToken;

    private TestServerCallContext(Metadata requestHeaders, CancellationToken cancellationToken)
    {
        this.requestHeaders = requestHeaders;
        this.cancellationToken = cancellationToken;
    }

    protected override Task WriteResponseHeadersAsyncCore(Metadata responseHeaders)
    {
        throw new NotImplementedException();
    }

    protected override ContextPropagationToken CreatePropagationTokenCore(ContextPropagationOptions? options)
    {
        throw new NotImplementedException();
    }

    protected override string MethodCore => "MethodName";
    protected override string HostCore => "HostName";
    protected override string PeerCore => "PeerName";
    protected override DateTime DeadlineCore { get; }
    protected override Metadata RequestHeadersCore => throw new NotImplementedException();
    protected override CancellationToken CancellationTokenCore => throw new NotImplementedException();
    protected override Metadata ResponseTrailersCore => throw new NotImplementedException();
    protected override Status StatusCore { get; set; }
    protected override WriteOptions? WriteOptionsCore { get; set; }
    protected override AuthContext AuthContextCore { get; }

    public static TestServerCallContext Create()
    {
        return new TestServerCallContext(new Metadata(), CancellationToken.None);
    }
}