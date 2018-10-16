using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Depot;
using Hidistro.Entities.Orders;
using Hidistro.SaleSystem.Depot;
using Hidistro.SaleSystem.Member;
using Hidistro.UI.Common.Controls;
using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class AppViewQRCode : AppshopMemberTemplatedWebControl
	{
		private HtmlInputHidden hfTakeCode;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-VViewQRCode.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			PageTitle.AddSiteNameTitle("查看提货码");
			this.hfTakeCode = (HtmlInputHidden)this.FindControl("hfTakeCode");
			string orderId = this.Page.Request.QueryString["orderId"].ToNullString();
			OrderInfo orderInfo = TradeHelper.GetOrderInfo(orderId);
			if (orderInfo != null && orderInfo.UserId == HiContext.Current.UserId)
			{
				HtmlImage htmlImage = this.FindControl("imgProduct") as HtmlImage;
				Literal control = this.FindControl("litItemCount") as Literal;
				Literal control2 = this.FindControl("litStoreName") as Literal;
				Literal control3 = this.FindControl("litStoreAddress") as Literal;
				Literal control4 = this.FindControl("litTel") as Literal;
				LineItemInfo lineItemInfo = orderInfo.LineItems.Values.FirstOrDefault();
				Literal control5 = this.FindControl("litTakeCode") as Literal;
				htmlImage.Src = (string.IsNullOrEmpty(lineItemInfo.ThumbnailsUrl) ? SettingsManager.GetMasterSettings().DefaultProductImage : lineItemInfo.ThumbnailsUrl);
				control.SetWhenIsNotNull(orderInfo.LineItems.Count.ToString());
				control5.SetWhenIsNotNull(orderInfo.TakeCode);
				this.hfTakeCode.SetWhenIsNotNull(Globals.HIPOSTAKECODEPREFIX + orderInfo.TakeCode);
				StoresInfo storeById = DepotHelper.GetStoreById(orderInfo.StoreId);
				if (storeById != null)
				{
					control2.SetWhenIsNotNull(storeById.StoreName);
					control4.SetWhenIsNotNull(storeById.Tel);
					control3.SetWhenIsNotNull(RegionHelper.GetFullRegion(storeById.RegionId, " ", true, 0) + " " + storeById.Address);
				}
			}
		}
	}
}
