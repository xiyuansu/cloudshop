using System.Runtime.InteropServices;

namespace Hidistro.Core
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct JavaScripts
	{
		public static readonly string regularNumberAndDecimal = "/[^\\d.]/g";

		public static readonly string regularNumber = "/[^\\d]/g";

		public static readonly string numberGreaterThanZero = "[0-9]\\\\d*(\\\\.\\\\d+)?";

		public static readonly string ZipCode = "[1-9][0-9]{5}";
	}
}
