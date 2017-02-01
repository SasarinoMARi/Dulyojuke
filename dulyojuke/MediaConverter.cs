using System.IO;
using MediaToolkit;
using MediaToolkit.Model;

namespace dulyojuke
{
	class MediaConverter
	{
		/// <summary>
		/// 다운받은 영상을 mp3로 변환한다. 
		/// </summary>
		/// <param name="inputPath"></param>
		/// <param name="clearOrigin">원본 파일을 삭제할지 여부</param>
		/// <returns>변환된 mp3 파일 디렉터리</returns>
		public static string ConvertVideo( string inputPath, bool clearOrigin = true )
		{
			var outputPath = Path.ChangeExtension(inputPath, "mp3");

			var inputFile = new MediaFile {Filename = inputPath};
			var outputFile = new MediaFile {Filename = outputPath};

			using ( var engine = new Engine( ) )
			{
				engine.Convert( inputFile, outputFile );
			}

			if ( clearOrigin )
			{
				File.Delete( inputPath );
			}

			return outputPath;
		}
	}
}
