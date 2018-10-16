using System;

namespace Hidistro.UI.Web.ashxBase
{
	public class HidistroAshxException : Exception
	{
		public HidistroAshxException()
		{
		}

		public HidistroAshxException(string message)
			: base(message)
		{
		}

		public HidistroAshxException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
