using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;

namespace Api2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string endpoint = "http://localhost:5001";
            Console.WriteLine($"Server listening on {endpoint}");
            new WebHostBuilder()
                    .UseKestrel()
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .UseStartup<Startup>()
                    .UseUrls(endpoint)
                    .Build()
                    .Run();
        }
    }
}
