namespace dulyojuke
{
	class Utility
	{
		/// <summary>
		/// 유효한 파일 이름을 반환합니다.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public static string MakeValidFileName( string name )
		{
			string invalidChars = System.Text.RegularExpressions.Regex.Escape( new string( System.IO.Path.GetInvalidFileNameChars() ) );
			string invalidRegStr = string.Format( @"([{0}]*\.+$)|([{0}]+)", invalidChars );

			return System.Text.RegularExpressions.Regex.Replace( name, invalidRegStr, "_" );
		}
	}
}
