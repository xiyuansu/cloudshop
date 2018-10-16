using Hidistro.Context;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Comments;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Catalog;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class AppMyConsultations : AppshopMemberTemplatedWebControl
	{
		private AppshopTemplatedRepeater rptProducts;

		private HtmlInputHidden txtTotal;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-VMyConsultations.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			MemberInfo user = HiContext.Current.User;
			if (user.UserId != 0)
			{
				this.rptProducts = (AppshopTemplatedRepeater)this.FindControl("rptProducts");
				this.txtTotal = (HtmlInputHidden)this.FindControl("txtTotal");
				int pageIndex = default(int);
				if (!int.TryParse(this.Page.Request.QueryString["page"], out pageIndex))
				{
					pageIndex = 1;
				}
				int pageSize = default(int);
				if (!int.TryParse(this.Page.Request.QueryString["size"], out pageSize))
				{
					pageSize = 20;
				}
				ProductConsultationAndReplyQuery productConsultationAndReplyQuery = new ProductConsultationAndReplyQuery();
				productConsultationAndReplyQuery.UserId = user.UserId;
				productConsultationAndReplyQuery.IsCount = true;
				productConsultationAndReplyQuery.PageIndex = pageIndex;
				productConsultationAndReplyQuery.PageSize = pageSize;
				productConsultationAndReplyQuery.SortBy = "ConsultationId";
				productConsultationAndReplyQuery.SortOrder = SortAction.Desc;
				DbQueryResult productConsultations = ProductBrowser.GetProductConsultations(productConsultationAndReplyQuery);
				this.rptProducts.DataSource = productConsultations.Data;
				this.rptProducts.DataBind();
				this.txtTotal.SetWhenIsNotNull(productConsultations.TotalRecords.ToString());
			}
			PageTitle.AddSiteNameTitle("商品咨询");
		}
	}
}
