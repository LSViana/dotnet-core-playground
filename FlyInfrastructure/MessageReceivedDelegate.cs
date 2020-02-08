using System;
using FlyInfrastructure.Messaging;

namespace FlyInfrastructure
{
    public delegate void MessageReceivedDelegate(
        IFlyNetworkComponent component,
        Guid id,
        Message message);
}