using System;
using System.Threading;
using Nancy.Hosting.Self;

namespace Naylor.Gateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var uri = new Uri("http://localhost:1234");
            var mongoEndpoint = Environment.GetEnvironmentVariable("MONGO_ENDPOINT");
            var bootstrapper = new Bootstrapper(mongoEndpoint);
            var hostConfig = new HostConfiguration { UrlReservations = new UrlReservations { CreateAutomatically = true } };
            using (NancyHost host = new NancyHost(bootstrapper, hostConfig, uri))
            {
                host.Start();
                Console.Out.Write("Host Started");

                Thread.Sleep(Timeout.Infinite);
            }
        }
    }
}
