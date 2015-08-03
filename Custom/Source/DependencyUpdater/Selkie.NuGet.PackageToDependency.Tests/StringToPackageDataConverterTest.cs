using System.Linq;
using NUnit.Framework;

namespace Selkie.NuGet.PackageToDependency.Tests
{
    [TestFixture]
    public class StringToPackageDataConverterTest
    {
        [TestFixture]
        public class PackageFileReaderForOnePackageTest
        {
            [SetUp]
            public void Setup()
            {
                m_Converter = new StringToPackageDataConverter();
                m_Converter.Convert(OnePackage);
            }

            private const string OnePackage =
                "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" + "<packages>\r\n" +
                "<package id=\"AutoFixture\" version=\"3.21.1\" targetFramework=\"net45\" />\r\n" + "</packages>\r\n";

            private StringToPackageDataConverter m_Converter;

            [Test]
            public void ConvertForEmptyStringDoesNothingTest()
            {
                var converter = new StringToPackageDataConverter();
                converter.Convert("");

                Assert.AreEqual(0,
                    converter.Data.Count());
            }

            [Test]
            public void ConvertForNoPackagesElementDoesNothingTest()
            {
                var converter = new StringToPackageDataConverter();
                converter.Convert("<data></data>");

                Assert.AreEqual(0,
                    converter.Data.Count());
            }

            [Test]
            public void CountTest()
            {
                Assert.AreEqual(1,
                    m_Converter.Data.Count());
            }

            [Test]
            public void IdTest()
            {
                var actual = m_Converter.Data.First();

                Assert.AreEqual("AutoFixture",
                    actual.Id);
            }

            [Test]
            public void TargetFrameworkTest()
            {
                var actual = m_Converter.Data.First();

                Assert.AreEqual("net45",
                    actual.TargetFramework);
            }

            [Test]
            public void VersionTest()
            {
                var actual = m_Converter.Data.First();

                Assert.AreEqual("3.21.1",
                    actual.Version);
            }
        }

        [TestFixture]
        public class PackageFileReaderForThreePackagesTest
        {
            [SetUp]
            public void Setup()
            {
                m_Converter = new StringToPackageDataConverter();
                m_Converter.Convert(ThreePackage);
            }

            private const string ThreePackage =
                "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" + "<packages>\r\n" +
                "<package id=\"AutoFixture\" version=\"3.21.1\" targetFramework=\"net45\" />\r\n" +
                "<package id=\"Castle.Core\" version=\"3.3.3\" targetFramework=\"net40\" />\r\n" +
                "<package id=\"NSubstitute\" version=\"1.8.0.0\" targetFramework=\"net35\" />\r\n" + "</packages>\r\n";

            private StringToPackageDataConverter m_Converter;

            [Test]
            public void CountTest()
            {
                Assert.AreEqual(3,
                    m_Converter.Data.Count());
            }

            [Test]
            public void IdForFirstPackageTest()
            {
                var actual = m_Converter.Data.First();

                Assert.AreEqual("AutoFixture",
                    actual.Id);
            }

            [Test]
            public void IdForSecondPackageTest()
            {
                var actual = m_Converter.Data.ToArray()[1];

                Assert.AreEqual("Castle.Core",
                    actual.Id);
            }

            [Test]
            public void IdForThirdPackageTest()
            {
                var actual = m_Converter.Data.ToArray()[2];

                Assert.AreEqual("NSubstitute",
                    actual.Id);
            }

            [Test]
            public void TargetFrameworkForFirstPackageTest()
            {
                var actual = m_Converter.Data.First();

                Assert.AreEqual("net45",
                    actual.TargetFramework);
            }

            [Test]
            public void TargetFrameworkForSecondPackageTest()
            {
                var actual = m_Converter.Data.ToArray()[1];

                Assert.AreEqual("net40",
                    actual.TargetFramework);
            }

            [Test]
            public void TargetFrameworkForThirdPackageTest()
            {
                var actual = m_Converter.Data.ToArray()[2];

                Assert.AreEqual("net35",
                    actual.TargetFramework);
            }

            [Test]
            public void VersionForFirstPackageTest()
            {
                var actual = m_Converter.Data.First();

                Assert.AreEqual("3.21.1",
                    actual.Version);
            }

            [Test]
            public void VersionForSecondPackageTest()
            {
                var actual = m_Converter.Data.ToArray()[1];

                Assert.AreEqual("3.3.3",
                    actual.Version);
            }

            [Test]
            public void VersionForThirdPackageTest()
            {
                var actual = m_Converter.Data.ToArray()[2];

                Assert.AreEqual("1.8.0.0",
                    actual.Version);
            }
        }
    }
}