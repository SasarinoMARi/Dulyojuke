using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace dulyojuke.Windows
{
	/// <summary>
	/// Interaction logic for AlbumartForm.xaml
	/// </summary>
	public partial class AlbumartForm : System.Windows.Controls.UserControl, PageInterface
	{
		string VideoUrl;
		private System.Drawing.Image AlbumArt;

		public AlbumartForm()
		{
			InitializeComponent();
		}

		private System.Drawing.Image OpenAlbumArt()
		{
			OpenFileDialog openFile = new OpenFileDialog();
			openFile.DefaultExt = "jpg";
			openFile.Filter = "Images Files(*.jpg; *.jpeg; *.gif; *.bmp; *.png)|*.jpg;*.jpeg;*.gif;*.bmp;*.png";
			openFile.ShowDialog();
			if (openFile.FileNames.Length > 0)
			{
				// 우선은 맨 앞의 것만 선택되도록
				foreach (string filename in openFile.FileNames)
				{
					return new Bitmap(filename);
				}
			}
			return null;
		}

		private System.Drawing.Image CaptureAlbumArt()
		{
			System.Drawing.Image capture = null;
			var stasisForm = new Stasisfield();
			stasisForm.ShowDialog();
			capture = stasisForm.CropedImage;
			return capture;
		}

		private System.Drawing.Image DownloadAlbumArt(string imageurl)
		{
			if (imageurl.StartsWith("http://") || imageurl.StartsWith("https://"))
			{
				try
				{
					System.Net.WebRequest request = System.Net.WebRequest.Create(imageurl);
					System.Net.WebResponse response = request.GetResponse();
					Stream responseStream = response.GetResponseStream();
					return new Bitmap(responseStream);
				}
				catch { }
			}
			return null;
		}


		void PageInterface.setContentChangeEvent(SceneSwitchAdapter @event)
		{
			@event.AttachEventHandlers(Button_Prev, Button_Next, null, null);
		}

		void PageInterface.setData(Dictionary<string, string> data)
		{
			this.VideoUrl = data["VideoUrl"];
		}

		Dictionary<string, string> PageInterface.getData()
		{
			var data = new Dictionary<string, string>();
			if (AlbumArt != null)
				data.Add("AlbumArt", Utility.SerializeImageToString(AlbumArt));
			return data;
		}

		private void Button_Albumart_FromImage_Click(object sender, RoutedEventArgs e)
		{
			AlbumArt = OpenAlbumArt();
			if (AlbumArt == null)
			{
				System.Windows.MessageBox.Show("잘못된 이미지입니다");
			}
			setAlbumArt2Control(AlbumArt);
			//this.Button_Next.RaiseEvent( new RoutedEventArgs( System.Windows.Controls.Button.ClickEvent ) );
		}

		private void Button_Albumart_FromCapture_Click(object sender, RoutedEventArgs e)
		{
			var capture = CaptureAlbumArt();
			if (capture == null)
			{
				System.Windows.MessageBox.Show("잘못된 이미지입니다");
			}

			var filename = Path.Combine(Utility.GetImageTempFolder(), DateTime.Now.Ticks.ToString());
			capture.Save(filename);
			AlbumArt = new Bitmap(filename);
			if (AlbumArt == null)
			{
				System.Windows.MessageBox.Show("잘못된 이미지입니다");
			}
			setAlbumArt2Control(AlbumArt);
			//this.Button_Next.RaiseEvent( new RoutedEventArgs( System.Windows.Controls.Button.ClickEvent ) );
		}

		private void Button_Albumart_FromWeb_Click(object sender, RoutedEventArgs e)
		{
			var form = new InputBox("다운로드할 이미지 주소를 입력하세요.");
			form.ShowDialog();
			if (form.IsCancled) return;
			Task.Factory.StartNew(delegate
			{
				Progress.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate
				{
					this.Label_NoImage.Visibility = Visibility.Collapsed;
					Progress.Visibility = Visibility.Visible;
				}));
				AlbumArt = DownloadAlbumArt(form.Text);
				Progress.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate
				{
					Progress.Visibility = Visibility.Collapsed;
					if (AlbumArt == null)
					{
						this.Label_NoImage.Visibility = Visibility.Visible;
						System.Windows.MessageBox.Show("잘못된 이미지입니다");
					}
					setAlbumArt2Control(AlbumArt);
				}));
			});
			//this.Button_Next.RaiseEvent( new RoutedEventArgs( System.Windows.Controls.Button.ClickEvent ) );
		}

		private void Button_Albumart_FromThumbnail_Click(object sender, RoutedEventArgs e)
		{
			Task.Factory.StartNew(delegate
			{
				Progress.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate
				{
					this.Label_NoImage.Visibility = Visibility.Collapsed;
					Progress.Visibility = Visibility.Visible;
				}));
				AlbumArt = MediaDownloader.GetThumbnail(VideoUrl);
				Progress.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate
				{
					Progress.Visibility = Visibility.Collapsed;
					if (AlbumArt == null)
					{
						this.Label_NoImage.Visibility = Visibility.Visible;
						System.Windows.MessageBox.Show("잘못된 이미지입니다");
					}
					setAlbumArt2Control(AlbumArt);
				}));
			});

			//this.Button_Next.RaiseEvent( new RoutedEventArgs( System.Windows.Controls.Button.ClickEvent ) );
		}

		private void setAlbumArt2Control(System.Drawing.Image art)
		{
			if (art == null)
			{
				this.Image_Thumbnail.Source = null;
				this.Label_NoImage.Visibility = Visibility.Visible;
				return;
			}

			var b = new Bitmap(art);
			var bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(b.GetHbitmap(),
										  IntPtr.Zero,
										  Int32Rect.Empty,
										  BitmapSizeOptions.FromEmptyOptions());
			this.Image_Thumbnail.Source = bitmapSource;
			this.Label_NoImage.Visibility = Visibility.Collapsed;
		}

		private void Button_Remove_Click(object sender, RoutedEventArgs e)
		{
			this.Image_Thumbnail.Source = null;
			this.Label_NoImage.Visibility = Visibility.Visible;
		}
	}
}
