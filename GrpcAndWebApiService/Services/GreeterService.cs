using Grpc.Core;
using Serilog;
using Serilog.Events;

namespace GrpcAndWebApiService.Services
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            _logger.Log(LogLevel.Information, new EventId(1001), "SayHello requested 1001 ");
            _logger.Log(LogLevel.Information, new EventId(1003), "SayHello requested 1003");
            return Task.FromResult(new HelloReply
            {
                Message = "Hello " + request.Name
            });
        }
    }
}