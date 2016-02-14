namespace CodeApes.Templateer.MessageHandlers
{
    public interface IMessageHandler
    {
        void BindTo(string serverAddress);
        
        void HandleMessages(BackendStatus status);
    }
}