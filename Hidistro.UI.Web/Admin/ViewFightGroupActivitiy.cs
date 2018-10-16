using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.SaleSystem.Commodities;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	public class ViewFightGroupActivitiy : AdminPage
	{
		private int productId;

		public string filterProductIds;

		public int fightGroupActivityId;

		protected Literal ltProductName;

		protected HtmlGenericControl liSkus;

		protected Repeater rptProductSkus;

		protected Image imgIcon;

		protected HtmlGenericControl liSalePrice;

		protected Label lblPrice;

		protected HtmlGenericControl liDefaultPrice;

		protected Literal ltPrice;

		protected HtmlGenericControl liDefaultStock;

		protected Literal ltStock;

		protected HtmlGenericControl liDefaultTotalCount;

		protected Literal ltTotalCount;

		protected Literal ltStartTime;

		protected Literal ltEndDate;

		protected Literal ltBoughtCount;

		protected Literal ltJoinNumber;

		protected Literal ltLimitedHour;

		protected Literal ltMaxCount;

		protected HiddenField hfFightGroupActivityId;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.fightGroupActivityId = this.Page.Request["fightGroupActivityId"].ToInt(0);
			this.productId = this.Page.Request["productId"].ToInt(0);
			if (!this.Page.IsPostBack)
			{
				this.BindFightGroupActivitiy();
				this.BindProduct();
				this.ShowSKUOrDefault();
			}
			this.hfFightGroupActivityId.Value = this.fightGroupActivityId.ToString();
			this.filterProductIds = VShopHelper.GetFightGroupActivitiyActiveProducts();
			if (this.productId > 0 && this.filterProductIds.Length > 0)
			{
				this.filterProductIds = this.filterProductIds + "," + this.productId;
			}
			else if (this.filterProductIds.Length == 0 && this.productId > 0)
			{
				this.filterProductIds = this.productId.ToString();
			}
		}

		private void BindFightGroupActivitiy()
		{
			FightGroupActivityInfo fightGroupActivitieInfo = VShopHelper.GetFightGroupActivitieInfo(this.fightGroupActivityId);
			IList<FightGroupSkuInfo> fightGroupSkus = VShopHelper.GetFightGroupSkus(this.fightGroupActivityId);
			int num = 0;
			if (fightGroupSkus.Count() == 1)
			{
				this.ltPrice.Text = fightGroupSkus[0].SalePrice.F2ToString("f2");
				this.ltTotalCount.Text = fightGroupSkus[0].TotalCount.ToNullString();
				num = fightGroupSkus[0].BoughtCount;
			}
			else
			{
				for (int i = 0; i < fightGroupSkus.Count(); i++)
				{
					num += fightGroupSkus[i].BoughtCount;
				}
			}
			Literal literal = this.ltMaxCount;
			int num2 = fightGroupActivitieInfo.MaxCount;
			literal.Text = num2.ToString();
			this.ltBoughtCount.Text = num.ToString();
			Literal literal2 = this.ltEndDate;
			DateTime dateTime = fightGroupActivitieInfo.EndDate;
			literal2.Text = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
			Literal literal3 = this.ltStartTime;
			dateTime = fightGroupActivitieInfo.StartDate;
			literal3.Text = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
			this.imgIcon.ImageUrl = fightGroupActivitieInfo.Icon;
			Literal literal4 = this.ltJoinNumber;
			num2 = fightGroupActivitieInfo.JoinNumber;
			literal4.Text = num2.ToString();
			Literal literal5 = this.ltLimitedHour;
			num2 = fightGroupActivitieInfo.LimitedHour;
			literal5.Text = num2.ToString();
		}

		private void ShowSKUOrDefault()
		{
			if (this.rptProductSkus.Items.Count == 0)
			{
				this.liSkus.Style.Add("display", "none");
				if (this.productId > 0)
				{
					this.liSalePrice.Style.Add("display", "block");
					this.liDefaultStock.Style.Add("display", "block");
				}
			}
			else
			{
				this.liDefaultPrice.Style.Add("display", "none");
				this.liDefaultStock.Style.Add("display", "none");
				this.liDefaultTotalCount.Style.Add("display", "none");
			}
		}

		private void BindProduct()
		{
			FightGroupActivityInfo fightGroupActivitieInfo = VShopHelper.GetFightGroupActivitieInfo(this.fightGroupActivityId);
			this.productId = ((this.productId == 0) ? fightGroupActivitieInfo.ProductId : this.productId);
			this.rptProductSkus.DataSource = VShopHelper.GetFightGroupSkus(this.fightGroupActivityId, this.productId);
			this.rptProductSkus.DataBind();
			IList<int> list = null;
			Dictionary<int, IList<int>> dictionary = default(Dictionary<int, IList<int>>);
			ProductInfo productDetails = ProductHelper.GetProductDetails(this.productId, out dictionary, out list);
			if (productDetails != null)
			{
				this.ltProductName.Text = productDetails.ProductName;
				this.ltStock.Text = productDetails.Stock.ToString();
				this.lblPrice.Text = productDetails.MinSalePrice.F2ToString("f2");
			}
		}
	}
}
