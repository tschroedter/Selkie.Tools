using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using NUnit.Framework;

namespace Selkie.NuGet.PackageToDependency.Tests
{
    [TestFixture]
    public class NuSpecDependenciesUpdaterTests
    {
        [TestFixture]
        public class NuSpecDependenciesUpdaterDevelopmentTests
        {
            [SetUp]
            public void Setup()
            {
                m_Updater = new NuSpecDependenciesUpdater();
            }

            private const string EmptyDependencies = "<dependencies>" + "</dependencies>";

            private const string OneGroupWithOneAutoFixtureDependency =
                "<dependencies>" + "<group targetFramework=\"net45\">" +
                "<dependency id=\"AutoFixture\" version=\"1.0.0\" />" + "</group>" + "</dependencies>";

            private const string OneGroupWithOneAutoFixtureDependencyAndNoDependencyElement =
                "<dependencies>" + "<group targetFramework=\"net45\">" +
                "<dependency id=\"AutoFixture\" version=\"1.0.0\" />" + "<notDependency></notDependency>" + "</group>" +
                "</dependencies>";

            private const string OneGroupWithOneDependency =
                "<group targetFramework=\"net45\">" + "<dependency id=\"Other\" version=\"1.2.3\" />" + "</group>";

            private const string OneDependencyWithNoVersion = "<dependency id=\"log4net\" />";
            private const string OneDependencyWithNoId = "<dependency version=\"1.2.10\" />";
            private const string OneDependency = "<dependency id=\"log4net\" version=\"1.2.10\" />";

            private const string OneDependenciesWithOneGroup =
                "<dependencies>" + "<group targetFramework=\"net40\">" +
                "<dependency id=\"log4net\" version=\"1.2.10\" />" + "</group>" + "</dependencies>";

            private const string OneDependencyInGroup =
                "<dependencies>" + "<notGroup></notGroup>" + "<group targetFramework=\"net40\">" +
                "<dependency id=\"log4net\" version=\"1.2.10\" />" + "</group>" + "<group targetFramework=\"net45\">" +
                "<dependency id=\"AutoFixture\" version=\"3.21.1\" />" + "</group>" + "</dependencies>";

            private const string OneSelkieDependency = "<dependency id=\"Selkie.Windsor\" version=\"1.2.10\" />";
            private NuSpecDependenciesUpdater m_Updater;

            private static PackageData CreateSelkiePackageData()
            {
                return new PackageData
                {
                    Id = "Selkie.Windsor",
                    Version = "3.21.1",
                    TargetFramework = "net45"
                };
            }

            private static PackageData CreatePackageData()
            {
                return new PackageData
                {
                    Id = "AutoFixture",
                    Version = "3.21.1",
                    TargetFramework = "net45"
                };
            }

            private static PackageData CreatePackageDataUnknown()
            {
                return new PackageData
                {
                    Id = "Unknown",
                    Version = "Unknown",
                    TargetFramework = "Unknown"
                };
            }

            private XElement CreateOneGroupWithOneAutoFixtureDependencyAndNoDependencyElement()
            {
                var reader = new StringReader(OneGroupWithOneAutoFixtureDependencyAndNoDependencyElement);
                var document = XDocument.Load(reader);

                var dependenciesElement = document.Root;

                return dependenciesElement;
            }

            private XElement CreateOneGroupWithOneAutoFixtureDependency()
            {
                var reader = new StringReader(OneGroupWithOneAutoFixtureDependency);
                var document = XDocument.Load(reader);

                var dependenciesElement = document.Root;

                return dependenciesElement;
            }

            private XElement CreateEmptyDependencies()
            {
                var reader = new StringReader(EmptyDependencies);
                var document = XDocument.Load(reader);

                var dependenciesElement = document.Root;

                return dependenciesElement;
            }

            private XElement CreateOneDependenciesWithOneGroup()
            {
                var reader = new StringReader(OneDependenciesWithOneGroup);
                var document = XDocument.Load(reader);

                var dependenciesElement = document.Root;

                return dependenciesElement;
            }

            private XElement CreateGroup()
            {
                var reader = new StringReader(OneGroupWithOneDependency);
                var document = XDocument.Load(reader);

                var dependenciesElement = document.Root;

                return dependenciesElement;
            }

            private XElement CreateDependencies()
            {
                var reader = new StringReader(OneDependencyInGroup);
                var document = XDocument.Load(reader);

                var dependenciesElement = document.Root;

                return dependenciesElement;
            }

            private XElement CreateOneDependency()
            {
                var reader = new StringReader(OneDependency);
                var document = XDocument.Load(reader);

                var dependenciesElement = document.Root;

                return dependenciesElement;
            }

