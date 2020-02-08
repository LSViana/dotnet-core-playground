using FlyChat;
using FlyInfrastructure.Messaging;
using FlyServer;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FlyAppTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var task = new Task(async () =>
            {
                var flyServer = new Server("0.0.0.0");
                flyServer.MessageReceived += FlyServer_MessageReceived;
                var startServerTask = flyServer.StartAsync(CancellationToken.None);
                await Task.Delay(100); // Delay to make sure it starts correctly
                var flyClient = new Client("127.0.0.1");
                var startClientTask = flyClient.StartAsync(CancellationToken.None);
                await Task.Delay(100); // Delay to make sure it starts correctly
                {
                    var message = new CommandMessage("Fly server is working!");
                    await flyClient.SendAsync(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message)), CancellationToken.None);
                    await Task.Delay(100); // Delay to make sure it sends correctly
                }
            });
            task.RunSynchronously();
            // Not the best way to do this, but it keeps the thread operating
            Thread.Sleep(30_000);
        }

        private static void FlyServer_MessageReceived(FlyInfrastructure.IFlyNetworkComponent component, Guid id, Message message)
        {
            Console.WriteLine("[Test App] " + "Message with ID " + id + " received: " + message.GetType().Name + " from " + component.GetType().Name);
            if(message is CommandMessage commandMessage)
            {
                Console.WriteLine("[Test App] " + "Command is: " + commandMessage.Command);
            }
        }
    }
}
