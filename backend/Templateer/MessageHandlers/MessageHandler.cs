namespace CodeApes.Templateer.MessageHandlers
{
    using CodeApes.Templateer.Commands;
    using CodeApes.Templateer.NetMQ;    
    
    public abstract class MessageHandler : IMessageHandler
    {
        private INetMqWrapper netMqWrapper;
        
        protected MessageHandler(INetMqWrapper netMqWrapper)
        {
            this.netMqWrapper = netMqWrapper;
        }
        
        public void BindTo(string serverAddress)
        {
            netMqWrapper.BindTo(serverAddress);
        }
        
        public void HandleMessages(BackendStatus status)
        {
            var message = netMqWrapper.GetNextMessage();
            CodeApes.Templateer.Commands.Command command = GenerateCommandForMessage(message);
            
            command.Execute(status);            
            var replyMessage = GenerateMessageForCommand(command);
            netMqWrapper.Send(replyMessage);
        }

        protected abstract string GenerateMessageForCommand(Command command);
        
        protected abstract Command GenerateCommandForMessage(string message);
    }
}