using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
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

        public static string GetDownloadClientPath()
        {
            return Path.Combine(Utility.getDataDirPath(), "Youtube-dl.exe");
        }

        private static void GetYoutueVideo(string url, string dir, Action<string> callback)
        {
            var videoName = Path.Combine(dir, GetTitle(url));

            Process downloader = new Process();
            downloader.StartInfo.FileName = GetDownloadClientPath();
            downloader.StartInfo.Arguments = string.Format("-f bestaudio/worstvideo \"{0}\" -o \"{1}\" --no-playlist", url, videoName);
            //downloader.StartInfo.RedirectStandardOutput = true;
            downloader.StartInfo.UseShellExecute = false;
            downloader.StartInfo.CreateNoWindow = true;
            downloader.EnableRaisingEvents = true;

            downloader.Start();
            downloader.WaitForExit();

            //if ( downloader.ExitCode != 0 ) throw new Exceptions.YoutubeDLException( );

            if (File.Exists(videoName)) callback(videoName);
            else GetYoutueVideo(url, dir, callback);
		}

		private static void GetNicoVideo( string url, string dir, Action<string> callback, string username, string password )
		{
			var account = string.Format("-u \"{0}\" -p \"{1}\"", username, password);
			var videoName = Path.Combine(dir, GetTitle(url, account));

			Process downloader = new Process();
			downloader.StartInfo.FileName = GetDownloadClientPath();
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

		internal static string GetTitle( string url, string option = "" )
		{
			var pt = Pre_Downloader.Instance.GetTitle(url);
			if (pt != null) return pt;

			Process downloader = new Process();
			downloader.StartInfo.FileName = GetDownloadClientPath();
            downloader.StartInfo.Arguments = string.Format("-e {0} {1} --no-playlist", url, option );
			downloader.StartInfo.RedirectStandardOutput = true;
			downloader.StartInfo.UseShellExecute = false;
			downloader.StartInfo.CreateNoWindow = true;
			downloader.EnableRaisingEvents = true;

			downloader.Start( );
			downloader.WaitForExit( );

			//if ( downloader.ExitCode != 0 ) throw new Exceptions.YoutubeDLException( );

			var output = downloader.StandardOutput.ReadToEnd();
            if (string.IsNullOrEmpty(output)) return GetTitle(url, option);
			var vfn = Utility.MakeValidFileName(output.Trim());
			Pre_Downloader.Instance.AddPDSName(url, vfn);
			return vfn;
		}

		internal static Bitmap GetThumbnail(string url)
		{
			var pt = Pre_Downloader.Instance.GetThumbnail(url);
			if (pt != null) return pt;

			var path = Path.Combine(Utility.GetImageTempFolder(), DateTime.Now.Ticks.ToString());

			Process downloader = new Process();
			downloader.StartInfo.FileName = GetDownloadClientPath();
            downloader.StartInfo.Arguments = string.Format("\"{0}\" --write-thumbnail -o \"{1}\"", url, path);
			downloader.StartInfo.UseShellExecute = false;
			downloader.StartInfo.CreateNoWindow = true;
			downloader.EnableRaisingEvents = true;

			downloader.Start();
			downloader.WaitForExit();

            if (File.Exists(path))
            {
                Bitmap rtn = null;
                if (File.Exists(path))
                {
                    File.Delete(path);

                    // 미친 소리 같지만 사실입니다.
                    if (File.Exists(path + ".jpg"))
                        rtn = new Bitmap(path + ".jpg");
                    if (File.Exists(path + ".png"))
                        rtn = new Bitmap(path + ".png");
                    if (File.Exists(path + ".gif"))
                        rtn = new Bitmap(path + ".gif");
                    if (File.Exists(path + ".jpeg"))
                        rtn = new Bitmap(path + ".jpeg");

                }

                Pre_Downloader.Instance.AddPDSBitmap(url, rtn);
                return rtn;
            }
            else return GetThumbnail(url);
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

	public class Pre_Downloader {

		private Pre_Downloader()
		{

		}

		private static Pre_Downloader instance;
		public static Pre_Downloader Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new Pre_Downloader();
				}
				return instance;
			}
		}

		private List<Pre_DownloadState> progresses = new List<Pre_DownloadState>();

		public bool isExist(string url) {
			foreach (var item in progresses)
			{
				if (item.url == url) {
					return true;
				}
			}
			return false;
		}

		internal void AddPDS(string url, string name, Bitmap path)
		{
			var t = getProgress(url);
			t.url = url;
			t.dougaName = name;
			t.imagePath = path;
			addProgress(t);
		}

		internal void AddPDSName(string url, string name)
		{
			var t = getProgress(url);
			t.url = url;
			t.dougaName = name;
			addProgress(t);
		}

		internal void AddPDSBitmap(string url, Bitmap map)
		{
			var t = getProgress(url);
			t.url = url;
			t.imagePath = map;
			addProgress(t);
		}

		private Pre_DownloadState getProgress(string url)
		{
			foreach (var item in progresses)
			{
				if (item.url == url)
				{
					return item;
				}
			}
			return new Pre_DownloadState();
		}

		private void addProgress(Pre_DownloadState progress) {
			foreach (var item in progresses)
			{
				if (item.url == progress.url)
				{
					progresses.Remove(item);
					break;
				}
			}
			progresses.Add(progress);
		}

		internal Bitmap GetThumbnail(string url)
		{
			foreach (var item in progresses)
			{
				if (item.url == url)
				{
					return item.imagePath;
				}
			}
			return null;
		}

		internal string GetTitle(string url)
		{
			foreach (var item in progresses)
			{
				if (item.url == url)
				{
					return item.dougaName;
				}
			}
			return null;
		}

		class Pre_DownloadState {
			public string url { get; set; }
			public Bitmap imagePath { get; set; }
			public string dougaName { get; set; }
		}

	}
}