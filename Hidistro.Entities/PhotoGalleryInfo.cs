using System;

namespace Hidistro.Entities
{
	[TableName("Hishop_PhotoGallery")]
	public class PhotoGalleryInfo
	{
		[FieldType(FieldType.KeyField)]
		[FieldType(FieldType.IncrementField)]
		public int PhotoId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int CategoryId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string PhotoName
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string PhotoPath
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int FileSize
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime UploadTime
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime LastUpdateTime
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int SupplierId
		{
			get;
			set;
		}
	}
}
