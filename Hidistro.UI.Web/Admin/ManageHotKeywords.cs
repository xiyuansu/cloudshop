using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.HotKeywords)]
	public class ManageHotKeywords : AdminPage
	{
		protected HtmlInputHidden txtHid;

		protected ProductCategoriesDropDownList dropCategory;

		protected TextBox txtHotKeywords;

		protected Button btnSubmitHotkeyword;

		protected Button btnEditHotkeyword;

		protected ProductCategoriesDropDownList dropEditCategory;

		protected TextBox txtEditHotKeyword;

		protected HtmlInputHidden hiHotKeyword;

		protected HtmlInputHidden hicategory;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.btnSubmitHotkeyword.Click += this.btnSubmitHotkeyword_Click;
			this.btnEditHotkeyword.Click += this.btnEditHotkeyword_Click;
			if (!this.Page.IsPostBack)
			{
				this.dropCategory.DataBind();
				this.dropEditCategory.DataBind();
			}
		}

		private void btnEditHotkeyword_Click(object sender, EventArgs e)
		{
			int hid = Convert.ToInt32(this.txtHid.Value);
			if (string.IsNullOrEmpty(this.txtEditHotKeyword.Text.Trim()) || this.txtEditHotKeyword.Text.Trim().Length > 60)
			{
				this.ShowMsg("热门关键字不能为空,长度限制在60个字符以内", false);
				return;
			}
			if (!this.dropEditCategory.SelectedValue.HasValue)
			{
				this.ShowMsg("请选择商品主分类", false);
				return;
			}
			Regex regex = new Regex("^(?!_)(?!.*?_$)(?!-)(?!.*?-$)[a-zA-Z0-9_一-龥-]+$");
			if (!regex.IsMatch(this.txtEditHotKeyword.Text.Trim()))
			{
				this.ShowMsg("热门关键字只能输入汉字,数字,英文,下划线,减号,不能以下划线、减号开头或结尾", false);
				return;
			}
			if (string.Compare(this.txtEditHotKeyword.Text.Trim(), this.hiHotKeyword.Value) != 0 && this.IsSame(this.txtEditHotKeyword.Text.Trim(), Convert.ToInt32(this.dropEditCategory.SelectedValue.Value)))
			{
				this.ShowMsg("存在相同的的关键字，编辑失败", false);
				return;
			}
			int value = this.dropEditCategory.SelectedValue.Value;
			int num;
			if (!(string.Compare(value.ToString(), this.hicategory.Value) == 0 & string.Compare(this.txtEditHotKeyword.Text, this.hiHotKeyword.Value) != 0) || !this.IsSame(this.txtEditHotKeyword.Text.Trim(), Convert.ToInt32(this.dropEditCategory.SelectedValue.Value)))
			{
				if (string.Compare(this.txtEditHotKeyword.Text.Trim(), this.hiHotKeyword.Value) == 0)
				{
					value = this.dropEditCategory.SelectedValue.Value;
					if (string.Compare(value.ToString(), this.hicategory.Value) != 0)
					{
						num = (this.IsSame(this.txtEditHotKeyword.Text.Trim(), Convert.ToInt32(this.dropEditCategory.SelectedValue.Value)) ? 1 : 0);
						goto IL_023f;
					}
				}
				num = 0;
			}
			else
			{
				num = 1;
			}
			goto IL_023f;
			IL_023f:
			if (num != 0)
			{
				this.ShowMsg("同一分类型不允许存在相同的关键字,编辑失败", false);
			}
			else
			{
				StoreHelper.UpdateHotWords(hid, this.dropEditCategory.SelectedValue.Value, this.txtEditHotKeyword.Text.Trim());
				this.ShowMsg("编辑热门关键字成功！", true);
				this.hicategory.Value = "";
				this.hiHotKeyword.Value = "";
			}
		}

		private void btnSubmitHotkeyword_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(this.txtHotKeywords.Text.Trim()))
			{
				this.ShowMsg("热门关键字不能为空", false);
			}
			else if (!this.dropCategory.SelectedValue.HasValue)
			{
				this.ShowMsg("请选择商品主分类", false);
			}
			else
			{
				string text = this.txtHotKeywords.Text.Trim().Replace("\r\n", "\n");
				string[] array = text.Replace("\n", "*").Split('*');
				int num = 0;
				string[] array2 = array;
				foreach (string text2 in array2)
				{
					Regex regex = new Regex("^(?!_)(?!.*?_$)(?!-)(?!.*?-$)[a-zA-Z0-9_一-龥-]+$");
					if (regex.IsMatch(text2) && !this.IsSame(text2, Convert.ToInt32(this.dropCategory.SelectedValue.Value)))
					{
						StoreHelper.AddHotkeywords(this.dropCategory.SelectedValue.Value, text2);
						num++;
					}
				}
				if (num > 0)
				{
					this.ShowMsg($"成功添加了{num}个热门关键字", true);
					this.txtHotKeywords.Text = "";
				}
				else
				{
					this.ShowMsg("添加失败，请检查是否存在同类型的同名关键字", false);
				}
			}
		}

		private bool IsSame(string word, int categoryId)
		{
			DataTable hotKeywords = StoreHelper.GetHotKeywords();
			foreach (DataRow row in hotKeywords.Rows)
			{
				string b = row["Keywords"].ToString();
				if (word == b && categoryId == Convert.ToInt32(row["CategoryId"].ToString()))
				{
					return true;
				}
			}
			return false;
		}
	}
}
