namespace CodeApes.Templateer.Commands
{    
    public class ExitCommand : Command
    {
        public override void Execute(BackendStatus status)
        {
            status.RequestShutdown();
        }

        public override object GenerateReply()
        {
            return "exiting";
        }
    }
}