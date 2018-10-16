using Hidistro.Context;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Member;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_Link_OpenId : WebControl
	{
		public string ImageUrl
		{
			get;
			set;
		}

		protected override void Render(HtmlTextWriter writer)
		{
			IList<OpenIdSettingInfo> configedItems = MemberProcessor.GetConfigedItems();
			if (!string.IsNullOrEmpty(this.ImageUrl))
			{
				if (this.ImageUrl.StartsWith("~"))
				{
					this.ImageUrl = base.ResolveUrl(this.ImageUrl);
				}
				else if (this.ImageUrl.StartsWith("/"))
				{
					this.ImageUrl = HiContext.Current.GetSkinPath() + this.ImageUrl;
				}
				else
				{
					this.ImageUrl = HiContext.Current.GetSkinPath() + "/" + this.ImageUrl;
				}
			}
			if (configedItems != null && configedItems.Count > 0)
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (OpenIdSettingInfo item in configedItems)
				{
					if (!(item.OpenIdType.ToLower() == "hishop.plugins.openid.weixin") && !(item.OpenIdType.ToLower() == "hishop.plugins.openid.wxapplet") && !(item.OpenIdType.ToLower() == "hishop.plugins.openid.o2owxapplet") && !item.OpenIdType.ToLower().Contains(".app"))
					{
						string imageUrl = this.ImageUrl;
						imageUrl = "/plugins/openid/images/" + item.OpenIdType + "2.gif";
						if (HiContext.Current.User == null || HiContext.Current.User.UserName == "Anonymous")
						{
							stringBuilder.AppendFormat("<a href=\"/OpenId/RedirectLogin.aspx?ot={0}\">", item.OpenIdType);
						}
						else
						{
							stringBuilder.Append("<a href=\"#\">");
						}
						stringBuilder.AppendFormat("<img src=\"{0}\" alt=\"{1}\" /> ", imageUrl, item.Name);
						stringBuilder.Append("</a>");
					}
				}
				writer.Write(stringBuilder.ToString());
			}
		}
	}
}
