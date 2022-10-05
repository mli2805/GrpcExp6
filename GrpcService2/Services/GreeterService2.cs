using Grpc.Core;

namespace GrpcService2.Services
{
    public class GreeterService2 : AnotherGreeter.AnotherGreeterBase
    {
        private readonly ILogger<GreeterService2> _logger;
        public GreeterService2(ILogger<GreeterService2> logger)
        {
            _logger = logger;
        }

        public override Task<AnotherHelloReply> AnotherSayHello(AnotherHelloRequest request, ServerCallContext context)
        {
            _logger.LogInformation("AnotherSayHello");
            return Task.FromResult(new AnotherHelloReply
            {
                Message = "Hello " + request.Name + $" ({request.UserId})"
            });
        }
    }
}