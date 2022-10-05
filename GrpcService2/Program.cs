using Grpc.Net.Client;
using GrpcService;
using GrpcService2.Services;

namespace GrpcService2;

class Program
{
    static void Main(string[] args)
    {
        Task.Factory.StartNew(Work);

        var builder = WebApplication.CreateBuilder(args);
        builder.WebHost.ConfigureKestrel(options =>
        {
            options.ListenAnyIP(6010);
            //options.ListenAnyIP(6011, listenOptions => { listenOptions.UseHttps(); });
        });

        // Add services to the container.
        builder.Services.AddGrpc();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        app.MapGrpcService<GreeterService2>();
        app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

        app.Run();
    }

    private static async void Work()
    {
        using var channel1 = GrpcChannel.ForAddress("http://localhost:6000");

        while (true)
        {
            var client1 = new Greeter.GreeterClient(channel1);
            var response = await client1.SayHelloAsync(new HelloRequest() { Name = DateTime.Now.ToShortTimeString() });
            Console.WriteLine($"Service1 response: {response.Message}");
            await Task.Delay(3000);
        }

        // ReSharper disable once FunctionNeverReturns
    }
}

