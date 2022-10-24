using Grpc.Net.Client;
using GrpcAndWebApiService;

namespace GrpcAndWebApiClient
{
    internal class Program
    {
        static async Task Main()
        {
            //var serverAddress = "localhost";
            var serverAddress = "192.168.96.111";
            using var channel1 = GrpcChannel.ForAddress($"http://{serverAddress}:11740");
            var client1 = new Greeter.GreeterClient(channel1);
            var response = await client1.SayHelloAsync(new HelloRequest() { Name = "Leanid" });
            if (response == null)
            {
                Console.WriteLine("response is null");
            }
            else
            {
                Console.WriteLine($"response is {response.Message}");
            }
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();

        }
    }
}