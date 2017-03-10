using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;

namespace CoreMicroService
{
    public class Program
    {
        public static void Main(string[] args)
        {
        //    var config = new ConfigurationBuilder()
        //.SetBasePath(Directory.GetCurrentDirectory())
        //.AddJsonFile("hosting.json", optional: true)
        //.Build();
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                //.UseStartup("CustomStartup")// uses the assembly named CustomStartup as startup assembly
                .Build();

            host.Run();
        }
    }
}
