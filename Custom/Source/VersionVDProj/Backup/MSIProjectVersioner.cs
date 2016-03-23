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
	/// Class that updates the version info of MSI projects
	/// </summary>
	public class MSIProjectVersioner : VDProjectVersioner
	{
		private readonly static Guid MSI_PROJECT_TYPE = new Guid( "{2C2AF0D9-9B47-4FE5-BEF2-169778172667}" );

		private Guid productCode = Guid.Empty;
		private Guid packageCode = Guid.Empty;
		private Guid upgradeCode = Guid.Empty;
		/// <summary>
		/// createas a new instance of the class
		/// </summary>
		/// <param name="file">the project file</param>
		public MSIProjectVersioner( FileInfo file ) : base( file, MSI_PROJECT_TYPE )
		{

		}

		protected override void SetOptions( Hashtable options )
		{
			// get the package code - generate if not specified
			if ( options.Contains("packagecode") )
				packageCode = new Guid( options["packagecode"].ToString() );
			else
				packageCode = Guid.NewGuid();

			// get the product code - set to package code if not specified
			if ( options.Contains("productcode") )
				productCode = new Guid( options["productcode"].ToString() );
			else
				productCode = packageCode;

			// get the upgrade code - leave empty if not specified
			if ( options.Contains("upgradecode") )
				upgradeCode = new Guid( options["upgradecode"].ToString() );
		}

		protected override string TranslateLine( string line )
		{
			// look for the properties we are interested in changing
			// if we find them, replace the old value with the new and return the new line string
			// otherwise just return the line string
			if ( line.IndexOf( "\"ProductVersion\"" ) != -1 )
				line = ReplaceValue( line, "ProductVersion", version.ThreePartVersion );
			
			else if ( productCode != Guid.Empty && line.IndexOf( "\"ProductCode\"" ) != -1 )
				line = ReplaceValue( line, "ProductCode", "{" + productCode.ToString().ToUpper() + "}" );

			else if ( packageCode != Guid.Empty && line.IndexOf( "\"PackageCode\"" ) != -1 )
				line = ReplaceValue( line, "PackageCode", "{" + packageCode.ToString().ToUpper() + "}" );

			else if ( upgradeCode != Guid.Empty && line.IndexOf( "\"UpgradeCode\"" ) != -1 )
				line = ReplaceValue( line, "UpgradeCode", "{" + upgradeCode.ToString().ToUpper() + "}" );

			return line;
		}
	}
}
