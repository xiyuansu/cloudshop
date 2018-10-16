using Hidistro.Context;
using System.IO;
using System.Web.UI;

namespace Hidistro.UI.Common.Controls
{
	internal class FormFixerHtml32TextWriter : Html32TextWriter
	{
		private string _url;

		internal FormFixerHtml32TextWriter(TextWriter writer)
			: base(writer)
		{
			this._url = HiContext.Current.Context.Request.RawUrl;
		}

		public override void WriteAttribute(string name, string value, bool encode)
		{
			if (this._url != null && string.Compare(name, "action", true) == 0)
			{
				value = this._url;
			}
			base.WriteAttribute(name, value, encode);
		}
	}
}
