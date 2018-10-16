using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class SourceOrderLable : Literal
	{
		public object SourceOrder
		{
			get
			{
				return this.ViewState["SourceOrder"];
			}
			set
			{
				this.ViewState["SourceOrder"] = value;
			}
		}

		protected override void Render(HtmlTextWriter writer)
		{
			base.Text = this.GetOrderSource((int)this.SourceOrder);
			base.Render(writer);
		}

		private string GetOrderSource(int sourceorder)
		{
			string result = "";
			switch (sourceorder)
			{
			case 2:
				result = "<i class=\"iconfont\"   data-toggle=\"tooltip\" data-placement=\"top\" data-original-title=\"淘宝订单\">&#xe613;</i>";
				break;
			case 3:
				result = "<i class=\"iconfont\"    data-toggle=\"tooltip\" data-placement=\"top\" data-original-title=\"微信订单\">&#xe614;</i>";
				break;
			case 4:
				result = "<i class=\"iconfont\"  data-toggle=\"tooltip\" data-placement=\"top\" data-original-title=\"WAP订单\">&#xe605;</i>";
				break;
			case 5:
				result = "<i class=\"iconfont\"   data-toggle=\"tooltip\" data-placement=\"top\" data-original-title=\"生活号（原支付宝服务窗）\">&#xe60a;</i>";
				break;
			case 6:
				result = "<i class=\"iconfont\"  data-toggle=\"tooltip\" data-placement=\"top\" data-original-title=\"APP订单\">&#xe600;</i>";
				break;
			case 7:
				result = "<i class=\"iconfont\"   data-toggle=\"tooltip\" data-placement=\"top\" data-original-title=\"京东订单\">&#xe604;</i>";
				break;
			case 8:
				result = "<i class=\"iconfont\"    data-toggle=\"tooltip\" data-placement=\"top\" data-original-title=\"微信小程序订单\">&#xe614;</i>";
				break;
			}
			return result;
		}
	}
}
