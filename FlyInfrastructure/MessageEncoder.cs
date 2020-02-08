using System;
using System.Linq;
using System.Reflection;
using System.Text;
using FlyInfrastructure.Messaging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FlyInfrastructure
{
    public static class MessageEncoder
    {
        private static readonly Type[] MessageTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(x => x.IsSubclassOf(typeof(Message)))
            .ToArray();

        public static string GetMessageString(Message message)
        {
            return JsonConvert.SerializeObject(
                message,
                Formatting.None);
        }

        public static byte[] GetMessageBytes(Message message)
        {
            return Encoding.UTF8.GetBytes(GetMessageString(message));
        }

        public static Message GetMessageFromString(string message)
        {
            var jToken = JToken.Parse(message);
            //
            var codeType = jToken.Value<string>(nameof(Message.Code));
            if (codeType is null)
                throw new InvalidOperationException(
                    string.Format("The {0} property wasn't found", nameof(Message.Code))
                    );
            foreach (var messageType in MessageTypes)
            {
                if (messageType.Name == codeType)
                    return (Message)jToken.ToObject(messageType);
            }
            throw new InvalidOperationException("Any message matched the code supplied");
        }

        public static Message GetMessageFromBytes(byte[] message)
        {
            var messageString = Encoding.UTF8.GetString(message);
            return GetMessageFromString(messageString);
        }
    }
}