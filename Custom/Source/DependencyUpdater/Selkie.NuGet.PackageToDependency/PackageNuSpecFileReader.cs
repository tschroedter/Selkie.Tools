﻿using System.Linq;
using System.Xml.Linq;
using JetBrains.Annotations;

namespace Selkie.NuGet.PackageToDependency
{
    public class PackageNuSpecFileReader : ReadAllLines
    {
        private readonly string m_Filename;
        private XElement m_Dependencies = new XElement("NoFileLoaded");
        private XDocument m_Document;

        public PackageNuSpecFileReader([NotNull] string filename)
        {
            m_Filename = filename;
        }

        [NotNull]
        public XElement Dependencies
        {
            get { return m_Dependencies; }
        }

        public void Read()
        {
            m_Document = XDocument.Load(m_Filename);

            var root = m_Document.Root;

            if (root != null)
            {
                var metadata = root.Elements("metadata")
                    .ToArray();

                var dependencies = metadata.Elements("dependencies")
                    .First();

                if (dependencies != null)
                {
                    m_Dependencies = dependencies;
                }
            }
        }

        public void Write()
        {
            m_Document.Save(m_Filename);
        }
    }
}