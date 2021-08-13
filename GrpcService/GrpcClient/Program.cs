using Grpc.Net.Client;
using GrpcService;
using System;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using static GrpcService.Greeter;

namespace GrpcClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var cert = new X509Certificate2("testcert.pfx", "pentacomp"); var handler = new HttpClientHandler();
            handler.ClientCertificates.Add(cert);
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            var httpClient = new HttpClient(handler);
            using var channel = GrpcChannel.ForAddress("https://localhost:5001", new GrpcChannelOptions
            {
                HttpClient = httpClient
            });
            var client = new GreeterClient(channel);

            var p1 = new Person() { FirstName = "Sebastian", LastName = "Enigman" };

            var reply = client.SayHelloAsync(
                              new HelloRequest { Person = p1 });
            Console.WriteLine("Greeting: " + reply.ResponseAsync.Result.Message);
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

    }
}
