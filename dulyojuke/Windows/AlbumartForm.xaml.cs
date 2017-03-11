using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

namespace dulyojuke.Windows
{
	/// <summary>
	/// Interaction logic for AlbumartForm.xaml
	/// </summary>
	public partial class AlbumartForm : System.Windows.Controls.UserControl, PageInterface
	{
		string VideoUrl;
		private System.Drawing.Image AlbumArt;

		public AlbumartForm( )
		{
			InitializeComponent( );
		}

		private void Button_Next_Click( object sender, RoutedEventArgs e )
		{

		}

		private System.Drawing.Image OpenAlbumArt( )
		{
			OpenFileDialog openFile = new OpenFileDialog();
			openFile.DefaultExt = "jpg";
			openFile.Filter = "Images Files(*.jpg; *.jpeg; *.gif; *.bmp; *.png)|*.jpg;*.jpeg;*.gif;*.bmp;*.png";
			openFile.ShowDialog( );
			if ( openFile.FileNames.Length > 0 )
			{
				// 우선은 맨 앞의 것만 선택되도록
				foreach ( string filename in openFile.FileNames )
				{
					return new Bitmap( filename );
				}
			}
			return null;
		}

		private System.Drawing.Image CaptureAlbumArt( )
		{
			System.Drawing.Image capture = null;
			var stasisForm = new Stasisfield( );
			stasisForm.ShowDialog( );
			capture = stasisForm.CropedImage;
			return capture;
		}

		private System.Drawing.Image DownloadAlbumArt( string imageurl )
		{
			if ( imageurl.StartsWith( "http://" ) || imageurl.StartsWith( "https://" ) )
			{
				try
				{
					System.Net.WebRequest request = System.Net.WebRequest.Create(imageurl);
					System.Net.WebResponse response = request.GetResponse();
					Stream responseStream = response.GetResponseStream();
					return new Bitmap( responseStream );
				}
				catch { }
			}
			return null;
		}

		private System.Drawing.Image GetThumbnail( )
		{
			return null;
		}


		void PageInterface.setContentChangeEvent( RoutedEventHandler toPrevEvent, RoutedEventHandler toNextEvent )
		{
			this.Button_Prev.Click += toPrevEvent;
			this.Button_Next.Click += toNextEvent;
		}

		void PageInterface.setData( Dictionary<string, string> data )
		{
			this.VideoUrl = data["VideoUrl"];
		}

		Dictionary<string, string> PageInterface.getData( )
		{
			var data = new Dictionary<string, string>();
			data.Add( "AlbumArt", Utility.SerializeImageToString( AlbumArt ) );
			return data;
		}

		private void Button_Albumart_FromImage_Click( object sender, RoutedEventArgs e )
		{
			AlbumArt = OpenAlbumArt( );
			if ( AlbumArt == null )
			{
				System.Windows.MessageBox.Show( "잘못된 이미지입니다" );
				return;
			}
			this.Button_Next.RaiseEvent( new RoutedEventArgs( System.Windows.Controls.Button.ClickEvent ) );
		}

		private void Button_Albumart_FromCapture_Click( object sender, RoutedEventArgs e )
		{
			var capture = CaptureAlbumArt( );
            if (capture != null)
			{
				System.Windows.MessageBox.Show( "잘못된 이미지입니다" );
				return;
			}

			var filename = "t"+DateTime.Now.Ticks;
			capture.Save( filename);
			AlbumArt = new Bitmap( filename );
			if ( AlbumArt == null )
			{
				System.Windows.MessageBox.Show( "잘못된 이미지입니다" );
				return;
			}
			this.Button_Next.RaiseEvent( new RoutedEventArgs( System.Windows.Controls.Button.ClickEvent ) );
		}

		private void Button_Albumart_FromWeb_Click( object sender, RoutedEventArgs e )
		{
			var form = new InputBox("다운로드할 이미지 주소를 입력하세요.");
			form.ShowDialog( );
			if ( form.IsCancled ) return;
			AlbumArt = DownloadAlbumArt( form.Text );
			if ( AlbumArt == null )
			{
				System.Windows.MessageBox.Show( "잘못된 이미지입니다" );
				return;
			}
			this.Button_Next.RaiseEvent( new RoutedEventArgs( System.Windows.Controls.Button.ClickEvent ) );
		}

		private void Button_Albumart_FromThumbnail_Click( object sender, RoutedEventArgs e )
		{
			System.Windows.MessageBox.Show( "아직 지원안하지롱" );
		}
	}
}
