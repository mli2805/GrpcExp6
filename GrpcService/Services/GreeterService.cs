using DatabaseLibrary;
using Google.Protobuf;
using Grpc.Core;

namespace GrpcService.Services
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;

        private readonly ProductRepository _productRepository;
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
            _productRepository = new ProductRepository(_logger);
        }

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply
            {
                Message = "Hello, " + request.Name
            });
        }

        public override async Task<PriceResponse> SayPrice(PriceRequest request, ServerCallContext context)
        {
            _logger.LogInformation($"Host {context.Host},  Peer {context.Peer}");
            var filename = AppContext.BaseDirectory + @"/data/Long Precise 3000ns.sor";

            var bytes = await File.ReadAllBytesAsync(filename);
            if (bytes.Length > StaticClass.GetInt(10000))
                _logger.LogInformation("Quite a big file");

            var fileLen = bytes.Length;
            byte[] fullBytes = new byte[fileLen * 10];
            for (int i = 0; i < 10; i++)
            {
                Array.ConstrainedCopy(bytes, 0, fullBytes, fileLen * i, fileLen);
            }

            var fullPrice = (await _productRepository.GetAllProducts()).Sum(x => x.Price * x.Stock);

            var priceResponse = new PriceResponse()
            {
                RequestId = request.RequestId,
                UserId = request.UserId,
                IsSuccess = true,
                Price = fullPrice,
                SorBytes = ByteString.CopyFrom(fullBytes),
                FileLen = fileLen,
            };
            return priceResponse;
        }


    }
}