using Hidistro.Context;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Member;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_LoginLink : Literal
	{
		public string ImageUrl
		{
			get;
			set;
		}

		protected override void Render(HtmlTextWriter writer)
		{
			string text = "";
			if (HiContext.Current.UserId != 0)
			{
				text = string.Format("<div class=\"dt cw-icon\"><input type=\"hidden\" id=\"hidIsLogin\" value=\"1\"/><em><a href=\"{0}\">个人中心</a><i><img src=\"/templates/master/{1}/images/jiantou_03.png\"></i></em></div><div class=\"dorpdown-layer\"><a href=\"{2}\">已买商品</a><a href=\"{3}\">我的收藏</a></div>", "/User/UserDefault", SettingsManager.GetMasterSettings().Theme, "/User/UserOrders", "/User/Favorites");
				writer.Write(text);
			}
			else
			{
				string text2 = string.Format("<a href=\"{0}\">登录</a><input type=\"hidden\" id=\"hidIsLogin\" value=\"0\"/>", "/User/login?ReturnUrl=" + HttpContext.Current.Request.RawUrl);
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
				if (configedItems != null && configedItems.Select(delegate(OpenIdSettingInfo c)
				{
					if (!(c.OpenIdType.ToLower() == "hishop.plugins.openid.weixin.weixinservice"))
					{
						return c.OpenIdType.ToLower() == "hishop.plugins.openid.qq.qqservice";
					}
					return true;
				}).Count() > 0)
				{
					StringBuilder stringBuilder = new StringBuilder();
					foreach (OpenIdSettingInfo item in configedItems)
					{
						if (!(item.OpenIdType.ToLower() != "hishop.plugins.openid.qq.qqservice") || !(item.OpenIdType.ToLower() != "hishop.plugins.openid.weixin.weixinservice"))
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
					text2 += stringBuilder.ToString();
				}
				writer.Write(text2);
			}
			base.Render(writer);
		}
	}
}
