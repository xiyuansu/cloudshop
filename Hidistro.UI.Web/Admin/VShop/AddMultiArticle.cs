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
	public class AddMultiArticle : AdminPage
	{
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
			if (!string.IsNullOrEmpty(base.Request.QueryString["cmd"]) && !string.IsNullOrEmpty(base.Request.Form["MultiArticle"]))
			{
				string value = base.Request.Form["MultiArticle"];
				List<ArticleList> list = JsonConvert.DeserializeObject(value, typeof(List<ArticleList>)) as List<ArticleList>;
				if (list != null && list.Count > 0)
				{
					NewsReplyInfo newsReplyInfo = new NewsReplyInfo();
					newsReplyInfo.MessageType = MessageType.List;
					newsReplyInfo.IsDisable = (base.Request.Form["radDisable"] != "true");
					if (base.Request.Form["chkKeys"] == "true")
					{
						newsReplyInfo.Keys = base.Request.Form.Get("Keys");
					}
					if (!string.IsNullOrWhiteSpace(newsReplyInfo.Keys) && ReplyHelper.HasReplyKey(newsReplyInfo.Keys))
					{
						base.Response.Write("key");
						base.Response.End();
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
					List<NewsMsgInfo> list2 = new List<NewsMsgInfo>();
					foreach (ArticleList item in list)
					{
						if (item.Status != "del")
						{
							NewsMsgInfo newsMsgInfo = item;
							if (newsMsgInfo != null)
							{
								newsMsgInfo.Reply = newsReplyInfo;
								list2.Add(newsMsgInfo);
							}
						}
					}
					newsReplyInfo.NewsMsg = list2;
					foreach (NewsMsgInfo item2 in newsReplyInfo.NewsMsg)
					{
						item2.PicUrl = Globals.SaveFile("article", item2.PicUrl, "/Storage/master/", true, false, "");
					}
					if (ReplyHelper.SaveReply(newsReplyInfo))
					{
						base.Response.Write("true");
						base.Response.End();
					}
				}
			}
		}

		protected void btnCreate_Click(object sender, EventArgs e)
		{
		}
	}
}
