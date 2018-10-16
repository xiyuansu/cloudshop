using Hidistro.Entities;
using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.AliOH
{
	public class EditMenu : AdminPage
	{
		protected TextBox txtMenuName;

		protected HtmlGenericControl liParent;

		protected Literal lblParent;

		protected HtmlGenericControl liBind;

		protected DropDownList ddlType;

		protected HtmlGenericControl liValue;

		protected HtmlGenericControl liTitle;

		protected DropDownList ddlValue;

		protected HtmlGenericControl liUrl;

		protected TextBox txtUrl;

		protected Button btnAddMenu;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.btnAddMenu.Click += this.btnAddMenu_Click;
			if (!this.Page.IsPostBack)
			{
				this.liValue.Visible = false;
				this.liUrl.Visible = false;
				int urlIntParam = base.GetUrlIntParam("MenuId");
				MenuInfo menu = VShopHelper.GetMenu(urlIntParam);
				this.txtMenuName.Text = menu.Name;
				if (menu.ParentMenuId == 0)
				{
					IList<MenuInfo> menusByParentId = VShopHelper.GetMenusByParentId(urlIntParam, ClientType.AliOH);
					if (menusByParentId.Count > 0)
					{
						this.liBind.Visible = false;
					}
					this.liParent.Visible = false;
				}
				else
				{
					this.lblParent.Text = VShopHelper.GetMenu(menu.ParentMenuId).Name;
				}
				this.ddlType.SelectedValue = Convert.ToString((int)menu.BindType);
				switch (menu.BindType)
				{
				case BindType.Url:
					this.liUrl.Visible = true;
					this.liValue.Visible = false;
					break;
				case BindType.Key:
				case BindType.Topic:
					this.liUrl.Visible = false;
					this.liValue.Visible = true;
					break;
				default:
					this.liUrl.Visible = false;
					this.liValue.Visible = false;
					break;
				}
				switch (menu.BindType)
				{
				case BindType.Key:
					this.ddlValue.DataSource = from a in ReplyHelper.GetAllReply()
					where !string.IsNullOrWhiteSpace(a.Keys)
					select a;
					this.ddlValue.DataTextField = "Keys";
					this.ddlValue.DataValueField = "Id";
					this.ddlValue.DataBind();
					this.ddlValue.SelectedValue = menu.ReplyId.ToString();
					break;
				case BindType.Topic:
					this.ddlValue.DataSource = VShopHelper.Gettopics();
					this.ddlValue.DataTextField = "Title";
					this.ddlValue.DataValueField = "TopicId";
					this.ddlValue.DataBind();
					this.ddlValue.SelectedValue = menu.Content;
					break;
				case BindType.Url:
					this.txtUrl.Text = menu.Content;
					break;
				}
			}
		}

		private void btnAddMenu_Click(object sender, EventArgs e)
		{
			int urlIntParam = base.GetUrlIntParam("MenuId");
			MenuInfo menu = VShopHelper.GetMenu(urlIntParam);
			menu.Name = this.txtMenuName.Text;
			menu.Client = ClientType.AliOH;
			menu.Type = "link";
			if (menu.ParentMenuId == 0)
			{
				menu.Type = "link";
			}
			else if (string.IsNullOrEmpty(this.ddlType.SelectedValue) || this.ddlType.SelectedValue == "0")
			{
				this.ShowMsg("二级菜单必须绑定一个对象", false);
				return;
			}
			menu.Bind = Convert.ToInt32(this.ddlType.SelectedValue);
			switch (menu.BindType)
			{
			case BindType.Url:
				menu.Content = this.txtUrl.Text.Trim();
				break;
			case BindType.Key:
				menu.ReplyId = Convert.ToInt32(this.ddlValue.SelectedValue);
				break;
			case BindType.Topic:
				menu.Content = this.ddlValue.SelectedValue;
				break;
			default:
				menu.Content = "";
				break;
			}
			if (VShopHelper.UpdateMenu(menu))
			{
				base.Response.Redirect("ManageMenu.aspx");
			}
			else
			{
				this.ShowMsg("添加失败", false);
			}
		}

		protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
		{
			BindType bindType = (BindType)Convert.ToInt32(this.ddlType.SelectedValue);
			switch (bindType)
			{
			case BindType.Url:
				this.liUrl.Visible = true;
				this.liValue.Visible = false;
				break;
			case BindType.Key:
			case BindType.Topic:
				this.liUrl.Visible = false;
				this.liValue.Visible = true;
				break;
			default:
				this.liUrl.Visible = false;
				this.liValue.Visible = false;
				break;
			}
			switch (bindType)
			{
			case BindType.Key:
				this.liTitle.InnerText = "选择关键字：";
				this.ddlValue.DataSource = from a in ReplyHelper.GetAllReply()
				where !string.IsNullOrWhiteSpace(a.Keys)
				select a;
				this.ddlValue.DataTextField = "Keys";
				this.ddlValue.DataValueField = "Id";
				this.ddlValue.DataBind();
				break;
			case BindType.Topic:
				this.liTitle.InnerText = "选择专题：";
				this.ddlValue.DataSource = VShopHelper.Gettopics();
				this.ddlValue.DataTextField = "Title";
				this.ddlValue.DataValueField = "TopicId";
				this.ddlValue.DataBind();
				break;
			}
		}
	}
}
