using System.IO;

namespace HiShop.API.Setting.Helpers
{
	public class FileHelper
	{
		public static FileStream GetFileStream(string fileName)
		{
			FileStream result = null;
			if (!string.IsNullOrEmpty(fileName) && File.Exists(fileName))
			{
				result = new FileStream(fileName, FileMode.Open);
			}
			return result;
		}
	}
}
