using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class SystemPageDropDownList : DropDownList
	{
		public override void DataBind()
		{
			this.Items.Clear();
			this.Items.Add(new ListItem("--请选择--", ""));
			this.Items.Add(new ListItem("下架区", "/ProductUnSales"));
			this.Items.Add(new ListItem("帮助中心", "/Helps"));
			this.Items.Add(new ListItem("公告列表", "/Affiches"));
			this.Items.Add(new ListItem("积分商城", "/OnlineGifts"));
			this.Items.Add(new ListItem("商城资讯", "/Articles"));
			this.Items.Add(new ListItem("限时抢购", "/CountDownProducts"));
			this.Items.Add(new ListItem("团购", "/GroupBuyProducts"));
			this.Items.Add(new ListItem("品牌专卖", "/brand"));
		}
	}
}
