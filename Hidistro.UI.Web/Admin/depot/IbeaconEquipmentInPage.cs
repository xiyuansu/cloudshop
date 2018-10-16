using Hidistro.Core;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using Senparc.Weixin;
using Senparc.Weixin.MP.AdvancedAPIs.ShakeAround;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.depot
{
	[WeiXinCheck(true)]
	public class IbeaconEquipmentInPage : AdminPage
	{
		private long page_id = 0L;

		protected Image imgThumbnail;

		protected Literal laTitle;

		protected Literal laSubtitle;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.page_id = base.Request.QueryString["page_id"].ToLong(0);
			if (!base.IsPostBack)
			{
				this.BindData();
			}
		}

		private void BindData()
		{
			SearchPagesResultJson searchPagesResultJson = WXStoreHelper.SearchPagesByPageId(new long[1]
			{
				this.page_id
			});
			if (searchPagesResultJson.errcode.Equals(ReturnCode.请求成功) && searchPagesResultJson.data.pages.Count > 0)
			{
				SearchPages_Data_Page searchPages_Data_Page = searchPagesResultJson.data.pages[0];
				this.laTitle.Text = searchPages_Data_Page.title;
				this.laSubtitle.Text = searchPages_Data_Page.description;
				this.imgThumbnail.ImageUrl = searchPages_Data_Page.icon_url;
			}
		}
	}
}
