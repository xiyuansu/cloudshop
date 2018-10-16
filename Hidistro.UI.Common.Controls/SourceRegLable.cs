using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class SourceRegLable : Literal
	{
		public object SourceReg
		{
			get
			{
				return this.ViewState["RegSouce"];
			}
			set
			{
				this.ViewState["RegSouce"] = value;
			}
		}

		protected override void Render(HtmlTextWriter writer)
		{
			base.Text = this.GetOrderSource((this.SourceReg != DBNull.Value) ? ((int)this.SourceReg) : 0);
			base.Render(writer);
		}

		private string GetOrderSource(int sourceorder)
		{
			string result = "";
			switch (sourceorder)
			{
			case 1:
				result = "";
				break;
			case 4:
				result = "<i class=\"iconfont\" title=\"生活号（原支付宝服务窗）\" stlye=\"cursor:pointer;\">&#xe60a;</i>";
				break;
			case 3:
				result = "<i class=\"iconfont\" title=\"微信注册\" stlye=\"cursor:pointer;\">&#xe614;</i>";
				break;
			case 2:
				result = "<i class=\"iconfont\" title=\"WAP注册\" stlye=\"cursor:pointer;\">&#xe605;</i>";
				break;
			case 5:
				result = "<i class=\"iconfont\" title=\"APP注册\" stlye=\"cursor:pointer;\">&#xe600;</i>";
				break;
			}
			return result;
		}
	}
}
