using System;
using Selkie.NuGet.PackageToDependency;

namespace Selkie.NuGet.DependencyUpdater
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("The program requires two parameters!");
                Console.WriteLine("Example: Updater.exe <package config Filename> <nuspec Filename>");
                Environment.Exit(-1);
            }

            try
            {
                Console.WriteLine("Updating dependencies...");

                var packageConfigFilename = args[0];
                var nuspecFilename = args[1];

                var updater = new Updater(packageConfigFilename,
                    nuspecFilename);

                updater.Update();

                Console.WriteLine("...Done!");
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: Didn't update dependencies because of an exception!");
                Console.WriteLine();
                Console.WriteLine(ex.ToString());
                Console.WriteLine();
                Console.WriteLine(ex.StackTrace);

                Environment.Exit(-1);
            }
        }
    }
}