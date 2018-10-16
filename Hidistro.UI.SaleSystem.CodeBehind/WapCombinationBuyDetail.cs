using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Promotions;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.SaleSystem.Tags;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class WapCombinationBuyDetail : WAPTemplatedWebControl
	{
		private int combinaid;

		private Literal litMasterproductname;

		private Literal litMasterprice;

		private Literal litMasterproductpic;

		private Literal litMastersaleprice;

		private WapTemplatedRepeater rptcombinaproduct;

		private HtmlGenericControl selectmainsku;

		private HtmlInputHidden hidproductid;

		private HtmlInputHidden hidmasterproducthassku;

		private HtmlInputHidden hidmasterselectsku;

		private HtmlInputHidden hidmasterstock;

		private HtmlInputHidden hidcombinaid;

		private DataTable AllSkus = null;

		protected override void OnInit(EventArgs e)
		{
			this.SkinName = ((this.SkinName == null) ? "Skin-vCombinationBuyDetail.html" : this.SkinName);
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			if (!int.TryParse(this.Page.Request.QueryString["combinaid"], out this.combinaid))
			{
				base.GotoResourceNotFound("");
			}
			else
			{
				CombinationBuyInfo combinationBuyById = CombinationBuyHelper.GetCombinationBuyById(this.combinaid);
				if (combinationBuyById == null)
				{
					base.GotoResourceNotFound("");
				}
				else
				{
					DateTime now = DateTime.Now;
					int num;
					if (!(now.Date > combinationBuyById.EndDate))
					{
						now = DateTime.Now;
						num = ((now.Date < combinationBuyById.StartDate) ? 1 : 0);
					}
					else
					{
						num = 1;
					}
					if (num != 0)
					{
						this.ShowWapMessage("活动未开始或者已结束", "ProductDetails.aspx?productId=" + combinationBuyById.MainProductId);
					}
					else
					{
						this.HasActivitiesToJumpUrl(combinationBuyById.MainProductId);
						if (base.ClientType.Equals(ClientType.VShop))
						{
							FightGroupActivitiyModel fightGroupActivitiyModel = VShopHelper.GetFightGroupActivities(new FightGroupActivitiyQuery
							{
								PageIndex = 1,
								PageSize = 1,
								ProductId = combinationBuyById.MainProductId,
								Status = EnumFightGroupActivitiyStatus.BeingCarried
							}).Models.FirstOrDefault();
							if (fightGroupActivitiyModel != null)
							{
								this.Page.Response.Redirect("FightGroupActivityDetails.aspx?fightGroupActivityId=" + fightGroupActivitiyModel.FightGroupActivityId);
							}
						}
						this.FindControls();
						this.SetControlsValue(combinationBuyById.MainProductId);
						PageTitle.AddSiteNameTitle("组合购详情页");
					}
				}
			}
		}

		private void FindControls()
		{
			this.litMasterproductname = (Literal)this.FindControl("litMasterproductname");
			this.litMasterprice = (Literal)this.FindControl("litMasterprice");
			this.litMasterproductpic = (Literal)this.FindControl("litMasterproductpic");
			this.litMastersaleprice = (Literal)this.FindControl("litMastersaleprice");
			this.rptcombinaproduct = (WapTemplatedRepeater)this.FindControl("rptcombinaproduct");
			this.selectmainsku = (HtmlGenericControl)this.FindControl("selectmainsku");
			this.hidproductid = (HtmlInputHidden)this.FindControl("hidproductid");
			this.hidmasterproducthassku = (HtmlInputHidden)this.FindControl("hidmasterproducthassku");
			this.hidmasterselectsku = (HtmlInputHidden)this.FindControl("hidmasterselectsku");
			this.hidmasterstock = (HtmlInputHidden)this.FindControl("hidmasterstock");
			this.hidcombinaid = (HtmlInputHidden)this.FindControl("hidcombinaid");
			this.rptcombinaproduct.ItemDataBound += this.rptcombinaproduct_ItemDataBound;
		}

		private void rptcombinaproduct_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				Control control = e.Item.Controls[0];
				CombinationBuyandProductUnionInfo combinationBuyandProductUnionInfo = (CombinationBuyandProductUnionInfo)e.Item.DataItem;
				if (combinationBuyandProductUnionInfo.HasSKU)
				{
					Panel panel = control.FindControl("Panelsku") as Panel;
					if (panel != null)
					{
						Common_ComBinaSKUSelector common_ComBinaSKUSelector = new Common_ComBinaSKUSelector();
						common_ComBinaSKUSelector.ProductId = combinationBuyandProductUnionInfo.ProductId;
						common_ComBinaSKUSelector.MinCombinationPrice = combinationBuyandProductUnionInfo.MinCombinationPrice;
						common_ComBinaSKUSelector.TotalStock = combinationBuyandProductUnionInfo.Totalstock;
						common_ComBinaSKUSelector.ThumbnailUrl180 = combinationBuyandProductUnionInfo.ThumbnailUrl180;
						common_ComBinaSKUSelector.Skus = this.AllSkus.Select("productId = " + common_ComBinaSKUSelector.ProductId);
						panel.Controls.Add(common_ComBinaSKUSelector);
					}
				}
			}
		}

		private void SetControlsValue(int productId)
		{
			List<CombinationBuyandProductUnionInfo> combinationProductListByProductId = CombinationBuyHelper.GetCombinationProductListByProductId(productId);
			if (combinationProductListByProductId == null || combinationProductListByProductId.Count > 1)
			{
				CombinationBuyandProductUnionInfo combinationBuyandProductUnionInfo = combinationProductListByProductId.FirstOrDefault((CombinationBuyandProductUnionInfo c) => c.ProductId == productId);
				if (combinationBuyandProductUnionInfo != null)
				{
					string text = string.Empty;
					for (int i = 0; i < combinationProductListByProductId.Count; i++)
					{
						text = text + combinationProductListByProductId[i].ProductId + ",";
					}
					this.hidproductid.Value = productId.ToString();
					this.litMasterproductname.Text = "<a href=\"ProductDetails.aspx?ProductId=" + combinationBuyandProductUnionInfo.ProductId + "\">" + combinationBuyandProductUnionInfo.ProductName + "</a>";
					Literal literal = this.litMasterprice;
					decimal num = combinationBuyandProductUnionInfo.MinCombinationPrice;
					literal.Text = num.ToString();
					Literal literal2 = this.litMasterproductpic;
					string[] obj = new string[5]
					{
						"<a href=\"ProductDetails.aspx?ProductId=",
						null,
						null,
						null,
						null
					};
					int num2 = combinationBuyandProductUnionInfo.ProductId;
					obj[1] = num2.ToString();
					obj[2] = "\" class=\"cart_1\"><img src=\"";
					obj[3] = (string.IsNullOrEmpty(combinationBuyandProductUnionInfo.ThumbnailUrl180) ? HiContext.Current.SiteSettings.DefaultProductImage : combinationBuyandProductUnionInfo.ThumbnailUrl180);
					obj[4] = "\"></a>";
					literal2.Text = string.Concat(obj);
					Literal literal3 = this.litMastersaleprice;
					num = combinationBuyandProductUnionInfo.MinSalePrice;
					literal3.Text = num.ToString();
					this.AllSkus = CombinationBuyHelper.GetSkus(text.Substring(0, text.Length - 1));
					if (!combinationBuyandProductUnionInfo.HasSKU)
					{
						this.selectmainsku.Visible = false;
						this.hidmasterproducthassku.Value = "0";
						this.hidmasterselectsku.Value = productId + "_0";
					}
					else
					{
						this.hidmasterselectsku.Value = "0";
						this.hidmasterproducthassku.Value = "1";
						Common_ComBinaSKUSelector common_ComBinaSKUSelector = new Common_ComBinaSKUSelector();
						common_ComBinaSKUSelector.ProductId = combinationBuyandProductUnionInfo.ProductId;
						common_ComBinaSKUSelector.MinCombinationPrice = combinationBuyandProductUnionInfo.MinCombinationPrice;
						common_ComBinaSKUSelector.TotalStock = combinationBuyandProductUnionInfo.Totalstock;
						common_ComBinaSKUSelector.ThumbnailUrl180 = combinationBuyandProductUnionInfo.ThumbnailUrl180;
						common_ComBinaSKUSelector.Skus = this.AllSkus.Select("productId = " + productId);
						this.Controls.Add(common_ComBinaSKUSelector);
					}
					HtmlInputHidden htmlInputHidden = this.hidmasterstock;
					num2 = combinationBuyandProductUnionInfo.Totalstock;
					htmlInputHidden.Value = num2.ToString();
					HtmlInputHidden htmlInputHidden2 = this.hidcombinaid;
					num2 = combinationBuyandProductUnionInfo.CombinationId;
					htmlInputHidden2.Value = num2.ToString();
					combinationProductListByProductId.Remove(combinationBuyandProductUnionInfo);
					this.rptcombinaproduct.DataSource = combinationProductListByProductId;
					this.rptcombinaproduct.DataBind();
					this.AllSkus.Dispose();
				}
				else
				{
					this.ShowWapMessage("活动已结束或不存在", "ProductDetails.aspx?productId=" + productId);
				}
			}
			else
			{
				this.ShowWapMessage("活动已结束或不存在", "ProductDetails.aspx?productId=" + productId);
			}
		}

		private void HasActivitiesToJumpUrl(int productId)
		{
			string text = string.Empty;
			CountDownInfo countDownInfo = PromoteHelper.ActiveCountDownByProductId(productId, 0);
			GroupBuyInfo groupBuyInfo = PromoteHelper.ActiveGroupBuyByProductId(productId);
			if (countDownInfo != null)
			{
				text = "/{0}/CountDownProductsDetails.aspx?countDownId=" + countDownInfo.CountDownId;
			}
			else if (groupBuyInfo != null)
			{
				text = "/{0}/GroupBuyProductDetails.aspx?groupBuyId=" + groupBuyInfo.GroupBuyId;
			}
			if (!string.IsNullOrEmpty(text))
			{
				this.Page.Response.Redirect(this.FillStringURL(text));
			}
		}

		private string FillStringURL(string url)
		{
			string text = url;
			string[] segments = this.Page.Request.Url.Segments;
			foreach (string input in segments)
			{
				string text2 = Globals.GetStringByRegularExpression(input, "[a-zA-Z]").ToLower();
				switch (text2)
				{
				case "vshop":
					text = string.Format(text, "Vshop");
					break;
				case "wapshop":
					text = string.Format(text, "wapshop");
					break;
				case "alioh":
					text = string.Format(text, "alioh");
					break;
				}
			}
			return text;
		}
	}
}
