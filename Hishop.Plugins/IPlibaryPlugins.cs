using System;
using System.IO;
using System.Web;

namespace Hishop.Plugins
{
	public class IPlibaryPlugins : PluginContainer
	{
		private static readonly object LockHelper = new object();

		private static volatile IPlibaryPlugins instance = null;

		protected override string PluginLocalPath
		{
			get
			{
				if (HttpContext.Current != null)
				{
					return HttpContext.Current.Request.MapPath("~/plugins/iplibary");
				}
				string text = "plugins/iplibary";
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
				return Utils.ApplicationPath + "/plugins/iplibary";
			}
		}

		protected override string IndexCacheKey
		{
			get
			{
				return "plugin-iplibary-index";
			}
		}

		protected override string TypeCacheKey
		{
			get
			{
				return "plugin-iplibary-type";
			}
		}

		private IPlibaryPlugins()
		{
		}

		public static IPlibaryPlugins Instance()
		{
			if (IPlibaryPlugins.instance == null)
			{
				lock (IPlibaryPlugins.LockHelper)
				{
					if (IPlibaryPlugins.instance == null)
					{
						IPlibaryPlugins.instance = new IPlibaryPlugins();
					}
				}
			}
			IPlibaryPlugins.instance.VerifyIndex();
			return IPlibaryPlugins.instance;
		}

		public override PluginItemCollection GetPlugins()
		{
			throw new NotImplementedException();
		}

		public override PluginItem GetPluginItem(string fullName)
		{
			return null;
		}
	}
}
