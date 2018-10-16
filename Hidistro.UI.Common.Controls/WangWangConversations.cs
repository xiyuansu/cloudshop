using Hidistro.Core;
using System.Web.UI;

namespace Hidistro.UI.Common.Controls
{
	public class WangWangConversations : Control
	{
		public string WangWangAccounts
		{
			get
			{
				if (this.ViewState["wangWangAccounts"] == null)
				{
					return null;
				}
				return (string)this.ViewState["wangWangAccounts"];
			}
			set
			{
				this.ViewState["wangWangAccounts"] = Globals.UrlEncode(value);
			}
		}

		protected override void Render(HtmlTextWriter writer)
		{
			if (!string.IsNullOrEmpty(this.WangWangAccounts))
			{
				writer.WriteLine(string.Format("<a target=\"_blank\" href=\"http://www.taobao.com/webww/ww.php?ver=3&touid={0}&siteid=cntaobao&status=1&charset=utf-8\" ><img border=\"0\" src=\"http://amos.alicdn.com/realonline.aw?v=2&uid={0}&site=cntaobao&s=1&charset=utf-8\" alt=\"点击这里给我发消息\" /></a>", this.WangWangAccounts));
			}
		}
	}
}
