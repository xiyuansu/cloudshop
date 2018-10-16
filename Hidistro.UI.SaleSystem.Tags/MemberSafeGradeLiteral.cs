using Hidistro.Context;
using Hidistro.Entities.Members;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class MemberSafeGradeLiteral : Literal
	{
		private string _cssclass = "panquandu";

		public string CssClass
		{
			get
			{
				return this._cssclass;
			}
			set
			{
				this._cssclass = value;
			}
		}

		protected override void Render(HtmlTextWriter writer)
		{
			MemberInfo user = HiContext.Current.User;
			int num = 0;
			if (user.EmailVerification)
			{
				num++;
			}
			if (user.CellPhoneVerification)
			{
				num++;
			}
			if (!string.IsNullOrEmpty(user.PasswordQuestion))
			{
				num++;
			}
			if (num <= 1)
			{
				base.Text = "<div class=\"" + this._cssclass + "A\"></div>\u3000<span class=\"hongse\">低</span>";
			}
			else if (num <= 2)
			{
				base.Text = "<div class=\"" + this._cssclass + "B\"></div>\u3000<span class=\"huangse\">中</span>";
			}
			else
			{
				base.Text = string.Format("<div class=\"" + this._cssclass + "{1}\"></div>\u3000<span class=\"green\">{0}</span>", (!string.IsNullOrWhiteSpace(user.TradePassword)) ? "高" : "中", (!string.IsNullOrWhiteSpace(user.TradePassword)) ? "C" : "B");
			}
			base.Render(writer);
		}
	}
}
