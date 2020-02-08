using System.Threading;
using System.Threading.Tasks;

namespace FlyInfrastructure
{
    public interface IFlyNetworkComponent
    {
        event MessageReceivedDelegate MessageReceived;

        Task StartAsync(CancellationToken cancellationToken);

        void Stop();
    }
}