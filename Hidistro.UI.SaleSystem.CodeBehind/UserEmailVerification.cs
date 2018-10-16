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
	public class UserEmailVerification : MemberTemplatedWebControl
	{
		private HtmlInputText txtcode;

		private HtmlInputText txtemail;

		private Button btnSubmit;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-UserEmailVerification.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.txtcode = (HtmlInputText)this.FindControl("txtcode");
			this.txtemail = (HtmlInputText)this.FindControl("txtemail");
			this.btnSubmit = (Button)this.FindControl("btnSubmit");
			this.btnSubmit.Click += this.btnSubmit_Click;
			this.txtemail.Value = HiContext.Current.User.Email.ToNullString();
		}

		private void btnSubmit_Click(object sender, EventArgs e)
		{
			string value = this.txtemail.Value;
			if (string.IsNullOrEmpty(value))
			{
				this.ShowMessage("邮箱不允许为空！", false, "", 1);
			}
			else if (value.Length > 256 || !Regex.IsMatch(value, "([a-zA-Z\\.0-9_-])+@([a-zA-Z0-9_-])+((\\.[a-zA-Z0-9_-]{2,4}){1,2})"))
			{
				this.ShowMessage("请输入正确的邮箱账号", false, "", 1);
			}
			else if (string.IsNullOrEmpty(this.txtcode.Value))
			{
				this.ShowMessage("验证码不允许为空！", false, "", 1);
			}
			else
			{
				object obj = HiCache.Get($"DataCache-EmailCode-{value}");
				if (obj == null)
				{
					this.ShowMessage("验证码错误！", false, "", 1);
				}
				else if (this.txtcode.Value.ToLower() != obj.ToString().ToLower())
				{
					this.ShowMessage("验证码输入错误！", false, "", 1);
				}
				else
				{
					MemberInfo user = HiContext.Current.User;
					if (user.Email != value.Trim() && user.UserName != value.Trim())
					{
						MemberInfo memberInfo = MemberProcessor.FindMemberByUsername(value.Trim());
						if (Users.MemberEmailIsExist(value.Trim()) || (memberInfo != null && memberInfo.UserName == value.Trim()))
						{
							this.ShowMessage("该邮箱已被其它用户使用了,请更换其它邮箱！", false, "", 1);
							return;
						}
						user.EmailVerification = false;
					}
					MemberInfo user2 = Users.GetUser(HiContext.Current.UserId);
					if (user2 == null)
					{
						this.ShowMessage("请您先登录！", false, "", 1);
					}
					else
					{
						if (user2.UserName.IndexOf("YSC_") >= 0 || user2.UserName == user2.Email)
						{
							user2.UserName = value;
						}
						user2.Email = value;
						user2.EmailVerification = true;
						if (MemberProcessor.UpdateMember(user2))
						{
							HiCache.Remove($"DataCache-EmailCode-{value}");
							this.Page.Response.Redirect("VerificationSuccess.aspx?type=1");
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
