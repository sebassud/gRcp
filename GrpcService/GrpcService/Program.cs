using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace GrpcService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        // Additional configuration is required to successfully run gRPC on macOS.
        // For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    var cert = new X509Certificate2("RootCA.pfx", "pentacomp"); 
                    var handler = new HttpClientHandler();
                    webBuilder.UseStartup<Startup>()
                    .ConfigureKestrel(kestrelServerOptions => {
                        kestrelServerOptions.ConfigureHttpsDefaults(opt =>
                        {
                            opt.ClientCertificateMode = ClientCertificateMode.AllowCertificate;
                            opt.ServerCertificate = cert;

                            //// Verify that client certificate was issued by same CA as server certificate
                            //opt.ClientCertificateValidation = (certificate, chain, errors) =>

                        });
                    });
                });
    }
}
