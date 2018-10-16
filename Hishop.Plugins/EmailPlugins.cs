using System;
using System.IO;
using System.Web;

namespace Hishop.Plugins
{
	public class EmailPlugins : PluginContainer
	{
		private static readonly object LockHelper = new object();

		private static volatile EmailPlugins instance = null;

		protected override string PluginLocalPath
		{
			get
			{
				if (HttpContext.Current != null)
				{
					return HttpContext.Current.Request.MapPath("~/plugins/email");
				}
				string text = "plugins/email";
				text = text.Replace("/", "\\");
				if (text.StartsWith("\\"))
				{
					text = text.TrimStart('\\');
				}
				return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, text);
			}
		}

		protected override string PluginVirtualPath
		{
			get
			{
				return Utils.ApplicationPath + "/plugins/email";
			}
		}

		protected override string IndexCacheKey
		{
			get
			{
				return "plugin-email-index";
			}
		}

		protected override string TypeCacheKey
		{
			get
			{
				return "plugin-email-type";
			}
		}

		private EmailPlugins()
		{
		}

		public static EmailPlugins Instance()
		{
			if (EmailPlugins.instance == null)
			{
				lock (EmailPlugins.LockHelper)
				{
					if (EmailPlugins.instance == null)
					{
						EmailPlugins.instance = new EmailPlugins();
					}
				}
			}
			EmailPlugins.instance.VerifyIndex();
			return EmailPlugins.instance;
		}

		public override PluginItemCollection GetPlugins()
		{
			return base.GetPlugins("EmailSender");
		}

		public override PluginItem GetPluginItem(string fullName)
		{
			return base.GetPluginItem("EmailSender", fullName);
		}
	}
}
