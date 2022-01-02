using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using GrpcService;

// The port number must match the port of the gRPC server.
// using var channel = GrpcChannel.ForAddress("https://localhost:7142");
using var channel = GrpcChannel.ForAddress("https://localhost:5001");
var client = new Greeter.GreeterClient(channel);

var priceRequest = CreateRequest();
var priceResponse = await client.SayPriceAsync(priceRequest);
if (priceResponse == null)
{
    Console.WriteLine("Failed to get price");
    return;
}

if (priceResponse.RequestId != priceRequest.RequestId)
{
    Console.WriteLine("Wrong Request ID!");
    return;
}

Console.WriteLine("Price response is: ");
Console.WriteLine($" requestId: {priceResponse.RequestId}");
Console.WriteLine($" userId: {priceResponse.UserId}");
Console.WriteLine($" isSuccess: {priceResponse.IsSuccess}");
Console.WriteLine($" errorMessage: {priceResponse.ErrorMessage}");
Console.WriteLine($" price: {priceResponse.Price}");
Console.Write(" sorBytes: ");
var bytes = priceResponse.SorBytes.ToByteArray();
foreach (var b in bytes)
{
    Console.Write($"{b} ");
}
Console.WriteLine("\nPress any key to exit...");
Console.ReadKey();


PriceRequest CreateRequest()
{
    var customer = new Customer() { Id = "VasyaId", Name = "Vasya" };
    for (int i = 0; i < 10; i++)
    {
        customer.Orders.Add(
            new Order
            {
                Id = i,
                Start = Timestamp.FromDateTime(DateTime.Today.AddDays(10 - i).ToUniversalTime()),
                Goods = { "Book" + i, "Pen" + i, "Bicycle" + 1 }
            });
    }

    return new PriceRequest()
        { RequestId = Guid.NewGuid().ToString(), UserId = "VasyaId", Customer = customer };
}