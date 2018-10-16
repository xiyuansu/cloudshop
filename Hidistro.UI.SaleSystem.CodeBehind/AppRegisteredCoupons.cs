using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Promotions;
using Hidistro.SaleSystem.Promotions;
using Hidistro.UI.Common.Controls;
using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class AppRegisteredCoupons : AppshopTemplatedWebControl
	{
		private AppshopTemplatedRepeater rptRegisterCoupons;

		private HiddenField hidIsOpenGiftCoupons;

		private Literal lblTotalPrice;

		private HyperLink btnToGo;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-registeredCoupons.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			MemberInfo user = HiContext.Current.User;
			if (!this.Page.IsPostBack)
			{
				this.rptRegisterCoupons = (AppshopTemplatedRepeater)this.FindControl("rptRegisterCoupons");
				this.hidIsOpenGiftCoupons = (HiddenField)this.FindControl("hidIsOpenGiftCoupons");
				this.lblTotalPrice = (Literal)this.FindControl("lblTotalPrice");
				this.btnToGo = (HyperLink)this.FindControl("btnToGo");
				this.BindRegisterCoupons();
			}
			PageTitle.AddSiteNameTitle("注册送券");
		}

		private void BindRegisterCoupons()
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (!masterSettings.IsOpenGiftCoupons)
			{
				HttpContext.Current.Response.Redirect("RegisteredCouponsEnd.aspx");
			}
			this.hidIsOpenGiftCoupons.Value = (masterSettings.IsOpenGiftCoupons ? "1" : "0");
			this.btnToGo.NavigateUrl = "";
			string sWhere = "and COUPONID IN ('" + masterSettings.GiftCouponList.Replace(",", "','") + "')";
			DbQueryResult couponInfos = CouponHelper.GetCouponInfos(this.GetCouponsSearch(), sWhere);
			decimal num = default(decimal);
			if (couponInfos.Data != null)
			{
				DataTable data = couponInfos.Data;
				data.Columns.Add("ItemIndex");
				data.Columns.Add("isEnd");
				int num2 = 0;
				foreach (DataRow row in data.Rows)
				{
					int couponSurplus = CouponHelper.GetCouponSurplus(row["CouponId"].ToInt(0));
					if (couponSurplus <= 0)
					{
						row["isEnd"] = "couponed";
					}
					if (row["ClosingTime"].ToDateTime().HasValue && row["ClosingTime"].ToDateTime().Value < DateTime.Now)
					{
						row["isEnd"] = "couponed";
					}
					num += row["Price"].ToDecimal(0);
					if (data.Rows.Count == num2 + 1 && data.Rows.Count % 2 == 1)
					{
						row["ItemIndex"] = -1;
					}
					else
					{
						row["ItemIndex"] = num2;
					}
					num2++;
				}
				this.rptRegisterCoupons.DataSource = data;
				this.rptRegisterCoupons.DataBind();
			}
			this.lblTotalPrice.Text = num.F2ToString("f2");
		}

		public CouponsSearch GetCouponsSearch()
		{
			return new CouponsSearch
			{
				ObtainWay = 1,
				PageIndex = 1,
				PageSize = 100
			};
		}
	}
}