            private XElement CreateOneDependencyWithNoId()
            {
                var reader = new StringReader(OneDependencyWithNoId);
                var document = XDocument.Load(reader);

                var dependenciesElement = document.Root;

                return dependenciesElement;
            }

            private XElement CreateOneDependencyWithNoVersion()
            {
                var reader = new StringReader(OneDependencyWithNoVersion);
                var document = XDocument.Load(reader);

                var dependenciesElement = document.Root;

                return dependenciesElement;
            }

            private XElement CreateOneSelkieDependency()
            {
                var reader = new StringReader(OneSelkieDependency);
                var document = XDocument.Load(reader);

                var dependenciesElement = document.Root;

                return dependenciesElement;
            }

            [Test]
            public void CreateAndAddNewDependencyAddsDependencyTest()
            {
                var data = CreatePackageData();
                var group = CreateGroup();

                m_Updater.CreateAndAddNewDependency(group,
                    data);

                Assert.AreEqual(2,
                    group.Elements()
                        .Count());
            }

            [Test]
            public void CreateAndAddNewDependencyCreatesNewDependencyTest()
            {
                var data = CreatePackageData();
                var group = CreateGroup();

                m_Updater.CreateAndAddNewDependency(group,
                    data);

                var actual = group.Elements()
                    .Last();

                Assert.AreEqual(data.Id,
                    actual.Attribute("id")
                        .Value);
                Assert.AreEqual(data.Version,
                    actual.Attribute("version")
                        .Value);
            }

            [Test]
            public void CreateGroupAddsGroupTest()
            {
                var data = CreatePackageData();
                var dependencies = CreateOneDependenciesWithOneGroup();

                m_Updater.CreateAndAddGroup(dependencies,
                    data);

                var actual = dependencies;

                Assert.AreEqual(2,
                    actual.Elements()
                        .Count());
            }

            [Test]
            public void CreateGroupAddsNewGroupTest()
            {
                var data = CreatePackageData();
                var dependencies = CreateOneDependenciesWithOneGroup();

                var actual = m_Updater.CreateAndAddGroup(dependencies,
                    data);

                Assert.AreEqual(data.TargetFramework,
                    actual.Attribute("targetFramework")
                        .Value);
            }

            [Test]
            public void FindGroupReturnsGroupElementForExistingGroupTest()
            {
                var data = CreatePackageData();
                var dependencies = CreateDependencies();

                var group = m_Updater.FindGroupElement(dependencies,
                    data);

                Assert.NotNull(group);

                var actual = group.Attribute("targetFramework")
                    .Value;

                Assert.AreEqual("net45",
                    actual);
            }

            [Test]
            public void FindGroupReturnsNullForDifferentTargetFrameworkTest()
            {
                var data = new PackageData
                {
                    Id = "log4net",
                    Version = "1.2.3",
                    TargetFramework = "net45"
                };

                var dependencies = CreateDependencies();

                var group = m_Updater.FindGroupElement(dependencies,
                    data);

                Assert.NotNull(group);
                Assert.AreEqual("net45",
                    group.Attribute("targetFramework")
                        .Value);
            }

            [Test]
            public void FindGroupReturnsNullForUnknownTest()
            {
                var data = CreatePackageDataUnknown();
                var dependencies = CreateDependencies();

                var group = m_Updater.FindGroupElement(dependencies,
                    data);

                Assert.Null(group);
            }

            [Test]
            public void RemoveFromGroupDoesNotRemoveUnknownDependencyTest()
            {
                var group = CreateGroup();
                var data = CreatePackageDataUnknown();

                m_Updater.RemoveFromGroup(group,
                    data);

                Assert.AreEqual(1,
                    group.Elements()
                        .Count());
            }

            [Test]
            public void RemoveFromGroupRemovesKnownDependencyTest()
            {
                var group = CreateGroup();
                var data = new PackageData
                {
                    Id = "Other",
                    Version = "1.2.3",
                    TargetFramework = "net45"
                };

                m_Updater.RemoveFromGroup(group,
                    data);

                Assert.AreEqual(0,
                    group.Elements()
                        .Count());
            }

            [Test]
            public void UdateDependencyAddsDependencyToEmptyDependenciesTest()
            {
                var data = CreatePackageData();
                var dependencies = CreateEmptyDependencies();

                m_Updater.UpdateDependency(dependencies,
                    data);

                var group = dependencies.Elements()
                    .First();
                var actual = group.Elements();

                Assert.AreEqual(1,
                    actual.Count());
            }

