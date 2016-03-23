using System;
using System.Collections;
using System.IO;

namespace VersionVDProj
{
    /// <summary>
    ///     Summary description for ArgumentParser.
    /// </summary>
    public class ArgumentParser
    {
        public readonly string Command;
        public readonly Hashtable Options;
        public readonly FileInfo ProjectFile;

        public ArgumentParser(string[] args)
        {
            Options = new Hashtable();

            if (args.Length < 2)
                throw new Exception("Wrong number of arguments");

            Command = ParseCommand(args[0]);
            ProjectFile = ParseFileName(args[1]);

            for (var i = 2; i < args.Length; i++)
                ParseArgument(args[i]);
        }

        private string ParseCommand(string command)
        {
            if (command.Substring(0, 1) != "-")
                throw new Exception(string.Format("Unrecognized command {0}", command));

            return command.Substring(1).ToLower();
        }

        private FileInfo ParseFileName(string path)
        {
            var file = new FileInfo(path);
            if (!file.Exists)
                throw new Exception(string.Format("The file {0} could not be found", path));

            return file;
        }

        private void ParseArgument(string arg)
        {
            var eqPos = arg.IndexOf('=', 0);

            if (eqPos == -1)
                throw new Exception(string.Format("The argument {0} is not in the format NAME=VALUE", arg));

            var name = arg.Substring(0, eqPos);
            var val = arg.Substring(eqPos + 1);

            Options.Add(name.ToLower(), val);
        }
    }
}