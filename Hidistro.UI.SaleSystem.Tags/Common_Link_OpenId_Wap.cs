using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Member;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_Link_OpenId_Wap : WebControl
	{
		public string ImageUrl
		{
			get;
			set;
		}

		protected override void Render(HtmlTextWriter writer)
		{
			IList<OpenIdSettingInfo> configedItems = MemberProcessor.GetConfigedItems();
			string text = "";
			text = HttpContext.Current.Request.QueryString["returnUrl"];
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
			string arg = "wapshop";
			if (HttpContext.Current.Request.Path.ToNullString().ToLower().IndexOf("/alioh/") > -1)
			{
				arg = "alioh";
			}
			else if (HttpContext.Current.Request.Path.ToNullString().ToLower().IndexOf("/vshop/") > -1)
			{
				arg = "vshop";
			}
			if (configedItems != null && configedItems.Count > 0)
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (OpenIdSettingInfo item in configedItems)
				{
					if (item.OpenIdType != "hishop.plugins.openid.weixin.weixinservice" && item.OpenIdType.ToLower() != "hishop.plugins.openid.o2owxapplet" && item.OpenIdType != "hishop.plugins.openid.weixin" && item.OpenIdType != "hishop.plugins.openid.wxapplet" && !item.OpenIdType.ToLower().Contains(".app"))
					{
						string imageUrl = this.ImageUrl;
						imageUrl = "/plugins/openid/images/wap/" + item.OpenIdType + ".png";
						if (HiContext.Current.User == null || HiContext.Current.User.UserName == "Anonymous")
						{
							stringBuilder.AppendFormat("<a href=\"/OpenId/RedirectLogin_Wap.aspx?ot={0}&client={2}&returnUrl={1}\">", item.OpenIdType, text, arg);
						}
						else
						{
							stringBuilder.Append("<a href=\"#\">");
						}
						stringBuilder.AppendFormat("<img src=\"{0}\" alt=\"{1}\" width=\"32px\" /> ", imageUrl, item.Name);
						stringBuilder.Append("</a>");
					}
				}
				writer.Write(stringBuilder.ToString());
			}
		}
	}
}
