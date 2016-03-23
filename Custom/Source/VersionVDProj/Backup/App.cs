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
	/// This is the class that has the Main method. This class is responsible for parsing command line
	/// arguments and dispatching them to the correct classes
	/// </summary>
	class App
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static int Main(string[] args)
		{
			if ( args.Length != 0 )
			{
				try
				{
					ArgumentParser arguments = new ArgumentParser( args );
					VDProjectVersioner versioner = null;

					switch ( arguments.Command )
					{
						case "msi":
							versioner = new MSIProjectVersioner( arguments.ProjectFile );
							break;
						case "msm":
							versioner = new MSMProjectVersioner( arguments.ProjectFile );
							break;
						default:
							throw new Exception( string.Format( "Unrecognized command {0}", arguments.Command ) );
					}

					versioner.UpdateFile( arguments.Options );
					return 0;
				}
				catch ( Exception e )
				{
					Console.WriteLine( "ERROR" );
					Console.WriteLine( e.Message );
					return 1;
				}
			}
			
			// with no arguments show the usage
			ShowUsage();
			return 0;
		}

		private static void ShowUsage()
		{
			Console.WriteLine( "VersionVDProj -msi <PATH> version=<VERSION> [package=<PACKAGECODE>]" );
			Console.WriteLine( "\t[product=<PRODUCTCODE>] [upgrade=<UPGRADECODE>]" );
			Console.WriteLine( "The -msi switch is used for updating MSI installer projects" );
			Console.WriteLine( "" );
			Console.WriteLine( "<PATH> is the path to the vdproj file (without braces)" );
			Console.WriteLine( "<VERSION> is the version to use in format #.#.# (without braces)" );
			Console.WriteLine( "<PACKAGECODE> Optional package guid (if not specified will be auto-generated)" );
			Console.WriteLine( "<PRODUCTCODE> Optional product guid (if not specified will be set to the" );
			Console.WriteLine( "\tsame value used to set the package guid)" );
			Console.WriteLine( "<UPGRADECODE> Optional upgrade guid (if not specified the upgrade code will" );
			Console.WriteLine( "\tnot be modified)" );
			Console.WriteLine( "" );
			Console.WriteLine( "VersionVDProj -msm <PATH> version=<VERSION> [signature=<SIGNATURE>]" );
			Console.WriteLine( "The -msm switch is used for updating merge module projects" );
			Console.WriteLine( "" );
			Console.WriteLine( "<PATH> is the path to the vdproj file (without braces)" );
			Console.WriteLine( "<VERSION> is the version to use in format #.#.# (without braces)" );
			Console.WriteLine( "<SIGNATURE> Optional unique id for the merge module (if not specified a guid" );
			Console.WriteLine( "\tbased signature will be auto-generated)" );
			Console.WriteLine( "" );
			Console.WriteLine( "Named arguments can be specified in any order" );
		}
	}
}
