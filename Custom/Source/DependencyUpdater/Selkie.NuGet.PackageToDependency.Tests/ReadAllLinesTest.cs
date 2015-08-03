using JetBrains.Annotations;
using NUnit.Framework;

namespace Selkie.NuGet.PackageToDependency.Tests
{
    [TestFixture]
    public class ReadAllLinesTest
    {
        [SetUp]
        public void Setup()
        {
            m_Reader = new TestReadAllLines();
        }

        private TestReadAllLines m_Reader;

        private class TestReadAllLines : ReadAllLines
        {
            public string Read([NotNull] string filename)
            {
                return ReadAll(filename);
            }
        }

        [Test]
        public void ReadTest()
        {
            const string expected = "Hello World!\r\n";
            var actual = m_Reader.Read("Hello World.txt");

            Assert.AreEqual(expected,
                actual);
        }
    }
}