using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using JetBrains.Annotations;

namespace Selkie.NuGet.PackageToDependency
{
    public class NuSpecDependenciesUpdater
    {
        public void Update([NotNull] XElement dependencies,
            [NotNull] IEnumerable<PackageData> packageDataList)
        {
            foreach (var packageData in packageDataList)
            {
                UpdateDependency(dependencies,
                    packageData);
            }
        }

        internal void UpdateDependency([NotNull] XElement dependencies,
            [NotNull] PackageData packageData)
        {
            var groupElement = FindOrCreateGroup(dependencies,
                packageData);

            AddOrUpdateDependency(groupElement,
                packageData);
        }

        private void AddOrUpdateDependency([NotNull] XElement groupElement,
            [NotNull] PackageData packageData)
        {
            var isExistingDependency = false;

            var elements = groupElement.Elements()
                .ToArray();

            foreach (var element in elements)
            {
                if (element.Name != "dependency")
                {
                    continue;
                }

                if (element.Attribute("id")
                    .Value == packageData.Id)
                {
                    UpdateExistingDependency(element,
                        packageData);

                    isExistingDependency = true;
                    break;
                }
            }

            if (!isExistingDependency)
            {
                CreateAndAddNewDependency(groupElement,
                    packageData);
            }
        }

        private XElement FindOrCreateGroup([NotNull] XElement dependencies,
            [NotNull] PackageData packageData)
        {
            var groupElement = FindGroupElement(dependencies,
                packageData) ?? CreateAndAddGroup(dependencies,
                    packageData);

            return groupElement;
        }

        internal void RemoveFromGroup([NotNull] XElement groupElement,
            [NotNull] PackageData packageData)
        {
            var dependency = groupElement.Elements()
                .FirstOrDefault(x => x.Attribute("id")
                    .Value == packageData.Id);

            if (dependency != null)
            {
                dependency.Remove();
            }
        }

        internal void CreateAndAddNewDependency([NotNull] XElement groupElement,
            [NotNull] PackageData packageData)
        {
            var dependency = new XElement("dependency",
                new XAttribute("id",
                    packageData.Id),
                new XAttribute("version",
                    packageData.Version));

            groupElement.Add(dependency);
        }

        /*
        private static string GetPackageVersion(PackageData packageData)
        {
            if ( !packageData.Id.StartsWith("Selkie.") )
            {
                return packageData.Version;
            }

            var version = string.Format("({0},)",
                                        packageData.Version);

            return version;
        }
        */

        internal void UpdateExistingDependency([NotNull] XElement element,
            [NotNull] PackageData packageData)
        {
            UpdateDependencyId(element,
                packageData);

            UpdateDependencyVersion(element,
                packageData);
        }

        private static void UpdateDependencyVersion(XElement element,
            PackageData packageData)
        {
            var version = element.Attribute("version");

            if (version == null)
            {
                version = new XAttribute("version",
                    packageData.Version);

                element.Add(version);
            }
            else
            {
                version.Value = packageData.Version;
            }
        }

        private static void UpdateDependencyId(XElement element,
            PackageData packageData)
        {
            var id = element.Attribute("id");

            if (id == null)
            {
                id = new XAttribute("id",
                    packageData.Id);

                element.Add(id);
            }
            else
            {
                id.Value = packageData.Id;
            }
        }

        internal XElement CreateAndAddGroup([NotNull] XElement dependencies,
            [NotNull] PackageData packageData)
        {
            var group = new XElement("group",
                new XAttribute("targetFramework",
                    packageData.TargetFramework));

            dependencies.Add(group);

            return group;
        }

        [CanBeNull]
        internal XElement FindGroupElement([NotNull] XElement dependencies,
            [NotNull] PackageData packageData)
        {
            var groupElement = FindAndRemoveDependencyIfRequired(dependencies,
                packageData) ??
                               TryFindExistingGroupElement(dependencies,
                                   packageData);

            return groupElement;
        }

        private XElement FindAndRemoveDependencyIfRequired(XElement dependencies,
            PackageData packageData)
        {
            var currentGroupElement = TryFindCurrentGroupElement(dependencies,
                packageData);
            // todo testing
            if (IsWrongGroup(packageData,
                currentGroupElement))
            {
                RemoveFromGroup(currentGroupElement,
                    packageData);

                return null;
            }

            return currentGroupElement;
        }

        private XElement TryFindCurrentGroupElement(XElement dependencies,
            PackageData packageData)
        {
            foreach (var group in dependencies.Elements())
            {
                if (group.Name != "group")
                {
                    continue;
                }

                foreach (var dependency in group.Elements())
                {
                    if (dependency.Attribute("id")
                        .Value == packageData.Id)
                    {
                        return group;
                    }
                }
            }

            return null;
        }

        private static bool IsWrongGroup(PackageData packageData,
            XElement groupElement)
        {
            return groupElement != null && groupElement.Attribute("targetFramework")
                .Value != packageData.TargetFramework;
        }

        private static XElement TryFindExistingGroupElement(XElement dependencies,
            PackageData packageData)
        {
            foreach (var element in dependencies.Elements())
            {
                if (element.Name != "group")
                {
                    continue;
                }

                var groupTargetFramework = element.Attribute("targetFramework")
                    .Value;

                if (groupTargetFramework == packageData.TargetFramework)
                {
                    return element;
                }
            }

            return null;
        }
    }
}