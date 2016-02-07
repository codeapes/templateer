using System;
using NetMQ;

namespace CodeApes.Templateer
{
	public class Program
	{
        private const string ServerAddress = "tcp://127.0.0.1:5556";
        private const bool verbose = true;
        private const string Unrecognized = "¡UNRECOGNIZED¡";
        
		public static void Main(string[] args)
		{            
            using (NetMQContext ctx = NetMQContext.Create())
            using (NetMQSocket serverSocket = ctx.CreateResponseSocket())
            {
                serverSocket.Bind(ServerAddress);

                while (true)
                {
                    HandleMessages(serverSocket, verbose);
                }
            }
		}

        private static void HandleMessages(NetMQSocket serverSocket, bool verbose)
        {
            string message = serverSocket.ReceiveFrameString();
            var reply = HandleMessage(message);
            reply.SendOver(serverSocket);
        }
        
        private static ReplyAction HandleMessage(string message)
        {
            if (message == "dummy")
            {
                return new ReplyAction("public class Dummy { }");
            }
            else if (message == "exit")
            {
                return new ReplyAction("exiting", () => { System.Threading.Thread.Sleep(1); Environment.Exit(0); } );
            }
            else
            {
                return new ReplyAction(Unrecognized);
            }
        }
    }
}
