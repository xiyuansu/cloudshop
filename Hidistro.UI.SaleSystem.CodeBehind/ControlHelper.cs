using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public static class ControlHelper
	{
		public static void SetWhenIsNotNull(this Control control, string value)
		{
			if (control == null)
			{
				return;
			}
			if (control is ITextControl)
			{
				ITextControl textControl = (ITextControl)control;
				textControl.Text = value;
				return;
			}
			if (control is HtmlInputControl)
			{
				HtmlInputControl htmlInputControl = (HtmlInputControl)control;
				htmlInputControl.Value = value;
				return;
			}
			if (control is HyperLink)
			{
				HyperLink hyperLink = (HyperLink)control;
				hyperLink.NavigateUrl = value;
				return;
			}
			throw new ApplicationException("未实现" + control.GetType().ToString() + "的SetWhenIsNotNull方法");
		}
	}
}
