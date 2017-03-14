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
