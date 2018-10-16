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
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class RegisteredCoupons : HtmlTemplatedWebControl
	{
		private ThemedTemplatedRepeater rptRegisterCoupons;

		private HiddenField hidIsOpenGiftCoupons;

		private Literal lblTotalPrice;

		private HyperLink btnToGo;

		private bool isOldUser = false;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-RegisteredCoupons.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.isOldUser = HttpContext.Current.Request["isOldUser"].ToBool();
			this.rptRegisterCoupons = (ThemedTemplatedRepeater)this.FindControl("rptRegisterCoupons");
			this.hidIsOpenGiftCoupons = (HiddenField)this.FindControl("hidIsOpenGiftCoupons");
			this.lblTotalPrice = (Literal)this.FindControl("lblTotalPrice");
			this.btnToGo = (HyperLink)this.FindControl("btnToGo");
			if (!this.Page.IsPostBack)
			{
				this.BindRegisterCoupons();
			}
		}

		private void BindRegisterCoupons()
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			this.hidIsOpenGiftCoupons.Value = (masterSettings.IsOpenGiftCoupons ? "1" : "0");
			this.btnToGo.Text = (masterSettings.IsOpenGiftCoupons ? "去注册" : "去购物");
			this.btnToGo.NavigateUrl = (masterSettings.IsOpenGiftCoupons ? "/Register.aspx" : "/");
			MemberInfo user = HiContext.Current.User;
			if (masterSettings.IsOpenGiftCoupons)
			{
				string sWhere = "and COUPONID IN ('" + masterSettings.GiftCouponList.Replace(",", "','") + "')";
				DbQueryResult couponInfos = CouponHelper.GetCouponInfos(this.GetCouponsSearch(), sWhere);
				decimal num = default(decimal);
				if (couponInfos.Data != null)
				{
					DataTable data = couponInfos.Data;
					data.Columns.Add("isEnd");
					foreach (DataRow row in data.Rows)
					{
						num += row["Price"].ToDecimal(0);
						int couponSurplus = CouponHelper.GetCouponSurplus(row["CouponId"].ToInt(0));
						if (couponSurplus <= 0)
						{
							row["isEnd"] = "end";
						}
						if (row["ClosingTime"].ToDateTime().HasValue && row["ClosingTime"].ToDateTime().Value < DateTime.Now)
						{
							row["isEnd"] = "end";
						}
					}
					this.rptRegisterCoupons.DataSource = couponInfos.Data;
					this.rptRegisterCoupons.DataBind();
				}
				this.lblTotalPrice.Text = num.F2ToString("f2");
				if (user.UserId > 0 && user.IsLogined)
				{
					this.btnToGo.NavigateUrl = "RegisteredCoupons.aspx?isOldUser=true";
				}
			}
			if (this.isOldUser)
			{
				this.hidIsOpenGiftCoupons.Value = "2";
				this.btnToGo.Text = "去购物";
				this.btnToGo.NavigateUrl = "/";
			}
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
