using Hidistro.Context;
using Hidistro.Entities.Members;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_CurrentUser : Literal
	{
		private string dafaultText = "您好，欢迎光临" + SettingsManager.GetMasterSettings().SiteName;

		public string DafaultText
		{
			get
			{
				return this.dafaultText;
			}
			set
			{
				this.dafaultText = value;
			}
		}

		protected override void Render(HtmlTextWriter writer)
		{
			MemberInfo user = HiContext.Current.User;
			if (user.UserId != 0)
			{
				if (string.IsNullOrWhiteSpace(user.TradePassword))
				{
					string str = "&nbsp;&nbsp;&nbsp;&nbsp;为了您的账号安全，请设置交易密码，<a href='/User/OpenBalance.aspx' style='color:#0099FF'>去设置</a>";
					base.Text = "您好，" + user.UserName + str;
				}
				else
				{
					base.Text = "您好，" + user.UserName;
				}
			}
			else
			{
				base.Text = this.DafaultText;
			}
			base.Render(writer);
		}
	}
}
