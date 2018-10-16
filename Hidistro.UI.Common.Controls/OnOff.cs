using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class OnOff : Panel
	{
		public Dictionary<string, object> Parameter;

		public int Timeout
		{
			get;
			set;
		}

		public bool SelectedValue
		{
			get
			{
				for (int i = 0; i < this.Controls.Count; i++)
				{
					if (this.Controls[i] is CheckBox)
					{
						CheckBox checkBox = this.Controls[i] as CheckBox;
						return checkBox.Checked;
					}
				}
				return false;
			}
			set
			{
				for (int i = 0; i < this.Controls.Count; i++)
				{
					if (this.Controls[i] is CheckBox)
					{
						CheckBox checkBox = this.Controls[i] as CheckBox;
						checkBox.Checked = value;
					}
				}
			}
		}

		public OnOff()
		{
			this.Parameter = new Dictionary<string, object>();
			this.SetParameter();
		}

		private void SetParameter()
		{
		}

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			CheckBox child = new CheckBox();
			base.Style.Add("display", "none");
			this.Controls.Add(child);
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			CheckBox checkBox = null;
			for (int i = 0; i < this.Controls.Count; i++)
			{
				checkBox = ((this.Controls[i] is CheckBox) ? (this.Controls[i] as CheckBox) : checkBox);
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("$.fn.bootstrapSwitch.defaults.size = 'small'; $('#{1}').show(); $('#{0}').bootstrapSwitch", checkBox.ClientID, this.ClientID);
			stringBuilder.Append("({");
			foreach (KeyValuePair<string, object> item in this.Parameter)
			{
				if (item.Value is int)
				{
					stringBuilder.AppendFormat("{0}:{1},", item.Key, item.Value);
				}
				else if (item.Value is string)
				{
					string text = item.Value.ToString();
					if (text.IndexOf("fu") == 0)
					{
						stringBuilder.AppendFormat("{0}:{1},", item.Key, item.Value);
					}
					else
					{
						stringBuilder.AppendFormat("{0}:'{1}',", item.Key, item.Value);
					}
				}
				else if (item.Value is bool)
				{
					stringBuilder.AppendFormat("{0}:{1},", item.Key, item.Value.ToString().ToLower());
				}
			}
			if (this.Parameter.Count > 0)
			{
				stringBuilder.Remove(stringBuilder.Length - 1, 1);
			}
			stringBuilder.Append("  });");
			if (checkBox.Checked)
			{
				foreach (KeyValuePair<string, object> item2 in this.Parameter)
				{
					if (item2.Value is string)
					{
						string text2 = item2.Value.ToString();
						if (text2.IndexOf("fu") == 0)
						{
							stringBuilder.AppendFormat("{0}(null,{1})", item2.Value, checkBox.Checked.ToString().ToLower());
						}
					}
				}
			}
			int num = (this.Timeout == 0) ? 100 : this.Timeout;
			string script = "$(function () { " + stringBuilder.ToString() + "  }); ";
			this.Page.ClientScript.RegisterClientScriptBlock(base.GetType(), Guid.NewGuid().ToString(), script, true);
		}
	}
}
