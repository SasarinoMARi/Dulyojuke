using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using MediaToolkit;
using MediaToolkit.Model;
using YoutubeExtractor;

namespace dulyojuke
{
	/// <summary>
	/// MainWindow.xaml에 대한 상호 작용 논리
	/// </summary>
	public partial class MainWindow : Window
	{
		int downloadCount = 0;
		int downloadDone = 0;
		int errorCount = 0;

		public MainWindow( )
		{
			InitializeComponent( );
			TagAssister.Initialize( );
			textbox_downloadpath.Text = GetDefaultDownloadFolder( );
			UpdateCounts( );
		}

		private void button_download_Click( object sender, RoutedEventArgs e )
		{
			downloadCount++;

			var downloadUrl = GetDownloadUrl();
			var downlaodPath = GetDownloadPath();
			var tags = GetTagInfos();

			Task.Factory.StartNew( delegate
			{
				try
				{
					IEnumerable<VideoInfo> videoInfos = DownloadUrlResolver.GetDownloadUrls(downloadUrl);
					var videoPath = MediaDownloader.DownloadVideo( videoInfos, downlaodPath );
					var audioPath = MediaConverter.ConvertVideo( videoPath );
					TagAssister.ApplyTag( audioPath, tags );
					downloadDone++;
				}
				catch
				{
					errorCount++;
				}
				finally
				{
					UpdateCounts( );
				}
			} );

			UpdateCounts( );
		}

		/// <summary>
		/// 다운로드 계기판을 갱신합니다.
		/// </summary>
		private void UpdateCounts( )
		{
			this.Dispatcher.Invoke( new Action( ( ) =>
			{
				text_downloadstate.Content = string.Format( "성공: {1} / {0}회, 에러: {2} 회", downloadCount, downloadDone, errorCount );
			} ) );
		}

		/// <summary>
		/// 폼에서 다운로드 경로 폴더를 불러옵니다.
		/// </summary>
		/// <returns></returns>
		private string GetDownloadPath( )
		{
			return textbox_downloadpath.Text;
		}

		/// <summary>
		/// 폼에서 사용자가 입력한 태그를 불러옵니다.
		/// </summary>
		/// <param name="cleanInfos">작업 이후 내용을 지울지 여부</param>
		/// <returns></returns>
		private TagNode GetTagInfos( )
		{
			var node = new TagNode();
			node.Title = textbox_tag_title.Text;
			node.Artists = new string[] { textbox_tag_artist.Text };
			node.Album = textbox_tag_album.Text;
			node.AlbumArt = new string[] { textbox_tag_albumart.Text };

			if ( !checkbox_tag_title.IsChecked.Value ) textbox_tag_title.Text = string.Empty;
			if ( !checkbox_tag_artist.IsChecked.Value ) textbox_tag_artist.Text = string.Empty;
			if ( !checkbox_tag_album.IsChecked.Value ) textbox_tag_album.Text = string.Empty;
			if ( !checkbox_tag_albumart.IsChecked.Value ) textbox_tag_albumart.Text = string.Empty;

			return node;
		}

		/// <summary>
		/// 폼에서 영상 url을 얻어옵니다.
		/// </summary>
		/// <returns></returns>
		private string GetDownloadUrl( )
		{
			var url = textbox_downloadurl.Text;
			textbox_downloadurl.Text = string.Empty;
			return url;
		}

		/// <summary>
		/// 기본 다운로드 폴더 경로를 얻어옵니다.
		/// </summary>
		/// <returns></returns>
		private string GetDefaultDownloadFolder( )
		{
			string downloads;
			SHGetKnownFolderPath( KnownFolder.Downloads, 0, IntPtr.Zero, out downloads );
			return downloads;
		}

		#region native funcs
		public static class KnownFolder
		{
			public static readonly Guid Downloads = new Guid("374DE290-123F-4565-9164-39C4925E467B");
		}
		[DllImport( "shell32.dll", CharSet = CharSet.Unicode )]
		static extern int SHGetKnownFolderPath( [MarshalAs( UnmanagedType.LPStruct )] Guid rfid, uint dwFlags, IntPtr hToken, out string pszPath );
		#endregion

	}
}
