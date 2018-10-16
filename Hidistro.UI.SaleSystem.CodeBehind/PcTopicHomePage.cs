using Hidistro.UI.Common.Controls;
using System;
using System.ComponentModel;
using System.Web.UI;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class PcTopicHomePage : HtmlTemplatedWebControl
	{
		[Bindable(true)]
		public string TempFilePath
		{
			get;
			set;
		}

		public int TopicId
		{
			get;
			set;
		}

		protected override void OnInit(EventArgs e)
		{
			this.TempFilePath = "Skin-PcTopicHomePage_{0}.html";
			if (this.SkinName == null)
			{
				this.SkinName = string.Format(this.TempFilePath, this.TopicId);
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
		}
	}
}
