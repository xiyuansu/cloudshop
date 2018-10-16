using Hidistro.Core;
using Hidistro.UI.Common.Controls;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_HeaderMune : WebControl
	{
		protected override void Render(HtmlTextWriter writer)
		{
			StringBuilder stringBuilder = new StringBuilder();
			DataTable headerMune = TagsHelper.GetHeaderMune();
			if (headerMune != null && headerMune.Rows.Count > 0)
			{
				DataView defaultView = headerMune.DefaultView;
				defaultView.Sort = "DisplaySequence Desc";
				headerMune = defaultView.ToTable();
				foreach (DataRow row in headerMune.Rows)
				{
					string arg = row["Category"].Equals("3") ? "target=\"_blank\"" : "";
					stringBuilder.AppendFormat("<li> <a {0} href=\"{1}\"><span>{2}</span></a></li>", arg, this.GetUrl((string)row["Category"], (string)row["Url"], (string)row["Where"]), row["Title"]);
				}
				writer.Write(stringBuilder.ToString());
			}
		}

		private string GetUrl(string category, string url, string where)
		{
			string text = url;
			if (category == "1")
			{
				text = url;
			}
			else if (category == "2")
			{
				string[] array = where.Split(',');
				text = $"/SubCategory.aspx?keywords={Globals.UrlEncode(array[5])}&minSalePrice={array[3]}&maxSalePrice={array[4]}";
				if (array[0] != "0")
				{
					text = text + "&categoryId=" + array[0];
				}
				if (array[1] != "0")
				{
					text = text + "&brand=" + array[1];
				}
				if (array[2] != "0")
				{
					text = text + "&TagIds=" + array[2];
				}
			}
			return text;
		}
	}
}
