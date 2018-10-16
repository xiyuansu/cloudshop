using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class Ueditor : TextBox
	{
		private bool autoHeightEnabled = true;

		public bool AutoHeightEnabled
		{
			get
			{
				return this.autoHeightEnabled;
			}
			set
			{
				this.autoHeightEnabled = value;
			}
		}

		public int SupplierId
		{
			get;
			set;
		}

		public int ImportLib
		{
			get;
			set;
		}

		public Ueditor()
		{
			this.SetParameter();
		}

		private void SetParameter()
		{
		}

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			this.TextMode = TextBoxMode.MultiLine;
			base.Attributes.Add("style", "min-height:300px;");
		}

		protected override void Render(HtmlTextWriter writer)
		{
			if (this.ImportLib == 0)
			{
				writer.WriteLine("<link rel=\"stylesheet\" href=\"/Utility/Ueditor/css/dist/component-min.css\" />");
				writer.WriteLine(" <link rel=\"stylesheet\" href=\"/Utility/Ueditor/plugins/uploadify/uploadify-min.css\" />");
				writer.WriteLine("<script language='javascript' type='text/javascript' src=\"/Utility/Ueditor/js/dist/lib-min.js\"></script>");
				writer.WriteLine("<script language='javascript' type='text/javascript' src=\"/Utility/Ueditor/js/config.js\"></script>");
				writer.WriteLine("<script language='javascript' type='text/javascript' src=\"/Utility/Ueditor/plugins/uploadify/jquery.uploadify.min.js?ver=940\"></script>");
				writer.WriteLine("<script language='javascript' type='text/javascript' src=\"/Utility/Ueditor/plugins/ueditor/ueditor.config.js\"></script>");
				writer.WriteLine("<script language='javascript' type='text/javascript' src=\"/Utility/Ueditor/plugins/ueditor/ueditor.all.min.js?v=3.0\"></script>");
				writer.WriteLine("<script language='javascript' type='text/javascript' src=\"/Utility/Ueditor/plugins/ueditor/diy_imgpicker.js\"></script>");
				writer.WriteLine("<script language='javascript' type='text/javascript' src=\"/Utility/Ueditor/js/dist/componentindex-min.js\"></script>");
				writer.WriteLine("<style type='text/css'>.CodeMirror-lines span {float: none !important; display: inline !important;} </style>");
			}
			base.Render(writer);
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("if(UE!=null&&UE!=undefined)UE.getEditor('{0}',{1}", this.ClientID, this.SupplierId);
			stringBuilder.Append(",{autoHeightEnabled:" + (this.AutoHeightEnabled ? "true" : "false") + "});");
			string script = "setTimeout(function () { " + stringBuilder.ToString() + "  }, 2000);";
			this.Page.ClientScript.RegisterClientScriptBlock(base.GetType(), Guid.NewGuid().ToString(), script, true);
		}
	}
}
