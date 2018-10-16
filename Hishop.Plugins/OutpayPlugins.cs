using System;
using System.IO;
using System.Web;

namespace Hishop.Plugins
{
	public class OutpayPlugins : PluginContainer
	{
		private static readonly object LockHelper = new object();

		private static volatile OutpayPlugins instance = null;

		protected override string PluginLocalPath
		{
			get
			{
				if (HttpContext.Current != null)
				{
					return HttpContext.Current.Request.MapPath("~/plugins/outpay");
				}
				string text = "plugins/outpay";
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
				return Utils.ApplicationPath + "/plugins/outpay";
			}
		}

		protected override string IndexCacheKey
		{
			get
			{
				return "plugin-outpay-index";
			}
		}

		protected override string TypeCacheKey
		{
			get
			{
				return "plugin-outpay-type";
			}
		}

		private OutpayPlugins()
		{
		}

		public static OutpayPlugins Instance()
		{
			if (OutpayPlugins.instance == null)
			{
				lock (OutpayPlugins.LockHelper)
				{
					if (OutpayPlugins.instance == null)
					{
						OutpayPlugins.instance = new OutpayPlugins();
					}
				}
			}
			OutpayPlugins.instance.VerifyIndex();
			return OutpayPlugins.instance;
		}

		public override PluginItemCollection GetPlugins()
		{
			return base.GetPlugins("OutpayRequest");
		}

		public override PluginItem GetPluginItem(string fullName)
		{
			return base.GetPluginItem("OutpayRequest", fullName);
		}
	}
}
