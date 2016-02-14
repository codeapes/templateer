namespace CodeApes.Templateer.MessageHandlers
{
    using System;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Json;
    using System.Text;
    using CodeApes.Templateer.NetMQ;
    using CodeApes.Templateer.Commands;
    
    public class JsonMessageHandler : MessageHandler
    {                
        public JsonMessageHandler(INetMqWrapper netMqWrapper)
            : base(netMqWrapper)
        { }

        protected override Command GenerateCommandForMessage(string messageString)
        {
            Message message;
            
            try 
            {
                using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(messageString ?? "")))
                {
                    var serializer = new DataContractJsonSerializer(typeof(Message));
                    message = (Message)serializer.ReadObject(stream);
                    
                    using (var strm = new MemoryStream())
                    {
                        serializer.WriteObject(strm, new Message { Type = MessageType.Exit });
                        Console.WriteLine(Encoding.UTF8.GetString(strm.ToArray()));
                    }
                }
            }
            catch (SerializationException)
            {
                return new UnrecognizedCommand(messageString);
            }
            
            switch (message.Type)
            {
                case MessageType.Exit:
                    return new ExitCommand(); 
            }
            
            return new UnrecognizedCommand(messageString);
        }

        protected override string GenerateMessageForCommand(Command command)
        {
            return command.GenerateReply().ToString();
        }
    }
    
    [DataContract]
    public class Message 
    {
        [DataMember]
        public MessageType Type { get; set; }
    }
    
    [DataContract]
    public enum MessageType
    {
        [EnumMember]
        Exit
    }
}