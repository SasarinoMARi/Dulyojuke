using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace dulyojuke.Windows
{
	/// <summary>
	/// Interaction logic for SettingForm.xaml
	/// </summary>
	public partial class SettingForm : UserControl, PageInterface
	{
		List<Listitem> items = new List<Listitem>();
		private bool _isRunning;

		public SettingForm()
		{
			InitializeComponent();
			this.Textbox_DownloadPath.Text = SharedPreference.Instance.DownloadPath;

			Log.ItemsSource = items;
		}
		private void Button_DownloadPathFinder_Click(object sender, RoutedEventArgs e)
		{
			using (var fbd = new System.Windows.Forms.FolderBrowserDialog())
			{
				System.Windows.Forms.DialogResult result = fbd.ShowDialog();

				if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
				{
					this.Textbox_DownloadPath.Text = fbd.SelectedPath;
				}
			}
		}

		Dictionary<string, string> PageInterface.getData()
		{
			_isRunning = false;
			SharedPreference.Instance.DownloadPath = this.Textbox_DownloadPath.Text;
			return null;
		}

		void PageInterface.setContentChangeEvent(SceneSwitchAdapter @event)
		{
			@event.AttachEventHandlers(null, null, null, Button_Next);
		}

		void PageInterface.setData(Dictionary<string, string> data)
		{
			_isRunning = true;
			Task.Factory.StartNew(delegate
			{
				while (_isRunning)
				{
					foreach (var item in ProgressManager.Instance.Progresses)
					{
						var kaburi = false;
						foreach (var ai in items)
						{
							if (item.url == ai.url)
							{
								ai.update(item);
								kaburi = true;
								continue;
							}
						}

						if (!kaburi)
						{
							var n = MediaDownloader.GetTitle(item.url);
							if (n == null) n = "불러오는 중...";
							items.Insert(0, new Listitem(item));
						}
					}

					Log.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate
					{
						Log.Items.Refresh();
					}));
					Thread.Sleep(1000);
				}
			});
		}

		private void Button_DownloadPathOpen_Click(object sender, RoutedEventArgs e)
		{
			Process.Start("explorer.exe", Textbox_DownloadPath.Text);
		}

		public class Listitem
		{
			public Listitem(ProgressInfo item)
			{
				update(item);
			}

			public string Name { get; set; }

			public string Status { get; set; }

			public string url { get; set; }

			internal void update(ProgressInfo item)
			{
				var n = MediaDownloader.GetTitle(item.url);
				if (n == null) n = "불러오는 중...";
				Name = n;
				Status = getLog(item);
				url = item.url;
			}

			private string getLog(ProgressInfo item)
			{
				string reason = string.Empty;
				switch (item.task.Status)
				{
					case System.Threading.Tasks.TaskStatus.Created:
						reason = "생성중";
						break;
					case System.Threading.Tasks.TaskStatus.WaitingForActivation:
						reason = "대기중";
						break;
					case System.Threading.Tasks.TaskStatus.WaitingToRun:
						reason = "대기중";
						break;
					case System.Threading.Tasks.TaskStatus.Running:
						reason = "진행중";
						break;
					case System.Threading.Tasks.TaskStatus.WaitingForChildrenToComplete:
						reason = "진행중";
						break;
					case System.Threading.Tasks.TaskStatus.RanToCompletion:
						reason = "완료됨";
						break;
					case System.Threading.Tasks.TaskStatus.Canceled:
						reason = "취소됨";
						break;
					case System.Threading.Tasks.TaskStatus.Faulted:
						reason = "실패함";
						break;
					default:
						break;
				}
				return reason;
			}

		}
	}
}
