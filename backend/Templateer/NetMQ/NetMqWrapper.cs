namespace CodeApes.Templateer.NetMQ
{
    using System;
    using global::NetMQ;

    public interface INetMqWrapper : IDisposable
    {
        void BindTo(string serverAddress);
        
        string GetNextMessage();
        
        void Send(string reply);
    }

    public class NetMqWrapper : INetMqWrapper
    {
        private readonly NetMQContext context;
        private readonly NetMQSocket socket;
        
        public NetMqWrapper()
        {
            context = NetMQContext.Create();
            socket = context.CreateResponseSocket();
        }

        public void BindTo(string serverAddress)
        {
            socket.Bind(serverAddress);
        }

        public void Dispose()
        {
            socket.Dispose();
            context.Dispose();
        }

        public string GetNextMessage()
        {
            return socket.ReceiveFrameString();
        }

        public void Send(string reply)
        {
            socket.SendFrame(reply);
            while (socket.HasOut) { } // Wait until message is actually sent out.
        }
    }
}