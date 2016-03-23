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
    ///     Class that updates the version info of merge module projects
    /// </summary>
    public class MsmProjectVersioner : VdProjectVersioner
    {
        private static readonly Guid MsmProjectType = new Guid("{DD7A5B58-C2F9-40FF-B2EF-0773356FB978}");

        private string _signature;

        /// <summary>
        ///     createas a new instance of the class
        /// </summary>
        /// <param name="file">the project file</param>
        public MsmProjectVersioner(FileInfo file) : base(file, MsmProjectType)
        {
        }

        protected override void SetOptions(Hashtable options)
        {
            // get the signature - generate if not specified
            if (options.Contains("signature"))
                _signature = options[_signature].ToString();
            else
                _signature = "MergeModule." + Guid.NewGuid().ToString().ToUpper().Replace("-", "");
        }

        protected override string TranslateLine(string line)
        {
            // look for the properties we are interested in changing
            // if we find them, replace the old value with the new and return the new line string
            // otherwise just return the line string
            if (line.IndexOf("\"Version\"", StringComparison.CurrentCultureIgnoreCase) != -1)
                line = ReplaceValue(line, "Version", Version.FourPartVersion);

            // module signature cannot have dashes
            else if (line.IndexOf("\"ModuleSignature\"", StringComparison.CurrentCultureIgnoreCase) != -1)
                line = ReplaceValue(line, "ModuleSignature", _signature);

            return line;
        }
    }
}