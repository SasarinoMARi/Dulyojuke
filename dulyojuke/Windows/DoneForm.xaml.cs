using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace dulyojuke.Windows
{
	/// <summary>
	/// Interaction logic for DoneForm.xaml
	/// </summary>
	public partial class DoneForm : UserControl, PageInterface
	{
		public DoneForm( )
		{
			InitializeComponent( );

		}

		private TagNode CreateTagNode( string title, string artist, string albumname, System.Drawing.Image albumart )
		{
			return new TagNode( ) { Title = title, Artists = new string[] { artist }, Album = albumname, AlbumArt = new System.Drawing.Image[] { albumart } };
		}

		private void StartDownload( string videoUrl, TagNode tag )
		{
			videoUrl = videoUrl.Trim();
			var downlaodPath = SharedPreference.Instance.DownloadPath;

			Task.Factory.StartNew( delegate
			{
				try
				{
					MediaDownloader.DownloadVideo( videoUrl, downlaodPath, delegate ( string videoPath )
					{
						var audioPath = MediaConverter.ConvertVideo( videoPath );
						TagAssister.ApplyTag( audioPath, tag );
					} );
				}
				catch ( Exception exception )
				{
					LogAssister.Output( exception.ToString( ) );
				}
				finally
				{

				}
			} );
		}

		void PageInterface.setContentChangeEvent( SceneSwitchAdapter @event )
		{
			@event.AttachEventHandlers( null, null, null, Button_Next);
		}

		void PageInterface.setData( Dictionary<string, string> data )
		{
			var VideoUrl = data["VideoUrl"];
			var Title = data["Title"];
			var Artist = data["Artist"];
			var AlbumName = data["AlbumName"];
			System.Drawing.Image AlbumArt = null;
			if(data.ContainsKey("AlbumArt") )
			{
				var albumArtString = data["AlbumArt"];
				AlbumArt = Utility.DeserializeImageToString( albumArtString );
			}

			StartDownload( VideoUrl, CreateTagNode( Title, Artist, AlbumName, AlbumArt ) );
		}

		Dictionary<string, string> PageInterface.getData( )
		{
			return null;
		}
	}
}
