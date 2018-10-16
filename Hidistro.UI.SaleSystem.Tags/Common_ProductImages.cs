using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	[ParseChildren(true)]
	public class Common_ProductImages : AscxTemplatedWebControl
	{
		public const string TagID = "common_ProductImages";

		private HiImage imgBig;

		private HiImage imgSmall1;

		private HiImage imgSmall2;

		private HiImage imgSmall3;

		private HiImage imgSmall4;

		private HiImage imgSmall5;

		private HyperLink zoom1;

		private HyperLink iptPicUrl1;

		private HyperLink iptPicUrl2;

		private HyperLink iptPicUrl3;

		private HyperLink iptPicUrl4;

		private HyperLink iptPicUrl5;

		private ProductInfo imageInfo;

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

		public ProductInfo ImageInfo
		{
			get
			{
				return this.imageInfo;
			}
			set
			{
				this.imageInfo = value;
			}
		}

		public bool Is410Image
		{
			get;
			set;
		}

		public bool Is60Image
		{
			get;
			set;
		}

		public Common_ProductImages()
		{
			base.ID = "common_ProductImages";
		}

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/ascx/tags/Common_ViewProduct/Skin-Common_ProductImages.ascx";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.imgBig = (HiImage)this.FindControl("imgBig");
			this.imgSmall1 = (HiImage)this.FindControl("imgSmall1");
			this.imgSmall2 = (HiImage)this.FindControl("imgSmall2");
			this.imgSmall3 = (HiImage)this.FindControl("imgSmall3");
			this.imgSmall4 = (HiImage)this.FindControl("imgSmall4");
			this.imgSmall5 = (HiImage)this.FindControl("imgSmall5");
			this.zoom1 = (HyperLink)this.FindControl("zoom1");
			this.iptPicUrl1 = (HyperLink)this.FindControl("iptPicUrl1");
			this.iptPicUrl2 = (HyperLink)this.FindControl("iptPicUrl2");
			this.iptPicUrl3 = (HyperLink)this.FindControl("iptPicUrl3");
			this.iptPicUrl4 = (HyperLink)this.FindControl("iptPicUrl4");
			this.iptPicUrl5 = (HyperLink)this.FindControl("iptPicUrl5");
			if (!this.Page.IsPostBack)
			{
				this.BindData();
			}
		}

		private void BindData()
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (string.IsNullOrEmpty(this.imageInfo.ImageUrl1) && string.IsNullOrEmpty(this.imageInfo.ImageUrl2) && string.IsNullOrEmpty(this.imageInfo.ImageUrl3) && string.IsNullOrEmpty(this.imageInfo.ImageUrl4) && string.IsNullOrEmpty(this.imageInfo.ImageUrl5))
			{
				this.imageInfo.ImageUrl1 = masterSettings.DefaultProductImage;
			}
			if (!string.IsNullOrEmpty(this.imageInfo.ImageUrl1) && this.imageInfo.ImageUrl1.IndexOf("http://") == -1 && this.imageInfo.ImageUrl1.IndexOf("https://") == -1)
			{
				this.imageInfo.ImageUrl1 = this.imageInfo.ImageUrl1.Replace("//", "/");
			}
			if (!string.IsNullOrEmpty(this.imageInfo.ImageUrl2) && this.imageInfo.ImageUrl2.IndexOf("http://") == -1 && this.imageInfo.ImageUrl2.IndexOf("https://") == -1)
			{
				this.imageInfo.ImageUrl2 = this.imageInfo.ImageUrl2.Replace("//", "/");
			}
			if (!string.IsNullOrEmpty(this.imageInfo.ImageUrl3) && this.imageInfo.ImageUrl3.IndexOf("http://") == -1 && this.imageInfo.ImageUrl3.IndexOf("https://") == -1)
			{
				this.imageInfo.ImageUrl3 = this.imageInfo.ImageUrl3.Replace("//", "/");
			}
			if (!string.IsNullOrEmpty(this.imageInfo.ImageUrl4) && this.imageInfo.ImageUrl4.IndexOf("http://") == -1 && this.imageInfo.ImageUrl4.IndexOf("https://") == -1)
			{
				this.imageInfo.ImageUrl4 = this.imageInfo.ImageUrl4.Replace("//", "/");
			}
			if (!string.IsNullOrEmpty(this.imageInfo.ImageUrl5) && this.imageInfo.ImageUrl5.IndexOf("http://") == -1 && this.imageInfo.ImageUrl5.IndexOf("https://") == -1)
			{
				this.imageInfo.ImageUrl5 = this.imageInfo.ImageUrl5.Replace("//", "/");
			}
			if (this.imageInfo != null)
			{
				string oldValue = "/storage/master/product/images/";
				string newValue = "/storage/master/product/thumbs310/310_";
				if (this.Is410Image)
				{
					newValue = "/storage/master/product/thumbs410/410_";
				}
				string newValue2 = "/storage/master/product/thumbs40/40_";
				if (this.Is60Image)
				{
					newValue2 = "/storage/master/product/thumbs60/60_";
				}
				string text = "";
				if (!string.IsNullOrEmpty(this.imageInfo.ImageUrl1))
				{
					text = this.imageInfo.ImageUrl1.Replace("//", "/");
					if (this.imageInfo.ImageUrl1.IndexOf("http://") >= 0 || this.imageInfo.ImageUrl1.IndexOf("https://") >= 0)
					{
						text = this.imageInfo.ImageUrl1;
					}
					this.imgBig.ImageUrl = Globals.GetImageServerUrl("http://", this.imageInfo.ImageUrl1.ToLower().Replace(oldValue, newValue));
					this.imgSmall1.ImageUrl = Globals.GetImageServerUrl("http://", this.imageInfo.ImageUrl1.ToLower().Replace(oldValue, newValue2));
					HyperLink hyperLink = this.zoom1;
					HyperLink hyperLink2 = this.iptPicUrl1;
					string text3 = hyperLink.NavigateUrl = (hyperLink2.NavigateUrl = Globals.GetImageServerUrl("http://", text));
					this.zoom1.Attributes["title"] = this.imageInfo.ProductName;
					this.iptPicUrl1.Attributes["title"] = this.imageInfo.ProductName;
					this.iptPicUrl1.Attributes["rel"] = "useZoom: 'zoom1', smallImage: '" + Globals.GetImageServerUrl("http://", this.imageInfo.ImageUrl1.ToLower().Replace(oldValue, newValue)) + "'";
				}
				if (!string.IsNullOrEmpty(this.imageInfo.ImageUrl2))
				{
					text = this.imageInfo.ImageUrl2;
					if (this.imageInfo.ImageUrl2.IndexOf("http://") >= 0 || this.imageInfo.ImageUrl2.IndexOf("https://") >= 0)
					{
						text = this.imageInfo.ImageUrl2;
					}
					this.iptPicUrl2.NavigateUrl = Globals.GetImageServerUrl("http://", text);
					this.iptPicUrl2.Attributes["title"] = this.imageInfo.ProductName;
					this.iptPicUrl2.Attributes["rel"] = "useZoom: 'zoom1', smallImage: '" + Globals.GetImageServerUrl("http://", this.imageInfo.ImageUrl2.ToLower().Replace(oldValue, newValue)) + "'";
					this.imgSmall2.ImageUrl = Globals.GetImageServerUrl("http://", this.imageInfo.ImageUrl2.ToLower().Replace(oldValue, newValue2));
				}
				if (!string.IsNullOrEmpty(this.imageInfo.ImageUrl3))
				{
					text = this.imageInfo.ImageUrl3;
					if (this.imageInfo.ImageUrl3.IndexOf("http://") >= 0 || this.imageInfo.ImageUrl3.IndexOf("https://") >= 0)
					{
						text = this.imageInfo.ImageUrl3;
					}
					this.iptPicUrl3.NavigateUrl = Globals.GetImageServerUrl("http://", text);
					this.iptPicUrl3.Attributes["title"] = this.imageInfo.ProductName;
					this.iptPicUrl3.Attributes["rel"] = "useZoom: 'zoom1', smallImage: '" + Globals.GetImageServerUrl("http://", this.imageInfo.ImageUrl3.ToLower().Replace(oldValue, newValue)) + "'";
					this.imgSmall3.ImageUrl = Globals.GetImageServerUrl("http://", this.imageInfo.ImageUrl3.ToLower().Replace(oldValue, newValue2));
				}
				if (!string.IsNullOrEmpty(this.imageInfo.ImageUrl4))
				{
					text = this.imageInfo.ImageUrl4;
					if (this.imageInfo.ImageUrl4.IndexOf("http://") >= 0 || this.imageInfo.ImageUrl4.IndexOf("https://") >= 0)
					{
						text = this.imageInfo.ImageUrl4;
					}
					this.iptPicUrl4.NavigateUrl = Globals.GetImageServerUrl("http://", text);
					this.iptPicUrl4.Attributes["title"] = this.imageInfo.ProductName;
					this.iptPicUrl4.Attributes["rel"] = "useZoom: 'zoom1', smallImage: '" + Globals.GetImageServerUrl("http://", this.imageInfo.ImageUrl4.ToLower().Replace(oldValue, newValue)) + "'";
					this.imgSmall4.ImageUrl = Globals.GetImageServerUrl("http://", this.imageInfo.ImageUrl4.ToLower().Replace(oldValue, newValue2));
				}
				if (!string.IsNullOrEmpty(this.imageInfo.ImageUrl5))
				{
					text = this.imageInfo.ImageUrl5;
					if (this.imageInfo.ImageUrl5.IndexOf("http://") >= 0 || this.imageInfo.ImageUrl5.IndexOf("https://") >= 0)
					{
						text = this.imageInfo.ImageUrl5;
					}
					this.iptPicUrl5.NavigateUrl = Globals.GetImageServerUrl("http://", text);
					this.iptPicUrl5.Attributes["title"] = this.imageInfo.ProductName;
					this.iptPicUrl5.Attributes["rel"] = "useZoom: 'zoom1', smallImage: '" + Globals.GetImageServerUrl("http://", this.imageInfo.ImageUrl5.ToLower().Replace(oldValue, newValue)) + "'";
					this.imgSmall5.ImageUrl = Globals.GetImageServerUrl("http://", this.imageInfo.ImageUrl5.ToLower().Replace(oldValue, newValue2));
				}
			}
		}
	}
}
