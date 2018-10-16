using Hidistro.Context;
using Hidistro.Core;
using Hidistro.SaleSystem.Member;
using Hidistro.UI.Common.Controls;
using System;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class WXQRCode : HtmlTemplatedWebControl
	{
		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-WXQRCode.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			string a = (this.Page.Request["action"] == null) ? "" : this.Page.Request["action"].ToString().ToLower();
			if (this.Page.Request["isrecharge"].ToInt(0) == 1)
			{
				HtmlGenericControl htmlGenericControl = (HtmlGenericControl)this.FindControl("divsteps");
				htmlGenericControl.Visible = false;
			}
			if (a == "getstatus")
			{
				HttpContext.Current.Response.Clear();
				int num = 0;
				int num2 = 0;
				while (num != 1)
				{
					num2++;
					Thread.Sleep(1000);
					string text = (this.Page.Request["orderId"] == null) ? "" : this.Page.Request["orderId"].ToString();
					if (text != "")
					{
						if (this.Page.Request["isrecharge"].ToInt(0) == 1)
						{
							num = (MemberProcessor.IsRechargeSuccess(text) ? 1 : (-1));
						}
						else
						{
							int orderStatus = TradeHelper.GetOrderStatus(text);
							num = ((orderStatus == 2) ? 1 : (-1));
						}
					}
					else
					{
						num = -1;
					}
					if (num2 == 25)
					{
						break;
					}
				}
				StringBuilder stringBuilder = new StringBuilder("{");
				stringBuilder.AppendFormat("\"Status\":\"{0}\"", num);
				stringBuilder.Append("}");
				HttpContext.Current.Response.ContentType = "application/json";
				HttpContext.Current.Response.Write(stringBuilder.ToString());
				HttpContext.Current.Response.End();
			}
			else
			{
				Image image = (Image)this.FindControl("imgCode");
				HtmlInputHidden htmlInputHidden = (HtmlInputHidden)this.FindControl("txt_OrderId");
				HtmlInputHidden htmlInputHidden2 = (HtmlInputHidden)this.FindControl("txt_isrecharge");
				HtmlAnchor htmlAnchor = (HtmlAnchor)this.FindControl("userLink");
				HtmlAnchor htmlAnchor2 = (HtmlAnchor)this.FindControl("orderLink");
				HtmlImage htmlImage = (HtmlImage)this.FindControl("leftimg");
				HtmlImage htmlImage2 = (HtmlImage)this.FindControl("rightimg");
				if (htmlInputHidden2 != null)
				{
					htmlInputHidden2.Value = this.Page.Request.QueryString["isrecharge"].ToNullString();
				}
				if (htmlImage != null)
				{
					htmlImage.Src = HiContext.Current.GetSkinPath() + "/images/erweima02.gif";
				}
				if (htmlImage2 != null)
				{
					htmlImage2.Src = HiContext.Current.GetSkinPath() + "/images/phone.jpg";
				}
				if (htmlInputHidden != null)
				{
					htmlInputHidden.Value = this.Page.Request.QueryString["orderId"].Trim();
				}
				string orderId = (this.Page.Request["orderId"] == null) ? "" : this.Page.Request["orderId"].ToString();
				if (htmlAnchor2 != null)
				{
					htmlAnchor2.HRef = base.GetRouteUrl("user_OrderDetails", new
					{
						OrderId = orderId
					});
				}
				if (htmlAnchor != null)
				{
					htmlAnchor.HRef = "/User/UserDefault";
				}
				if (image != null)
				{
					image.ImageUrl = "http://www.thonky.com/qr-encoder/encode.php?maxw=270&ec=&s=" + this.Page.Request.QueryString["code_url"];
				}
			}
		}
	}
}
