using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
		public SettingForm( )
		{
			InitializeComponent( );
			this.Textbox_DownloadPath.Text = SharedPreference.Instance.DownloadPath;

			foreach (var item in ProgressManager.Instance.Progresses)
			{
				Log.Text += getLog(item);
			}
		}

		private string getLog(ProgressInfo item)
		{
			string reason = string.Empty;
			switch (item.task.Status)
			{
				case System.Threading.Tasks.TaskStatus.Created:
					reason = "생성";
					break;
				case System.Threading.Tasks.TaskStatus.WaitingForActivation:
					reason = "대기";
					break;
				case System.Threading.Tasks.TaskStatus.WaitingToRun:
					reason = "대기";
					break;
				case System.Threading.Tasks.TaskStatus.Running:
					reason = "진행";
					break;
				case System.Threading.Tasks.TaskStatus.WaitingForChildrenToComplete:
					reason = "진행";
					break;
				case System.Threading.Tasks.TaskStatus.RanToCompletion:
					reason = "완료";
					break;
				case System.Threading.Tasks.TaskStatus.Canceled:
					reason = "취소";
					break;
				case System.Threading.Tasks.TaskStatus.Faulted:
					reason = "실패";
					break;
				default:
					break;
			}
			return item.tag.Title + " / 상태 : " + reason + "\n";
		}

		private void Button_DownloadPathFinder_Click( object sender, RoutedEventArgs e )
		{
			using ( var fbd = new System.Windows.Forms.FolderBrowserDialog( ) )
			{
				System.Windows.Forms.DialogResult result = fbd.ShowDialog();

				if ( result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace( fbd.SelectedPath ) )
				{
					this.Textbox_DownloadPath.Text = fbd.SelectedPath;
				}
			}
		}

		Dictionary<string, string> PageInterface.getData( )
		{
			SharedPreference.Instance.DownloadPath = this.Textbox_DownloadPath.Text;
			return null;
		}

		void PageInterface.setContentChangeEvent( SceneSwitchAdapter @event )
		{
			@event.AttachEventHandlers( null, null, null, Button_Next);
		}

		void PageInterface.setData( Dictionary<string, string> data )
		{

		}
	}
}
