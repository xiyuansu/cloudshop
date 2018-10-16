using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Member;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class RegisterUserSave : HtmlTemplatedWebControl
	{
		private TextBox txtTradeKey;

		private TextBox txtTradeKey2;

		private TextBox txtQuestion;

		private TextBox txtAnswer;

		private TextBox txtRealName;

		private RegionSelector dropRegions;

		private TextBox txtAddress;

		private TextBox txtQQ;

		private TextBox txtWW;

		private TextBox txtMSN;

		private TextBox txtTel;

		private TextBox txtHandSet;

		private IButton btnSaveUser;

		private int userId;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-RegisterUserSave.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			if (string.IsNullOrEmpty(this.Page.Request["UserId"]) || !int.TryParse(this.Page.Request.QueryString["UserId"], out this.userId))
			{
				base.GotoResourceNotFound();
			}
			if (HiContext.Current.UserId != this.userId)
			{
				base.GotoResourceNotFound();
			}
			this.txtTradeKey = (TextBox)this.FindControl("txtTradeKey");
			this.txtTradeKey2 = (TextBox)this.FindControl("txtTradeKey2");
			this.txtQuestion = (TextBox)this.FindControl("txtQuestion");
			this.txtAnswer = (TextBox)this.FindControl("txtAnswer");
			this.txtRealName = (TextBox)this.FindControl("txtRealName");
			this.dropRegions = (RegionSelector)this.FindControl("dropRegions");
			this.txtAddress = (TextBox)this.FindControl("txtAddress");
			this.txtQQ = (TextBox)this.FindControl("txtQQ");
			this.txtMSN = (TextBox)this.FindControl("txtMSN");
			this.txtTel = (TextBox)this.FindControl("txtTel");
			this.txtHandSet = (TextBox)this.FindControl("txtHandSet");
			this.btnSaveUser = ButtonManager.Create(this.FindControl("btnSaveUser"));
			this.txtWW = (TextBox)this.FindControl("txtWW");
			this.btnSaveUser.Click += this.btnSaveUser_Click;
			if (!this.Page.IsPostBack)
			{
				this.dropRegions.DataBind();
				MemberInfo user = Users.GetUser(this.userId);
				this.txtHandSet.Text = user.CellPhone;
			}
		}

		private void btnSaveUser_Click(object sender, EventArgs e)
		{
			if ((!string.IsNullOrEmpty(this.txtQuestion.Text) && string.IsNullOrEmpty(this.txtAnswer.Text)) || (string.IsNullOrEmpty(this.txtQuestion.Text) && !string.IsNullOrEmpty(this.txtAnswer.Text)))
			{
				this.ShowMessage("密码问题和问题答案要设置的话就两者都必须填写", false, "", 1);
			}
			else
			{
				MemberInfo user = Users.GetUser(this.userId);
				if (!string.IsNullOrEmpty(this.txtTradeKey.Text))
				{
					if (this.txtTradeKey.Text.Length < 6 || this.txtTradeKey.Text.Length > 20)
					{
						this.ShowMessage("交易密码长度必须为6-20个字符", false, "", 1);
						return;
					}
					if (string.Compare(this.txtTradeKey.Text, this.txtTradeKey2.Text) != 0)
					{
						this.ShowMessage("两次输入的交易密码不一致", false, "", 1);
						return;
					}
					user.IsOpenBalance = true;
					user.TradePassword = this.txtTradeKey.Text;
				}
				if (!string.IsNullOrEmpty(this.txtQuestion.Text) && !string.IsNullOrEmpty(this.txtAnswer.Text))
				{
					MemberProcessor.ChangePasswordQuestionAndAnswer("", Globals.HtmlEncode(this.txtQuestion.Text), Globals.HtmlEncode(this.txtAnswer.Text));
				}
				user.RealName = this.txtRealName.Text;
				if (this.dropRegions.GetSelectedRegionId().HasValue)
				{
					user.RegionId = this.dropRegions.GetSelectedRegionId().Value;
					user.TopRegionId = RegionHelper.GetTopRegionId(user.RegionId, true);
				}
				user.Address = Globals.HtmlEncode(this.txtAddress.Text);
				user.QQ = this.txtQQ.Text;
				user.WeChat = this.txtMSN.Text;
				user.CellPhone = this.txtHandSet.Text;
				user.Wangwang = this.txtWW.Text;
				if (MemberProcessor.UpdateMember(user))
				{
					this.Page.Response.Redirect("/user/MyAccountSummary.aspx");
				}
			}
		}
	}
}
