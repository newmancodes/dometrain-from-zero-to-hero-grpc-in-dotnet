using Basics;
using Grpc.Core;
using Grpc.Net.Client;

var options = new GrpcChannelOptions
{

};

await Task.Delay(2000);

using var cancellationTokenSource = new CancellationTokenSource();
using var channel = GrpcChannel.ForAddress("http://localhost:5009", options);
var client = new FirstServiceDefinition.FirstServiceDefinitionClient(channel);

await Unary(client, cancellationTokenSource.Token);
await ClientStreaming(client, cancellationTokenSource.Token);
await ServerStreaming(client, cancellationTokenSource.Token);
await BidirectionalStreaming(client, cancellationTokenSource.Token);

Console.ReadLine();
cancellationTokenSource.Cancel();
return;

async Task Unary(FirstServiceDefinition.FirstServiceDefinitionClient client, CancellationToken cancellationToken)
{
    var request = new Request
    {
        Content = "Hello"
    };

    var response = await client.UnaryAsync(request, /*deadline: DateTime.UtcNow.AddMilliseconds(1), */cancellationToken: cancellationToken);

    Console.WriteLine(response.Message);
}

async Task ClientStreaming(
    FirstServiceDefinition.FirstServiceDefinitionClient client,
    CancellationToken cancellationToken)
{
    using var call = client.ClientStream(cancellationToken: cancellationToken);
    for (var i = 0; i < 1000; i++)
    {
        await call.RequestStream.WriteAsync(new Request { Content = i.ToString() }, cancellationToken);
    }

    await call.RequestStream.CompleteAsync();
    var response = await call.ResponseAsync;
    Console.WriteLine(response.Message);
}

async Task ServerStreaming(
    FirstServiceDefinition.FirstServiceDefinitionClient client,
    CancellationToken cancellationToken)
{
    try
    {
        var metadata = new Metadata
        {
            { "my-first-key", "my-first-value" },
            { "my-second-key", "my-second-value" }
        };

        using var call = client.ServerStream(
            new Request { Content = "Hello!" },
            headers: metadata,
            cancellationToken: cancellationToken);

        await foreach (var response in call.ResponseStream.ReadAllAsync(cancellationToken))
        {
            Console.WriteLine(response.Message);
        }

        var trailers = call.GetTrailers();
        foreach (var trailer in trailers)
        {
            Console.WriteLine($"{trailer.Key} {trailer.Value}");
        }
    }
    catch (RpcException ex) when (ex.StatusCode == StatusCode.Cancelled)
    {
    }
}

async Task BidirectionalStreaming(
    FirstServiceDefinition.FirstServiceDefinitionClient client,
    CancellationToken cancellationToken)
{
    using var call = client.BiDirectionalStream(cancellationToken: cancellationToken);
    var request = new Request();
    for (var i = 0; i < 10; i++)
    {
        request.Content = i.ToString();
        await call.RequestStream.WriteAsync(request, cancellationToken);
    }

    await foreach (var response in call.ResponseStream.ReadAllAsync(cancellationToken))
    {
        Console.WriteLine(response.Message);
    }

    await call.RequestStream.CompleteAsync();
}