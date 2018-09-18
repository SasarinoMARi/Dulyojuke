using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dulyojuke
{
	public class ProgressManager
	{
		private ProgressManager()
		{

		}

		private static ProgressManager instance;
		public static ProgressManager Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new ProgressManager();
				}
				return instance;
			}
		}

		private List<ProgressInfo> progresses;
		public List<ProgressInfo> Progresses
		{
			get
			{
				if (progresses == null)
				{
					progresses = new List<ProgressInfo>();
				}
				return progresses;
			}
		}

		public void AddProgress(Task task, TagNode tag, string url)
		{
			Progresses.Add(new ProgressInfo(task, tag, url));
		}

		internal void AddProgress(ProgressInfo p)
		{
			Progresses.Add(p);
		}
	}

	public class ProgressInfo
	{
		public Task task;
		public TagNode tag;
		internal string url;

		public ProgressInfo(Task task, TagNode tag, string url)
		{
			this.task = task;
			this.tag = tag;
			this.url = url;
		}
	}
}
