using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Depot;
using Hidistro.SaleSystem;
using Hidistro.SaleSystem.Depot;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class AppStoreHome : AppshopTemplatedWebControl
	{
		private int storeId = 0;

		private HtmlImage storeLogo;

		private Literal litStoreName;

		private Literal litStoreName2;

		private Literal litOpenDate;

		private Literal litStoreDelive;

		private HtmlInputHidden hidLatitude;

		private HtmlInputHidden hidLongitude;

		private Repeater rp_markting;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-StoreHome.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			PageTitle.AddSiteNameTitle("门店首页");
			this.storeLogo = (HtmlImage)this.FindControl("storeLogo");
			this.litStoreName = (Literal)this.FindControl("litStoreName");
			this.litStoreName2 = (Literal)this.FindControl("litStoreName2");
			this.litOpenDate = (Literal)this.FindControl("litOpenDate");
			this.litStoreDelive = (Literal)this.FindControl("litStoreDelive");
			this.hidLatitude = (HtmlInputHidden)this.FindControl("hidLatitude");
			this.hidLongitude = (HtmlInputHidden)this.FindControl("hidLongitude");
			this.rp_markting = (Repeater)this.FindControl("rp_markting");
			int.TryParse(this.Page.Request.QueryString["storeId"], out this.storeId);
			if (this.storeId > 0)
			{
				StoresInfo storeById = StoresHelper.GetStoreById(this.storeId);
				if (storeById != null)
				{
					this.storeLogo.Src = storeById.StoreImages;
					Literal literal = this.litStoreName;
					Literal literal2 = this.litStoreName2;
					string text2 = literal.Text = (literal2.Text = storeById.StoreName);
					string text3 = (storeById.OpenEndDate < storeById.OpenStartDate) ? "次日" : "";
					Literal literal3 = this.litOpenDate;
					DateTime dateTime = storeById.OpenStartDate;
					string arg = dateTime.ToString("HH:mm");
					string arg2 = text3;
					dateTime = storeById.OpenEndDate;
					literal3.Text = string.Format("{0} 至 {1}{2}", arg, arg2, dateTime.ToString("HH:mm"));
					HtmlInputHidden htmlInputHidden = this.hidLatitude;
					double? nullable = storeById.Latitude;
					htmlInputHidden.Value = nullable.ToString();
					HtmlInputHidden htmlInputHidden2 = this.hidLongitude;
					nullable = storeById.Longitude;
					htmlInputHidden2.Value = nullable.ToString();
					if (storeById.IsStoreDelive)
					{
						decimal? minOrderPrice = storeById.MinOrderPrice;
						int num;
						if (minOrderPrice.GetValueOrDefault() > default(decimal) && minOrderPrice.HasValue)
						{
							Literal literal4 = this.litStoreDelive;
							num = storeById.MinOrderPrice.ToInt(0);
							literal4.Text = $"￥{num.ToString()}起送，";
						}
						minOrderPrice = storeById.StoreFreight;
						if (minOrderPrice.GetValueOrDefault() > default(decimal) && minOrderPrice.HasValue)
						{
							Literal literal5 = this.litStoreDelive;
							string text4 = literal5.Text;
							num = storeById.StoreFreight.ToInt(0);
							literal5.Text = text4 + $"配送费￥{num.ToString()}";
						}
						else
						{
							Literal literal6 = this.litStoreDelive;
							literal6.Text += "免配送费";
						}
					}
					if (this.rp_markting != null)
					{
						List<StoreMarktingInfo> storeMarktingInfoList = StoreMarktingHelper.GetStoreMarktingInfoList();
						this.rp_markting.DataSource = storeMarktingInfoList;
						this.rp_markting.DataBind();
					}
					PageTitle.AddSiteNameTitle(storeById.StoreName);
				}
			}
		}

		private string GetImageFullPath(string imageUrl)
		{
			if (string.IsNullOrEmpty(imageUrl))
			{
				return Globals.FullPath(HiContext.Current.SiteSettings.DefaultProductThumbnail8);
			}
			if (imageUrl.StartsWith("http://"))
			{
				return imageUrl;
			}
			return Globals.FullPath(imageUrl);
		}
	}
}
