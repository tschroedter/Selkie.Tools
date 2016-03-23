//
// copyright 2003 Don Kackman - mailto:dkackman_2000@yahoo.com
//
// no warranty expressed or implied
// use however you'd like
//

using System;
using System.Collections;
using System.IO;

namespace VersionVDProj
{
    /// <summary>
    ///     Class that updates the version info of MSI projects
    /// </summary>
    public class MsiProjectVersioner : VdProjectVersioner
    {
        private static readonly Guid MsiProjectType = new Guid("{978C614F-708E-4E1A-B201-565925725DBA}");
        private Guid _packageCode = Guid.Empty;

        private Guid _productCode = Guid.Empty;
        private Guid _upgradeCode = Guid.Empty;

        /// <summary>
        ///     createas a new instance of the class
        /// </summary>
        /// <param name="file">the project file</param>
        public MsiProjectVersioner(FileInfo file) : base(file, MsiProjectType)
        {
        }

        protected override void SetOptions(Hashtable options)
        {
            // get the package code - generate if not specified
            _packageCode = options.Contains("packagecode")
                ? new Guid(options["packagecode"].ToString())
                : Guid.NewGuid();

            // get the product code - set to package code if not specified
            _productCode = options.Contains("productcode") ? new Guid(options["productcode"].ToString()) : _packageCode;

            // get the upgrade code - leave empty if not specified
            if (options.Contains("upgradecode"))
                _upgradeCode = new Guid(options["upgradecode"].ToString());
        }

        protected override string TranslateLine(string line)
        {
            // look for the properties we are interested in changing
            // if we find them, replace the old value with the new and return the new line string
            // otherwise just return the line string
            if (line.IndexOf("\"ProductVersion\"", StringComparison.CurrentCultureIgnoreCase) != -1)
                line = ReplaceValue(line, "ProductVersion", Version.ThreePartVersion);

            else if (_productCode != Guid.Empty &&
                     line.IndexOf("\"ProductCode\"", StringComparison.CurrentCultureIgnoreCase) != -1)
                line = ReplaceValue(line, "ProductCode", "{" + _productCode.ToString().ToUpper() + "}");

            else if (_packageCode != Guid.Empty &&
                     line.IndexOf("\"PackageCode\"", StringComparison.CurrentCultureIgnoreCase) != -1)
                line = ReplaceValue(line, "PackageCode", "{" + _packageCode.ToString().ToUpper() + "}");

            else if (_upgradeCode != Guid.Empty &&
                     line.IndexOf("\"UpgradeCode\"", StringComparison.CurrentCultureIgnoreCase) != -1)
                line = ReplaceValue(line, "UpgradeCode", "{" + _upgradeCode.ToString().ToUpper() + "}");
            else if (line.IndexOf("#VERSION#", StringComparison.CurrentCultureIgnoreCase) != -1)
            {
                line = line.Replace("#VERSION#", Version.ThreePartVersion);
            }

            return line;
        }
    }
}