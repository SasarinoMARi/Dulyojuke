using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Script.Serialization;

namespace dulyojuke
{
	class SharedPreference
	{
		private static SharedPreference instance;
		public static SharedPreference Instance { get { if (instance == null) instance = new SharedPreference(); return instance; } }

		private SharedPreference()
		{
			Load();
		}

		public static string getSettingFilePath()
		{
			return Path.Combine(Utility.getDataDirPath(), "settings");
		}

		public void Load()
		{
			try
			{
				Dictionary<string, object> list = null;
				var json = File.ReadAllText(getSettingFilePath());
				list = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(json);
				DownloadPath = list["fileDownloadPath"].ToString();
				Dictionary<string, object> nico = (Dictionary<string, object>)list["nicodouga"];
				NicoVideoUsername = nico["username"].ToString();
				NicoVideoUsername = nico["password"].ToString();
			}
			catch (IOException e)
			{
				Console.WriteLine(e);

				DownloadPath = Utility.GetDefaultDownloadFolder();
			}
		}

		public void Save()
		{
			NicoVideoPassword = "TestTextTest10@!_4";

			var json = new Dictionary<string, object>();
			json.Add("fileDownloadPath", DownloadPath);
			var nico = new Dictionary<string, string>();
			nico.Add("username", NicoVideoUsername);
			nico.Add("password", NicoVideoPassword);
			json.Add("nicodouga", nico);

			JavaScriptSerializer js = new JavaScriptSerializer();
			string serializedString = js.Serialize(json);
			File.WriteAllText(getSettingFilePath(), serializedString);
		}

		public string NicoVideoUsername { get; set; } = string.Empty;
		public string NicoVideoPassword { get; set; } = string.Empty;
		public string DownloadPath { get; set; } = string.Empty;
	}
}
