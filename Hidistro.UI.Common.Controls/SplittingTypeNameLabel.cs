using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class SplittingTypeNameLabel : Label
	{
		public string SplittingType
		{
			get;
			set;
		}

		protected override void Render(HtmlTextWriter writer)
		{
			switch (this.SplittingType)
			{
			case "1":
				base.Text = "注册分销奖励";
				break;
			case "2":
				base.Text = "直接下级奖励";
				break;
			case "3":
				base.Text = "下二级奖励";
				break;
			case "4":
				base.Text = "下三级奖励";
				break;
			case "5":
				base.Text = "提现";
				break;
			default:
				base.Text = "其他";
				break;
			}
			base.Render(writer);
		}
	}
}
