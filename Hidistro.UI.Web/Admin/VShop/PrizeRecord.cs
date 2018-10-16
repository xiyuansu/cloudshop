using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.vshop
{
	public class PrizeRecord : AdminPage
	{
		protected int activeid;

		protected bool nodata = false;

		protected HtmlAnchor alist;

		protected Literal LitListTitle;

		protected Literal LitTitle;

		protected Repeater rpMaterial;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (int.TryParse(base.Request.QueryString["id"], out this.activeid))
			{
				if (!this.Page.IsPostBack)
				{
					this.BindPrizeList();
				}
			}
			else
			{
				this.ShowMsg("参数错误", false);
			}
		}

		protected void BindPrizeList()
		{
			PrizeQuery prizeQuery = new PrizeQuery();
			prizeQuery.ActivityId = this.activeid;
			List<PrizeRecordInfo> prizeList = VShopHelper.GetPrizeList(prizeQuery);
			if (prizeList != null && prizeList.Count > 0)
			{
				this.LitTitle.Text = prizeList[0].ActivityName;
			}
			this.nodata = (prizeList.Count == 0);
			this.rpMaterial.DataSource = prizeList;
			this.rpMaterial.DataBind();
		}
	}
}
