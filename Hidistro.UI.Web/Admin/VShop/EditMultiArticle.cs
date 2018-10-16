using Hidistro.Core;
using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Web.Admin.Ascx;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.vshop
{
	public class EditMultiArticle : AdminPage
	{
		protected int MaterialID;

		protected string articleJson;

		protected CheckBox chkKeys;

		protected CheckBox chkSub;

		protected CheckBox chkNo;

		protected TextBox txtKeys;

		protected YesNoRadioButtonList radMatch;

		protected YesNoRadioButtonList radDisable;

		protected HiddenField hdpic;

		protected TextBox fkContent;

		protected ImageList ImageList;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!int.TryParse(base.Request.QueryString["id"], out this.MaterialID))
			{
				base.Response.Redirect("ReplyOnKey.aspx");
			}
			this.radMatch.Items[0].Text = "模糊匹配";
			this.radMatch.Items[1].Text = "精确匹配";
			this.radDisable.Items[0].Text = "启用";
			this.radDisable.Items[1].Text = "禁用";
			this.chkNo.Enabled = (ReplyHelper.GetMismatchReply() == null);
			this.chkSub.Enabled = (ReplyHelper.GetSubscribeReply() == null);
			if (!this.chkNo.Enabled)
			{
				this.chkNo.ToolTip = "该类型已被使用";
			}
			if (!this.chkSub.Enabled)
			{
				this.chkSub.ToolTip = "该类型已被使用";
			}
			if (!string.IsNullOrEmpty(base.Request.QueryString["cmd"]))
			{
				if (!string.IsNullOrEmpty(base.Request.Form["MultiArticle"]))
				{
					string value = base.Request.Form["MultiArticle"];
					List<ArticleList> list = JsonConvert.DeserializeObject(value, typeof(List<ArticleList>)) as List<ArticleList>;
					if (list != null && list.Count > 0)
					{
						NewsReplyInfo newsReplyInfo = ReplyHelper.GetReply(this.MaterialID) as NewsReplyInfo;
						newsReplyInfo.IsDisable = (base.Request.Form["radDisable"] != "true");
						string text = base.Request.Form.Get("Keys");
						if (base.Request.Form["chkKeys"] == "true")
						{
							if (!string.IsNullOrEmpty(text) && newsReplyInfo.Keys != text && ReplyHelper.HasReplyKey(text))
							{
								base.Response.Write("key");
								base.Response.End();
							}
							newsReplyInfo.Keys = text;
						}
						else
						{
							newsReplyInfo.Keys = string.Empty;
						}
						newsReplyInfo.MatchType = ((base.Request.Form["radMatch"] == "true") ? MatchType.Like : MatchType.Equal);
						newsReplyInfo.ReplyType = ReplyType.None;
						if (base.Request.Form["chkKeys"] == "true")
						{
							newsReplyInfo.ReplyType |= ReplyType.Keys;
						}
						if (base.Request.Form["chkSub"] == "true")
						{
							newsReplyInfo.ReplyType |= ReplyType.Subscribe;
						}
						if (base.Request.Form["chkNo"] == "true")
						{
							newsReplyInfo.ReplyType |= ReplyType.NoMatch;
						}
						foreach (NewsMsgInfo item in newsReplyInfo.NewsMsg)
						{
							ReplyHelper.DeleteNewsMsg(item.Id);
						}
						List<NewsMsgInfo> list2 = new List<NewsMsgInfo>();
						foreach (ArticleList item2 in list)
						{
							if (item2.Status != "del")
							{
								NewsMsgInfo newsMsgInfo = item2;
								if (newsMsgInfo != null)
								{
									newsMsgInfo.Reply = newsReplyInfo;
									list2.Add(newsMsgInfo);
								}
							}
						}
						newsReplyInfo.NewsMsg = list2;
						foreach (NewsMsgInfo item3 in newsReplyInfo.NewsMsg)
						{
							item3.PicUrl = Globals.SaveFile("article", item3.PicUrl, "/Storage/master/", true, false, "");
						}
						if (ReplyHelper.UpdateReply(newsReplyInfo))
						{
							base.Response.Write("true");
							base.Response.End();
						}
					}
				}
			}
			else
			{
				NewsReplyInfo newsReplyInfo2 = ReplyHelper.GetReply(this.MaterialID) as NewsReplyInfo;
				if (newsReplyInfo2 != null)
				{
					List<ArticleList> list3 = new List<ArticleList>();
					if (newsReplyInfo2.NewsMsg != null && newsReplyInfo2.NewsMsg.Count > 0)
					{
						int num = 1;
						foreach (NewsMsgInfo item4 in newsReplyInfo2.NewsMsg)
						{
							ArticleList articleList = new ArticleList();
							articleList.PicUrl = item4.PicUrl;
							articleList.Title = item4.Title;
							articleList.Url = item4.Url;
							articleList.Description = item4.Description;
							articleList.Content = item4.Content;
							ArticleList articleList2 = articleList;
							int num2 = num;
							num = num2 + 1;
							articleList2.BoxId = num2.ToString();
							articleList.Status = "";
							list3.Add(articleList);
						}
						this.articleJson = JsonConvert.SerializeObject(list3);
					}
					this.fkContent.Text = newsReplyInfo2.NewsMsg[0].Content;
					this.txtKeys.Text = newsReplyInfo2.Keys;
					this.radMatch.SelectedValue = (newsReplyInfo2.MatchType == MatchType.Like);
					this.radDisable.SelectedValue = !newsReplyInfo2.IsDisable;
					this.chkKeys.Checked = (ReplyType.Keys == (newsReplyInfo2.ReplyType & ReplyType.Keys));
					this.chkSub.Checked = (ReplyType.Subscribe == (newsReplyInfo2.ReplyType & ReplyType.Subscribe));
					this.chkNo.Checked = (ReplyType.NoMatch == (newsReplyInfo2.ReplyType & ReplyType.NoMatch));
					if (this.chkNo.Checked)
					{
						this.chkNo.Enabled = true;
						this.chkNo.ToolTip = "";
					}
					if (this.chkSub.Checked)
					{
						this.chkSub.Enabled = true;
						this.chkSub.ToolTip = "";
					}
				}
			}
		}

		protected void btnCreate_Click(object sender, EventArgs e)
		{
		}
	}
}
