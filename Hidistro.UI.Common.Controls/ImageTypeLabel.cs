using Hidistro.SaleSystem.Store;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class ImageTypeLabel : Label
	{
		protected override void Render(HtmlTextWriter writer)
		{
			string text = "<ul>";
			string empty = string.Empty;
			DataTable photoCategories = GalleryHelper.GetPhotoCategories(0);
			int defaultPhotoCount = GalleryHelper.GetDefaultPhotoCount();
			string text2 = this.Page.Request.QueryString["ImageTypeId"];
			string empty2 = string.Empty;
			if (!string.IsNullOrEmpty(text2))
			{
				text2 = this.Page.Request.QueryString["ImageTypeId"];
			}
			text = ((!(text2 == "0")) ? (text + "<li><a href=\"/admin/store/ImageData?pageSize=20&ImageTypeId=0\"><s></s><strong>默认分类<span>(" + defaultPhotoCount + ")</span></strong></a></li>") : (text + "<li><a href=\"/admin/store/ImageData?pageSize=20&ImageTypeId=0\" class='classLink'><s></s><strong>默认分类<span>(" + defaultPhotoCount + ")</span></strong></a></li>"));
			foreach (DataRow row in photoCategories.Rows)
			{
				empty2 = ((!(row["CategoryId"].ToString() == text2)) ? "" : "class='classLink'");
				empty = string.Format("<li><a href=\"/admin/store/ImageData?pageSize=20&ImageTypeId={0}\" " + empty2 + "><s></s>{1}<span>({2})</span></a></li>", row["CategoryId"], row["CategoryName"], row["PhotoCounts"].ToString());
				text += empty;
			}
			text = (base.Text = text + "</ul>");
			base.Render(writer);
		}
	}
}
