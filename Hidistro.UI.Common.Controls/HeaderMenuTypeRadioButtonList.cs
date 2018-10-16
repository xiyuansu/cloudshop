using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class HeaderMenuTypeRadioButtonList : RadioButtonList
	{
		protected override void Render(HtmlTextWriter writer)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("<input id=\"radHeaderMenuType_1\" type=\"radio\" name=\"radHeaderMenuType\" value=\"{0}\" class=\"icheck\" /><label>系统页面</label>", 1);
			stringBuilder.AppendFormat("<input id=\"radHeaderMenuType_2\" type=\"radio\" name=\"radHeaderMenuType\" value=\"{0}\" class=\"icheck\" /><label>商品搜索链接</label>", 2);
			stringBuilder.AppendFormat("<input id=\"radHeaderMenuType_3\" type=\"radio\" name=\"radHeaderMenuType\" value=\"{0}\" class=\"icheck\" /><label>自定义链接</label>", 3);
			writer.Write(stringBuilder.ToString());
		}
	}
}
