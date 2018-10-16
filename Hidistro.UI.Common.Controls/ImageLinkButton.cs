using Hidistro.Context;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class ImageLinkButton : LinkButton
	{
		private string imageFormat = "<img border=\"0\" src=\"{0}\" alt=\"{1}\" />";

		private ImagePosition position = ImagePosition.Left;

		private bool isShow = false;

		private string deleteMsg = "确定要执行该删除操作吗？删除后将不可以恢复！";

		private string alt;

		private bool showText = true;

		public bool IsShow
		{
			get
			{
				return this.isShow;
			}
			set
			{
				this.isShow = value;
			}
		}

		public string DeleteMsg
		{
			get
			{
				return this.deleteMsg;
			}
			set
			{
				this.deleteMsg = value;
			}
		}

		public string Alt
		{
			get
			{
				return this.alt;
			}
			set
			{
				this.alt = value;
			}
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
					string value2 = (!value.StartsWith("~")) ? ((!value.StartsWith("/")) ? (HiContext.Current.GetSkinPath() + "/" + value) : (HiContext.Current.GetSkinPath() + value)) : base.ResolveUrl(value);
					this.ViewState["Src"] = value2;
				}
				else
				{
					this.ViewState["Src"] = null;
				}
			}
		}

		protected override void Render(HtmlTextWriter writer)
		{
			string empty = string.Empty;
			if (this.IsShow)
			{
				string value = $"return  confirm('{this.DeleteMsg}');";
				base.Attributes.Add("OnClick", value);
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
