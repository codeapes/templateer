using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetMQ;
using NetMQ.Sockets;

namespace CodeApes.Templateer
{
	public class Program
	{
		public static void Main(string[] args)
		{
            const string serverAddress = "@tcp://localhost:5556";
            
            using (var server = new NetMQ.Sockets.ResponseSocket(serverAddress)) // bind
            using (var client = new RequestSocket(">tcp://localhost:5556"))  // connect
            {
                // Send a message from the client socket
                client.SendFrame("Hello");

                // Receive the message from the server socket
                string m1 = server.ReceiveFrameString();
                Console.WriteLine("From Client: {0}", m1);

                // Send a response back from the server
                server.SendFrame("Hi Back");

                // Receive the response from the client socket
                string m2 = client.ReceiveFrameString();
                Console.WriteLine("From Server: {0}", m2);
            }
		}
	}
}
