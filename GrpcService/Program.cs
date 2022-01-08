using GrpcService.Services;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc();

var app = builder.Build();

// Configure the HTTP request pipeline.
// app.MapGrpcService<GreeterService>().RequireHost("*:5001");
// app.MapGrpcService<SecondService>().RequireHost("*:5001");

app.UseRouting();
app.MapWhen(context => context.Connection.LocalPort == 1000, newApp => {
    newApp.UseRouting();
    newApp.UseEndpoints(endpoints =>
    {
        endpoints.MapGrpcService<GreeterService>();
    });
});
app.MapWhen(context => context.Connection.LocalPort == 2000, newApp => {
    newApp.UseRouting();
    newApp.UseEndpoints(endpoints =>
    {
        endpoints.MapGrpcService<SecondService>();
    });
});


app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
