using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Orders;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Shopping;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class WAPMemberOrdersVCode : WAPMemberTemplatedWebControl
	{
		private const string VERIFICATION_CODE_QRCODE_SAVE_RELATIVE_PATH = "/Storage/master/ServiceQRCode/";

		private HtmlGenericControl spanvcodecount;

		private HtmlGenericControl spanvcodetime;

		private Repeater rptVCodes;

		private string orderId;

		private OrderInfo order;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-VMemberOrdersVCode.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.spanvcodecount = (HtmlGenericControl)this.FindControl("spanvcodecount");
			this.spanvcodetime = (HtmlGenericControl)this.FindControl("spanvcodetime");
			this.rptVCodes = (Repeater)this.FindControl("rptVCodes");
			this.orderId = this.Page.Request.QueryString["orderId"];
			this.order = ShoppingProcessor.GetOrderInfo(this.orderId);
			if (this.order == null || this.order.UserId != HiContext.Current.UserId || this.order.OrderType != OrderType.ServiceOrder)
			{
				base.GotoResourceNotFound("此订单已不存在");
			}
			IList<OrderVerificationItemInfo> orderVerificationItems = TradeHelper.GetOrderVerificationItems(this.orderId);
			this.CreateVerificationCodeQRCode(orderVerificationItems);
			LineItemInfo value = this.order.LineItems.FirstOrDefault().Value;
			this.spanvcodecount.InnerHtml = orderVerificationItems.Count().ToString();
			string innerHtml = "长期有效";
			if (!value.IsValid && value.ValidStartDate.HasValue && value.ValidEndDate.HasValue)
			{
				DateTime value2 = value.ValidStartDate.Value;
				string str = value2.ToString("yyyy-MM-dd");
				value2 = value.ValidEndDate.Value;
				innerHtml = str + " 至 " + value2.ToString("yyyy-MM-dd");
			}
			this.spanvcodetime.InnerHtml = innerHtml;
			this.rptVCodes.ItemDataBound += this.rptVCodes_ItemDataBound;
			this.rptVCodes.DataSource = orderVerificationItems;
			this.rptVCodes.DataBind();
		}

		private void CreateVerificationCodeQRCode(IList<OrderVerificationItemInfo> orderVerCodes)
		{
			string format = "/Storage/master/ServiceQRCode/{0}_{1}.png";
			foreach (OrderVerificationItemInfo orderVerCode in orderVerCodes)
			{
				if (orderVerCode != null && !string.IsNullOrWhiteSpace(orderVerCode.VerificationPassword))
				{
					string qrCodeUrl = string.Format(format, orderVerCode.Id, orderVerCode.VerificationPassword);
					Globals.CreateQRCode(orderVerCode.VerificationPassword, qrCodeUrl, false, ImageFormats.Png);
				}
			}
		}

		private string GetVerificationCodeQRCodePath(OrderVerificationItemInfo data)
		{
			string result = "";
			if (data != null && !string.IsNullOrWhiteSpace(data.VerificationPassword))
			{
				string format = Globals.FullPath("/Storage/master/ServiceQRCode/{0}_{1}.png");
				result = string.Format(format, data.Id, data.VerificationPassword);
			}
			return result;
		}

		private void rptVCodes_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				OrderVerificationItemInfo orderVerificationItemInfo = e.Item.DataItem as OrderVerificationItemInfo;
				Image image = e.Item.FindControl("codeqrcode") as Image;
				HtmlGenericControl htmlGenericControl = e.Item.FindControl("vcodepassword") as HtmlGenericControl;
				HtmlGenericControl htmlGenericControl2 = e.Item.FindControl("vqrcodebox") as HtmlGenericControl;
				image.ImageUrl = this.GetVerificationCodeQRCodePath(orderVerificationItemInfo);
				htmlGenericControl.InnerHtml = this.GetShowVerificationPassword(orderVerificationItemInfo.VerificationPassword);
				htmlGenericControl2.Attributes["class"] = "vcode-qrcodebox vcodestatus" + orderVerificationItemInfo.VerificationStatus;
			}
		}

		private string GetShowVerificationPassword(string VerificationPassword)
		{
			string text = "";
			text = VerificationPassword;
			if (!string.IsNullOrWhiteSpace(text))
			{
				text = Regex.Replace(text, "(\\d{4})(\\d{4})(\\d+)", "$1 $2 $3");
			}
			return text;
		}
	}
}
