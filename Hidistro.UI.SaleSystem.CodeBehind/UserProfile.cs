using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Member;
using Hidistro.UI.Common.Controls;
using System;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class UserProfile : MemberTemplatedWebControl
	{
		private SmallStatusMessage Statuses;

		private TextBox txtRealName;

		private TextBox txtEmail;

		private TextBox txtAddress;

		private TextBox txtQQ;

		private TextBox txtMSN;

		private TextBox txtTel;

		private TextBox txtHandSet;

		private RegionSelector dropRegionsSelect;

		private GenderRadioButtonList gender;

		private CalendarPanel calendDate;

		private IButton btnOK1;

		private HiddenField hidePicture;

		private Image smallpic;

		protected virtual void ShowMsgs(SmallStatusMessage state, string msg, bool success)
		{
			if (state != null)
			{
				state.Success = success;
				state.Text = msg;
				state.Visible = true;
			}
		}

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-UserProfile.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.txtRealName = (TextBox)this.FindControl("txtRealName");
			this.txtEmail = (TextBox)this.FindControl("txtEmail");
			this.dropRegionsSelect = (RegionSelector)this.FindControl("dropRegions");
			this.gender = (GenderRadioButtonList)this.FindControl("gender");
			this.calendDate = (CalendarPanel)this.FindControl("calendDate");
			this.txtAddress = (TextBox)this.FindControl("txtAddress");
			this.txtQQ = (TextBox)this.FindControl("txtQQ");
			this.txtMSN = (TextBox)this.FindControl("txtMSN");
			this.txtTel = (TextBox)this.FindControl("txtTel");
			this.txtHandSet = (TextBox)this.FindControl("txtHandSet");
			this.btnOK1 = ButtonManager.Create(this.FindControl("btnOK1"));
			this.Statuses = (SmallStatusMessage)this.FindControl("Statuses");
			this.hidePicture = (HiddenField)this.FindControl("fmSrc");
			this.smallpic = (Image)this.FindControl("smallpic");
			this.btnOK1.Click += this.btnOK1_Click;
			PageTitle.AddSiteNameTitle("个人信息");
			if (!this.Page.IsPostBack)
			{
				MemberInfo user = HiContext.Current.User;
				if (user.UserId != 0)
				{
					this.BindData(user);
				}
			}
		}

		private void btnOK1_Click(object sender, EventArgs e)
		{
			MemberInfo user = HiContext.Current.User;
			string userName = user.UserName;
			string cellPhone = user.CellPhone;
			if (this.txtRealName.Text.Trim().Length > 20 || (this.txtRealName.Text.Trim().Length > 0 && !Regex.IsMatch(this.txtRealName.Text.Trim(), "^[A-Za-z\\u4e00-\\u9fa5]+$")))
			{
				this.ShowMessage("真实姓名限制在20个字符以内,只能由中文或英文组成", false, "", 1);
			}
			else
			{
				user.RealName = Globals.HtmlEncode(this.txtRealName.Text.Trim());
				if (!this.dropRegionsSelect.GetSelectedRegionId().HasValue)
				{
					user.RegionId = 0;
				}
				else
				{
					user.RegionId = this.dropRegionsSelect.GetSelectedRegionId().Value;
					user.TopRegionId = RegionHelper.GetTopRegionId(user.RegionId, true);
				}
				user.Gender = this.gender.SelectedValue;
				user.BirthDate = this.calendDate.SelectedDate;
				user.Address = Globals.HtmlEncode(this.txtAddress.Text.Trim());
				user.QQ = Globals.HtmlEncode(this.txtQQ.Text.Trim());
				user.NickName = Globals.HtmlEncode(this.txtMSN.Text.Trim());
				string imageServerUrl = Globals.GetImageServerUrl();
				user.Picture = (string.IsNullOrEmpty(imageServerUrl) ? Globals.SaveFile("user", this.hidePicture.Value, "/Storage/master/", true, false, "") : this.hidePicture.Value);
				if (MemberProcessor.UpdateMember(user))
				{
					Image image = this.smallpic;
					HiddenField hiddenField = this.hidePicture;
					string text2 = image.ImageUrl = (hiddenField.Value = user.Picture);
					this.ShowMessage("成功的修改了用户的个人资料", true, "", 1);
				}
				else
				{
					this.ShowMessage("修改用户个人资料失败", false, "", 1);
				}
			}
		}

		private void BindData(MemberInfo user)
		{
			this.txtRealName.Text = Globals.HtmlDecode(user.RealName);
			this.txtEmail.Text = Globals.HtmlDecode(user.Email);
			this.gender.SelectedValue = user.Gender;
			if (user.BirthDate > (DateTime?)DateTime.MinValue)
			{
				this.calendDate.SelectedDate = user.BirthDate;
			}
			this.dropRegionsSelect.SetSelectedRegionId(user.RegionId);
			this.txtAddress.Text = Globals.HtmlDecode(user.Address);
			this.txtQQ.Text = Globals.HtmlDecode(user.QQ);
			this.txtMSN.Text = Globals.HtmlDecode(user.NickName);
			this.txtHandSet.Text = Globals.HtmlDecode(user.CellPhone);
			if (!string.IsNullOrEmpty(user.Picture))
			{
				this.smallpic.ImageUrl = user.Picture;
				this.hidePicture.Value = user.Picture;
			}
			else
			{
				this.hidePicture.Value = this.smallpic.ImageUrl;
			}
		}
	}
}
