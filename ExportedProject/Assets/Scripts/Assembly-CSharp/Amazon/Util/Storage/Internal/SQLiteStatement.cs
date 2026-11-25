using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Amazon.Runtime;

namespace Amazon.Util.Storage.Internal
{
	public class SQLiteStatement
	{
		private IntPtr hStmt = IntPtr.Zero;

		private SQLiteDatabase db;

		public Dictionary<string, SQLiteField> Fields = new Dictionary<string, SQLiteField>();

		internal SQLiteStatement(SQLiteDatabase db, string query)
		{
			this.db = db;
			SQLiteDatabase.Result result = SQLiteDatabase.sqlite3_prepare_v2(db.hDb, query, -1, out hStmt, IntPtr.Zero);
			if (result != SQLiteDatabase.Result.SQLITE_OK)
			{
				throw new AmazonClientException(string.Concat(result, " - ", db.ErrorMsg()));
			}
		}

		public SQLiteStatement Step()
		{
			SQLiteDatabase.Result result = SQLiteDatabase.sqlite3_step(hStmt);
			if (result != SQLiteDatabase.Result.SQLITE_DONE)
			{
				throw new AmazonClientException(db.ErrorMsg());
			}
			return this;
		}

		public SQLiteStatement BindText(int column, string inputString)
		{
			SQLiteDatabase.Result result = ((inputString != null) ? SQLiteDatabase.sqlite3_bind_text(hStmt, column, SQLiteDatabase.StringToUTF8ByteArray(inputString), -1, new IntPtr(-1)) : SQLiteDatabase.sqlite3_bind_null(hStmt, column));
			if (result != SQLiteDatabase.Result.SQLITE_OK)
			{
				throw new AmazonServiceException(string.Concat(result, " - ", db.ErrorMsg()));
			}
			return this;
		}

		public SQLiteStatement BindInt(int column, long? inputInt)
		{
			SQLiteDatabase.Result result = (inputInt.HasValue ? SQLiteDatabase.sqlite3_bind_int64(hStmt, column, inputInt.Value) : SQLiteDatabase.sqlite3_bind_null(hStmt, column));
			if (result != SQLiteDatabase.Result.SQLITE_OK)
			{
				throw new AmazonClientException(string.Concat(result, " - ", db.ErrorMsg()));
			}
			return this;
		}

		public SQLiteStatement BindDateTime(int column, DateTime? inputDateTime)
		{
			return BindText(column, inputDateTime.HasValue ? inputDateTime.Value.ToUniversalTime().Ticks.ToString() : null);
		}

		public bool Read()
		{
			bool result = false;
			SQLiteDatabase.Result result2 = SQLiteDatabase.sqlite3_step(hStmt);
			switch (result2)
			{
			case SQLiteDatabase.Result.SQLITE_ROW:
			{
				int num = SQLiteDatabase.sqlite3_column_count(hStmt);
				Fields.Clear();
				for (int i = 0; i < num; i++)
				{
					SQLiteField sQLiteField = new SQLiteField(hStmt);
					sQLiteField.column = i;
					sQLiteField.name = Marshal.PtrToStringAnsi(SQLiteDatabase.sqlite3_column_name(hStmt, i));
					sQLiteField.Read();
					Fields.Add(sQLiteField.name, sQLiteField);
				}
				result = true;
				break;
			}
			default:
				throw new AmazonClientException(string.Concat(result2, "\n", SQLiteDatabase.sqlite3_errmsg(db.hDb)));
			case SQLiteDatabase.Result.SQLITE_DONE:
				break;
			}
			return result;
		}

		public SQLiteStatement Reset()
		{
			if (SQLiteDatabase.sqlite3_reset(hStmt) != SQLiteDatabase.Result.SQLITE_OK)
			{
				throw new AmazonClientException(db.ErrorMsg());
			}
			return this;
		}

		public SQLiteStatement ClearBindings()
		{
			if (SQLiteDatabase.sqlite3_clear_bindings(hStmt) != SQLiteDatabase.Result.SQLITE_OK)
			{
				throw new AmazonClientException(db.ErrorMsg());
			}
			return this;
		}

		public SQLiteStatement FinalizeStm()
		{
			if (SQLiteDatabase.sqlite3_finalize(hStmt) != SQLiteDatabase.Result.SQLITE_OK)
			{
				throw new AmazonClientException(db.ErrorMsg());
			}
			hStmt = IntPtr.Zero;
			return this;
		}
	}
}
