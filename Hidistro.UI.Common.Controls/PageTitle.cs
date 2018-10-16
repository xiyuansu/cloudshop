using Hidistro.Context;
using System;
using System.Globalization;
using System.Web;
using System.Web.UI;

namespace Hidistro.UI.Common.Controls
{
	[PersistChildren(true)]
	[ParseChildren(false)]
	public class PageTitle : Control
	{
		private const string titleKey = "Hishop.Title.Value";

		public static void AddTitle(string title, HttpContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			context.Items["Hishop.Title.Value"] = title;
		}

		public static void AddTitle(string title)
		{
			if (HttpContext.Current == null)
			{
				throw new ArgumentNullException("context");
			}
			HttpContext.Current.Items["Hishop.Title.Value"] = title;
		}

		public static void AddSiteNameTitle(string title)
		{
			PageTitle.AddTitle(string.Format(CultureInfo.InvariantCulture, "{0} - {1}", new object[2]
			{
				title,
				HiContext.Current.SiteSettings.SiteName
			}), HiContext.Current.Context);
		}

		protected override void Render(HtmlTextWriter writer)
		{
			string text = this.Context.Items["Hishop.Title.Value"] as string;
			if (string.IsNullOrEmpty(text))
			{
				text = HiContext.Current.SiteSettings.SiteName;
			}
			writer.WriteLine("<title>{0}</title>", text);
		}
	}
}
