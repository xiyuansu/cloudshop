using System;
using System.ComponentModel;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class Pager : WebControl
	{
		private string urlFormat;

		private bool _ShowNumsButton = true;

		private int defaultPageSize = 10;

		private string aname = string.Empty;

		[Browsable(false)]
		public int PageIndex
		{
			get
			{
				int num = 1;
				if (!string.IsNullOrEmpty(this.Context.Request.QueryString[this.PageIndexFormat]))
				{
					int.TryParse(this.Context.Request.QueryString[this.PageIndexFormat], out num);
				}
				if (num <= 0)
				{
					return 1;
				}
				int num2 = this.CalculateTotalPages();
				if (num2 > 0 && num > num2)
				{
					return num2;
				}
				return num;
			}
		}

		public string PageIndexFormat
		{
			get;
			set;
		}

		[Browsable(false)]
		public int PageSize
		{
			get
			{
				int num = this.defaultPageSize;
				if (!string.IsNullOrEmpty(this.Context.Request.QueryString["pagesize"]))
				{
					int.TryParse(this.Context.Request.QueryString["pagesize"], out num);
				}
				if (num <= 0)
				{
					return this.defaultPageSize;
				}
				return num;
			}
		}

		[Browsable(false)]
		public int TotalRecords
		{
			get
			{
				if (this.ViewState["TotalRecords"] == null)
				{
					return 0;
				}
				return (int)this.ViewState["TotalRecords"];
			}
			set
			{
				this.ViewState["TotalRecords"] = value;
			}
		}

		public bool ShowNumsButton
		{
			get
			{
				return this._ShowNumsButton;
			}
			set
			{
				this._ShowNumsButton = value;
			}
		}

		public int DefaultPageSize
		{
			get
			{
				return this.defaultPageSize;
			}
			set
			{
				this.defaultPageSize = value;
			}
		}

		public bool ShowTotalPages
		{
			get;
			set;
		}

		public string BreakCssClass
		{
			get;
			set;
		}

		public string PrevCssClass
		{
			get;
			set;
		}

		public string CurCssClass
		{
			get;
			set;
		}

		public string NextCssClass
		{
			get;
			set;
		}

		public string SkipPanelCssClass
		{
			get;
			set;
		}

		public string SkipTxtCssClass
		{
			get;
			set;
		}

		public string SkipBtnCssClass
		{
			get;
			set;
		}

		public string Aname
		{
			get
			{
				if (!string.IsNullOrEmpty(this.aname) && !this.aname.StartsWith("#"))
				{
					this.aname = "#" + this.aname;
				}
				return this.aname;
			}
			set
			{
				this.aname = value;
			}
		}

		private bool HasPrevious
		{
			get
			{
				return this.PageIndex > 1;
			}
		}

		private bool HasNext
		{
			get
			{
				return this.PageIndex < this.CalculateTotalPages();
			}
		}

		public Pager()
		{
			this.PageIndexFormat = "pageindex";
			this.BreakCssClass = "page-break";
			this.PrevCssClass = "page-prev";
			this.CurCssClass = "page-cur";
			this.NextCssClass = "page-next";
			this.SkipPanelCssClass = "page-skip";
			this.SkipTxtCssClass = "text";
			this.SkipBtnCssClass = "button";
		}

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			this.urlFormat = this.Context.Request.RawUrl;
			if (this.urlFormat.IndexOf("?") >= 0)
			{
				string text = this.urlFormat.Substring(this.urlFormat.IndexOf("?") + 1);
				string[] array = text.Split(Convert.ToChar("&"));
				this.urlFormat = this.urlFormat.Replace(text, "");
				string[] array2 = array;
				foreach (string text2 in array2)
				{
					if (!text2.ToLower().StartsWith(this.PageIndexFormat.ToLower() + "="))
					{
						this.urlFormat = this.urlFormat + text2 + "&";
					}
				}
				this.urlFormat = this.urlFormat + this.PageIndexFormat + "=";
			}
			else
			{
				this.urlFormat = this.urlFormat + "?" + this.PageIndexFormat + "=";
			}
		}

		protected override void Render(HtmlTextWriter writer)
		{
			if (this.TotalRecords > 0)
			{
				int num = this.CalculateTotalPages();
				this.RenderPrevious(writer);
				if (this.ShowNumsButton)
				{
					this.RenderPagingButtons(writer, num);
				}
				if (num > 6 && this.PageIndex + 2 < num && this.ShowNumsButton)
				{
					this.RenderMore(writer);
				}
				this.RenderNext(writer);
				if (this.ShowTotalPages)
				{
					this.RenderGotoPage(writer, num);
				}
			}
		}

		private void RenderMore(HtmlTextWriter writer)
		{
			WebControl webControl = new WebControl(HtmlTextWriterTag.A);
			webControl.Attributes.Add("class", this.BreakCssClass);
			webControl.Controls.Add(new LiteralControl("..."));
			webControl.RenderControl(writer);
		}

		private void RenderPrevious(HtmlTextWriter writer)
		{
			if (this.HasPrevious)
			{
				WebControl webControl = new WebControl(HtmlTextWriterTag.A);
				webControl.Controls.Add(new LiteralControl("上一页"));
				webControl.Attributes.Add("class", this.PrevCssClass);
				webControl.Attributes.Add("href", this.urlFormat + (this.PageIndex - 1).ToString(CultureInfo.InvariantCulture) + this.Aname);
				webControl.RenderControl(writer);
			}
		}

		private void RenderPagingButtons(HtmlTextWriter writer, int totalPages)
		{
			if (totalPages <= 6)
			{
				this.RenderButtonRange(writer, 1, totalPages);
			}
			else
			{
				int num = this.PageIndex - 3;
				int num2 = this.PageIndex + 2;
				if (num <= 0)
				{
					num2 -= --num;
					num = 1;
				}
				if (num2 > totalPages)
				{
					num2 = totalPages;
				}
				this.RenderButtonRange(writer, num, num2);
			}
		}

		private void RenderButtonRange(HtmlTextWriter writer, int startIndex, int endIndex)
		{
			for (int i = startIndex; i <= endIndex; i++)
			{
				this.RenderButton(writer, i);
			}
		}

		private void RenderButton(HtmlTextWriter writer, int buttonIndex)
		{
			if (buttonIndex == this.PageIndex)
			{
				new LiteralControl("<a class=\"" + this.CurCssClass + "\">" + buttonIndex.ToString(CultureInfo.InvariantCulture) + "</a>").RenderControl(writer);
			}
			else
			{
				WebControl webControl = new WebControl(HtmlTextWriterTag.A);
				webControl.Controls.Add(new LiteralControl(buttonIndex.ToString(CultureInfo.InvariantCulture)));
				webControl.Attributes.Add("href", this.urlFormat + buttonIndex.ToString(CultureInfo.InvariantCulture) + this.Aname);
				webControl.RenderControl(writer);
			}
		}

		private void RenderNext(HtmlTextWriter writer)
		{
			if (this.HasNext)
			{
				WebControl webControl = new WebControl(HtmlTextWriterTag.A);
				webControl.Controls.Add(new LiteralControl("下一页"));
				webControl.Attributes.Add("class", this.NextCssClass);
				webControl.Attributes.Add("href", this.urlFormat + (this.PageIndex + 1).ToString(CultureInfo.InvariantCulture) + this.Aname);
				webControl.RenderControl(writer);
			}
		}

		private void RenderGotoPage(HtmlTextWriter writer, int totalPages)
		{
			WebControl webControl = new WebControl(HtmlTextWriterTag.Span);
			webControl.Attributes.Add("class", this.SkipPanelCssClass);
			ControlCollection controls = webControl.Controls;
			object arg = this.PageIndex;
			string arg2 = totalPages.ToString(CultureInfo.InvariantCulture);
			int num = this.TotalRecords;
			controls.Add(new LiteralControl($"第{arg}/{arg2}页 共{num.ToString(CultureInfo.InvariantCulture)}记录"));
			WebControl webControl2 = new WebControl(HtmlTextWriterTag.Input);
			webControl2.Attributes.Add("type", "text");
			webControl2.Attributes.Add("class", this.SkipTxtCssClass);
			System.Web.UI.AttributeCollection attributes = webControl2.Attributes;
			num = this.PageIndex;
			attributes.Add("value", num.ToString(CultureInfo.InvariantCulture));
			webControl2.Attributes.Add("size", "3");
			webControl2.Attributes.Add("id", "txtGoto");
			webControl.Controls.Add(webControl2);
			webControl.Controls.Add(new LiteralControl("页"));
			WebControl webControl3 = new WebControl(HtmlTextWriterTag.Input);
			webControl3.Attributes.Add("type", "button");
			webControl3.Attributes.Add("class", this.SkipBtnCssClass);
			webControl3.Attributes.Add("value", "确定");
			webControl3.Attributes.Add("onclick", $"location.href=AppendParameter('{this.PageIndexFormat}',  $.trim($('#txtGoto').val()));");
			webControl.Controls.Add(webControl3);
			webControl.RenderControl(writer);
		}

		private int CalculateTotalPages()
		{
			if (this.TotalRecords == 0)
			{
				return 0;
			}
			int num = this.TotalRecords / this.PageSize;
			if (this.TotalRecords % this.PageSize > 0)
			{
				num++;
			}
			return num;
		}
	}
}
