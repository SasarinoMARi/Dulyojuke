using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using IniParser;
using IniParser.Model;

namespace dulyojuke
{
	class SharedPreference
	{
		private static SharedPreference instance;
		public static SharedPreference Instance { get { if ( instance == null ) instance = new SharedPreference( ); return instance; } }

		private SharedPreference( )
		{
			Load( );
		}
        
        public static string getSettingFilePath()
        {
            return Path.Combine(Utility.getDataDirPath(), "settings.ini");
        }

        public void Load()
		{
			var parser = new FileIniDataParser();
			if ( File.Exists(getSettingFilePath()) )
			{
				IniData data = parser.ReadFile(getSettingFilePath());
				if ( data.Sections.ContainsSection( "pragma" ) )
				{
					if ( data["pragma"].ContainsKey( "DownloadPath" ) ) DownloadPath = data["pragma"]["DownloadPath"];
				}
				else
				{
					DownloadPath = Utility.GetDefaultDownloadFolder( );
				}
				if ( data.Sections.ContainsSection( "nicovideo" ) )
				{
					if ( data["nicovideo"].ContainsKey( "NicoVideoUsername" ) ) NicoVideoUsername = data["nicovideo"]["NicoVideoUsername"];
					if ( data["nicovideo"].ContainsKey( "NicoVideoPassword" ) ) NicoVideoPassword = data["nicovideo"]["NicoVideoPassword"];
				}
			}
			else {
				DownloadPath = Utility.GetDefaultDownloadFolder( );
			}
		}

		public void Save()
		{
			var parser = new FileIniDataParser();
			IniData data = new IniData();
			data.Sections.AddSection( "pragma" );
			data["pragma"].AddKey("DownloadPath", DownloadPath);
			data.Sections.AddSection( "nicovideo" );
			data["nicovideo"].AddKey( "NicoVideoUsername", NicoVideoUsername );
			data["nicovideo"].AddKey( "NicoVideoPassword", NicoVideoPassword );
			parser.WriteFile(getSettingFilePath(), data );
		}

		public string NicoVideoUsername { get; set; } = string.Empty;
		public string NicoVideoPassword { get; set; } = string.Empty;
		public string DownloadPath { get; set; } = string.Empty;
	}
}
