using Grpc.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GrpcService
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        private readonly IHttpContextAccessor httpContext;
        public GreeterService(ILogger<GreeterService> logger, IHttpContextAccessor context)
        {
            _logger = logger;
            httpContext = context;
        }

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            var user = httpContext.HttpContext.User;
            var thumbprint = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Thumbprint);
            var result = @$"Hello 
                            Imie: {request.Person.FirstName} 
                            Nazwisko: {request.Person.LastName}
                            Age: {request.Person.Age}
                            Pesel: {request.Person.Pesel}
                            Sex: {request.Person.Sex}
                            Amount: {request.Person.Amount}
                            DateBirth: {request.Person.DateBirth?.ToDateTime()}
                            Animals: {String.Join(", ", request.Animals)}
                            ({thumbprint.Value})";

            _logger.LogInformation(result);
            return Task.FromResult(new HelloReply
            {
                Message = result
            });
        }
    }
}
