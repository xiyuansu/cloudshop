using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.PictureMange)]
	public class ImageData : AdminPage
	{
		public string GlobalsPath = Globals.GetImageServerUrl();

		protected ImageDataGradeDropDownList dropSearchImageFtp;

		protected HiddenField hidImageType;

		protected ImageOrderDropDownList ImageOrder;

		protected HiddenField HiddenFieldImag;

		protected HiddenField hfPhotoId;

		protected HiddenField ReImageDataNameId;

		protected TextBox ReImageDataName;

		protected HiddenField RePlaceImg;

		protected HiddenField RePlaceId;

		protected FileUpload FileUpload;

		protected ImageDataGradeDropDownList dropImageFtp;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				this.dropSearchImageFtp.DataBind();
				this.ImageOrder.DataBind();
				this.dropImageFtp.DataBind();
			}
		}
	}
}
