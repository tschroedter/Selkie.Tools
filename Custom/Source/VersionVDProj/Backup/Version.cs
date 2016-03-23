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
	/// Class representing a three or four part version string
	/// </summary>
	public class Version
	{
		private string[] parts;

		public Version( string v )
		{
			if ( !ValidateVersion( v ) )
				throw new ArgumentException( "The version must be in the format #.#.# or #.#.#.#" );
		}

		public string ThreePartVersion
		{
			get
			{
				return string.Join( ".", parts, 0, 3 );
			}
		}

		public string FourPartVersion
		{
			get
			{
				if ( parts.Length == 3 )
					return ThreePartVersion + ".0";
				else
					return string.Join( ".", parts, 0, 4 );
			}
		}

		private bool ValidateVersion( string version )
		{
			string[] tmp = version.Split( new char[] { '.' } );

			if ( tmp.Length != 3 && tmp.Length != 4 )
				return false;
			
			try
			{
				// loop through each member of the split array and make sure it's numeric
				foreach ( string s in tmp )
					Convert.ToInt32( s );
			}
			catch ( FormatException )
			{
				return false;
			}

			parts = tmp;

			return true;
		}
	}
}
