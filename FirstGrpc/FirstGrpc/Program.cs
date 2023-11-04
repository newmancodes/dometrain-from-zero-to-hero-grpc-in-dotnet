using System.IO.Compression;
using FirstGrpc.Interceptors;
using FirstGrpc.Services;
using Microsoft.Extensions.Diagnostics.HealthChecks;

public class Program
{
    public static void Main(string[] args)
    {

        var builder = WebApplication.CreateBuilder(args);

        // Additional configuration is required to successfully run gRPC on macOS.
        // For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

        // Add services to the container.
        builder.Services.AddTransient<ServerLoggingInterceptor>();
        builder.Services.AddGrpc(options =>
        {
            options.ResponseCompressionAlgorithm = "gzip";
            options.ResponseCompressionLevel = CompressionLevel.SmallestSize;
            options.Interceptors.Add<ServerLoggingInterceptor>();
        });
        builder.Services.AddGrpcReflection();
        builder.Services.AddGrpcHealthChecks(options =>
        {
        }).AddCheck("my cool service", () => HealthCheckResult.Healthy(), new[] { "grpc", "live" });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        app.MapGrpcService<FirstService>();
        app.MapGrpcService<GreeterService>();
        app.MapGrpcHealthChecksService();
        app.MapGrpcReflectionService();
        app.MapGet("/",
            () =>
                "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

        app.Run();
    }
}