//
// copyright 2003 Don Kackman - mailto:dkackman_2000@yahoo.com
//
// no warranty expressed or implied
// use however you'd like
//

using System;
using System.Collections;
using System.IO;
using System.Text;

namespace VersionVDProj
{
    /// <summary>
    ///     Base class for updating the version and guid of Visual Studio deployment projects
    /// </summary>
    public abstract class VdProjectVersioner
    {
        private const string Header = "\"DeployProject\"";

        private readonly FileInfo _projectFile;
        protected Version Version;

        /// <summary>
        ///     Creates a new isntance of the class
        /// </summary>
        /// <param name="file">the project file</param>
        /// <param name="projectType">Identifier for the type of project file</param>
        protected VdProjectVersioner(FileInfo file, Guid projectType)
        {
            _projectFile = file;

            using (var reader = new StreamReader(_projectFile.OpenRead()))
            {
                var line = reader.ReadLine();

                // make sure it's a deployment project
                if (line != null && line.IndexOf(Header, StringComparison.CurrentCultureIgnoreCase) < 0)
                    throw new ArgumentException(string.Format("The file {0} is not a VS deployment project file ",
                        _projectFile.FullName));

                // make sure it is the type of project that the derived class expects
                if (projectType != GetProjectType(reader))
                    throw new Exception(string.Format("The file {0} is not of the expected type", _projectFile.FullName));

                Console.WriteLine("Processing {0}", _projectFile.FullName);
            }
        }

        /// <summary>
        ///     Deployment project each have a guis that denotes what type of project they are
        ///     (msi vs msm). This method digs the guid out of the file and returns it
        /// </summary>
        /// <param name="reader">Reader for the already opened project</param>
        /// <returns>The project type guid (<see cref="Guid.Empty" /> if an error occurs)</returns>
        private static Guid GetProjectType(StreamReader reader)
        {
            var ret = Guid.Empty;
            try
            {
                var line = reader.ReadLine();

                //loop through the file, find the ProjectType line and extract the guid
                while (line != null)
                {
                    if (line.IndexOf("\"ProjectType\"", StringComparison.CurrentCultureIgnoreCase) != -1)
                    {
                        var openbracket = line.IndexOf('{');
                        var closebracket = line.IndexOf('}');

                        if (openbracket == -1 || closebracket == -1)
                            throw new Exception("File format does not conform to the expected layout");

                        ret = new Guid(line.Substring(openbracket, closebracket - openbracket + 1));
                        break;
                    }
                    line = reader.ReadLine();
                }
            }
            catch
            {
                return Guid.Empty;
            }
            return ret;
        }

        /// <summary>
        ///     Updates the file passes to the constructor
        /// </summary>
        /// <param name="options">name value pairs to use for updating the file's contents</param>
        public void UpdateFile(Hashtable options)
        {
            if (!options.Contains("version"))
                throw new Exception("Expected version argument");

            Version = new Version(options["version"].ToString());

            SetOptions(options);

            Console.WriteLine("Updating project file");
            var newFileContent = TranslateFile(); //get the new file contents

            Console.WriteLine("Saving project file");

            // overwrite the existing file with the updated ocntents
            using (var writer = new StreamWriter(_projectFile.OpenWrite()))
                writer.Write(newFileContent);
        }

        /// <summary>
        ///     Implemeneted by derived classes to validate that required arguments are present
        ///     and set internal state based on name value pairs passed to the program
        /// </summary>
        /// <param name="options">name value pairs</param>
        protected abstract void SetOptions(Hashtable options);

        private string TranslateFile()
        {
            using (var reader = new StreamReader(_projectFile.OpenRead()))
            {
                // create a string buffer and trnaslate the current file contents updating the properties we need to along the way
                var sb = new StringBuilder();
                var writer = new StringWriter(sb);

                var line = reader.ReadLine();
                while (line != null)
                {
                    writer.WriteLine(TranslateLine(line));
                    line = reader.ReadLine();
                }

                writer.Flush();

                // return the translated file contents as a string
                return sb.ToString();
            }
        }

        /// <summary>
        ///     Method to be implmented by concrete class to process the file line by line
        /// </summary>
        /// <param name="line">The line being translated</param>
        /// <returns>The translated line</returns>
        protected abstract string TranslateLine(string line);

        /// <summary>
        ///     In a line of the format
        ///     name = "8:value"
        ///     will set nameed value to a new value
        /// </summary>
        /// <param name="valueLine">The line being translated</param>
        /// <param name="name">The name of the pair</param>
        /// <param name="newValue">The new value</param>
        /// <returns>The translated line</returns>
        protected string ReplaceValue(string valueLine, string name, string newValue)
        {
            // find the position of the equal sign
            var eqPos = valueLine.IndexOf('=');
            if (eqPos == -1)
                throw new Exception("File format does not conform to the expected layout");

            Console.WriteLine("Setting {0} to {1}", name, newValue);

            // then splice the part before the equal sign with the new value 
            // (including the '8:' that the vdproj format uses for these properties)
            var newString = valueLine.Substring(0, eqPos + 1);
            newString += string.Format(" \"8:{0}\"", newValue);

            return newString;
        }
    }
}