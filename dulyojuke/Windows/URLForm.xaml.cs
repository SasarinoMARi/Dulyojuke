using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace dulyojuke.Windows
{
	public partial class URLForm : UserControl, PageInterface
	{
		public URLForm( )
		{
			InitializeComponent( );
		}

		void PageInterface.setData( Dictionary<string, string> data )
		{
			
		}

		Dictionary<string, string> PageInterface.getData( )
		{
			var data = new Dictionary<string, string>();
			data.Add( "VideoUrl", this.Textbox_URL.Text );
			return data;
		}

		private void Button_Settings_Click( object sender, RoutedEventArgs e )
		{

		}

		void PageInterface.setContentChangeEvent( SceneSwitchAdapter @event )
		{
			@event.AttachEventHandlers( null, Button_Next, Button_Settings, null );
		}
	}
}
