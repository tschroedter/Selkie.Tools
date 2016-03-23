//
// copyright 2003 Don Kackman - mailto:dkackman_2000@yahoo.com
//
// no warranty expressed or implied
// use however you'd like
//
using System;
using System.IO;
using System.Collections;

namespace VersionVDProj
{
	/// <summary>
	/// Class that updates the version info of merge module projects
	/// </summary>
	public class MSMProjectVersioner : VDProjectVersioner
	{
		private readonly static Guid MSM_PROJECT_TYPE = new Guid( "{DD7A5B58-C2F9-40FF-B2EF-0773356FB978}" );

		private string signature;

		/// <summary>
		/// createas a new instance of the class
		/// </summary>		 
		/// <param name="file">the project file</param>
		public MSMProjectVersioner( FileInfo file ) : base( file, MSM_PROJECT_TYPE )
		{
		}

		protected override void SetOptions( Hashtable options )
		{
			// get the signature - generate if not specified
			if ( options.Contains( "signature" ) )
				signature = options[signature].ToString();
			else
				signature = "MergeModule." + Guid.NewGuid().ToString().ToUpper().Replace( "-", ""  );
		}

		protected override string TranslateLine( string line )
		{
			// look for the properties we are interested in changing
			// if we find them, replace the old value with the new and return the new line string
			// otherwise just return the line string
			if ( line.IndexOf( "\"Version\"" ) != -1 )
				line = ReplaceValue( line, "Version", version.FourPartVersion );
			
			// module signature cannot have dashes
			else if ( line.IndexOf( "\"ModuleSignature\"" ) != -1 )
				line = ReplaceValue( line, "ModuleSignature", signature );

			return line;
		}
	}
}
