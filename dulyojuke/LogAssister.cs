using System;
using System.IO;

namespace dulyojuke
{
	class LogAssister
	{
		public static void Output( string str )
		{
			File.WriteAllText(
			Path.Combine(
				Directory.GetCurrentDirectory( ),
				string.Format( "ErrorReport-{0}.txt", Utility.MakeValidFileName( DateTime.Now.ToString( "yyyy-MM-dd_hh-mm-ss") ) )
			), str );
		}
	}
}
