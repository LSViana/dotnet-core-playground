using System;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;

namespace Multithreading
{
    public class PLINQMultiplePings
    {
        public static void Start(string[] args)
        {
            var sites = new[]
            {
                "www.albahari.com",
                "www.linqpad.net",
                "www.oreilly.com",
                "www.takeonit.com",
                "stackoverflow.com",
                "www.rebeccarey.com"  
            };
            var pings = sites
                .AsParallel()
                .WithDegreeOfParallelism(sites.Length)
                .Select(site =>
                {
                    var result = new Ping().Send(site);
                    return new
                    {
                        Site = site,
                        Result = result.Status,
                        Time = result.RoundtripTime,
                    };
                });
            foreach (var item in pings)
            {
                Console.WriteLine(item);
            }
        }
    }
}