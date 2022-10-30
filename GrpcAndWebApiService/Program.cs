using System.Diagnostics;
using GrpcAndWebApiService.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Serilog;
using Serilog.Events;
using Serilog.Filters;

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

            // var logger = new LoggerConfiguration()
            //     .ReadFrom.Configuration(builder.Configuration)
            //     .Enrich.FromLogContext()
            //     .CreateLogger();

            // var logger = new LoggerConfiguration()
            //     .WriteTo.Logger(c => c
            //         .Filter.ByIncludingOnly(Matching.WithProperty("EventId", 1))
            //         .WriteTo.File(@"..\file1-.log"))
            //     .WriteTo.Logger(c => c
            //         .Filter.ByIncludingOnly(Matching.WithProperty("EventId", 2))
            //         .WriteTo.File(@"..\file2-.log"))
            //     .CreateLogger();
            // Log.Logger = logger;

            var template = "[{Timestamp:HH:mm:ss} {CorrelationId} {Level:u3}] {Username} {Message:lj}{NewLine}{Exception}";
            var logger = new LoggerConfiguration()
                .WriteTo.Logger(cc => cc
                    .Filter.ByIncludingOnly(
                        WithProperty("EventId",1001))
                    .WriteTo
                    .File("../log-grpc/LogGrpc-.txt", outputTemplate: template, restrictedToMinimumLevel: LogEventLevel.Debug, 
                        rollingInterval: RollingInterval.Day, flushToDiskInterval: TimeSpan.FromSeconds(1)))
                .WriteTo.Logger(cc => cc
                    .Filter.ByIncludingOnly(
                        WithProperty("EventId", 1002))
                    .WriteTo
                    .File("../log-web-api/LogWebApi-.txt", outputTemplate:template, restrictedToMinimumLevel: LogEventLevel.Debug, 
                        rollingInterval: RollingInterval.Day, flushToDiskInterval: TimeSpan.FromSeconds(1)))
                .Enrich.FromLogContext()
                .CreateLogger();

            
            builder.Logging.ClearProviders();
            builder.Logging.AddSerilog(logger);

            var app = builder.Build();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. " +
                //                      "To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                endpoints.MapGrpcService<GreeterService>();
                endpoints.MapControllers();
            });

           
            app.Run();
        }

        private static Func<LogEvent, bool> WithProperty(string propertyName, object scalarValue)
        {
            if (propertyName == null) throw new ArgumentNullException(nameof(propertyName));           
            ScalarValue scalar = new ScalarValue(scalarValue);
            return e=>
            {
                if (e.Properties.TryGetValue(propertyName, out var propertyValue))
                {
                    if (propertyValue is StructureValue stValue)
                    {
                        var value = stValue.Properties.FirstOrDefault(cc => cc.Name == "Id");
                        if (value == null) return false;
                        bool result = scalar.Equals(value.Value);
                        return result;
                    }
                }
                return false;
            };
        }
    }
}