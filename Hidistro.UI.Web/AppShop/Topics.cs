using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Members;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.SaleSystem.CodeBehind;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.AppShop
{
	public class Topics : Page
	{
		private int topicId = 0;

		public string pageTitle = string.Empty;

		protected Literal litImageServerUrl;

		protected Literal MeiQia_OnlineServer;

		protected Panel PanelTheme;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!base.IsPostBack)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				if (masterSettings.MeiQiaActivated == "1")
				{
					string empty = string.Empty;
					int num = 0;
					string text = masterSettings.MeiQiaUnitid.ToNullString();
					string text2 = string.Empty;
					string text3 = string.Empty;
					string empty2 = string.Empty;
					string text4 = string.Empty;
					string empty3 = string.Empty;
					string empty4 = string.Empty;
					string empty5 = string.Empty;
					string empty6 = string.Empty;
					string empty7 = string.Empty;
					string text5 = string.Empty;
					string text6 = string.Empty;
					string text7 = string.Empty;
					string text8 = string.Empty;
					string empty8 = string.Empty;
					string empty9 = string.Empty;
					string text9 = string.Empty;
					string text10 = string.Empty;
					string text11 = string.Empty;
					string text12 = string.Empty;
					string empty10 = string.Empty;
					MemberInfo user = HiContext.Current.User;
					if (user != null)
					{
						text2 = user.UserName.ToNullString();
						empty7 = text2;
						empty6 = user.UserName.ToNullString();
						empty4 = ((user.Picture == null) ? "" : (masterSettings.SiteUrl + user.Picture));
						text3 = ((user.Gender != Gender.Female) ? ((user.Gender != Gender.Male) ? "保密" : "男") : "女");
						empty2 = user.BirthDate.ToNullString();
						object obj;
						if (!user.BirthDate.HasValue)
						{
							obj = "";
						}
						else
						{
							DateTime dateTime = DateTime.Now;
							int year = dateTime.Year;
							dateTime = user.BirthDate.Value;
							obj = (year - dateTime.Year).ToNullString();
						}
						text4 = (string)obj;
						text5 = user.CellPhone.ToNullString();
						text6 = user.Email.ToNullString();
						text7 = RegionHelper.GetFullRegion(user.RegionId, "", true, 0).ToNullString() + user.Address.ToNullString();
						text8 = user.QQ.ToNullString();
						text9 = user.WeChat.ToNullString();
						text10 = user.Wangwang.ToNullString();
						DateTime createDate = user.CreateDate;
						text11 = ((user.CreateDate == DateTime.MinValue) ? "" : user.CreateDate.ToNullString());
						MemberGradeInfo memberGrade = MemberHelper.GetMemberGrade(user.GradeId);
						text12 = ((memberGrade == null) ? "" : memberGrade.Name.ToNullString());
					}
					empty = "<script type='text/javascript'>                                  \r\n                                    (function (m, ei, q, i, a, j, s) {\r\n                                        m[a] = m[a] || function () {\r\n                                            (m[a].a = m[a].a || []).push(arguments)\r\n                                        };\r\n                                        j = ei.createElement(q),\r\n                                            s = ei.getElementsByTagName(q)[0];\r\n                                        j.async = true;\r\n                                        j.charset = 'UTF-8';\r\n                                        j.src = i;\r\n                                        s.parentNode.insertBefore(j, s)\r\n                                    })(window, document, 'script', '//eco-api.meiqia.com/dist/meiqia.js', '_MEIQIA');\r\n                                    _MEIQIA('entId', " + text + ");\r\n                                    _MEIQIA('metadata', { \r\n                                                address: '" + text7 + "', // 地址\r\n                                                age: '" + text4 + "', // 年龄\r\n                                                comment: '" + empty5 + "', // 备注\r\n                                                email: '" + text6 + "', // 邮箱\r\n                                                gender: '" + text3 + "', // 性别\r\n                                                name: '" + text2 + "', // 名字\r\n                                                qq: '" + text8 + "', // QQ\r\n                                                tel: '" + text5 + "', // 电话\r\n                                                weibo: '" + empty8 + "', // 微博\r\n                                                weixin: '" + empty9 + "', // 微信 \r\n                                                '会员等级': '" + text12 + "',\r\n                                                'MSN': '" + text9 + "',\r\n                                                '旺旺': '" + text10 + "',                                        \r\n                                                '账号创建时间': '" + text11 + "' " + empty10 + "\r\n                                    });\r\n                                </script>";
					Literal literal = (Literal)this.FindControl("MeiQia_OnlineServer");
					if (literal != null)
					{
						literal.Text = empty;
					}
				}
			}
		}

		protected void GotoResourceNotFound(string errorMsg = "")
		{
			this.Page.Response.Redirect("ResourceNotFound.aspx?errorMsg=" + errorMsg);
		}

		protected override void OnInit(EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["TopicId"], out this.topicId))
			{
				this.GotoResourceNotFound("");
			}
			TopicInfo topic = VshopBrowser.GetTopic(this.topicId);
			if (topic == null || topic.TopicType == 0 || topic.TopicType != 2)
			{
				this.GotoResourceNotFound("专题不存在");
			}
			AppTopicHomePage appTopicHomePage = new AppTopicHomePage();
			appTopicHomePage.TopicId = topic.TopicId;
			this.pageTitle = topic.Title;
			this.PanelTheme.Controls.Add(appTopicHomePage);
			base.OnInit(e);
		}
	}
}
