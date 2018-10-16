using Hidistro.Core;
using Hidistro.UI.SaleSystem.CodeBehind;
using System.Web;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.Web.pay
{
	public class WapInpourReturn : WapInpourPage
	{
		protected HtmlAnchor areturn;

		public WapInpourReturn()
			: base(false)
		{
		}

		protected override void DisplayMessage(string status)
		{
			HttpCookie httpCookie = HttpContext.Current.Request.Cookies["clienttype"];
			if (httpCookie != null)
			{
				switch (httpCookie.Value.ToInt(0))
				{
				case 2:
					this.areturn.HRef = "/wapshop/MyAccountSummary.aspx";
					break;
				case 3:
					this.areturn.HRef = "/appshop/MyAccountSummary.aspx";
					break;
				case 4:
					this.areturn.HRef = "/alioh/MyAccountSummary.aspx";
					break;
				case 1:
					this.areturn.HRef = "/vshop/MyAccountSummary.aspx";
					break;
				}
			}
			string empty = string.Empty;
			switch (status)
			{
			case "gatewaynotfound":
				empty = "没有找到与此次充值对应的支付方式，系统无法自动完成操作，请联系管理员";
				break;
			case "waitconfirm":
				empty = "您使用的是担保交易付款，在您确认收货以后系统就会为您的预付款账户充入相应的金额";
				break;
			case "success":
				empty = string.Format("恭喜您，此次预付款充值已成功完成，本次充值金额：{0}", base.Amount.F2ToString("f2"));
				break;
			case "success2":
				empty = string.Format("恭喜您，此次预付款充值已成功完成!");
				break;
			case "fail":
				empty = string.Format("您已成功支付，但是系统在处理过程中遇到问题，请联系管理员</br>支付金额：{0}", base.Amount.F2ToString("f2"));
				break;
			case "verifyfaild":
				empty = "支付返回验证失败，操作已停止";
				break;
			default:
				empty = "未知错误，操作已停止";
				break;
			}
			base.Response.Write("<p style=\"font-size:16px;\">" + empty + "</p>");
		}
	}
}
