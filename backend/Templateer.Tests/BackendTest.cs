namespace CodeApes.Templateer
{
    using System;
    using NUnit.Framework;
    
    [TestFixture]
    public sealed partial class BackendTest : IDisposable
    {
        private Fixture fixture;
        
        [SetUp]
        public void SetUp()
        {
            fixture = new Fixture();
        }
        
        [TearDown]
        public void Dispose()
        {
            if (fixture != null)
            {
                fixture.Dispose();
                fixture = null;
            }
        }
        
        [Test]
        public void Start_ShouldBindAndCallHandleMessagesUntilStopped()
        {
            fixture.PrepareTwoMessages();
            var testObject = fixture.CreateTestObject();
            
            testObject.Start();
            
            fixture.VerifyHandleMessagesCalledExactlyThreeTimes();
        }
    }
}