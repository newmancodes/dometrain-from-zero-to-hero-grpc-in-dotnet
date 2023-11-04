﻿using Basics;
using Grpc.Core;

namespace FirstGrpc.Services;

public class FirstService : FirstServiceDefinition.FirstServiceDefinitionBase
{
    private readonly ILogger<FirstService> _logger;

    public FirstService(ILogger<FirstService> logger)
    {
        _logger = logger;
    }

    public override Task<Response> Unary(Request request, ServerCallContext context)
    {
        var response = new Response
        {
            Message = request.Content + " from server"
        };
        return Task.FromResult(response);
    }

    public override async Task<Response> ClientStream(IAsyncStreamReader<Request> requestStream, ServerCallContext context)
    {
        var response = new Response { Message = "I got " };
        var isFirstMessage = true;

        while (await requestStream.MoveNext())
        {
            var requestPayload = requestStream.Current;
            
            if (!isFirstMessage)
            {
                response.Message += ", ";
            }

            response.Message += requestPayload.Content;
            isFirstMessage = false;
        }

        return response;
    }

    public override async Task ServerStream(Request request, IServerStreamWriter<Response> responseStream, ServerCallContext context)
    {
        for (var i = 0; i < 100; i++)
        {
            if (context.CancellationToken.IsCancellationRequested)
            {
                break;
            }

            var response = new Response
            {
                Message = i.ToString()
            };

            await responseStream.WriteAsync(response);
        }
    }

    public override async Task BiDirectionalStream(
        IAsyncStreamReader<Request> requestStream,
        IServerStreamWriter<Response> responseStream,
        ServerCallContext context)
    {
        var response = new Response { Message = string.Empty };
        while (await requestStream.MoveNext())
        {
            var requestPayload = requestStream.Current;
            response.Message = requestPayload.ToString();
            await responseStream.WriteAsync(response);
        }
    }
}