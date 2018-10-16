using Hidistro.Context;
using Hidistro.Entities;
using Hidistro.Entities.Members;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.SaleSystem.Tags;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class WAPUserProfile : WAPMemberTemplatedWebControl
	{
		private HtmlInputHidden hidUserPicture;

		private HtmlInputControl txtRealName;

		private HtmlInputControl txtSex;

		private HtmlInputControl txtBirthday;

		private HtmlInputControl txtQQ;

		private HtmlInputControl txtMSN;

		private Common_WAPLocateAddress WapLocateAddress;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-VUserProfile.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.hidUserPicture = (HtmlInputHidden)this.FindControl("hidUserPicture");
			this.txtRealName = (HtmlInputControl)this.FindControl("txtRealName");
			this.txtSex = (HtmlInputControl)this.FindControl("txtSex");
			this.txtBirthday = (HtmlInputControl)this.FindControl("txtBirthday");
			this.txtQQ = (HtmlInputControl)this.FindControl("txtQQ");
			this.txtMSN = (HtmlInputControl)this.FindControl("txtMSN");
			this.WapLocateAddress = (Common_WAPLocateAddress)this.FindControl(Common_WAPLocateAddress.TagId);
			MemberInfo user = HiContext.Current.User;
			if (!string.IsNullOrEmpty(user.Picture))
			{
				this.hidUserPicture.Value = user.Picture;
			}
			else
			{
				this.hidUserPicture.Value = "/templates/common/images/headerimg.png";
			}
			if (!string.IsNullOrEmpty(user.RealName))
			{
				this.txtRealName.Value = user.RealName;
			}
			switch (user.Gender)
			{
			case Gender.NotSet:
				this.txtSex.Value = "";
				break;
			case Gender.Male:
				this.txtSex.Value = "男士";
				break;
			case Gender.Female:
				this.txtSex.Value = "女士";
				break;
			}
			if (user.BirthDate.HasValue)
			{
				this.txtBirthday.Value = user.BirthDate.Value.ToString("yyyy-MM-dd");
			}
			if (!string.IsNullOrEmpty(user.QQ))
			{
				this.txtQQ.Value = user.QQ;
			}
			if (!string.IsNullOrEmpty(user.NickName))
			{
				this.txtMSN.Value = user.NickName;
			}
			if (this.WapLocateAddress != null)
			{
				this.WapLocateAddress.RegionId = user.RegionId;
				this.WapLocateAddress.Address = user.Address;
			}
			PageTitle.AddSiteNameTitle("个人信息");
		}
	}
}
