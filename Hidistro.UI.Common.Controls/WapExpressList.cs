using Hidistro.Context;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class WapExpressList : WebControl
	{
		public string SelectedValue
		{
			get;
			set;
		}

		protected override void Render(HtmlTextWriter writer)
		{
			string format = "<li><label class=\"label-checkbox item-content\"><input type=\"radio\" {2} name=\"chk_express\" value=\"{0}\"><div class=\"item-media\"><i class=\"icon icon-form-checkbox\"></i></div><div class=\"pay_name\">{1}</div></label></li>";
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("<ul class=\"pay_list\">");
			IList<string> allExpressName = ExpressHelper.GetAllExpressName(true);
			foreach (string item in allExpressName)
			{
				if (item == this.SelectedValue)
				{
					stringBuilder.AppendLine(string.Format(format, item, item, "checked=\"checked\""));
				}
				else
				{
					stringBuilder.AppendLine(string.Format(format, item, item, ""));
				}
			}
			stringBuilder.AppendLine("</ul>");
			writer.Write(stringBuilder.ToString());
		}
	}
}
