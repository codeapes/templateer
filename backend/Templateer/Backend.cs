namespace CodeApes.Templateer
{
    using CodeApes.Templateer.MessageHandlers;

    public class Backend : IBackend
    {
        private const string ServerAddress = "tcp://127.0.0.1:5556";
        
        private readonly IMessageHandler messageHandler;
        private readonly BackendStatus status;

        public Backend(IMessageHandler messageHandler)
        {
            this.messageHandler = messageHandler;
            this.status = new BackendStatus();
        }

        public void Start()
        {
            messageHandler.BindTo(ServerAddress);

            while (status.IsRunning)
            {
                messageHandler.HandleMessages(status);
            }
        }
    }
}