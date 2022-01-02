using Google.Protobuf;
using Grpc.Core;

namespace GrpcService.Services
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
            return Task.FromResult(new HelloReply
            {
                Message = "Hello, " + request.Name
            });
        }

        public override Task<PriceResponse> SayPrice(PriceRequest request, ServerCallContext context)
        {
            var bytes = new byte[] { 12, 34, 56, 78, 100, 122};
            var priceResponse = new PriceResponse()
            {
                RequestId = request.RequestId, 
                UserId = request.UserId, 
                IsSuccess = true, 
                Price = 12345.6789012345,
                SorBytes = ByteString.CopyFrom(bytes),
            };
            return Task.FromResult(priceResponse);
        }

        
    }
}