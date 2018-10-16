using Hidistro.Core.Entities;
using Hidistro.SaleSystem.Catalog;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class LookLineItems : HtmlTemplatedWebControl
	{
		private int productId = 0;

		private ThemedTemplatedRepeater rptRecords;

		private Pager pager;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-LookLineItems.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			if (!int.TryParse(this.Page.Request.QueryString["productId"], out this.productId))
			{
				base.GotoResourceNotFound();
			}
			this.rptRecords = (ThemedTemplatedRepeater)this.FindControl("rptRecords");
			this.pager = (Pager)this.FindControl("pager");
			if (!this.Page.IsPostBack)
			{
				PageTitle.AddSiteNameTitle("商品成交记录");
				this.BindData();
			}
		}

		private void ReBind()
		{
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString(CultureInfo.InvariantCulture));
			base.ReloadPage(nameValueCollection);
		}

		private void BindData()
		{
			Pagination pagination = new Pagination();
			pagination.PageIndex = this.pager.PageIndex;
			pagination.PageSize = this.pager.PageSize;
			DbQueryResult lineItems = ProductBrowser.GetLineItems(pagination, this.productId);
			DataTable data = lineItems.Data;
			if (data != null && data.Rows.Count != 0)
			{
				IList<string> list = new List<string>();
				for (int i = 0; i < data.Rows.Count; i++)
				{
					string text = (data.Rows[i]["Username"] == DBNull.Value) ? "" : data.Rows[i]["UserName"].ToString();
					text = ((!(text.ToLower() == "anonymous") && !string.IsNullOrEmpty(text)) ? (text.Substring(0, 1) + "**" + text.Substring(text.Length - 1)) : "匿名用户");
					list.Add(text);
				}
				data.Columns.Remove("Username");
				data.Columns.Add(new DataColumn("Username"));
				for (int j = 0; j < data.Rows.Count; j++)
				{
					data.Rows[j]["Username"] = list[j];
				}
				this.rptRecords.DataSource = data;
				this.rptRecords.DataBind();
				this.pager.TotalRecords = lineItems.TotalRecords;
			}
		}
	}
}
