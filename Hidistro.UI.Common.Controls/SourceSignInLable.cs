using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class SourceSignInLable : Literal
	{
		public object SourceSignIn
		{
			get
			{
				return this.ViewState["SignInSouce"];
			}
			set
			{
				this.ViewState["SignInSouce"] = value;
			}
		}

		protected override void Render(HtmlTextWriter writer)
		{
			base.Text = this.GetSignInSource((this.SourceSignIn != DBNull.Value) ? ((int)this.SourceSignIn) : 0);
			base.Render(writer);
		}

		private string GetSignInSource(int sourceSignIn)
		{
			string result = "";
			switch (sourceSignIn)
			{
			case 1:
				result = "";
				break;
			case 4:
				result = "<img src=\"" + HttpContext.Current.Request.ApplicationPath + "/Utility/pics/tao.gif\" title=\"生活号签到\"/>";
				break;
			case 3:
				result = "<img src=\"" + HttpContext.Current.Request.ApplicationPath + "/Utility/pics/wx.gif\"  title=\"微信签到\"/>";
				break;
			case 2:
				result = "<img src=\"" + HttpContext.Current.Request.ApplicationPath + "/Utility/pics/wap.gif\" title=\"触屏版签到\"/>";
				break;
			case 5:
				result = "<img src=\"" + HttpContext.Current.Request.ApplicationPath + "/Utility/pics/androi.gif\" title=\"APP签到\"/>";
				break;
			}
			return result;
		}
	}
}
