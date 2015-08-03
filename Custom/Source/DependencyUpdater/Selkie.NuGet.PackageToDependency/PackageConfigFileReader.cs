using System.Collections.Generic;
using JetBrains.Annotations;

namespace Selkie.NuGet.PackageToDependency
{
    public class PackageConfigFileReader : ReadAllLines
    {
        private readonly StringToPackageDataConverter m_Converter;
        private readonly string m_Filename;

        public PackageConfigFileReader([NotNull] string filename)
        {
            m_Filename = filename;

            m_Converter = new StringToPackageDataConverter();
        }

        [NotNull]
        public IEnumerable<PackageData> PackageData
        {
            get { return m_Converter.Data; }
        }

        public void Read()
        {
            var xml = ReadAll(m_Filename);
            m_Converter.Convert(xml);
        }
    }
}