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
	/// <summary>
	/// Interaction logic for TagForm.xaml
	/// </summary>
	public partial class TagForm : UserControl, PageInterface
	{		
		public TagForm( )
		{
			InitializeComponent( );
		}

		void PageInterface.setContentChangeEvent( SceneSwitchAdapter @event )
		{
			@event.AttachEventHandlers( Button_Prev, Button_Next, null, null );
		}

		void PageInterface.setData( Dictionary<string, string> data )
		{
			
		}

		Dictionary<string, string> PageInterface.getData( )
		{
			var data = new Dictionary<string, string>();
			data.Add( "Title", this.Textbox_Tag_Title.Text );
			data.Add( "Artist", this.Textbox_Tag_Artist.Text );
			data.Add( "AlbumName", this.Textbox_Tag_AlbumName.Text );
			return data;
		}

		private void textboxKeydown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				this.Button_Next.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Button.ClickEvent));
			}
			if (e.Key == Key.Escape)
			{
				this.Button_Prev.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Button.ClickEvent));
			}
		}
	}
}
