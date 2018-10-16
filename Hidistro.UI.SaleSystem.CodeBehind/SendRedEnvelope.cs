using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Orders;
using Hidistro.SaleSystem.Shopping;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using System;
using System.Web;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class SendRedEnvelope : WAPTemplatedWebControl
	{
		private HiddenField hdAppId;

		private HiddenField hdTimestamp;

		private HiddenField hdNonceStr;

		private HiddenField hdSignature;

		private HiddenField hdTitle;

		private HiddenField hdDesc;

		private HiddenField hdImgUrl;

		private HiddenField hdLink;

		private HiddenField hdSendCode;

		private HiddenField hdRedEnvelopeId;

		private HiddenField hdOrderId;

		private string orderId = string.Empty;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-SendRedEnvelope.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.CheckSendRedEnvelope();
			this.hdAppId = (HiddenField)this.FindControl("hdAppId");
			this.hdTimestamp = (HiddenField)this.FindControl("hdTimestamp");
			this.hdNonceStr = (HiddenField)this.FindControl("hdNonceStr");
			this.hdSignature = (HiddenField)this.FindControl("hdSignature");
			this.hdTitle = (HiddenField)this.FindControl("hdTitle");
			this.hdDesc = (HiddenField)this.FindControl("hdDesc");
			this.hdImgUrl = (HiddenField)this.FindControl("hdImgUrl");
			this.hdLink = (HiddenField)this.FindControl("hdLink");
			this.hdSendCode = (HiddenField)this.FindControl("hdSendCode");
			this.hdRedEnvelopeId = (HiddenField)this.FindControl("hdRedEnvelopeId");
			this.hdOrderId = (HiddenField)this.FindControl("hdOrderId");
			this.hdAppId.Value = base.site.WeixinAppId;
			string jsApiTicket = base.GetJsApiTicket(true);
			string text = WAPTemplatedWebControl.GenerateNonceStr();
			string text2 = WAPTemplatedWebControl.GenerateTimeStamp();
			string absoluteUri = this.Page.Request.Url.AbsoluteUri;
			this.hdTimestamp.Value = text2;
			this.hdNonceStr.Value = text;
			this.hdSignature.Value = base.GetSignature(jsApiTicket, text, text2, absoluteUri);
			WeiXinRedEnvelopeInfo openedWeiXinRedEnvelope = WeiXinRedEnvelopeProcessor.GetOpenedWeiXinRedEnvelope();
			string text3 = this.orderId;
			int id = openedWeiXinRedEnvelope.Id;
			RedEnvelopeSendRecord redEnvelopeSendRecord = WeiXinRedEnvelopeProcessor.GetRedEnvelopeSendRecord(text3, id.ToString());
			Guid guid;
			if (redEnvelopeSendRecord != null)
			{
				this.hdRedEnvelopeId.Value = redEnvelopeSendRecord.RedEnvelopeId.ToString();
				this.hdTitle.Value = openedWeiXinRedEnvelope.Name;
				this.hdDesc.Value = openedWeiXinRedEnvelope.ShareDetails;
				this.hdImgUrl.Value = Globals.FullPath(openedWeiXinRedEnvelope.ShareIcon);
				HiddenField hiddenField = this.hdSendCode;
				guid = redEnvelopeSendRecord.SendCode;
				hiddenField.Value = guid.ToString();
				this.hdLink.Value = Globals.FullPath("/Vshop/GetRedEnvelope?SendCode=" + this.hdSendCode.Value + "&OrderId=" + this.orderId);
				this.hdOrderId.Value = this.orderId;
			}
			else
			{
				HiddenField hiddenField2 = this.hdRedEnvelopeId;
				id = openedWeiXinRedEnvelope.Id;
				hiddenField2.Value = id.ToString();
				this.hdTitle.Value = openedWeiXinRedEnvelope.Name;
				this.hdDesc.Value = openedWeiXinRedEnvelope.ShareDetails;
				this.hdImgUrl.Value = Globals.FullPath(openedWeiXinRedEnvelope.ShareIcon);
				HiddenField hiddenField3 = this.hdSendCode;
				guid = Guid.NewGuid();
				hiddenField3.Value = guid.ToString();
				this.hdLink.Value = Globals.FullPath("/Vshop/GetRedEnvelope?SendCode=" + this.hdSendCode.Value + "&OrderId=" + this.orderId);
				this.hdOrderId.Value = this.orderId;
			}
		}

		public bool CheckSendRedEnvelope()
		{
			HttpCookie httpCookie = HiContext.Current.Context.Request.Cookies["OrderIdCookie"];
			if (httpCookie != null)
			{
				this.orderId = httpCookie.Value;
			}
			else
			{
				this.orderId = HiContext.Current.Context.Request.Params["OrderId"];
			}
			if (string.IsNullOrEmpty(this.orderId))
			{
				return false;
			}
			OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(this.orderId);
			WeiXinRedEnvelopeInfo openedWeiXinRedEnvelope = WeiXinRedEnvelopeProcessor.GetOpenedWeiXinRedEnvelope();
			if (openedWeiXinRedEnvelope == null)
			{
				this.Page.Response.Redirect("/Vshop/RedEnvelopeError.aspx?errorInfo=没有找到任何红包活动", true);
			}
			else
			{
				if (openedWeiXinRedEnvelope.ActiveStartTime > DateTime.Now)
				{
					this.Page.Response.Redirect("/Vshop/RedEnvelopeError.aspx?errorInfo=红包活动还没有开始", true);
				}
				if (openedWeiXinRedEnvelope.ActiveEndTime < DateTime.Now)
				{
					this.Page.Response.Redirect("/Vshop/RedEnvelopeError.aspx?errorInfo=红包活动已经过期", true);
				}
				decimal amount = orderInfo.GetAmount(false);
				if (amount > decimal.Zero && amount >= openedWeiXinRedEnvelope.EnableIssueMinAmount)
				{
					return true;
				}
				this.Page.Response.Redirect("/Vshop/RedEnvelopeError.aspx?errorInfo=你没有满足发红包的金额条件", true);
			}
			return true;
		}
	}
}
