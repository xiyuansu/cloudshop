using Hidistro.Core;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;

namespace Hidistro.Jobs
{
	public class BaseDao
	{
		protected Database database;

		public BaseDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}

		public virtual long Add(object model, DbTransaction dbTran = null)
		{
			Type type = model.GetType();
			TableNameAttribute tableNameAttribute = (TableNameAttribute)Attribute.GetCustomAttribute(type, typeof(TableNameAttribute));
			PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" ");
			string text = "";
			string text2 = "";
			bool flag = false;
			PropertyInfo[] array = properties;
			foreach (PropertyInfo propertyInfo in array)
			{
				object[] customAttributes = propertyInfo.GetCustomAttributes(typeof(FieldTypeAttribute), true);
				if (customAttributes.Length != 0)
				{
					if (this.IsIncrementField(customAttributes))
					{
						flag = true;
					}
					else
					{
						text = text + propertyInfo.Name + ",";
						this.database.AddInParameter(sqlStringCommand, propertyInfo.Name, this.GetDbType(propertyInfo.PropertyType), propertyInfo.GetValue(model, null));
					}
				}
			}
			text = text.Remove(text.Length - 1);
			text2 = "@" + text.Replace(",", ",@");
			sqlStringCommand.CommandText = "INSERT INTO " + tableNameAttribute.TableName + "(" + text + ")VALUES(" + text2 + ")";
			if (flag)
			{
				DbCommand dbCommand = sqlStringCommand;
				dbCommand.CommandText += " SELECT @@IDENTITY";
				if (dbTran != null)
				{
					return this.database.ExecuteScalar(sqlStringCommand, dbTran).ToLong(0);
				}
				return this.database.ExecuteScalar(sqlStringCommand).ToLong(0);
			}
			if (dbTran != null)
			{
				return this.database.ExecuteNonQuery(sqlStringCommand, dbTran);
			}
			return this.database.ExecuteNonQuery(sqlStringCommand);
		}

		public virtual bool Update(object model, DbTransaction dbTran = null)
		{
			Type type = model.GetType();
			TableNameAttribute tableNameAttribute = (TableNameAttribute)Attribute.GetCustomAttribute(type, typeof(TableNameAttribute));
			PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" ");
			string text = "";
			string text2 = "";
			PropertyInfo[] array = properties;
			foreach (PropertyInfo propertyInfo in array)
			{
				object[] customAttributes = propertyInfo.GetCustomAttributes(typeof(FieldTypeAttribute), true);
				if (customAttributes.Length != 0)
				{
					if (this.IsKeyField(customAttributes))
					{
						text2 = text2 + propertyInfo.Name + "=@" + propertyInfo.Name + " AND ";
					}
					else
					{
						text = text + propertyInfo.Name + "=@" + propertyInfo.Name + ",";
					}
					this.database.AddInParameter(sqlStringCommand, propertyInfo.Name, this.GetDbType(propertyInfo.PropertyType), propertyInfo.GetValue(model, null));
				}
			}
			text = text.Remove(text.Length - 1);
			text2 = text2.Remove(text2.Length - 4);
			sqlStringCommand.CommandText = "UPDATE " + tableNameAttribute.TableName + " SET " + text + " WHERE " + text2;
			if (dbTran == null)
			{
				return this.database.ExecuteNonQuery(sqlStringCommand) >= 1;
			}
			return this.database.ExecuteNonQuery(sqlStringCommand, dbTran) >= 1;
		}

		public virtual bool Delete<T>(long keyField)
		{
			Type typeFromHandle = typeof(T);
			TableNameAttribute tableNameAttribute = (TableNameAttribute)Attribute.GetCustomAttribute(typeFromHandle, typeof(TableNameAttribute));
			PropertyInfo[] properties = typeFromHandle.GetProperties(BindingFlags.Instance | BindingFlags.Public);
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" ");
			string text = "";
			PropertyInfo[] array = properties;
			foreach (PropertyInfo propertyInfo in array)
			{
				object[] customAttributes = propertyInfo.GetCustomAttributes(typeof(FieldTypeAttribute), true);
				if (customAttributes.Length != 0 && this.IsKeyField(customAttributes))
				{
					text = text + propertyInfo.Name + "=@" + propertyInfo.Name;
					this.database.AddInParameter(sqlStringCommand, propertyInfo.Name, this.GetDbType(propertyInfo.PropertyType), keyField);
					break;
				}
			}
			sqlStringCommand.CommandText = "DELETE FROM " + tableNameAttribute.TableName + " WHERE " + text;
			return this.database.ExecuteNonQuery(sqlStringCommand) >= 1;
		}

		public virtual T Get<T>(long keyField) where T : new()
		{
			Type typeFromHandle = typeof(T);
			T val = new T();
			TableNameAttribute tableNameAttribute = (TableNameAttribute)Attribute.GetCustomAttribute(typeFromHandle, typeof(TableNameAttribute));
			PropertyInfo[] properties = typeFromHandle.GetProperties(BindingFlags.Instance | BindingFlags.Public);
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" ");
			string text = "";
			PropertyInfo[] array = properties;
			foreach (PropertyInfo propertyInfo in array)
			{
				object[] customAttributes = propertyInfo.GetCustomAttributes(typeof(FieldTypeAttribute), true);
				if (customAttributes.Length != 0 && this.IsKeyField(customAttributes))
				{
					text = text + propertyInfo.Name + "=@" + propertyInfo.Name;
					this.database.AddInParameter(sqlStringCommand, propertyInfo.Name, this.GetDbType(propertyInfo.PropertyType), keyField);
					break;
				}
			}
			sqlStringCommand.CommandText = "SELECT * FROM " + tableNameAttribute.TableName + " WHERE " + text;
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					int fieldCount = dataReader.FieldCount;
					for (int j = 0; j < fieldCount; j++)
					{
						if (((IDataRecord)dataReader)[j] != DBNull.Value)
						{
							PropertyInfo property = typeFromHandle.GetProperty(dataReader.GetName(j), BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty);
							if (property != (PropertyInfo)null)
							{
								Type type = property.PropertyType;
								if (type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
								{
									NullableConverter nullableConverter = new NullableConverter(type);
									type = nullableConverter.UnderlyingType;
								}
								if (type.IsEnum)
								{
									object value = Enum.ToObject(type, ((IDataRecord)dataReader)[j]);
									property.SetValue(val, value, null);
								}
								else
								{
									object obj = Convert.ChangeType(((IDataRecord)dataReader)[j], type);
									if (type.Equals(typeof(string)) && obj == null)
									{
										obj = string.Empty;
									}
									property.SetValue(val, obj, null);
								}
							}
						}
					}
					goto end_IL_0111;
				}
				return default(T);
				end_IL_0111:;
			}
			return val;
		}

		public virtual IList<T> Gets<T>(string sortBy, SortAction sortOrder, int? maxNum = default(int?)) where T : new()
		{
			Type typeFromHandle = typeof(T);
			List<T> list = new List<T>();
			TableNameAttribute tableNameAttribute = (TableNameAttribute)Attribute.GetCustomAttribute(typeFromHandle, typeof(TableNameAttribute));
			string text = "SELECT";
			if (maxNum.HasValue)
			{
				text = text + " TOP " + maxNum.Value;
			}
			text = text + " * FROM " + tableNameAttribute.TableName + " ORDER BY " + sortBy + " " + sortOrder.ToString();
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					T val = new T();
					int fieldCount = dataReader.FieldCount;
					for (int i = 0; i < fieldCount; i++)
					{
						if (((IDataRecord)dataReader)[i] != DBNull.Value)
						{
							PropertyInfo property = typeFromHandle.GetProperty(dataReader.GetName(i), BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty);
							if (property != (PropertyInfo)null)
							{
								Type type = property.PropertyType;
								if (type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
								{
									NullableConverter nullableConverter = new NullableConverter(type);
									type = nullableConverter.UnderlyingType;
								}
								if (type.IsEnum)
								{
									object value = Enum.ToObject(type, ((IDataRecord)dataReader)[i]);
									property.SetValue(val, value, null);
								}
								else
								{
									object obj = Convert.ChangeType(((IDataRecord)dataReader)[i], type);
									if (type.Equals(typeof(string)) && obj == null)
									{
										obj = string.Empty;
									}
									property.SetValue(val, obj, null);
								}
							}
						}
					}
					list.Add(val);
				}
			}
			return list;
		}

		public virtual bool SaveSequence<T>(int keyId, int sequence, string keyName = null) where T : new()
		{
			bool result = false;
			try
			{
				Type typeFromHandle = typeof(T);
				TableNameAttribute tableNameAttribute = (TableNameAttribute)Attribute.GetCustomAttribute(typeFromHandle, typeof(TableNameAttribute));
				DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" ");
				if (!string.IsNullOrEmpty(keyName))
				{
					sqlStringCommand.CommandText = "update " + tableNameAttribute.TableName + " set DisplaySequence=" + sequence + " WHERE " + keyName + "=" + keyId;
				}
				else
				{
					PropertyInfo[] array = HiCache.Get<PropertyInfo[]>($"Table-PropertyInfo-{tableNameAttribute.TableName}");
					if (array == null)
					{
						array = typeFromHandle.GetProperties(BindingFlags.Instance | BindingFlags.Public);
						HiCache.Insert($"Table-PropertyInfo-{tableNameAttribute.TableName}", array);
					}
					PropertyInfo propertyInfo = (from t in array
					where this.IsKeyField(t.GetCustomAttributes(typeof(FieldTypeAttribute), true))
					select t).FirstOrDefault();
					if (propertyInfo != (PropertyInfo)null)
					{
						sqlStringCommand.CommandText = "update " + tableNameAttribute.TableName + " set DisplaySequence=" + sequence + " WHERE " + propertyInfo.Name + "=" + keyId;
					}
				}
				result = (this.database.ExecuteNonQuery(sqlStringCommand) >= 1);
			}
			catch (Exception)
			{
			}
			return result;
		}

		public virtual int GetMaxDisplaySequence<T>() where T : new()
		{
			Type typeFromHandle = typeof(T);
			TableNameAttribute tableNameAttribute = (TableNameAttribute)Attribute.GetCustomAttribute(typeFromHandle, typeof(TableNameAttribute));
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" ");
			if (!string.IsNullOrEmpty(tableNameAttribute.TableName))
			{
				sqlStringCommand.CommandText = "select isnull(max(DisplaySequence),0)+1 from " + tableNameAttribute.TableName;
				return (int)this.database.ExecuteScalar(sqlStringCommand);
			}
			return 0;
		}

		private bool IsIncrementField(object[] fieldTypes)
		{
			foreach (object obj in fieldTypes)
			{
				if (((FieldTypeAttribute)obj).FieldType == FieldType.IncrementField)
				{
					return true;
				}
			}
			return false;
		}

		private bool IsKeyField(object[] fieldTypes)
		{
			foreach (object obj in fieldTypes)
			{
				if (((FieldTypeAttribute)obj).FieldType == FieldType.KeyField)
				{
					return true;
				}
			}
			return false;
		}

		private DbType GetDbType(Type t)
		{
			try
			{
				if (t.IsEnum)
				{
					return DbType.Int32;
				}
				if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>))
				{
					t = t.GetGenericArguments()[0];
				}
				return (DbType)Enum.Parse(typeof(DbType), t.Name);
			}
			catch
			{
				return DbType.Object;
			}
		}
	}
}
