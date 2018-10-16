using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Commodities;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.ProductCategory)]
	public class SetCategoryTemplate : AdminPage
	{
		protected FileUpload fileThame;

		protected Button btnUpload;

		protected DropDownList dropThmes;

		protected ImageLinkButton btnDelete;

		protected Repeater rptCategries;

		protected Button btnSaveAll;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.btnUpload.Click += this.btnUpload_Click;
			this.btnDelete.Click += this.btnDelete_Click;
			this.btnSaveAll.Click += this.btnSaveAll_Click;
			if (!this.Page.IsPostBack)
			{
				this.BindDorpDown(this.dropThmes, null, "");
				this.BindData();
			}
		}

		private void btnUpload_Click(object sender, EventArgs e)
		{
			if (this.fileThame.HasFile)
			{
				if (!this.fileThame.PostedFile.FileName.EndsWith(".htm") && !this.fileThame.PostedFile.FileName.EndsWith(".html"))
				{
					this.ShowMsg("请检查您上传文件的格式是否为html或htm", false);
				}
				else
				{
					string virtualPath = HiContext.Current.GetSkinPath() + "/categorythemes/" + SetCategoryTemplate.GetFilename(Path.GetFileName(this.fileThame.PostedFile.FileName), Path.GetExtension(this.fileThame.PostedFile.FileName));
					this.fileThame.PostedFile.SaveAs(HiContext.Current.Context.Request.MapPath(virtualPath));
					this.BindDorpDown(this.dropThmes, null, "");
					this.BindData();
					this.ShowMsg("上传成功", true);
				}
			}
			else
			{
				this.ShowMsg("上传失败！", false);
			}
		}

		private static string GetFilename(string filename, string extension)
		{
			return filename.Substring(0, filename.IndexOf(".")) + extension;
		}

		private void btnDelete_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(this.dropThmes.SelectedValue))
			{
				this.ShowMsg("请选择您要删除的模板", false);
			}
			else if (!this.validata(this.dropThmes.SelectedItem.Text))
			{
				this.ShowMsg("您要删除的模板正在被使用，不能删除", false);
			}
			else
			{
				string virtualPath = HiContext.Current.GetSkinPath() + "/categorythemes/" + this.dropThmes.SelectedValue;
				virtualPath = HiContext.Current.Context.Request.MapPath(virtualPath);
				if (!File.Exists(virtualPath))
				{
					this.ShowMsg($"删除失败!模板{this.dropThmes.SelectedValue}已经不存在", false);
				}
				else
				{
					File.Delete(virtualPath);
					this.BindDorpDown(this.dropThmes, null, "");
					this.BindData();
					this.ShowMsg("删除模板成功", true);
				}
			}
		}

		private void btnSaveAll_Click(object sender, EventArgs e)
		{
			this.SaveAll();
			this.BindDorpDown(this.dropThmes, null, "");
			this.BindData();
			this.ShowMsg("批量保存分类模板成功", true);
		}

		private bool validata(string theme)
		{
			foreach (RepeaterItem item in this.rptCategries.Items)
			{
				string selectedValue = (item.FindControl("dropTheme") as DropDownList).SelectedValue;
				if (selectedValue == theme)
				{
					return false;
				}
			}
			return true;
		}

		private void SaveAll()
		{
			foreach (RepeaterItem item in this.rptCategries.Items)
			{
				string selectedValue = (item.FindControl("dropTheme") as DropDownList).SelectedValue;
				int categoryId = (item.FindControl("hidCategoryId") as TextBox).Text.ToInt(0);
				CatalogHelper.SetCategoryThemes(categoryId, selectedValue);
			}
		}

		private void BindDorpDown(DropDownList box, ListItem firstItem = null, string selectvalue = "")
		{
			if (firstItem == null)
			{
				firstItem = new ListItem("请选择模板文件", "");
			}
			box.Items.Clear();
			box.Items.Add(firstItem);
			IList<ManageThemeInfo> themes = this.GetThemes();
			if (themes != null && themes.Count > 0)
			{
				foreach (ManageThemeInfo item in themes)
				{
					ListItem listItem = new ListItem(item.Name, item.ThemeName);
					if (!string.IsNullOrWhiteSpace(selectvalue) && selectvalue.Equals(listItem.Value))
					{
						listItem.Selected = true;
					}
					box.Items.Add(listItem);
				}
			}
		}

		private void BindData()
		{
			IEnumerable<CategoryInfo> mainCategories = CatalogHelper.GetMainCategories();
			this.rptCategries.DataSource = mainCategories;
			this.rptCategries.DataBind();
		}

		protected IList<ManageThemeInfo> GetThemes()
		{
			HttpContext context = HiContext.Current.Context;
			IList<ManageThemeInfo> list = new List<ManageThemeInfo>();
			string path = context.Request.MapPath(HiContext.Current.GetSkinPath() + "/categorythemes");
			string[] array = Directory.Exists(path) ? Directory.GetFiles(path) : null;
			ManageThemeInfo manageThemeInfo = null;
			if (array != null)
			{
				string[] array2 = array;
				foreach (string text in array2)
				{
					if (text.EndsWith(".html"))
					{
						manageThemeInfo = new ManageThemeInfo();
						ManageThemeInfo manageThemeInfo2 = manageThemeInfo;
						ManageThemeInfo manageThemeInfo3 = manageThemeInfo;
						string text3 = manageThemeInfo2.ThemeName = (manageThemeInfo3.Name = Path.GetFileName(text));
						list.Add(manageThemeInfo);
					}
				}
			}
			return list;
		}

		protected void rptCategries_ItemCommand(object source, RepeaterCommandEventArgs e)
		{
			if (e.CommandName == "Save")
			{
				TextBox textBox = e.Item.FindControl("hidCategoryId") as TextBox;
				int categoryId = textBox.Text.ToInt(0);
				DropDownList dropDownList = (DropDownList)e.Item.FindControl("dropTheme");
				string selectedValue = dropDownList.SelectedValue;
				if (CatalogHelper.SetCategoryThemes(categoryId, selectedValue))
				{
					this.BindData();
					this.ShowMsg("保存分类模板成功", true);
				}
			}
		}

		protected void rptCategries_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				CategoryInfo categoryInfo = (CategoryInfo)e.Item.DataItem;
				DropDownList box = (DropDownList)e.Item.FindControl("dropTheme");
				this.BindDorpDown(box, new ListItem("无", ""), categoryInfo.Theme);
			}
		}
	}
}
