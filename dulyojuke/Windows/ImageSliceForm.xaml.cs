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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace dulyojuke.Windows
{
	/// <summary>
	/// Interaction logic for ImageSliceForm.xaml
	/// </summary>
	public partial class ImageSliceForm : UserControl, PageInterface
	{
		public ImageSliceForm( )
		{
			InitializeComponent( );
		}

		Dictionary<string, string> PageInterface.getData( )
		{
			throw new NotImplementedException( );
		}

		void PageInterface.setContentChangeEvent( RoutedEventHandler toPrevEvent, RoutedEventHandler toNextEvent )
		{
			throw new NotImplementedException( );
		}

		void PageInterface.setData( Dictionary<string, string> data )
		{
			throw new NotImplementedException( );
		}
	}
}
