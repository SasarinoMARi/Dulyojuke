using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace dulyojuke.Windows
{
	interface PageInterface
	{
		void setContentChangeEvent( RoutedEventHandler toPrevEvent, RoutedEventHandler toNextEvent );
		void setData( Dictionary<string, string> data );
		Dictionary<string, string> getData( );
	}
}
