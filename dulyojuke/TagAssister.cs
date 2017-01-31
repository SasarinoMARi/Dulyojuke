using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dulyojuke
{
	class TagAssister
	{
		/// <summary>
		/// 이거 해야함. 인코딩 관련 문제로 Id3v2로 고정시키는 부분
		/// </summary>
		public static void Initialize( )
		{
			TagLib.Id3v2.Tag.DefaultVersion = 3;
			TagLib.Id3v2.Tag.ForceDefaultVersion = true;
		}

		/// <summary>
		/// ID3v2태그를 적용시킵니다.
		/// </summary>
		/// <param name="audioPath"></param>
		/// <param name="node"></param>
		public static void ApplyTag( string audioPath, TagNode node )
		{
			using ( TagLib.File file = TagLib.File.Create( audioPath ) )
			{
				TagLib.Id3v2.Tag tag = (TagLib.Id3v2.Tag)file.GetTag(TagLib.TagTypes.Id3v2, true);
				tag.Title = node.Title;
				tag.Artists = node.Artists;
				tag.Album = node.Album;

				if ( node.AlbumArt != null )
				{
					var pictures = new List<TagLib.Picture>();
					foreach ( var picture in node.AlbumArt )
					{
						TagLib.Picture pic = new TagLib.Picture();
						pic.Type = TagLib.PictureType.FrontCover;
						pic.Description = "Cover";
						pic.MimeType = System.Net.Mime.MediaTypeNames.Image.Jpeg;
						MemoryStream ms = new MemoryStream();
						Bitmap bitmap = null;
						if ( picture.StartsWith( "http://" ) || picture.StartsWith( "https://" ) )
						{
							System.Net.WebRequest request = System.Net.WebRequest.Create(picture);
							System.Net.WebResponse response = request.GetResponse();
							Stream responseStream = response.GetResponseStream();
							bitmap = new Bitmap( responseStream );
						}
						else
						{
							if ( File.Exists( picture ) )
							{
								bitmap = new Bitmap( picture );
							}
						}
						if ( bitmap == null ) continue;
						bitmap.Save( ms, System.Drawing.Imaging.ImageFormat.Jpeg );
						ms.Position = 0;
						pic.Data = TagLib.ByteVector.FromStream( ms );

						pictures.Add( pic );
					}
					tag.Pictures = pictures.ToArray( );
				}

				file.Save( );
			}
		}
	}
}
