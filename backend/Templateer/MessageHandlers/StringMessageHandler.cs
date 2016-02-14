namespace CodeApes.Templateer.MessageHandlers
{
    using CodeApes.Templateer.Commands;
    using CodeApes.Templateer.NetMQ;
    
    public class StringMessageHandler : MessageHandler
    {
        public StringMessageHandler(INetMqWrapper netMqWrapper)
            : base(netMqWrapper)
        { }
        
        protected override Command GenerateCommandForMessage(string message)
        {
            switch (message)
            {
                case "dummy":
                    return new TemplateRequestCommand("dummy");
                case "exit":
                    return new ExitCommand();
            }
            
            return new UnrecognizedCommand(message);
        }

        protected override string GenerateMessageForCommand(Command command)
        {
            return command.GenerateReply().ToString();
        }
    }
}