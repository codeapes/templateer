namespace CodeApes.Templateer.Commands
{
    public abstract class Command
    {        
        public abstract void Execute(BackendStatus status);
        
        public abstract object GenerateReply();
    }
}