namespace FlyInfrastructure.Messaging
{
    public abstract class Message
    {
        public string Code { get; private set; }
        
        protected Message()
        {
            Code = GetType().Name;
        }
    }
}