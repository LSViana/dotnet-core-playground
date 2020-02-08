namespace FlyInfrastructure.Messaging
{
    public class CommandMessage : Message
    {
        public string Command { get; private set; }

        public CommandMessage(
            string command)
            : base()
        {
            Command = command;
        }
    }
}