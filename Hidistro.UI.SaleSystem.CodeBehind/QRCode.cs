using Hidistro.Core;
using Hidistro.Entities.Orders;
using Hidistro.SaleSystem.Member;
using Hidistro.UI.Common.Controls;
using System;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class QRCode : HtmlTemplatedWebControl
	{
		private HtmlGenericControl divMessage;

		private HtmlGenericControl ErrorMsg;

		private HtmlGenericControl divError;

		private HtmlGenericControl paymoney;

		private HtmlGenericControl divSuccess;

		private HtmlImage QRCodeImg;

		private HtmlInputHidden txt_OrderId;

		private HtmlAnchor defaultLink;

		private HtmlAnchor userLink;

		private HtmlAnchor orderLink;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-QRCode.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			string text = "";
			string text2 = "";
			string text3 = "";
			decimal num = default(decimal);
			bool flag = false;
			string a = (this.Page.Request["action"] == null) ? "" : this.Page.Request["action"].ToString().ToLower();
			if (a == "getstatus")
			{
				HttpContext.Current.Response.Clear();
				int num2 = 0;
				int num3 = 0;
				while (num2 != 1)
				{
					num3++;
					Thread.Sleep(500);
					text3 = ((this.Page.Request["orderId"] == null) ? "" : this.Page.Request["orderId"].ToString());
					if (text3 != "")
					{
						if (this.Page.Request["isrecharge"].ToInt(0) == 1)
						{
							num2 = (MemberProcessor.IsRechargeSuccess(text3) ? 1 : (-1));
						}
						else
						{
							int orderStatus = TradeHelper.GetOrderStatus(text3);
							num2 = ((orderStatus == 2) ? 1 : (-1));
						}
					}
					else
					{
						num2 = -1;
					}
					if (num3 == 30)
					{
						break;
					}
				}
				StringBuilder stringBuilder = new StringBuilder("{");
				stringBuilder.AppendFormat("\"Status\":\"{0}\"", num2);
				stringBuilder.Append("}");
				HttpContext.Current.Response.ContentType = "application/json";
				HttpContext.Current.Response.Write(stringBuilder.ToString());
				HttpContext.Current.Response.End();
			}
			else
			{
				text = HttpContext.Current.Request.QueryString["QRCodeImg"];
				text2 = HttpContext.Current.Request.QueryString["QrCodeUrl"];
				text3 = HttpContext.Current.Request.QueryString["OrderId"];
				int num4 = 0;
				string text4 = "";
				int.TryParse(HttpContext.Current.Request.QueryString["status"], out num4);
				if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(text2) || num4 != 1)
				{
					text4 = "错误的二维码信息";
				}
				if (num4 == 1 && text4 == "")
				{
					OrderInfo orderInfo = TradeHelper.GetOrderInfo(text3);
					if (orderInfo == null)
					{
						text4 = "错误的订单信息";
					}
					else if (orderInfo.OrderStatus != OrderStatus.WaitBuyerPay)
					{
						text4 = "订单当前状态不能支付";
					}
					if (orderInfo != null)
					{
						if (orderInfo.OrderStatus == OrderStatus.BuyerAlreadyPaid)
						{
							text4 = "订单已支付";
						}
						num = orderInfo.GetTotal(true);
					}
				}
				this.divMessage = (HtmlGenericControl)this.FindControl("divMessage");
				this.divSuccess = (HtmlGenericControl)this.FindControl("divSuccess");
				this.divError = (HtmlGenericControl)this.FindControl("divError");
				this.ErrorMsg = (HtmlGenericControl)this.FindControl("ErrorMsg");
				this.QRCodeImg = (HtmlImage)this.FindControl("QRCodeImg");
				this.paymoney = (HtmlGenericControl)this.FindControl("paymoney");
				this.txt_OrderId = (HtmlInputHidden)this.FindControl("txt_OrderId");
				this.defaultLink = (HtmlAnchor)this.FindControl("defaultLink");
				this.userLink = (HtmlAnchor)this.FindControl("userLink");
				this.orderLink = (HtmlAnchor)this.FindControl("orderLink");
				if (this.txt_OrderId != null)
				{
					this.txt_OrderId.Value = text3;
				}
				if (this.orderLink != null)
				{
					this.orderLink.HRef = base.GetRouteUrl("user_OrderDetails", new
					{
						OrderId = text3
					});
				}
				if (this.userLink != null)
				{
					this.userLink.HRef = "/User/UserDefault";
				}
				if (this.defaultLink != null)
				{
					this.defaultLink.HRef = "/";
				}
				if (this.divError != null && this.divMessage != null)
				{
					if (text4 != "")
					{
						this.divError.Visible = true;
						this.divMessage.Visible = false;
						this.ErrorMsg.InnerHtml = text4;
					}
					else
					{
						this.divMessage.Visible = true;
						this.divError.Visible = false;
						if (this.paymoney != null)
						{
							this.paymoney.InnerHtml = "&yen;" + num.F2ToString("f2");
						}
						if (this.QRCodeImg != null)
						{
							this.QRCodeImg.Src = text;
						}
					}
				}
			}
		}
	}
}
