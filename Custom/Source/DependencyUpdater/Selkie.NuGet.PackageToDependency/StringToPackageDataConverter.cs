using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using JetBrains.Annotations;

namespace Selkie.NuGet.PackageToDependency
{
    public class StringToPackageDataConverter
    {
        private readonly List<PackageData> m_Data = new List<PackageData>();

        [NotNull]
        public IEnumerable<PackageData> Data
        {
            get { return m_Data; }
        }

        public void Convert([NotNull] string text)
        {
            m_Data.Clear();

            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            var packagesElement = GetPackagesElement(text);

            if (packagesElement == null)
            {
                return;
            }

            ProcessPackageElements(packagesElement);
        }

        private void ProcessPackageElements(XElement packagesElement)
        {
            foreach (var packageElement in packagesElement.Elements())
            {
                var data = CreatePackageData(packageElement);

                m_Data.Add(data);
            }
        }

        private XElement GetPackagesElement([NotNull] string text)
        {
            var reader = new StringReader(text);
            var document = XDocument.Load(reader);

            var rootElement = document.Root;

            if (rootElement != null &&
                rootElement.Name == "packages")
            {
                return rootElement;
            }

            return null;
        }

        private PackageData CreatePackageData(XElement packageElement)
        {
            var data = new PackageData
            {
                Id = packageElement.Attribute("id")
                    .Value,
                Version = packageElement.Attribute("version")
                    .Value,
                TargetFramework = packageElement.Attribute("targetFramework")
                    .Value
            };
            return data;
        }
    }
}