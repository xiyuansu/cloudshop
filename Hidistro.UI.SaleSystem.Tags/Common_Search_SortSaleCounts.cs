using System;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_Search_SortSaleCounts : LinkButton
	{
		public delegate void SortingHandler(string sortOrder, string sortOrderBy);

		private string imageFormat = "<img border=\"0\" src=\"{0}\" alt=\"{1}\" />";

		public const string TagID = "btn_Common_Search_SortSaleCounts";

		private bool showText = true;

		private ImagePosition position;

		public override string ID
		{
			get
			{
				return base.ID;
			}
			set
			{
			}
		}

		public string DefaultImageUrl
		{
			get;
			set;
		}

		public string AscImageUrl
		{
			get;
			set;
		}

		public string DescImageUrl
		{
			get;
			set;
		}

		public string Alt
		{
			get;
			set;
		}

		public bool ShowText
		{
			get
			{
				return this.showText;
			}
			set
			{
				this.showText = value;
			}
		}

		public ImagePosition ImagePosition
		{
			get
			{
				return this.position;
			}
			set
			{
				this.position = value;
			}
		}

		public string ImageUrl
		{
			get
			{
				if (this.ViewState["Src"] != null)
				{
					return (string)this.ViewState["Src"];
				}
				return null;
			}
			set
			{
				if (!string.IsNullOrEmpty(value))
				{
					this.ViewState["Src"] = value;
				}
				else
				{
					this.ViewState["Src"] = null;
				}
			}
		}

		public event SortingHandler Sorting;

		private void OnSorting(string sortOrder, string sortOrderBy)
		{
			if (this.Sorting != null)
			{
				this.Sorting(sortOrder, sortOrderBy);
			}
		}

		public Common_Search_SortSaleCounts()
		{
			base.ID = "btn_Common_Search_SortSaleCounts";
			this.ShowText = false;
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			base.Click += this.Common_Search_SortSaleCounts_Click;
		}

		private void Common_Search_SortSaleCounts_Click(object sender, EventArgs e)
		{
			string empty = string.Empty;
			empty = ((!(this.Page.Request.QueryString["sortOrder"] == "Desc")) ? "Desc" : "Asc");
			this.OnSorting(empty, "ShowSaleCounts");
		}

		protected override void Render(HtmlTextWriter writer)
		{
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["sortOrder"]) && this.Page.Request.QueryString["sortOrderBy"] == "ShowSaleCounts")
			{
				if (this.Page.Request.QueryString["sortOrder"] == "Desc")
				{
					this.ImageUrl = this.DescImageUrl;
					this.Alt = "按销量升序排序";
				}
				else
				{
					this.ImageUrl = this.AscImageUrl;
					this.Alt = "按销量降序排序";
				}
				this.ToolTip = this.Alt;
			}
			else
			{
				this.ImageUrl = this.DefaultImageUrl;
				this.ToolTip = "按销量排序";
			}
			base.Attributes.Add("name", this.NamingContainer.UniqueID + "$" + this.ID);
			string imageTag = this.GetImageTag();
			if (!this.ShowText)
			{
				base.Text = "";
			}
			if (this.ImagePosition == ImagePosition.Right)
			{
				base.Text += imageTag;
			}
			else
			{
				base.Text = imageTag + base.Text;
			}
			base.Render(writer);
		}

		private string GetImageTag()
		{
			if (string.IsNullOrEmpty(this.ImageUrl))
			{
				return string.Empty;
			}
			return string.Format(CultureInfo.InvariantCulture, this.imageFormat, new object[2]
			{
				this.ImageUrl,
				this.Alt
			});
		}
	}
}
