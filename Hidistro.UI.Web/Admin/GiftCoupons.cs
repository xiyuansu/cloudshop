using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Store;
using Hidistro.Messages;
using Hidistro.SaleSystem.Promotions;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.Members)]
	public class GiftCoupons : AdminCallBackPage
	{
		protected int UserNum = 0;

		protected string UserIds;

		protected Button btnSure;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.UserIds = this.Page.Request["UserIds"].ToNullString();
			int[] array = (from d in this.UserIds.Split(',')
			where !string.IsNullOrWhiteSpace(d)
			select int.Parse(d)).ToArray();
			this.UserNum = array.Count();
			this.UserIds = string.Join(",", array);
			this.btnSure.Click += this.btnSure_Click;
		}

		private void btnSure_Click(object sender, EventArgs e)
		{
			string text = base.Request.Form["CheckBoxGroup"].ToNullString();
			string text2 = this.Page.Request["UserIds"].ToNullString();
			if (string.IsNullOrEmpty(text))
			{
				this.ShowMsg("请选择赠送的优惠券", false);
			}
			else
			{
				List<CouponInfo> list = new List<CouponInfo>();
				string[] array = text.Split(',');
				foreach (string text3 in array)
				{
					if (!string.IsNullOrEmpty(text3))
					{
						int num = 0;
						if (int.TryParse(text3, out num) && num > 0)
						{
							CouponInfo eFCoupon = CouponHelper.GetEFCoupon(num);
							if (eFCoupon != null)
							{
								list.Add(eFCoupon);
							}
						}
					}
				}
				if (list.Count == 0)
				{
					this.btnSure.Text = "确 定";
					this.btnSure.Enabled = true;
					this.ShowMsg("请选择赠送的优惠券", false);
				}
				else
				{
					string empty = string.Empty;
					int num2 = 0;
					int num3 = 0;
					int num4 = 0;
					string[] array2 = text2.Split(',');
					foreach (string s in array2)
					{
						int num5 = 0;
						if (int.TryParse(s, out num5) && num5 > 0)
						{
							MemberInfo user = Users.GetUser(num5);
							num4 = 0;
							decimal num6 = default(decimal);
							foreach (CouponInfo item in list)
							{
								CouponItemInfo couponItemInfo = new CouponItemInfo();
								couponItemInfo.UserId = user.UserId;
								couponItemInfo.UserName = user.UserName;
								couponItemInfo.CanUseProducts = item.CanUseProducts;
								couponItemInfo.ClosingTime = item.ClosingTime;
								couponItemInfo.CouponId = item.CouponId;
								couponItemInfo.CouponName = item.CouponName;
								couponItemInfo.OrderUseLimit = item.OrderUseLimit;
								couponItemInfo.Price = item.Price;
								couponItemInfo.StartTime = item.StartTime;
								couponItemInfo.UseWithGroup = item.UseWithGroup;
								couponItemInfo.UseWithPanicBuying = item.UseWithPanicBuying;
								couponItemInfo.UseWithFireGroup = item.UseWithFireGroup;
								couponItemInfo.GetDate = DateTime.Now;
								CouponActionStatus couponActionStatus = CouponHelper.AddCouponItemInfo(couponItemInfo);
								num6 += item.Price;
								if (couponActionStatus == CouponActionStatus.Success)
								{
									num2++;
									num4++;
								}
								else
								{
									num3++;
								}
							}
							if (num4 > 0)
							{
								Messenger.GiftCoupons(user, num4, num6);
							}
						}
					}
					if (num2 + num3 > 0)
					{
						base.CloseWindow("成功发送" + num2 + "张优惠券，" + num3 + "张优惠券发送失败");
					}
					else
					{
						this.ShowMsg("成功失败", false);
					}
				}
			}
		}

		public string GetCouponSurplus(int CouponId)
		{
			int couponSurplus = CouponHelper.GetCouponSurplus(CouponId);
			int cantObtainUserNum = CouponHelper.GetCantObtainUserNum(CouponId);
			if (couponSurplus >= this.UserNum - cantObtainUserNum)
			{
				return couponSurplus.ToString();
			}
			return "<span>" + couponSurplus + "</span><span style='color:red;'>(数量不足)</span>";
		}
	}
}
