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
	public partial class MainWindow : Window
	{

		/*
		 * 응용 프로그램 내의 장면 전환은 모두 이곳에서 처리합니다. 장면이란 PageInterface 인터페이스를 상속받은 클래스를 의미합니다.
		 * setContentChangeEvent() 함수를 사용해 각각의 장면에 이 클래스의 뒤로가기, 다음으로 버튼 이벤트를 전달합니다.
		 * 화면 전환이 이루어 질 때 setData() 및 getData() 함수를 통해 각 장면의 초기화에 필요한 데이터와 반환받을 데이터를 관리합니다.
		 */

		public MainWindow( )
		{
			InitializeComponent( );
			TagAssister.Initialize( );
			MakeSomeNoooooooooooise( );
			this.Content = Contents[ContentIndex].Content;

		}

		UserControl[] Contents;
		Dictionary<string, string> ContentData = new Dictionary<string, string>();
		int ContentIndex;

		/*
		 *  장면을 전부 초기화합니다.
		 */
		public void MakeSomeNoooooooooooise( )
		{
			ContentIndex = 0;
			
			Contents = new UserControl[]{

				new URLForm( ),
				new TagForm( ),
				new AlbumartForm( ),
				//new ImageSliceForm( ),
				new DoneForm( ),
				new SettingForm( ),

	 		 };

			ContentData.Clear( );

			var eventAdapter = new SceneSwitchAdapter(Previousbutton_Click, Nextbutton_Click, Settingsbutton_Click, Homebutton_Click);
			foreach ( PageInterface content in Contents )
			{
				content.setContentChangeEvent( eventAdapter );
			}
		}

		#region Button events

		/*
		 * 뒤로가기 버튼 클릭 이벤트
		 */
		private void Previousbutton_Click( object sender, RoutedEventArgs e )
		{
			if ( ContentIndex > 0 )
			{
				AttachContentData( );
				this.Content = Contents[--ContentIndex].Content;
				ProvideContentData( );

			}
		}

		/*
		 * 다음으로 버튼 클릭 이벤트
		 */
		private void Nextbutton_Click( object sender, RoutedEventArgs e )
		{
			if ( ContentIndex < Contents.Length - 1 )
			{
				AttachContentData( );
				this.Content = Contents[++ContentIndex].Content;
				ProvideContentData( );
			}
		}

		/*
		 * 설정 버튼 클릭 이벤트
		 */
		private void Settingsbutton_Click( object sender, RoutedEventArgs e )
		{
			AttachContentData( );
			ContentIndex = 4;
			this.Content = Contents[ContentIndex].Content;
			ProvideContentData( );
		}

		/*
		 * 메인가기 버튼 클릭 이벤트
		 */
		private void Homebutton_Click( object sender, RoutedEventArgs e )
		{
			AttachContentData( );
			this.Content = null;
			MakeSomeNoooooooooooise( );
			this.Content = Contents[ContentIndex].Content;
			ProvideContentData( );
		}

		#endregion

		#region Data provider

		/*
		 *	현재 장면의 data를 MainWindow의 data에 병합
		 *	만약 key가 겹치는 경우 새로운 value를 write한다.
		 */
		private void AttachContentData( )
		{
			var result = ( ( PageInterface ) Contents[ContentIndex] ).getData( );
			if ( result == null ) return;
			result.ToList( ).ForEach( ( x ) => { if ( ContentData.ContainsKey( x.Key ) ) ContentData[x.Key] = x.Value; else ContentData.Add( x.Key, x.Value ); } );
		}

		/* 
		 *	각 장면별로 필요한 data를 공급
		 */
		private void ProvideContentData( )
		{
			var data = new Dictionary<string, string>();

			if ( Contents[ContentIndex] is URLForm )
			{

			}
			if ( Contents[ContentIndex] is TagForm )
			{

			}
			if ( Contents[ContentIndex] is AlbumartForm )
			{
				if ( ContentData.ContainsKey( "VideoUrl" ) )
					data.Add( "VideoUrl", ContentData["VideoUrl"] );
			}
			if ( Contents[ContentIndex] is ImageSliceForm )
			{
				if ( ContentData.ContainsKey( "AlbumArt" ) )
					data.Add( "AlbumArt", ContentData["AlbumArt"] );
			}
			if ( Contents[ContentIndex] is DoneForm )
			{
				if ( ContentData.ContainsKey( "VideoUrl" ) )
					data.Add( "VideoUrl", ContentData["VideoUrl"] );
				if ( ContentData.ContainsKey( "Title" ) )
					data.Add( "Title", ContentData["Title"] );
				if ( ContentData.ContainsKey( "Artist" ) )
					data.Add( "Artist", ContentData["Artist"] );
				if ( ContentData.ContainsKey( "AlbumName" ) )
					data.Add( "AlbumName", ContentData["AlbumName"] );
				if ( ContentData.ContainsKey( "AlbumArt" ) )
					data.Add( "AlbumArt", ContentData["AlbumArt"] );
			}
			if ( Contents[ContentIndex] is SettingForm )
			{

			}

			( ( PageInterface ) Contents[ContentIndex] ).setData( data );
		}

		#endregion

		private void Window_Closing( object sender, System.ComponentModel.CancelEventArgs e )
		{
			Utility.cleanupTempFolder();
			SharedPreference.Instance.Save( );
		}
	}
}
