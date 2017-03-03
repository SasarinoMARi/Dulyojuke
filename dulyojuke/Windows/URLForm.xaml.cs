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

		void PageInterface.setContentChangeEvent( RoutedEventHandler toPrevEvent, RoutedEventHandler toNextEvent )
		{
			this.Button_Next.Click += toNextEvent;
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
	}
}