            [Test]
            public void UdateDependencyAddsGroupToEmptyDependenciesTest()
            {
                var data = CreatePackageData();
                var dependencies = CreateEmptyDependencies();

                m_Updater.UpdateDependency(dependencies,
                    data);

                var actual = dependencies.Elements();

                Assert.AreEqual(1,
                    actual.Count());
            }

            [Test]
            public void UdateDependencyKeepsGroupForOneGroupWithOneAutoFixtureDependencyAndNoGroupElementTest()
            {
                var data = CreatePackageData();
                var dependencies = CreateOneGroupWithOneAutoFixtureDependencyAndNoDependencyElement();

                m_Updater.UpdateDependency(dependencies,
                    data);

                var group = dependencies.Elements()
                    .First();
                var actual = group.Elements();

                Assert.AreEqual(2,
                    actual.Count());
            }

            [Test]
            public void UdateDependencyKeepsGroupToDependenciesWithOneGroupAndDependencyTest()
            {
                var data = CreatePackageData();
                var dependencies = CreateOneGroupWithOneAutoFixtureDependency();

                m_Updater.UpdateDependency(dependencies,
                    data);

                var actual = dependencies.Elements()
                    .ToArray();

                Assert.AreEqual(1,
                    actual.Count());
            }

            [Test]
            public void UdateDependencyUpdatesAndKeepsDependenciesWithOneGroupAndDependencyTestTest()
            {
                var data = CreatePackageData();
                var dependencies = CreateOneGroupWithOneAutoFixtureDependency();

                m_Updater.UpdateDependency(dependencies,
                    data);

                var group = dependencies.Elements()
                    .First();
                var actual = group.Elements();

                Assert.AreEqual(1,
                    actual.Count());
            }

            [Test]
            public void UdateDependencyUpdatesDependenciesWithOneGroupAndDependencyTestTest()
            {
                var data = CreatePackageData();
                var dependencies = CreateOneGroupWithOneAutoFixtureDependency();

                m_Updater.UpdateDependency(dependencies,
                    data);

                var group = dependencies.Elements()
                    .First();
                var actual = group.Elements()
                    .First();

                Assert.AreEqual(data.Id,
                    actual.Attribute("id")
                        .Value);
                Assert.AreEqual(data.Version,
                    actual.Attribute("version")
                        .Value);
            }

            [Test]
            public void UpdateExistingDependencyUpdatesElementIdWhenAttributeIdDoesNotExistsTest()
            {
                var data = CreatePackageData();
                var dependencies = CreateOneDependencyWithNoId();

                m_Updater.UpdateExistingDependency(dependencies,
                    data);

                var actual = dependencies;

                Assert.AreEqual(data.Id,
                    actual.Attribute("id")
                        .Value);
                Assert.AreEqual(data.Version,
                    actual.Attribute("version")
                        .Value);
            }

            [Test]
            public void UpdateExistingDependencyUpdatesElementTest()
            {
                var data = CreatePackageData();
                var dependencies = CreateOneDependency();

                m_Updater.UpdateExistingDependency(dependencies,
                    data);

                var actual = dependencies;

                Assert.AreEqual(data.Id,
                    actual.Attribute("id")
                        .Value);
                Assert.AreEqual(data.Version,
                    actual.Attribute("version")
                        .Value);
            }

            [Test]
            public void UpdateExistingDependencyUpdatesElementVersionWhenAttributeVersionDoesNotExistsTest()
            {
                var data = CreatePackageData();
                var dependencies = CreateOneDependencyWithNoVersion();

                m_Updater.UpdateExistingDependency(dependencies,
                    data);

                var actual = dependencies;

                Assert.AreEqual(data.Id,
                    actual.Attribute("id")
                        .Value);
                Assert.AreEqual(data.Version,
                    actual.Attribute("version")
                        .Value);
            }

            [Test]
            public void UpdateExistingSelkieDependencyUpdatesElementTest()
            {
                var data = CreateSelkiePackageData();
                var dependencies = CreateOneSelkieDependency();

                m_Updater.UpdateExistingDependency(dependencies,
                    data);

                var actual = dependencies;

                Assert.AreEqual(data.Id,
                    actual.Attribute("id")
                        .Value);
                Assert.AreEqual(data.Version,
                    actual.Attribute("version")
                        .Value);
            }
        }

        [TestFixture]
        public class NuSpecDependenciesUpdaterRealLifeTests
        {
            [SetUp]
            public void Setup()
            {
                m_Dependencies = CreateDependencies();
                m_PackageDataList = CreatePackageDataList();

                m_Updater = new NuSpecDependenciesUpdater();
            }

