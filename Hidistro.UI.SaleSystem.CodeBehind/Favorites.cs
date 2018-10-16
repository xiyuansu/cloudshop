using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.SaleSystem.Catalog;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class Favorites : MemberTemplatedWebControl
	{
		private TextBox txtKeyWord;

		private ThemedTemplatedRepeater favorites;

		private ThemedTemplatedRepeater favoritesTags;

		private Pager pager;

		private Button btnSearch;

		private LinkButton btnDeleteSelect;

		private string tagname = string.Empty;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-Favorites.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.favorites = (ThemedTemplatedRepeater)this.FindControl("rptFavorites");
			this.favoritesTags = (ThemedTemplatedRepeater)this.FindControl("rptFavoritesTags");
			this.btnSearch = (Button)this.FindControl("imgbtnSearch");
			this.txtKeyWord = (TextBox)this.FindControl("txtKeyWord");
			this.pager = (Pager)this.FindControl("pager");
			this.btnDeleteSelect = (LinkButton)this.FindControl("btnDeleteSelect");
			this.btnSearch.Click += this.btnSearch_Click;
			this.btnDeleteSelect.Click += this.btnDeleteSelect_Click;
			PageTitle.AddSiteNameTitle("商品收藏夹");
			if (!this.Page.IsPostBack)
			{
				this.BindList();
				this.BindFavoritesTags();
			}
		}

		private void BindFavoritesTags()
		{
			this.favoritesTags.DataSource = ProductBrowser.GetFavoritesTypeTags();
			this.favoritesTags.DataBind();
		}

		protected void btnDeleteSelect_Click(object sender, EventArgs e)
		{
			string ids = this.Page.Request["CheckboxGroup"];
			if (!ProductBrowser.DeleteFavorites(ids))
			{
				this.ShowMessage("删除失败", false, "", 1);
			}
			this.ReloadFavorites();
		}

		protected void btnSearch_Click(object sender, EventArgs e)
		{
			this.ReloadFavorites();
		}

		private void BindList()
		{
			Pagination pagination = new Pagination();
			pagination.PageIndex = this.pager.PageIndex;
			pagination.PageSize = this.pager.PageSize;
			string text = string.Empty;
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["keyword"]))
			{
				text = Globals.HtmlDecode(this.Page.Request.QueryString["keyword"]);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["tags"]))
			{
				this.tagname = Globals.HtmlDecode(this.Page.Request.QueryString["tags"]);
			}
			DbQueryResult dbQueryResult = ProductBrowser.GetFavorites(text, this.tagname, pagination, true);
			this.favorites.DataSource = dbQueryResult.Data;
			this.favorites.DataBind();
			this.txtKeyWord.Text = text;
			this.pager.TotalRecords = dbQueryResult.TotalRecords;
		}

		private void ReloadFavorites()
		{
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection.Add("keyword", Globals.HtmlEncode(this.txtKeyWord.Text.Trim()));
			nameValueCollection.Add("tags", Globals.HtmlEncode(this.tagname));
			base.ReloadPage(nameValueCollection);
		}
	}
}
