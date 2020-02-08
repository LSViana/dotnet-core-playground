using System;
using System.Net;
using System.Threading;

namespace Multithreading
{
    public class BeginEndWebClient
    {
        private static AutoResetEvent _autoResetEvent = new AutoResetEvent(false);
        
        public static void Start(string[] args)
        {
            Console.WriteLine($"Running on Thread: {Thread.CurrentThread.ManagedThreadId}");
            using (var wc = new WebClient())
            {
                wc.DownloadStringAsync(new Uri("http://www.pudim.com.br/"), wc);
                wc.DownloadStringCompleted += WcOnDownloadStringCompleted;
            }
            _autoResetEvent.WaitOne();
        }

        private static void WcOnDownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            Console.WriteLine($"Running on Thread: {Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine($"Downloaded string length: {e.Result.Length}");
            //
            _autoResetEvent.Set();
        }
    }
}