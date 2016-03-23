//
// copyright 2003 Don Kackman - mailto:dkackman_2000@yahoo.com
//
// no warranty expressed or implied
// use however you'd like
//

using System;

namespace VersionVDProj
{
    /// <summary>
    ///     Class representing a three or four part version string
    /// </summary>
    public class Version
    {
        private string[] _parts;

        public Version(string v)
        {
            if (!ValidateVersion(v))
                throw new ArgumentException("The version must be in the format #.#.# or #.#.#.#");
        }

        public string ThreePartVersion
        {
            get { return string.Join(".", _parts, 0, 3); }
        }

        public string FourPartVersion
        {
            get
            {
                if (_parts.Length == 3)
                    return ThreePartVersion + ".0";
                return string.Join(".", _parts, 0, 4);
            }
        }

        private bool ValidateVersion(string version)
        {
            var tmp = version.Split('.');

            if (tmp.Length != 3 && tmp.Length != 4)
                return false;

            try
            {
                // loop through each member of the split array and make sure it's numeric
                foreach (var s in tmp)
                    // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                    Convert.ToInt32(s);
            }
            catch (FormatException)
            {
                return false;
            }

            _parts = tmp;

            return true;
        }
    }
}