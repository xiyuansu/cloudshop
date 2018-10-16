using System;
using System.IO;
using System.Web;

namespace Hishop.Plugins
{
	public class RefundPlugins : PluginContainer
	{
		private static readonly object LockHelper = new object();

		private static volatile RefundPlugins instance = null;

		protected override string PluginLocalPath
		{
			get
			{
				if (HttpContext.Current != null)
				{
					return HttpContext.Current.Request.MapPath("~/plugins/refund");
				}
				string text = "plugins/refund";
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
				return Utils.ApplicationPath + "/plugins/refund";
			}
		}

		protected override string IndexCacheKey
		{
			get
			{
				return "plugin-refund-index";
			}
		}

		protected override string TypeCacheKey
		{
			get
			{
				return "plugin-refund-type";
			}
		}

		private RefundPlugins()
		{
		}

		public static RefundPlugins Instance()
		{
			if (RefundPlugins.instance == null)
			{
				lock (RefundPlugins.LockHelper)
				{
					if (RefundPlugins.instance == null)
					{
						RefundPlugins.instance = new RefundPlugins();
					}
				}
			}
			RefundPlugins.instance.VerifyIndex();
			return RefundPlugins.instance;
		}

		public override PluginItemCollection GetPlugins()
		{
			return base.GetPlugins("RefundRequest");
		}

		public override PluginItem GetPluginItem(string fullName)
		{
			return base.GetPluginItem("RefundRequest", fullName);
		}
	}
}
