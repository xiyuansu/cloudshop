using Hidistro.Core;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Promotions;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Web.Admin.Ascx;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.vshop
{
	public class EditSingleArticle : AdminPage
	{
		private int id;

		protected Label LbimgTitle;

		protected HtmlImage uploadpic;

		protected Label Lbmsgdesc;

		protected CheckBox chkKeys;

		protected CheckBox chkSub;

		protected CheckBox chkNo;

		protected TextBox txtKeys;

		protected YesNoRadioButtonList radMatch;

		protected YesNoRadioButtonList radDisable;

		protected Button btnCreate;

		protected TextBox Tbtitle;

		protected HiddenField HiddenField1;

		protected TextBox Tbdescription;

		protected TextBox TbUrl;

		protected DropDownList ddlsubType;

		protected TextBox TbUrltoSub;

		protected DropDownList ddlCoupon;

		protected Ueditor fkContent;

		protected HiddenField hdpic;

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
			IList<CouponInfo> allUsedCoupons = CouponHelper.GetAllUsedCoupons(1);
			if (allUsedCoupons != null)
			{
				foreach (CouponInfo item in allUsedCoupons)
				{
					if (CouponHelper.GetCouponSurplus(item.CouponId) > 0)
					{
						this.ddlCoupon.Items.Add(new ListItem(item.CouponName.ToNullString(), item.CouponId.ToNullString()));
					}
				}
			}
			if (this.ddlCoupon.Items.Count == 0)
			{
				this.ddlsubType.Items.RemoveAt(1);
				this.ddlCoupon.Items.Add(new ListItem("请选择优惠券", "0"));
			}
			if (!base.IsPostBack)
			{
				this.id = base.GetUrlIntParam("id");
				this.BindSingleArticle(this.id);
			}
			else
			{
				this.uploadpic.Src = this.hdpic.Value;
			}
		}

		protected void btnCreate_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(this.Tbtitle.Text) || string.IsNullOrEmpty(this.Tbdescription.Text) || string.IsNullOrEmpty(this.hdpic.Value))
			{
				this.ShowMsg("您填写的信息不完整!", false);
			}
			else
			{
				int urlIntParam = base.GetUrlIntParam("id");
				NewsReplyInfo newsReplyInfo = ReplyHelper.GetReply(urlIntParam) as NewsReplyInfo;
				if (newsReplyInfo == null || newsReplyInfo.NewsMsg == null || newsReplyInfo.NewsMsg.Count == 0)
				{
					base.GotoResourceNotFound();
				}
				else if (!string.IsNullOrEmpty(this.txtKeys.Text) && newsReplyInfo.Keys != this.txtKeys.Text.Trim() && ReplyHelper.HasReplyKey(this.txtKeys.Text.Trim()))
				{
					this.ShowMsg("关键字重复!", false);
				}
				else
				{
					newsReplyInfo.IsDisable = !this.radDisable.SelectedValue;
					if (this.chkKeys.Checked && !string.IsNullOrWhiteSpace(this.txtKeys.Text))
					{
						newsReplyInfo.Keys = this.txtKeys.Text.Trim();
					}
					else
					{
						newsReplyInfo.Keys = string.Empty;
					}
					newsReplyInfo.MatchType = (this.radMatch.SelectedValue ? MatchType.Like : MatchType.Equal);
					newsReplyInfo.ReplyType = ReplyType.None;
					if (this.chkKeys.Checked)
					{
						newsReplyInfo.ReplyType |= ReplyType.Keys;
					}
					if (this.chkSub.Checked)
					{
						newsReplyInfo.ReplyType |= ReplyType.Subscribe;
					}
					if (this.chkNo.Checked)
					{
						newsReplyInfo.ReplyType |= ReplyType.NoMatch;
					}
					if (newsReplyInfo.ReplyType == ReplyType.None)
					{
						this.ShowMsg("请选择回复类型", false);
					}
					else
					{
						string text = "";
						if (string.IsNullOrEmpty(this.Tbtitle.Text))
						{
							this.ShowMsg("请输入标题", false);
						}
						else if (string.IsNullOrEmpty(this.hdpic.Value))
						{
							this.ShowMsg("请上传封面图", false);
						}
						else if (string.IsNullOrEmpty(this.Tbdescription.Text))
						{
							this.ShowMsg("请输入摘要", false);
						}
						else
						{
							if (this.chkSub.Checked)
							{
								if (this.ddlsubType.SelectedValue == "0")
								{
									if (string.IsNullOrEmpty(this.fkContent.Text) && string.IsNullOrEmpty(this.TbUrltoSub.Text))
									{
										this.ShowMsg("请输入内容或自定义链接", false);
										return;
									}
									text = this.TbUrltoSub.Text;
								}
								else
								{
									if (string.IsNullOrEmpty(this.fkContent.Text) && string.IsNullOrEmpty(this.ddlCoupon.SelectedValue))
									{
										this.ShowMsg("请输入内容或选择优惠券", false);
										return;
									}
									text = "http://" + HttpContext.Current.Request.Url.Host + "/Vshop/ShakeCouponsForAttention.aspx?cid=" + this.ddlCoupon.SelectedValue;
								}
							}
							else
							{
								if (string.IsNullOrEmpty(this.fkContent.Text) && string.IsNullOrEmpty(this.TbUrl.Text))
								{
									this.ShowMsg("请输入内容或自定义链接", false);
									return;
								}
								text = this.TbUrl.Text;
							}
							string picUrl = Globals.SaveFile("reply", this.hdpic.Value, "/Storage/master/", true, false, "");
							newsReplyInfo.NewsMsg[0].Content = this.fkContent.Text;
							newsReplyInfo.NewsMsg[0].Description = this.Tbdescription.Text;
							newsReplyInfo.NewsMsg[0].PicUrl = picUrl;
							newsReplyInfo.NewsMsg[0].Title = this.Tbtitle.Text;
							newsReplyInfo.NewsMsg[0].Url = text;
							if (ReplyHelper.UpdateReply(newsReplyInfo))
							{
								this.ShowMsg("修改成功", true, "ReplyOnKey.aspx");
							}
							else
							{
								this.ShowMsg("修改失败", false);
							}
						}
					}
				}
			}
		}

		protected void BindSingleArticle(int id)
		{
			NewsReplyInfo newsReplyInfo = ReplyHelper.GetReply(id) as NewsReplyInfo;
			if (newsReplyInfo == null || newsReplyInfo.NewsMsg == null || newsReplyInfo.NewsMsg.Count == 0)
			{
				base.GotoResourceNotFound();
			}
			else
			{
				this.ViewState["MsgId"] = newsReplyInfo.ReplyId;
				this.txtKeys.Text = newsReplyInfo.Keys;
				this.radMatch.SelectedValue = (newsReplyInfo.MatchType == MatchType.Like);
				this.radDisable.SelectedValue = !newsReplyInfo.IsDisable;
				this.chkKeys.Checked = (ReplyType.Keys == (newsReplyInfo.ReplyType & ReplyType.Keys));
				this.chkSub.Checked = (ReplyType.Subscribe == (newsReplyInfo.ReplyType & ReplyType.Subscribe));
				this.chkNo.Checked = (ReplyType.NoMatch == (newsReplyInfo.ReplyType & ReplyType.NoMatch));
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
				this.Tbtitle.Text = newsReplyInfo.NewsMsg[0].Title;
				this.LbimgTitle.Text = newsReplyInfo.NewsMsg[0].Title;
				this.Tbdescription.Text = HttpUtility.HtmlDecode(newsReplyInfo.NewsMsg[0].Description);
				this.fkContent.Text = newsReplyInfo.NewsMsg[0].Content;
				this.Lbmsgdesc.Text = newsReplyInfo.NewsMsg[0].Description;
				this.uploadpic.Src = newsReplyInfo.NewsMsg[0].PicUrl;
				this.hdpic.Value = newsReplyInfo.NewsMsg[0].PicUrl;
				string url = newsReplyInfo.NewsMsg[0].Url;
				if (this.chkSub.Checked)
				{
					if (url.ToNullString().ToLower().Contains("http://" + HttpContext.Current.Request.Url.Host + "/vshop/shakecouponsforattention.aspx?cid="))
					{
						this.ddlsubType.SelectedValue = "1";
						this.ddlCoupon.SelectedValue = url.ToNullString().ToLower().Replace("http://" + HttpContext.Current.Request.Url.Host + "/vshop/shakecouponsforattention.aspx?cid=", "");
					}
					else
					{
						this.ddlsubType.SelectedValue = "0";
						this.TbUrltoSub.Text = url;
					}
				}
				else
				{
					this.TbUrl.Text = url;
				}
			}
		}
	}
}