            private const string Dependencies =
                "<dependencies>" + "<group targetFramework=\"net40\">" +
                "<dependency id=\"log4net\" version=\"1.2.10\" />" + "</group>" + "<group targetFramework=\"net45\">" +
                "<dependency id=\"Castle.Core\" version=\"3.3.3\" />" +
                "<dependency id=\"EasyNetQ\" version=\"0.39.4.338\" />" + "</group>" + "</dependencies>";

            private XElement m_Dependencies;
            private IEnumerable<PackageData> m_PackageDataList;
            private NuSpecDependenciesUpdater m_Updater;

            private static PackageData CreateNewPackageData()
            {
                return new PackageData
                {
                    Id = "AutoFixture",
                    Version = "3.21.1",
                    TargetFramework = "net45"
                };
            }

            private static PackageData CreateExistingPackageData()
            {
                return new PackageData
                {
                    Id = "Castle.Core",
                    Version = "4.4.4",
                    TargetFramework = "net45"
                };
            }

            private static PackageData CreateNewPackageDataInNewGroup()
            {
                return new PackageData
                {
                    Id = "NUnit",
                    Version = "4.4.4",
                    TargetFramework = "net1000"
                };
            }

            private static PackageData CreateExistingPackageDataInNewGroup()
            {
                return new PackageData
                {
                    Id = "log4net",
                    Version = "5.5.5",
                    TargetFramework = "net45"
                };
            }

            private IEnumerable<PackageData> CreatePackageDataList()
            {
                var data = new List<PackageData>
                {
                    CreateNewPackageData(),
                    CreateExistingPackageData(),
                    CreateNewPackageDataInNewGroup(),
                    CreateExistingPackageDataInNewGroup()
                };

                return data;
            }

            private XElement CreateDependencies()
            {
                var reader = new StringReader(Dependencies);
                var document = XDocument.Load(reader);

                var dependenciesElement = document.Root;

                return dependenciesElement;
            }

            [Test]
            public void UpdateAddsMovesDependencyTest()
            {
                m_Updater.Update(m_Dependencies,
                    m_PackageDataList);

                var groups = m_Dependencies.Elements()
                    .Where(x => x.Name == "group")
                    .ToArray();

                Assert.NotNull(groups);

                var group = groups.Where(x => x.Attribute("targetFramework")
                    .Value == "net45")
                    .ToArray();

                var dependencies = group.First()
                    .Elements();

                var actual = dependencies.FirstOrDefault(x => x.Attribute("id")
                    .Value == "log4net");

                Assert.NotNull(actual);

                Assert.AreEqual("5.5.5",
                    actual.Attribute("version")
                        .Value);
            }

            [Test]
            public void UpdateAddsNewGroupAndDependencyTest()
            {
                m_Updater.Update(m_Dependencies,
                    m_PackageDataList);

                var groups = m_Dependencies.Elements()
                    .Where(x => x.Name == "group")
                    .ToArray();

                Assert.NotNull(groups);

                var group = groups.Where(x => x.Attribute("targetFramework")
                    .Value == "net1000")
                    .ToArray();

                var actual = group.First();

                Assert.NotNull(actual);
                Assert.AreEqual(1,
                    actual.Elements()
                        .Count());
            }

            [Test]
            public void UpdateAddsRemoveOldDependencyInGroupTest()
            {
                m_Updater.Update(m_Dependencies,
                    m_PackageDataList);

                var groups = m_Dependencies.Elements()
                    .Where(x => x.Name == "group")
                    .ToArray();

                Assert.NotNull(groups);

                var group = groups.Where(x => x.Attribute("targetFramework")
                    .Value == "net40")
                    .ToArray();

                var dependencies = group.First()
                    .Elements();

                var actual = dependencies.FirstOrDefault(x => x.Attribute("id")
                    .Value == "log4net");

                Assert.Null(actual);
            }

            [Test]
            public void UpdateAddsUpdatesDependencyTest()
            {
                m_Updater.Update(m_Dependencies,
                    m_PackageDataList);

                var groups = m_Dependencies.Elements()
                    .Where(x => x.Name == "group")
                    .ToArray();

                Assert.NotNull(groups);

                var group = groups.Where(x => x.Attribute("targetFramework")
                    .Value == "net45")
                    .ToArray();

                var dependencies = group.First()
                    .Elements();

                var actual = dependencies.FirstOrDefault(x => x.Attribute("id")
                    .Value == "Castle.Core");

                Assert.NotNull(actual);

                Assert.AreEqual("4.4.4",
                    actual.Attribute("version")
                        .Value);
            }
        }
    }
}