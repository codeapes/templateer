namespace CodeApes.Templateer.Commands
{
    public class TemplateRequestCommand : Command
    {
        private string templateName;
        
        public TemplateRequestCommand(string templateName)
        {
            this.templateName = templateName;
        }
        
        public override void Execute(BackendStatus status)
        { }

        public override object GenerateReply()
        {
            return "public class Dummy { }";
        }
    }
}