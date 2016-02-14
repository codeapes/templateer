namespace CodeApes.Templateer
{
    using CodeApes.Templateer.MessageHandlers;
    using CodeApes.Templateer.NetMQ;
    
	public class Program
	{        
		public static void Main(string[] args)
		{    
            using (var netMqWrapper = new NetMqWrapper())
            {
                IBackend backend = new Backend(new JsonMessageHandler(netMqWrapper));
                backend.Start();
            }            
		}
    }
}
