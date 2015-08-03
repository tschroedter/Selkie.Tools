using JetBrains.Annotations;

namespace Selkie.NuGet.PackageToDependency
{
    public class Updater
    {
        private readonly PackageNuSpecFileReader m_NuSpec;
        private readonly NuSpecDependenciesUpdater m_NuSpecUpdater;
        private readonly PackageConfigFileReader m_PackageConfig;

        public Updater([NotNull] string packageConfigFilename,
            [NotNull] string nuspecFilename)
        {
            m_NuSpecUpdater = new NuSpecDependenciesUpdater();

            m_PackageConfig = new PackageConfigFileReader(packageConfigFilename);
            m_PackageConfig.Read();

            m_NuSpec = new PackageNuSpecFileReader(nuspecFilename);
            m_NuSpec.Read();
        }

        public void Update()
        {
            m_NuSpecUpdater.Update(m_NuSpec.Dependencies,
                m_PackageConfig.PackageData);

            m_NuSpec.Write();
        }
    }
}