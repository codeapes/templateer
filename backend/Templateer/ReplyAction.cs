using System;
using NetMQ;

namespace CodeApes.Templateer
{
    public class ReplyAction
    {
        private string replyMessage;
        private Action action;
        
        public ReplyAction(string replyMessage)
        {
            this.replyMessage = replyMessage;
        }
        
        public ReplyAction(string replyMessage, Action action)
            : this(replyMessage)
        {
            this.action = action;
        }

        public void SendOver(NetMQSocket serverSocket)
        {
            serverSocket.SendFrame(replyMessage);
            while (serverSocket.HasOut) { } // Wait until message is actually sent out.
            ExecuteAction();
        }
        
        private void ExecuteAction()
        {
            if (action == null)
            {
                return;
            }
            
            action.Invoke();   
        }
    }
}