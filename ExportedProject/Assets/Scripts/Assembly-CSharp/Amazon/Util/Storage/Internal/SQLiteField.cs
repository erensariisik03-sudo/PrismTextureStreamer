using System;
using Amazon.Runtime;

namespace Amazon.Util.Storage.Internal
{
	public class SQLiteField
	{
		public string name;

		public int column;

		public SQLiteDatabase.DataType eType;

		public long nValue;

		public string sValue;

		public double fValue;

		public bool bNull;

		private IntPtr hStm = IntPtr.Zero;

		public bool BOOL
		{
			get
			{
				if (eType != SQLiteDatabase.DataType.INTEGER)
				{
					throw new AmazonClientException("Datatype: " + eType);
				}
				return nValue != 0;
			}
		}

		public string TEXT
		{
			get
			{
				if (IsNull())
				{
					return null;
				}
				if (eType != SQLiteDatabase.DataType.TEXT)
				{
					throw new AmazonClientException("Datatype: " + eType);
				}
				return sValue;
			}
		}

		public long INTEGER
		{
			get
			{
				if (eType != SQLiteDatabase.DataType.INTEGER)
				{
					throw new AmazonClientException("Datatype: " + eType);
				}
				return nValue;
			}
		}

		public DateTime? DATETIME
		{
			get
			{
				if (IsNull())
				{
					return null;
				}
				if (eType != SQLiteDatabase.DataType.TEXT)
				{
					throw new AmazonClientException("Datatype: " + eType);
				}
				return (sValue != null) ? new DateTime?(new DateTime(long.Parse(sValue)).ToLocalTime()) : ((DateTime?)null);
			}
		}

		public SQLiteField(IntPtr hStm)
		{
			this.hStm = hStm;
		}

		public void Read()
		{
			bNull = false;
			nValue = 0L;
			sValue = string.Empty;
			fValue = 0.0;
			eType = SQLiteDatabase.sqlite3_column_type(hStm, column);
			switch (eType)
			{
			case SQLiteDatabase.DataType.NULL:
				bNull = true;
				break;
			case SQLiteDatabase.DataType.INTEGER:
				nValue = SQLiteDatabase.sqlite3_column_int64(hStm, column);
				break;
			case SQLiteDatabase.DataType.TEXT:
				sValue = SQLiteDatabase.PtrToString(SQLiteDatabase.sqlite3_column_text(hStm, column));
				break;
			default:
				throw new AmazonClientException("Unknown datatype");
			}
		}

		public bool IsNull()
		{
			return bNull;
		}
	}
}
