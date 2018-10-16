using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Commodities;
using Hidistro.UI.Common.Controls;
using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class Desig_Templete : HtmlTemplatedWebControl
	{
		private const string templetestr = "<div id=\"assistdiv\" class=\"assistdiv\"></div><div class=\"edit_div\" id=\"grounddiv\"><div class=\"cover\"></div></div><div class=\"edit_bar\" id=\"groundeidtdiv\"><a href=\"javascript:Hidistro_designer.EditeDesigDialog();\" title=\"编辑\" id=\"a_design_Edit\">编辑</a><a href=\"javascript:Hidistro_designer.MoveUp()\" class=\"up\" id=\"a_design_up\" title=\"上移\">上移</a><a href=\"javascript:Hidistro_designer.MoveDown()\" class=\"down\" title=\"下移\" id=\"a_design_down\">下移</a><a href=\"javascript:void(0);\" id=\"a_design_delete\" title=\"删除\" onclick=\"Hidistro_designer.del_element()\">删除</a><a class=\"controlinfo\" href=\"javascript:void(0);\" onclick=\"Hidistro_designer.gethelpdailog();\" title=\"控件说明\" rel=\"#SetingTempalte\">控件说明</a></div> <div class=\"apple_overlay\" id=\"taboverlaycontent\"></div><div id=\"tempdiv\" style=\"height: 260px; display: none;\"></div><div class=\"design_coverbg\" id=\"design_coverbg\"></div><div class=\"controlnamediv\" id=\"ctrnamediv\">图片控件轮播组件</div><div class=\"movediv\" id=\"parentmove\">上下移动</div><script>Hidistro_designer.Design_Page_Init();</script>";

		protected string skintemp = "";

		protected string tempurl = "";

		protected string viewname = "";

		protected string skinparams = "0";

		protected override void OnInit(EventArgs e)
		{
			ManagerInfo manager = HiContext.Current.Manager;
			if (manager == null)
			{
				this.Page.Response.Redirect(Globals.GetAdminAbsolutePath("login.aspx"), true);
			}
			this.SetDesignSkinName();
			if (this.SkinName == null || this.tempurl == "")
			{
				base.GotoResourceNotFound();
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			Literal literal = (Literal)this.FindControl("litPageName");
			Literal literal2 = (Literal)this.FindControl("litTempete");
			Literal literal3 = (Literal)this.FindControl("litaccount");
			Literal literal4 = (Literal)this.FindControl("litview");
			Literal literal5 = (Literal)this.FindControl("litDefault");
			if (!this.Page.IsPostBack)
			{
				if (literal != null)
				{
					literal.Text = "<script>Hidistro_designer.CurrentPageName='" + this.skintemp + "';Hidistro_designer.CurrentParams=" + this.skinparams + "</script>";
				}
				if (literal2 != null)
				{
					literal2.Text = "<div id=\"assistdiv\" class=\"assistdiv\"></div><div class=\"edit_div\" id=\"grounddiv\"><div class=\"cover\"></div></div><div class=\"edit_bar\" id=\"groundeidtdiv\"><a href=\"javascript:Hidistro_designer.EditeDesigDialog();\" title=\"编辑\" id=\"a_design_Edit\">编辑</a><a href=\"javascript:Hidistro_designer.MoveUp()\" class=\"up\" id=\"a_design_up\" title=\"上移\">上移</a><a href=\"javascript:Hidistro_designer.MoveDown()\" class=\"down\" title=\"下移\" id=\"a_design_down\">下移</a><a href=\"javascript:void(0);\" id=\"a_design_delete\" title=\"删除\" onclick=\"Hidistro_designer.del_element()\">删除</a><a class=\"controlinfo\" href=\"javascript:void(0);\" onclick=\"Hidistro_designer.gethelpdailog();\" title=\"控件说明\" rel=\"#SetingTempalte\">控件说明</a></div> <div class=\"apple_overlay\" id=\"taboverlaycontent\"></div><div id=\"tempdiv\" style=\"height: 260px; display: none;\"></div><div class=\"design_coverbg\" id=\"design_coverbg\"></div><div class=\"controlnamediv\" id=\"ctrnamediv\">图片控件轮播组件</div><div class=\"movediv\" id=\"parentmove\">上下移动</div><script>Hidistro_designer.Design_Page_Init();</script>";
				}
				if (literal3 != null)
				{
					ManagerInfo manager = HiContext.Current.Manager;
					if (manager != null)
					{
						literal3.Text = "<a>我的账号：" + manager.UserName + "</a>";
					}
				}
				if (literal5 != null)
				{
					literal5.Text = "<a href=\"/\">查看店铺</a>";
				}
				if (literal4 != null)
				{
					string str = "/";
					literal4.Text = "<a href=\"" + str + "\" target=\"_blank\" class=\"button\">预览</a>";
				}
			}
		}

		protected void SetDesignSkinName()
		{
			if (string.IsNullOrEmpty(this.Page.Request.QueryString["skintemp"]))
			{
				base.GotoResourceNotFound();
			}
			this.skintemp = this.Page.Request.QueryString["skintemp"];
			switch (this.skintemp)
			{
			case "default":
				this.SkinName = "Skin-Desig_Templete.html";
				this.tempurl = Globals.PhysicalPath(HiContext.Current.GetPCHomePageSkinPath() + "/Skin-Default.html");
				break;
			case "login":
				this.SkinName = "Skin-Desig_login.html";
				this.tempurl = Globals.PhysicalPath(HiContext.Current.GetSkinPath() + "/Skin-Login.html");
				break;
			case "brand":
				this.SkinName = "Skin-Desig_Brand.html";
				this.tempurl = Globals.PhysicalPath(HiContext.Current.GetSkinPath() + "/Skin-Brand.html");
				break;
			case "branddetail":
				this.SkinName = "Skin-Desig_BrandDetails.html";
				this.tempurl = Globals.PhysicalPath(HiContext.Current.GetSkinPath() + "/Skin-BrandDetails.html");
				break;
			case "product":
				this.SkinName = "Skin-Desig_SubCategory.html";
				this.tempurl = Globals.PhysicalPath(HiContext.Current.GetSkinPath() + "/Skin-SubCategory.html");
				break;
			case "productdetail":
				this.SkinName = "Skin-Desig_ProductDetails.html";
				this.tempurl = Globals.PhysicalPath(HiContext.Current.GetSkinPath() + "/Skin-ProductDetails.html");
				break;
			case "article":
				this.SkinName = "Skin-Desig_Articles.html";
				this.tempurl = Globals.PhysicalPath(HiContext.Current.GetSkinPath() + "/Skin-Articles.html");
				break;
			case "articledetail":
				this.SkinName = "Skin-Desig_ArticleDetails.html";
				this.tempurl = Globals.PhysicalPath(HiContext.Current.GetSkinPath() + "/Skin-ArticleDetails.html");
				break;
			case "cuountdown":
				this.SkinName = "Skin-Desig_CountDownProducts.html";
				this.tempurl = Globals.PhysicalPath(HiContext.Current.GetSkinPath() + "/Skin-CountDownProducts.html");
				break;
			case "cuountdowndetail":
				this.SkinName = "Skin-Desig_CountDownProductsDetails.html";
				this.tempurl = Globals.PhysicalPath(HiContext.Current.GetSkinPath() + "/Skin-CountDownProductsDetails.html");
				break;
			case "groupbuy":
				this.SkinName = "Skin-Desig_GroupBuyProducts.html";
				this.tempurl = Globals.PhysicalPath(HiContext.Current.GetSkinPath() + "/Skin-GroupBuyProducts.html");
				break;
			case "groupbuydetail":
				this.SkinName = "Skin-Desig_GroupBuyProductDetails.html";
				this.tempurl = Globals.PhysicalPath(HiContext.Current.GetSkinPath() + "/Skin-GroupBuyProductDetails.html");
				break;
			case "help":
				this.SkinName = "Skin-Desig_Helps.html";
				this.tempurl = Globals.PhysicalPath(HiContext.Current.GetSkinPath() + "/Skin-Helps.html");
				break;
			case "helpdetail":
				this.SkinName = "Skin-Desig_HelpDetails.html";
				this.tempurl = Globals.PhysicalPath(HiContext.Current.GetSkinPath() + "/Skin-HelpDetails.html");
				break;
			case "gift":
				this.SkinName = "Skin-Desig_OnlineGifts.html";
				this.tempurl = Globals.PhysicalPath(HiContext.Current.GetSkinPath() + "/Skin-OnlineGifts.html");
				break;
			case "giftdetail":
				this.SkinName = "Skin-Desig_GiftDetails.html";
				this.tempurl = Globals.PhysicalPath(HiContext.Current.GetSkinPath() + "/Skin-GiftDetails.html");
				break;
			case "shopcart":
				this.SkinName = "Skin-Desig_ShoppingCart.html";
				this.tempurl = Globals.PhysicalPath(HiContext.Current.GetSkinPath() + "/Skin-ShoppingCart.html");
				break;
			case "categorycustom":
			{
				int categoryId = 0;
				int.TryParse(this.Page.Request.QueryString["cid"], out categoryId);
				CategoryInfo category = CatalogHelper.GetCategory(categoryId);
				this.skinparams = categoryId.ToString();
				this.SkinName = "Skin-Desig_Custom.html";
				this.tempurl = Globals.PhysicalPath(HiContext.Current.GetPCHomePageSkinPath() + "/categorythemes/" + category.Theme);
				break;
			}
			case "brandcustom":
			{
				this.SkinName = "Skin-Desig_Custom.html";
				int brandId = 0;
				int.TryParse(this.Page.Request.QueryString["brandId"], out brandId);
				this.skinparams = brandId.ToString();
				BrandCategoryInfo brandCategory = CatalogHelper.GetBrandCategory(brandId);
				this.tempurl = Globals.PhysicalPath(HiContext.Current.GetPCHomePageSkinPath() + "/brandcategorythemes/" + brandCategory.Theme);
				break;
			}
			case "customthemes":
			{
				this.SkinName = "Skin-Desig_Custom.html";
				int tid = 0;
				int.TryParse(this.Page.Request.QueryString["tid"], out tid);
				this.skinparams = tid.ToString();
				this.tempurl = Globals.PhysicalPath(HiContext.Current.GetPCHomePageSkinPath() + "/customthemes/" + this.GetCustomSkinName(tid));
				break;
			}
			default:
				this.SkinName = null;
				break;
			}
		}

		private string GetCustomSkinName(int tid)
		{
			string filename = HttpContext.Current.Request.MapPath(HiContext.Current.GetPCHomePageSkinPath() + "/" + HiContext.Current.SiteSettings.Theme + ".xml");
			XmlDocument xmlDocument = null;
			xmlDocument = new XmlDocument();
			xmlDocument.Load(filename);
			return xmlDocument.SelectSingleNode("//CustomTheme/Theme[@Tid=" + tid + "]").Attributes["SkinName"].Value;
		}
	}
}
