using NUnit.Framework;

namespace CodeApes.Templateer.Tests
{
    [TestFixture]
    public class ProgramTest
    {
		[Test]
		public void Test()
		{
			Program program = new Program();
			Assert.IsNotNull(program);
		}
    }
}
