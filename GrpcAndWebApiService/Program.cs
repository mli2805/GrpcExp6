using GrpcAndWebApiService.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace GrpcAndWebApiService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.WebHost.ConfigureKestrel(options =>
            {
                // grpc
                options.ListenAnyIP(11740, o => o.Protocols = HttpProtocols.Http2);
                // http
                options.ListenAnyIP(11738, o => o.Protocols = HttpProtocols.Http1);
            });

            // Add services to the container.
            builder.Services.AddGrpc();
            builder.Services.AddControllers();

            var app = builder.Build();
            app.UseRouting();
            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. " +
                //                      "To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                endpoints.MapGrpcService<GreeterService>();
                endpoints.MapControllers();
            });

           
            app.Run();
        }
    }
}