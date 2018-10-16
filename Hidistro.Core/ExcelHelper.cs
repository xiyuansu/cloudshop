using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace Hidistro.Core
{
	public class ExcelHelper
	{
		private DownloadHelper downHelper = null;

		public ExcelHelper()
		{
			this.downHelper = new DownloadHelper();
		}

		public DataSet ExcelToDataSet(string path)
		{
			DataSet dataSet = new DataSet();
			IWorkbook workbook = WorkbookFactory.Create(path);
			for (int i = 0; i < workbook.NumberOfSheets; i++)
			{
				ISheet sheetAt = workbook.GetSheetAt(i);
				DataTable dataTable = new DataTable(sheetAt.SheetName);
				if (sheetAt.PhysicalNumberOfRows >= 1)
				{
					int num = sheetAt.LastRowNum + 1;
					int num2 = 0;
					IRow row = sheetAt.GetRow(0);
					if (row != null)
					{
						num2 = row.LastCellNum;
					}
					for (int j = 0; j < num2; j++)
					{
						IRow row2 = sheetAt.GetRow(0);
						if (row2 != null)
						{
							ICell cell = row2.GetCell(j);
							if (cell != null)
							{
								dataTable.Columns.Add(cell.ToString());
							}
						}
					}
					for (int k = 1; k < num; k++)
					{
						DataRow row3 = dataTable.NewRow();
						IRow row4 = sheetAt.GetRow(k);
						if (row4 != null)
						{
							for (int l = 0; l < num2; l++)
							{
								ICell cell2 = row4.GetCell(l);
								if (cell2 != null)
								{
									row3.SetField(l, cell2.ToString());
								}
							}
						}
						dataTable.Rows.Add(row3);
					}
					dataSet.Tables.Add(dataTable);
				}
			}
			return dataSet;
		}

		public void DataSetToExcel(DataSet ds, string fileName)
		{
			if (ds != null && ds.Tables.Count > 0)
			{
				IWorkbook wb = this.CreateSheet(fileName);
				foreach (DataTable table in ds.Tables)
				{
					if (table != null && table.Rows.Count > 0)
					{
						this.ImportToWorkbook(table, ref wb);
					}
				}
				this.downHelper.DownloadByOutputStreamBlock(new MemoryStream(this.ToByte(wb)), fileName);
			}
		}

		public void DataSetToExcel(DataSet ds, string path, string fileName)
		{
			if (ds != null && ds.Tables.Count > 0)
			{
				IWorkbook workbook = this.CreateSheet(fileName);
				foreach (DataTable table in ds.Tables)
				{
					if (table != null && table.Rows.Count > 0)
					{
						this.ImportToWorkbook(table, ref workbook);
					}
				}
				string str = HttpContext.Current.Server.MapPath(path);
				using (FileStream fileStream = new FileStream(str + "\\" + fileName, FileMode.Create, FileAccess.Write))
				{
					workbook.Write(fileStream);
					fileStream.Flush();
				}
			}
		}

		public void ListToExcel(IList[] listArray, string fileName)
		{
			this.DataSetToExcel(this.ConvertToDataSet(listArray), fileName);
		}

		public void DataTableToExcel(DataTable dt, string fileName)
		{
			if (dt != null && dt.Rows.Count > 0)
			{
				IWorkbook wb = this.CreateSheet(fileName);
				this.ImportToWorkbook(dt, ref wb);
				this.downHelper.DownloadByOutputStreamBlock(new MemoryStream(this.ToByte(wb)), fileName);
			}
		}

		private DataSet ConvertToDataSet(IList[] listArray)
		{
			DataSet dataSet = new DataSet();
			foreach (IList list in listArray)
			{
				if (list != null && list.Count > 0)
				{
					object obj = list[0];
					string name = obj.GetType().Name;
					object[] customAttributes = obj.GetType().GetCustomAttributes(typeof(EntityMappingAttribute), true);
					if (customAttributes.Length != 0)
					{
						name = ((EntityMappingAttribute)customAttributes[0]).Name;
					}
					DataTable dataTable = new DataTable(name);
					PropertyInfo[] properties = obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
					PropertyInfo[] array = properties;
					foreach (PropertyInfo propertyInfo in array)
					{
						object[] customAttributes2 = propertyInfo.GetCustomAttributes(typeof(EntityMappingAttribute), true);
						if (customAttributes2.Length != 0)
						{
							dataTable.Columns.Add(((EntityMappingAttribute)customAttributes2[0]).Name);
						}
						else
						{
							dataTable.Columns.Add(propertyInfo.Name);
						}
					}
					for (int k = 0; k < list.Count; k++)
					{
						DataRow row = dataTable.NewRow();
						object obj2 = list[k];
						PropertyInfo[] properties2 = obj2.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
						for (int l = 0; l < properties2.Count(); l++)
						{
							row.SetField(l, properties2[l].GetValue(obj, null));
						}
						dataTable.Rows.Add(row);
					}
					dataSet.Tables.Add(dataTable);
				}
			}
			return dataSet;
		}

		private void ImportToWorkbook(DataTable dt, ref IWorkbook wb)
		{
			string sheetname = (!string.IsNullOrEmpty(dt.TableName)) ? dt.TableName : ("Sheet" + (wb.NumberOfSheets + 1).ToString());
			ISheet sheet = wb.CreateSheet(sheetname);
			IRow row = sheet.CreateRow(0);
			this.SetRow(row, this.GetCloumnNames(dt), this.GetCellStyle(sheet.Workbook, FontBoldWeight.Bold));
			IRow row2 = null;
			ICellStyle cellStyle = this.GetCellStyle(sheet.Workbook);
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				row2 = sheet.CreateRow(i + 1);
				this.SetRow(row2, this.GetRowValues(dt.Rows[i]), cellStyle);
			}
			this.AutoSizeColumn(sheet);
		}

		private byte[] ToByte(IWorkbook wb)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				wb.Write(memoryStream);
				return memoryStream.ToArray();
			}
		}

		private IWorkbook CreateSheet(string path)
		{
			IWorkbook result = new HSSFWorkbook();
			string a = Path.GetExtension(path).ToLower();
			if (a == ".xls")
			{
				result = new HSSFWorkbook();
			}
			else if (a == ".xlsx")
			{
				result = new XSSFWorkbook();
			}
			return result;
		}

		private int GetWidth(DataTable dt, int columnIndex)
		{
			IList<int> list = new List<int>();
			foreach (DataRow row in dt.Rows)
			{
				list.Add(Convert.ToString(row[columnIndex]).Length * 256);
			}
			return list.Max();
		}

		private IList<string> GetRowValues(DataRow dr)
		{
			List<string> list = new List<string>();
			for (int i = 0; i < dr.Table.Columns.Count; i++)
			{
				if (dr[i] != null && dr[i] != DBNull.Value)
				{
					list.Add(dr[i].ToString());
				}
				else
				{
					list.Add(string.Empty);
				}
			}
			return list;
		}

		private IList<string> GetCloumnNames(DataTable dt)
		{
			List<string> list = new List<string>();
			foreach (DataColumn column in dt.Columns)
			{
				list.Add(column.ColumnName);
			}
			return list;
		}

		private void SetRow(IRow row, IList<string> values)
		{
			this.SetRow(row, values, null);
		}

		private void SetRow(IRow row, IList<string> values, ICellStyle cellStyle)
		{
			for (int i = 0; i < values.Count; i++)
			{
				ICell cell = row.CreateCell(i);
				cell.SetCellValue(values[i]);
				if (cellStyle != null)
				{
					cell.CellStyle = cellStyle;
				}
			}
		}

		private ICellStyle GetCellStyle(IWorkbook wb)
		{
			return this.GetCellStyle(wb, FontBoldWeight.None);
		}

		private ICellStyle GetCellStyle(IWorkbook wb, FontBoldWeight boldweight)
		{
			ICellStyle cellStyle = wb.CreateCellStyle();
			IFont font = wb.CreateFont();
			font.FontHeightInPoints = 10;
			font.FontName = "微软雅黑";
			font.Color = 32767;
			font.Boldweight = (short)boldweight;
			cellStyle.SetFont(font);
			cellStyle.Alignment = HorizontalAlignment.Center;
			cellStyle.VerticalAlignment = VerticalAlignment.Center;
			cellStyle.BorderTop = BorderStyle.Thin;
			cellStyle.BorderBottom = BorderStyle.Thin;
			cellStyle.BorderLeft = BorderStyle.Thin;
			cellStyle.BorderRight = BorderStyle.Thin;
			cellStyle.FillForegroundColor = 9;
			cellStyle.FillPattern = FillPattern.SolidForeground;
			cellStyle.WrapText = false;
			cellStyle.Indention = 0;
			return cellStyle;
		}

		private void AutoSizeColumn(ISheet sheet)
		{
			IRow row = sheet.GetRow(0);
			if (row != null)
			{
				for (int i = 0; i < row.LastCellNum; i++)
				{
					sheet.AutoSizeColumn(i);
				}
			}
		}

		private void AutoSizeColumn(ISheet sheet, int columnNum)
		{
			int num = sheet.GetColumnWidth(columnNum) / 256;
			for (int i = 0; i <= sheet.LastRowNum; i++)
			{
				IRow row = sheet.GetRow(i);
				if (row != null)
				{
					ICell cell = row.GetCell(columnNum);
					if (cell != null)
					{
						int num2 = Encoding.Default.GetBytes(cell.ToString()).Length;
						if (num < num2)
						{
							num = num2;
						}
					}
				}
			}
			sheet.SetColumnWidth(columnNum, num * 256);
		}

		public static string GetXLSFieldsTD(object argFields, bool istext)
		{
			if (argFields == null)
			{
				argFields = string.Empty;
			}
			else
			{
				string a = argFields.GetType().ToString();
				if (a == "System.DateTime")
				{
					DateTime? nullable = argFields.ToDateTime();
					argFields = ((!nullable.HasValue || nullable.Equals("0001/1/1 0:00:00")) ? "" : argFields);
				}
			}
			string arg = istext ? " style='vnd.ms-excel.numberformat:@'" : "";
			return $"<td{arg}>{argFields}</td>";
		}
	}
}
