using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Promotions;
using Hidistro.SaleSystem.Commodities;
using Hidistro.SaleSystem.Promotions;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class WapCountDownProductList : WAPTemplatedWebControl
	{
		private int categoryId;

		private string keyWord;

		private HiImage imgUrl;

		private Literal litContent;

		private WapTemplatedRepeater rptProducts;

		private WapTemplatedRepeater rptCategories;

		private HtmlInputHidden txtTotal;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-VCountDownProductList.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			int.TryParse(this.Page.Request.QueryString["categoryId"], out this.categoryId);
			this.keyWord = this.Page.Request.QueryString["keyWord"];
			this.imgUrl = (HiImage)this.FindControl("imgUrl");
			this.litContent = (Literal)this.FindControl("litContent");
			this.rptProducts = (WapTemplatedRepeater)this.FindControl("rptCountDownProducts");
			this.txtTotal = (HtmlInputHidden)this.FindControl("txtTotal");
			this.rptCategories = (WapTemplatedRepeater)this.FindControl("rptCategories");
			if (this.rptCategories != null)
			{
				IEnumerable<CategoryInfo> subCategories = CatalogHelper.GetSubCategories(this.categoryId);
				this.rptCategories.DataSource = subCategories;
				this.rptCategories.DataBind();
			}
			int page = default(int);
			if (!int.TryParse(this.Page.Request.QueryString["page"], out page))
			{
				page = 1;
			}
			int size = default(int);
			if (!int.TryParse(this.Page.Request.QueryString["size"], out size))
			{
				size = 10;
			}
			int storeId = this.Page.Request.QueryString["StoreId"].ToInt(0);
			int num = default(int);
			DataTable countDownProductList = PromoteHelper.GetCountDownProductList(this.categoryId, this.keyWord, page, size, storeId, out num, false);
			this.rptProducts.ItemDataBound += this.rptProduct_ItemDataBound;
			this.rptProducts.DataSource = countDownProductList;
			this.rptProducts.DataBind();
			this.txtTotal.SetWhenIsNotNull(num.ToString());
			PageTitle.AddSiteNameTitle("限时抢购");
		}

		protected void rptProduct_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				DataRowView dataRowView = (DataRowView)e.Item.DataItem;
				HtmlInputButton htmlInputButton = e.Item.Controls[0].FindControl("btnNow") as HtmlInputButton;
				HtmlInputButton htmlInputButton2 = e.Item.Controls[0].FindControl("btnWill") as HtmlInputButton;
				HtmlInputButton htmlInputButton3 = e.Item.Controls[0].FindControl("btnOver") as HtmlInputButton;
				htmlInputButton.Visible = false;
				htmlInputButton2.Visible = false;
				htmlInputButton3.Visible = false;
				DateTime? nullable = dataRowView["StartDate"].ToDateTime();
				DateTime? nullable2 = dataRowView["EndDate"].ToDateTime();
				int num = dataRowView["TotalCount"].ToInt(0);
				int num2 = dataRowView["BoughtCount"].ToInt(0);
				CountDownInfo countDownInfo = PromoteHelper.GetCountDownInfo(dataRowView["CountDownId"].ToInt(0), this.Page.Request.QueryString["StoreId"].ToInt(0));
				if (htmlInputButton != null && htmlInputButton2 != null && htmlInputButton3 != null)
				{
					if (nullable.Value >= DateTime.Now)
					{
						htmlInputButton2.Visible = true;
					}
					else if (nullable.Value <= DateTime.Now && nullable2.Value >= DateTime.Now)
					{
						htmlInputButton.Visible = true;
						if (!countDownInfo.IsRunning)
						{
							htmlInputButton.Visible = false;
							htmlInputButton3.Visible = true;
						}
					}
					else if (nullable2.Value < DateTime.Now)
					{
						htmlInputButton3.Visible = true;
					}
				}
			}
		}
	}
}
