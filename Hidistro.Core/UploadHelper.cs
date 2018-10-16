using System.IO;
using System.Web;
using System.Web.UI.WebControls;

namespace Hidistro.Core
{
	public class UploadHelper
	{
		public static string FileUpload(string virtualPath, FileUpload fileUpload)
		{
			return UploadHelper.FileUpload(virtualPath, fileUpload.PostedFile);
		}

		public static string FileUpload(string virtualPath, HttpPostedFile postedFile)
		{
			string text = HttpContext.Current.Server.MapPath(virtualPath);
			string fileName = Path.GetFileName(postedFile.FileName);
			string text2 = text + "\\" + fileName;
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			if (File.Exists(text2))
			{
				File.Delete(text2);
			}
			postedFile.SaveAs(text2);
			return text2;
		}

		public static bool IsExcelFile(string fileName)
		{
			string extension = Path.GetExtension(fileName);
			string[] array = new string[2]
			{
				".xls",
				".xlsx"
			};
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].ToLower() == extension.ToLower())
				{
					return true;
				}
			}
			return false;
		}
	}
}
