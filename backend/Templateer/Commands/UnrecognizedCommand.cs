namespace CodeApes.Templateer.Commands
{
    public class UnrecognizedCommand : Command
    {
        private string unrecognizedMessage;
        
        public UnrecognizedCommand(string unrecognizedMessage)
        {
            this.unrecognizedMessage = unrecognizedMessage;
        }
        
        public override void Execute(BackendStatus status)
        { }

        public override object GenerateReply()
        {
            return string.Format("Unrecognized command: '{0}'", unrecognizedMessage);
        }
    }
}