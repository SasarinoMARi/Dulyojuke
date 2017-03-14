using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace dulyojuke
{
	class MediaDownloader
	{
		public class Exceptions
		{
			public class YoutubeDLException : Exception { }
			public class NotFoundOutputFileException : Exception { }
		}

		private static void GetYoutueVideo( string url, string dir, Action<string> callback )
		{
			var videoName = Path.Combine(dir, GetTitle(url));

			Process downloader = new Process();
			downloader.StartInfo.FileName = "youtube-dl.exe";
			downloader.StartInfo.Arguments = string.Format("-f bestaudio/worstvideo \"{0}\" -o \"{1}\" --no-playlist", url, videoName );
			//downloader.StartInfo.RedirectStandardOutput = true;
			downloader.StartInfo.UseShellExecute = false;
			downloader.StartInfo.CreateNoWindow = true;
			downloader.EnableRaisingEvents = true;

			downloader.Start( );
			downloader.WaitForExit( );

			//if ( downloader.ExitCode != 0 ) throw new Exceptions.YoutubeDLException( );

			callback( videoName );
		}

		private static void GetNicoVideo( string url, string dir, Action<string> callback, string username, string password )
		{
			var account = string.Format("-u \"{0}\" -p \"{1}\"", username, password);
			var videoName = Path.Combine(dir, GetTitle(url, account));

			Process downloader = new Process();
			downloader.StartInfo.FileName = "youtube-dl.exe";
			downloader.StartInfo.Arguments = string.Format( "{0} -o \"{1}\" {2}", url, videoName, account );
			//downloader.StartInfo.RedirectStandardOutput = true;
			downloader.StartInfo.UseShellExecute = false;
			downloader.StartInfo.CreateNoWindow = true;
			downloader.EnableRaisingEvents = true;

			downloader.Start( );
			downloader.WaitForExit( );

			//if ( downloader.ExitCode != 0 ) throw new Exceptions.YoutubeDLException( );

			callback( videoName );
		}

		private static string GetTitle( string url, string option = "" )
		{
			Process downloader = new Process();
			downloader.StartInfo.FileName = "youtube-dl.exe";
			downloader.StartInfo.Arguments = string.Format("-e {0} \"{1}\" --no-playlist", url, option );
			downloader.StartInfo.RedirectStandardOutput = true;
			downloader.StartInfo.UseShellExecute = false;
			downloader.StartInfo.CreateNoWindow = true;
			downloader.EnableRaisingEvents = true;

			downloader.Start( );
			downloader.WaitForExit( );

			//if ( downloader.ExitCode != 0 ) throw new Exceptions.YoutubeDLException( );

			var output = downloader.StandardOutput.ReadToEnd();
			return Utility.MakeValidFileName(output.Trim( ));
		}

		internal static void DownloadVideo( string downloadUrl, string downlaodPath, Action<string> callback )
		{
			if ( downloadUrl.StartsWith( "https://www.youtube.com" ) || downloadUrl.StartsWith( "https://youtu.be" ) )
			{
				GetYoutueVideo( downloadUrl, downlaodPath, callback );
			}
			else if ( downloadUrl.Contains( "http://www.nicovideo.jp" ) )
			{
				GetNicoVideo( downloadUrl, downlaodPath, callback, SharedPreference.Instance.NicoVideoUsername, SharedPreference.Instance.NicoVideoPassword );
			}
		}
	}
}