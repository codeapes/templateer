namespace CodeApes.Templateer
{
    using System;
    using Moq;
    using CodeApes.Templateer.MessageHandlers;
    using System.Collections.Generic;

    public sealed partial class BackendTest
    {
        private class Fixture : IDisposable
        {
            private readonly MockRepository mockRepository = new MockRepository(MockBehavior.Strict);
            private readonly Mock<IMessageHandler> messageHandlerMock;
            
            public Fixture()
            {
                messageHandlerMock = mockRepository.Create<IMessageHandler>();
            }
            
            public void Dispose()
            {
                mockRepository.VerifyAll();
            }
            
            public IBackend CreateTestObject()
            {
                return new Backend(messageHandlerMock.Object);
            }

            public void VerifyHandleMessagesCalledExactlyThreeTimes()
            {
                messageHandlerMock.Verify(x => x.HandleMessages(It.IsAny<BackendStatus>()), Times.Exactly(3));
            }

            public void PrepareTwoMessages()
            {
                PrepareBindTo();
                PrepareHandleMessage(false, false, true);
            }

            private void PrepareHandleMessage(params bool[] stopExecutions)
            {
                var queue = new Queue<bool>(stopExecutions);
                
                messageHandlerMock
                    .Setup(x => x.HandleMessages(It.Is<BackendStatus>(status => status.IsRunning)))
                    .Callback<BackendStatus>(status => 
                    {
                        if (queue.Dequeue())
                        {
                            status.RequestShutdown();
                        }
                    });
            }

            private void PrepareBindTo()
            {
                messageHandlerMock
                    .Setup(x => x.BindTo(It.Is<string>(serverAddress => serverAddress == "tcp://127.0.0.1:5556")));
            }
        }
    }
}