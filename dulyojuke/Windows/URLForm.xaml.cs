using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace dulyojuke.Windows
{
	public partial class URLForm : UserControl, PageInterface
	{
		private Task bgw;
		private string _url = string.Empty;
		private bool bgw_running = true;
		public URLForm()
		{
			InitializeComponent();
		}

		void PageInterface.setData(Dictionary<string, string> data)
		{
			_url = Textbox_URL.Text = data["VideoUrl"];
			bgw_running = true;
			bgw = Task.Factory.StartNew(delegate
			{
				while (bgw_running)
				{
					if (sw != null && sw.ElapsedMilliseconds > 750)
					{
						sw.Stop();
						sw = null;

						Progress.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate
						{
							Progress.Visibility = Visibility.Visible;
						}));
						var n = MediaDownloader.GetTitle(_url);
						Progress.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate
						{
							if (n != null)
							{
								Label_UrlTitle.Visibility = Visibility.Visible;
								Label_UrlTitle.Content = n;
							}
						}));
						var t = MediaDownloader.GetThumbnail(_url);
						Progress.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate
						{
							Progress.Visibility = Visibility.Hidden;
							if (t != null)
							{
								var bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(t.GetHbitmap(),
									  IntPtr.Zero,
									  Int32Rect.Empty,
									  BitmapSizeOptions.FromEmptyOptions());
								gPanel.Background = new ImageBrush(bitmapSource);
							}
						}));
					}
					Thread.Sleep(10);
				}
			});
		}

		Dictionary<string, string> PageInterface.getData()
		{
			bgw_running = false;
			var data = new Dictionary<string, string>();
			data.Add("VideoUrl", Utility.TrimUrl(this.Textbox_URL.Text));
			return data;
		}

		private void Button_Settings_Click(object sender, RoutedEventArgs e)
		{

		}

		void PageInterface.setContentChangeEvent(SceneSwitchAdapter @event)
		{
			@event.AttachEventHandlers(null, Button_Next, Button_Settings, null);
		}

		private void Button_List_Click(object sender, RoutedEventArgs e)
		{

		}


		private System.Diagnostics.Stopwatch sw = null;
		private void Textbox_URL_TextChanged(object sender, TextChangedEventArgs e)
		{
			Uri uriResult;
			bool result = Uri.TryCreate(Textbox_URL.Text, UriKind.Absolute, out uriResult)
				&& (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
			if (result)
			{
				_url = Textbox_URL.Text;
				sw = new System.Diagnostics.Stopwatch();
				sw.Start();
			}
			else
			{
				gPanel.Background = null;
				Label_UrlTitle.Content = null;
				Label_UrlTitle.Visibility = Visibility.Hidden;
			}
		}

		private void Textbox_URL_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter) {
				this.Button_Next.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Button.ClickEvent));
			}
		}
	}
}
