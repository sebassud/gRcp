using Grpc.Net.Client;
using GrpcService;
using System;
using static GrpcService.Greeter;

namespace GrpcClient
{
    class Program
    {
        static void Main(string[] args)
        {
            using var channel = GrpcChannel.ForAddress("http://localhost:5000");
            var client = new GreeterClient(channel);
            var reply = client.SayHelloAsync(
                              new HelloRequest { FirstName = "Sebastian", LastName = "Enigman" });
            Console.WriteLine("Greeting: " + reply.ResponseAsync.Result.Message);
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
