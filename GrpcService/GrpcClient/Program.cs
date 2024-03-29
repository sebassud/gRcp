﻿using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using GrpcService;
using System;
using System.Collections.Generic;
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
            var p2 = new Person() 
            { 
                FirstName = "Sebastian", 
                LastName = "Enigman",
                Age = 25,
                Amount = 1234.99,
                Sex = Sex.Male,
                Pesel = "95042903912",
                DateBirth = Timestamp.FromDateTime(DateTime.UtcNow)
            };

            var request = new HelloRequest { Person = p1 };           
            var reply = client.SayHelloAsync(request);
            Console.WriteLine("Greeting: " + reply.ResponseAsync.Result.Message);

            var animals = new List<string>() { "Pies", "Kot", "Królik" };
            var request2 = new HelloRequest { Person = p1 };
            request2.Animals.AddRange(animals);
            var reply2 = client.SayHelloAsync(request2);
            Console.WriteLine("Greeting: " + reply2.ResponseAsync.Result.Message);

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

    }
}
