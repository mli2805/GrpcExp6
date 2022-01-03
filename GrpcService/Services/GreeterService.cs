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
            var bytes = File.ReadAllBytes(@"c:\temp\sor\Long Precise 3000ns.sor");
            var fileLen = bytes.Length;
            byte[] fullBytes = new byte[fileLen * 10];
            for (int i = 0; i < 10; i++)
            {
                Array.ConstrainedCopy(bytes, 0, fullBytes, fileLen * i, fileLen);
            }

            var priceResponse = new PriceResponse()
            {
                RequestId = request.RequestId,
                UserId = request.UserId,
                IsSuccess = true,
                Price = 12345.6789012345,
                SorBytes = ByteString.CopyFrom(fullBytes),
                FileLen = fileLen,
            };
            return Task.FromResult(priceResponse);
        }


    }
}