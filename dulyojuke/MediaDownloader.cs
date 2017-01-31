using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeExtractor;

namespace dulyojuke
{
	class MediaDownloader
	{
		/// <summary>
		/// url로부터 오디오를 얻어오는 함수. 작동 안함
		/// </summary>
		/// <param name="videoInfos"></param>
		public static void DownloadAudio( IEnumerable<VideoInfo> videoInfos )
		{
			VideoInfo video = videoInfos
				.Where(info => info.CanExtractAudio)
				.OrderByDescending(info => info.AudioBitrate)
				.First();

			if ( video.RequiresDecryption )
			{
				DownloadUrlResolver.DecryptDownloadUrl( video );
			}

			var audioDownloader = new AudioDownloader(video, Path.Combine("D:/Downloads", video.Title + video.AudioExtension));

			audioDownloader.DownloadProgressChanged += ( s, args ) => Console.WriteLine( args.ProgressPercentage * 0.85 );
			audioDownloader.AudioExtractionProgressChanged += ( s, args ) => Console.WriteLine( 85 + args.ProgressPercentage * 0.15 );

			audioDownloader.Execute( );
		}

		/// <summary>
		/// url로부터 영상을 얻어오는 함수.
		/// </summary>
		/// <param name="videoInfos"></param>
		/// <returns>다운로드받은 영상 디렉터리</returns>
		public static string DownloadVideo( IEnumerable<VideoInfo> videoInfos, string downloadPath )
		{
			// 영상 포맷들 중 해상도가 0인 영상을 받아온다.
			VideoInfo video = videoInfos.First(info => info.VideoType == VideoType.Mp4 && info.Resolution == 0);

			if ( video.RequiresDecryption )
			{
				DownloadUrlResolver.DecryptDownloadUrl( video );
			}

			var outputFilename = Path.Combine(downloadPath, MakeValidFileName( video.Title) + video.VideoExtension);
			var videoDownloader = new YoutubeExtractor.VideoDownloader( video,outputFilename );

			videoDownloader.DownloadProgressChanged += ( sender, args ) => Console.WriteLine( args.ProgressPercentage );
			videoDownloader.Execute( );

			return outputFilename;
		}

		/// <summary>
		/// 유효한 파일 이름을 반환합니다.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		private static string MakeValidFileName( string name )
		{
			string invalidChars = System.Text.RegularExpressions.Regex.Escape( new string( System.IO.Path.GetInvalidFileNameChars() ) );
			string invalidRegStr = string.Format( @"([{0}]*\.+$)|([{0}]+)", invalidChars );

			return System.Text.RegularExpressions.Regex.Replace( name, invalidRegStr, "_" );
		}
	}
}