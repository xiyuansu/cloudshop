using System.IO;
using System.Net;
using System.Text;
using System.Web.UI;

namespace Hidistro.UI.Common.Controls
{
	[ParseChildren(true)]
	[PersistChildren(false)]
	public abstract class VActivityidTemplatedWebControl : WAPMemberTemplatedWebControl
	{
		private new string GetResponseResult(string url)
		{
			WebRequest webRequest = WebRequest.Create(url);
			using (HttpWebResponse httpWebResponse = (HttpWebResponse)webRequest.GetResponse())
			{
				using (Stream stream = httpWebResponse.GetResponseStream())
				{
					using (StreamReader streamReader = new StreamReader(stream, Encoding.UTF8))
					{
						return streamReader.ReadToEnd();
					}
				}
			}
		}
	}
}
