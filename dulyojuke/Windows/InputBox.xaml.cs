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
	/// Interaction logic for InputBox.xaml
	/// </summary>
	public partial class InputBox : Window
	{
		public InputBox(string title )
		{
			InitializeComponent( );
			this.Title.Content = title;
		}

		private bool isCancled = true;
		private string text;

		private void Cancle_Click( object sender, RoutedEventArgs e )
		{
			this.Close( );
		}

		private void OK_Click( object sender, RoutedEventArgs e )
		{
			isCancled = false;
			text = this.Field.Text;
			this.Close( );
		}

		public bool IsCancled { get { return isCancled; } }
		public string Text { get { return text; } }
	}
}
