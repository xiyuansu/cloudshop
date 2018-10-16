using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Member;
using System;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class UserCellPhoneVerification : MemberTemplatedWebControl
	{
		private HtmlInputText txtcode;

		private HtmlInputText txtcellphone;

		private Button btnSubmit;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-UserCellPhoneVerification.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.txtcode = (HtmlInputText)this.FindControl("txtcode");
			this.txtcellphone = (HtmlInputText)this.FindControl("txtcellphone");
			this.txtcellphone.Value = HiContext.Current.User.CellPhone;
			this.btnSubmit = (Button)this.FindControl("btnSubmit");
			this.btnSubmit.Click += this.btnSubmit_Click;
		}

		private void btnSubmit_Click(object sender, EventArgs e)
		{
			string value = this.txtcellphone.Value;
			MemberInfo user = HiContext.Current.User;
			if (HiContext.Current.User.CellPhone != value && HiContext.Current.User.UserName != value)
			{
				MemberInfo memberInfo = MemberProcessor.FindMemberByUsername(value);
				if (MemberProcessor.IsUseCellphone(value) || (memberInfo != null && memberInfo.UserName == value))
				{
					this.ShowMessage("当前手机已被其他用户使用不能验证!", false, "", 1);
					return;
				}
			}
			if (string.IsNullOrEmpty(value))
			{
				this.ShowMessage("手机号码不允许为空！", false, "", 1);
			}
			else if (!Regex.IsMatch(value, "^(13|14|15|17|18)\\d{9}$"))
			{
				this.ShowMessage("手机号码格式不正确！", false, "", 1);
			}
			else if (string.IsNullOrEmpty(this.txtcode.Value))
			{
				this.ShowMessage("验证码不允许为空！", false, "", 1);
			}
			else
			{
				object obj = HiCache.Get($"DataCache-PhoneCode-{value}");
				if (obj == null)
				{
					this.ShowMessage("验证码错误！", false, "", 1);
				}
				else if (this.txtcode.Value.ToNullString().ToLower().Trim() != obj.ToNullString().ToLower().Trim())
				{
					this.ShowMessage("验证码输入错误！", false, "", 1);
				}
				else
				{
					MemberInfo user2 = Users.GetUser(HiContext.Current.UserId);
					if (user2 == null)
					{
						this.ShowMessage("请您先登录！", false, "", 1);
					}
					else
					{
						if (user2.UserName.IndexOf("YSC_") >= 0 || user2.UserName == user2.CellPhone)
						{
							user2.UserName = value;
						}
						user2.CellPhone = value;
						user2.CellPhoneVerification = true;
						if (MemberProcessor.UpdateMember(user2))
						{
							HiCache.Remove($"DataCache-PhoneCode-{value}");
							this.Page.Response.Redirect("VerificationSuccess.aspx");
						}
						else
						{
							this.ShowMessage("发送验证码失败", false, "", 1);
						}
					}
				}
			}
		}
	}
}
