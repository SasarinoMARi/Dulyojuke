﻿using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace dulyojuke
{
	class Utility
	{
		/// <summary>
		/// 유효한 파일 이름을 반환합니다.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public static string MakeValidFileName( string name )
		{
			string invalidChars = System.Text.RegularExpressions.Regex.Escape( new string( System.IO.Path.GetInvalidFileNameChars() ) );
			string invalidRegStr = string.Format( @"([{0}]*\.+$)|([{0}]+)", invalidChars );

			return System.Text.RegularExpressions.Regex.Replace( name, invalidRegStr, "_" );
		}


		/// <summary>
		/// 기본 다운로드 폴더 경로를 얻어옵니다.
		/// </summary>
		/// <returns></returns>
		public static string GetDefaultDownloadFolder( )
		{
			string downloads;
			SHGetKnownFolderPath( KnownFolder.Downloads, 0, IntPtr.Zero, out downloads );
			return downloads;
		}

		public static string SerializeImageToString( Image image )
		{
			MemoryStream ms = new MemoryStream();
			image.Save( ms, image.RawFormat );
			byte[] array = ms.ToArray();
			return Convert.ToBase64String( array );
		}

		public static Image DeserializeImageToString( string imageString )
		{
			byte[] array = Convert.FromBase64String(imageString);
			Image image = Image.FromStream(new MemoryStream(array));
			return image;
		}

		#region native funcs
		private static class KnownFolder
		{
			public static readonly Guid Downloads = new Guid("374DE290-123F-4565-9164-39C4925E467B");
		}
		[DllImport( "shell32.dll", CharSet = CharSet.Unicode )]
		static extern int SHGetKnownFolderPath( [MarshalAs( UnmanagedType.LPStruct )] Guid rfid, uint dwFlags, IntPtr hToken, out string pszPath );
		#endregion

		public static string GetImageTempFolder( )
		{
			var p = Path.Combine( Directory.GetCurrentDirectory( ), "temp" );
			if ( !Directory.Exists( p ) ) Directory.CreateDirectory( p );
			return p;
		}

		public static BitmapImage BitmapToImageSource( Image bitmap )
		{
			using ( MemoryStream memory = new MemoryStream( ) )
			{
				bitmap.Save( memory, System.Drawing.Imaging.ImageFormat.Bmp );
				memory.Position = 0;
				BitmapImage bitmapimage = new BitmapImage();
				bitmapimage.BeginInit( );
				bitmapimage.StreamSource = memory;
				bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
				bitmapimage.EndInit( );

				return bitmapimage;
			}
		}

		public static Image ImageSourceToBitmap( ImageSource image )
		{
			MemoryStream ms = new MemoryStream();
			var encoder = new BmpBitmapEncoder();
			encoder.Frames.Add( BitmapFrame.Create( image as BitmapSource ) );
			encoder.Save( ms );
			ms.Flush( );
			return System.Drawing.Image.FromStream( ms );
		}
	}
}
