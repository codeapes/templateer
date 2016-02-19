namespace CodeApes.Templateer
{
    using System;
    using NUnit.Framework;
    
    [TestFixture]
    public sealed partial class BackendStatusTest : IDisposable
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
                fixture = null;
            }
        }
        
        [Test]
        public void IsRunning_NoStopRequested_ShouldBeTrue()
        {
            var testObject = fixture.CreateTestObject();
            
            Assert.That(testObject.IsRunning, Is.True);
        }
        
        [Test]
        public void IsRunning_StopRequested_ShouldBeFalse()
        {
            var testObject = fixture.CreateTestObject();
            
            testObject.RequestShutdown();
            
            Assert.That(testObject.IsRunning, Is.False);
        }
    }
}