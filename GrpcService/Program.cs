using GrpcService.Services;

namespace GrpcService;

class Program
{
    static void Main(string[] args)
    {

        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        builder.WebHost.ConfigureKestrel(options =>
        {
            options.ListenAnyIP(6000);
            options.ListenAnyIP(6002);
            options.ListenAnyIP(6001, listenOptions => { listenOptions.UseHttps(); });
            options.ListenAnyIP(6003, listenOptions => { listenOptions.UseHttps(); });
        });

        // Additional configuration is required to successfully run gRPC on macOS.
        // For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

        // Add services to the container.
        builder.Services.AddGrpc();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        app.MapGrpcService<GreeterService>();
        app.MapGrpcService<SecondService>();

        app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

        app.Run();
    }

}