using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Threading;
using DAP.Adorners;

namespace dulyojuke.Windows
{
	/// <summary>
	/// Interaction logic for ImageSliceForm.xaml
	/// </summary>
	public partial class ImageSliceForm : UserControl, PageInterface
	{
		CroppingAdorner _clp;
		FrameworkElement _felCur = null;
		Brush _brOriginal;

		public ImageSliceForm( )
		{
			InitializeComponent( );
		}

		#region Adorner

		private void RemoveCropFromCur( )
		{
			AdornerLayer aly = AdornerLayer.GetAdornerLayer(_felCur);
			aly.Remove( _clp );
		}
		private void AddCropToElement( FrameworkElement fel )
		{
			if ( _felCur != null )
			{
				RemoveCropFromCur( );
			}
			Rect rcInterior = new Rect(
				fel.ActualWidth * 0,
				fel.ActualHeight * 0,
				fel.ActualWidth * 1,
				fel.ActualHeight * 1);
			AdornerLayer aly = AdornerLayer.GetAdornerLayer(fel);
			_clp = new CroppingAdorner( fel, rcInterior );
			aly.Add( _clp );
			ImageCropped.Source = _clp.BpsCrop( );
			_clp.CropChanged += CropChanged;
			_felCur = fel;
			if ( _clp != null )
			{
				Color clr = Colors.Black;
				clr.A = 110;
				_clp.Fill = new SolidColorBrush( clr );
			}
		}

		private void RefreshCropImage( )
		{
			if ( _clp != null )
			{
				Rect rc = _clp.ClippingRectangle;

				/*
				tblkClippingRectangle.Text = string.Format(
					"Clipping Rectangle: ({0:N1}, {1:N1}, {2:N1}, {3:N1})",
					rc.Left,
					rc.Top,
					rc.Right,
					rc.Bottom );
					*/

				ImageCropped.Source = _clp.BpsCrop( );
			}
		}

		private void CropChanged( Object sender, RoutedEventArgs rea )
		{
			RefreshCropImage( );
		}

		#endregion


		void PageInterface.setContentChangeEvent( SceneSwitchAdapter @event )
		{
			@event.AttachEventHandlers( Button_Prev, Button_Next, null, null );
		}

		Dictionary<string, string> PageInterface.getData( )
		{
			var data = new Dictionary<string, string>();
			data.Add( "AlbumArt", Utility.SerializeImageToString( Utility.ImageSourceToBitmap( this.ImageCropped.Source ) ) );
			return data;
		}

		void PageInterface.setData( Dictionary<string, string> data )
		{
			if ( data.ContainsKey( "AlbumArt" ) )
			{
				var albumArtString = data["AlbumArt"];
				var AlbumArt = Utility.DeserializeImageToString( albumArtString );
				ImageBox.Source = Utility.BitmapToImageSource( AlbumArt );
				System.Threading.Tasks.Task.Factory.StartNew( delegate
				 {
					 Thread.Sleep( 100 ); // ㅂㅅ애자녈같은 동기화 기다리기
					 Dispatcher.Invoke( DispatcherPriority.Normal, new Action( delegate
					 {
						 AddCropToElement( ImageBox );
						 _brOriginal = _clp.Fill;
						 RefreshCropImage( );
					 } ) );
				 } );
			}
			else
			{
				this.Button_Next.RaiseEvent( new RoutedEventArgs( System.Windows.Controls.Button.ClickEvent ) );
			}
		}
	}
}
