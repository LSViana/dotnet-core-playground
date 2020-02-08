using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Multithreading
{
    public class HttpClientTest
    {
        public static void Start(string[] args)
        {
            RunAsync().Wait();
        }

        private static async Task RunAsync()
        {
            var client = new HttpClient();
            Console.WriteLine("Performing request");
            var currentThread = Thread.CurrentThread;
            var syncContext = System.Threading.SynchronizationContext.Current;
            Console.WriteLine($"Running on same Thread? {currentThread == Thread.CurrentThread} [{Thread.CurrentThread.ManagedThreadId}] [{Thread.CurrentThread.IsThreadPoolThread}]");
            var response = await client.GetAsync(new Uri("http://www.pudim.com.br/"));
            Console.WriteLine($"Running on same Thread? {currentThread == Thread.CurrentThread} [{Thread.CurrentThread.ManagedThreadId}] [{Thread.CurrentThread.IsThreadPoolThread}]");
            Console.WriteLine("Performed request");
            var stringResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"String length: {stringResponse.Length}");
        }
    }
}