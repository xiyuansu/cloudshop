using Hidistro.Context;
using System.Globalization;
using System.Web.UI;

namespace Hidistro.UI.Common.Controls
{
	[ParseChildren(true)]
	[PersistChildren(false)]
	public abstract class AscxTemplatedWebControl : TemplatedWebControl
	{
		private string skinName;

		protected virtual string SkinPath
		{
			get
			{
				if (this.IsThemeTags())
				{
					if (this.SkinName.StartsWith("/"))
					{
						return HiContext.Current.GetPCHomePageSkinPath() + this.SkinName;
					}
					return HiContext.Current.GetPCHomePageSkinPath() + "/" + this.SkinName;
				}
				if (this.SkinName.StartsWith("/"))
				{
					return HiContext.Current.GetSkinPath() + this.SkinName;
				}
				return HiContext.Current.GetSkinPath() + "/" + this.SkinName;
			}
		}

		public virtual string SkinName
		{
			get
			{
				return this.skinName;
			}
			set
			{
				if (!string.IsNullOrEmpty(value))
				{
					value = value.ToLower(CultureInfo.InvariantCulture);
					if (value.EndsWith(".ascx"))
					{
						this.skinName = value;
					}
				}
			}
		}

		private bool SkinFileExists
		{
			get
			{
				return !string.IsNullOrEmpty(this.SkinName);
			}
		}

		protected override void CreateChildControls()
		{
			this.Controls.Clear();
			if (this.LoadThemedControl())
			{
				this.AttachChildControls();
				return;
			}
			throw new SkinNotFoundException(this.SkinPath);
		}

		protected virtual bool LoadThemedControl()
		{
			if (this.SkinFileExists && this.Page != null)
			{
				Control control = this.Page.LoadControl(this.SkinPath);
				control.ID = "_";
				this.Controls.Add(control);
				return true;
			}
			return false;
		}

		private bool IsThemeTags()
		{
			return this.SkinName.ToLower().Contains("hometags");
		}
	}
}
