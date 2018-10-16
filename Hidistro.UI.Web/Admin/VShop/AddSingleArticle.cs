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
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.vshop
{
	public class AddSingleArticle : AdminPage
	{
		protected Label LbimgTitle;

		protected Label Lbmsgdesc;

		protected CheckBox chkNo;

		protected CheckBox chkKeys;

		protected CheckBox chkSub;

		protected TextBox txtKeys;

		protected YesNoRadioButtonList radMatch;

		protected YesNoRadioButtonList radDisable;

		protected Button btnCreate;

		protected TextBox Tbtitle;

		protected HiddenField hdpic;

		protected TextBox Tbdescription;

		protected TextBox TbUrl;

		protected DropDownList ddlsubType;

		protected TextBox TbUrltoSub;

		protected DropDownList ddlCoupon;

		protected Ueditor fkContent;

		protected ImageList ImageList;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.radMatch.Items[0].Text = "模糊匹配";
			this.radMatch.Items[1].Text = "精确匹配";
			this.radDisable.Items[0].Text = "启用";
			this.radDisable.Items[1].Text = "禁用";
			this.chkNo.Enabled = (ReplyHelper.GetMismatchReply() == null);
			this.chkSub.Enabled = (ReplyHelper.GetSubscribeReply() == null);
			IList<CouponInfo> allUsedCoupons = CouponHelper.GetAllUsedCoupons(1);
			if (allUsedCoupons != null && allUsedCoupons.Count > 0)
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
			if (!this.chkNo.Enabled)
			{
				this.chkNo.ToolTip = "该类型已被使用";
			}
			if (!this.chkSub.Enabled)
			{
				this.chkSub.ToolTip = "该类型已被使用";
			}
		}

		protected void btnCreate_Click(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(this.Tbtitle.Text) && !string.IsNullOrEmpty(this.Tbdescription.Text) && !string.IsNullOrEmpty(this.hdpic.Value))
			{
				if (!string.IsNullOrEmpty(this.txtKeys.Text) && ReplyHelper.HasReplyKey(this.txtKeys.Text.Trim()))
				{
					this.ShowMsg("关键字重复!", false);
				}
				else
				{
					NewsReplyInfo newsReplyInfo = new NewsReplyInfo();
					string text = "";
					newsReplyInfo.IsDisable = !this.radDisable.SelectedValue;
					if (this.chkKeys.Checked && !string.IsNullOrWhiteSpace(this.txtKeys.Text))
					{
						newsReplyInfo.Keys = this.txtKeys.Text.Trim();
					}
					if (this.chkKeys.Checked && string.IsNullOrWhiteSpace(this.txtKeys.Text))
					{
						this.ShowMsg("你选择了关键字回复，必须填写关键字！", false);
					}
					else
					{
						newsReplyInfo.MatchType = (this.radMatch.SelectedValue ? MatchType.Like : MatchType.Equal);
						newsReplyInfo.MessageType = MessageType.News;
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
						else if (string.IsNullOrEmpty(this.Tbtitle.Text))
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
							NewsMsgInfo item = new NewsMsgInfo
							{
								Reply = newsReplyInfo,
								Content = this.fkContent.Text,
								Description = HttpUtility.HtmlEncode(this.Tbdescription.Text),
								PicUrl = picUrl,
								Title = HttpUtility.HtmlEncode(this.Tbtitle.Text),
								Url = text.Trim()
							};
							newsReplyInfo.NewsMsg = new List<NewsMsgInfo>();
							newsReplyInfo.NewsMsg.Add(item);
							if (ReplyHelper.SaveReply(newsReplyInfo))
							{
								this.ShowMsg("添加成功", true, "ReplyOnKey.aspx");
							}
							else
							{
								this.ShowMsg("添加失败", false);
							}
						}
					}
				}
			}
			else
			{
				this.ShowMsg("您填写的信息不完整!", false);
			}
		}
	}
}
