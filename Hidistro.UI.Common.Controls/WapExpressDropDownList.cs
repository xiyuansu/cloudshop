using Hidistro.Context;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class WapExpressDropDownList : WebControl
	{
		private string _nullToDisplay = "请选择快递公司";

		public string SelectedValue
		{
			get;
			set;
		}

		public string NullToDisplay
		{
			get
			{
				return this._nullToDisplay;
			}
			set
			{
				this._nullToDisplay = value;
			}
		}

		protected override void Render(HtmlTextWriter writer)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			StringBuilder stringBuilder = new StringBuilder();
			if (!string.IsNullOrEmpty(this.SelectedValue))
			{
				stringBuilder.Append("<button type=\"button\" class=\"btn btn-default dropdown-toggle\" data-toggle=\"dropdown\">" + this.SelectedValue + "<span class=\"caret\"></span></button>");
			}
			else
			{
				stringBuilder.Append("<button type=\"button\" class=\"btn btn-default dropdown-toggle\" data-toggle=\"dropdown\">" + this.NullToDisplay + "<span class=\"caret\"></span></button>");
			}
			stringBuilder.AppendLine("<ul id=\"selectExpress\" class=\"dropdown-menu\" role=\"menu\">");
			IList<string> allExpressName = ExpressHelper.GetAllExpressName(true);
			foreach (string item in allExpressName)
			{
				stringBuilder.AppendLine($"<li><a href=\"#\" name=\"{item}\">{item}</a></li>");
			}
			stringBuilder.AppendLine("</ul>");
			writer.Write(stringBuilder.ToString());
		}
	}
}
