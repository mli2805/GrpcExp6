using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using GrpcService;
using GrpcService2;

namespace GrpcClient;

class Program
{
    static async Task Main()
    {
        // var serverAddress = "localhost";
        var serverAddress = "192.168.96.135";
        // The port number must match the port of the gRPC server.
        using var channel1 = GrpcChannel.ForAddress($"http://{serverAddress}:6000");
        var client1 = new Greeter.GreeterClient(channel1);

        var priceRequest = CreateRequest();
        PriceResponse priceResponse;
        try
        {
            priceResponse = await client1.SayPriceAsync(priceRequest);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return;
        }

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
        Console.Write(@" sorBytes: /data/received.sor");
        var bytes = priceResponse.SorBytes.ToByteArray();
        byte[] fileBytes = new byte[priceResponse.FileLen];
        Array.ConstrainedCopy(bytes, priceResponse.FileLen * 9, fileBytes, 0, priceResponse.FileLen);

        var filename = AppContext.BaseDirectory + @"/data/Long Precise 3000ns.sor";
        File.WriteAllBytes(filename, fileBytes);
        //File.WriteAllBytes(@"data/received.sor", fileBytes);
        Console.WriteLine("\n\nPress any key to use second service...\n\n");
        Console.ReadKey();

        using var channel3 = GrpcChannel.ForAddress($"http://{serverAddress}:6002");
        var secondClient = new Seconder.SeconderClient(channel3);
        await RegisterUser(secondClient, "PetyaId");
        await RegisterUser(secondClient, "VasyaId");
        await RegisterUser(secondClient, "SeriyId");

        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();

        using var anotherChannel = GrpcChannel.ForAddress($"http://{serverAddress}:6010");
        var anotherClient = new AnotherGreeter.AnotherGreeterClient(anotherChannel);

        var response = await anotherClient.AnotherSayHelloAsync(new AnotherHelloRequest()
        { Name = "AnotherName", UserId = Guid.NewGuid().ToString() });
        Console.WriteLine(response.Message);
        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }

    static PriceRequest CreateRequest()
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

    static async Task RegisterUser(Seconder.SeconderClient seconderClient, string s)
    {
        var response = await seconderClient.RegisterMeAsync(new RegistrationDto()
        { RequestId = Guid.NewGuid().ToString(), UserId = s });
        Console.WriteLine(response.IsSuccess ? $"{s} registered successfully!\n\n" : $"Failed to register {s}!\n\n");
    }
}