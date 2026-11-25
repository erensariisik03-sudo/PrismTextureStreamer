using System;
using System.Runtime.InteropServices;
using System.Text;
using Amazon.Runtime;

namespace Amazon.Util.Storage.Internal
{
	public class SQLiteDatabase
	{
		public enum Result
		{
			SQLITE_OK = 0,
			SQLITE_ERROR = 1,
			SQLITE_INTERNAL = 2,
			SQLITE_PERM = 3,
			SQLITE_ABORT = 4,
			SQLITE_BUSY = 5,
			SQLITE_LOCKED = 6,
			SQLITE_NOMEM = 7,
			SQLITE_READONLY = 8,
			SQLITE_INTERRUPT = 9,
			SQLITE_IOERR = 10,
			SQLITE_CORRUPT = 11,
			SQLITE_NOTFOUND = 12,
			SQLITE_FULL = 13,
			SQLITE_CANTOPEN = 14,
			SQLITE_PROTOCOL = 15,
			SQLITE_EMPTY = 16,
			SQLITE_SCHEMA = 17,
			SQLITE_TOOBIG = 18,
			SQLITE_CONSTRAINT = 19,
			SQLITE_MISMATCH = 20,
			SQLITE_MISUSE = 21,
			SQLITE_NOLFS = 22,
			SQLITE_AUTH = 23,
			SQLITE_FORMAT = 24,
			SQLITE_RANGE = 25,
			SQLITE_NOTADB = 26,
			SQLITE_NOTICE = 27,
			SQLITE_WARNING = 28,
			SQLITE_ROW = 100,
			SQLITE_DONE = 101
		}

		public enum DataType
		{
			INTEGER = 1,
			FLOAT = 2,
			TEXT = 3,
			BLOB = 4,
			NULL = 5
		}

		private string dbPath;

		internal IntPtr hDb = IntPtr.Zero;

		public SQLiteDatabase(string dbPath)
		{
			if (dbPath == null)
			{
				throw new ArgumentNullException("dataPath");
			}
			this.dbPath = dbPath;
			if (sqlite3_open(dbPath, out hDb) != Result.SQLITE_OK)
			{
				throw new AmazonClientException("Could not open database file: " + dbPath);
			}
		}

		[DllImport("sqlite3")]
		public static extern Result sqlite3_open(string filename, out IntPtr db);

		[DllImport("sqlite3")]
		public static extern Result sqlite3_close(IntPtr db);

		[DllImport("sqlite3")]
		public static extern Result sqlite3_prepare_v2(IntPtr db, string zSql, int nByte, out IntPtr ppStmpt, IntPtr pzTail);

		[DllImport("sqlite3.dll", EntryPoint = "sqlite3_exec")]
		public static extern Result exec(IntPtr hDb, string sql, IntPtr callback, IntPtr args, out IntPtr errorMessage);

		[DllImport("sqlite3")]
		public static extern Result sqlite3_step(IntPtr hStmt);

		[DllImport("sqlite3")]
		public static extern Result sqlite3_finalize(IntPtr hStmt);

		[DllImport("sqlite3")]
		public static extern IntPtr sqlite3_errmsg(IntPtr db);

		[DllImport("sqlite3")]
		public static extern int sqlite3_column_count(IntPtr hStmt);

		[DllImport("sqlite3")]
		public static extern IntPtr sqlite3_column_name(IntPtr hStmt, int iCol);

		[DllImport("sqlite3")]
		public static extern DataType sqlite3_column_type(IntPtr hStmt, int iCol);

		[DllImport("sqlite3.dll")]
		public static extern long sqlite3_column_int64(IntPtr hStmt, int nCol);

		[DllImport("sqlite3")]
		public static extern IntPtr sqlite3_column_text(IntPtr hStmt, int iCol);

		[DllImport("sqlite3")]
		public static extern IntPtr sqlite3_column_blob(IntPtr hStmt, int iCol);

		[DllImport("sqlite3")]
		public static extern Result sqlite3_column_bytes(IntPtr hStmt, int iCol);

		[DllImport("sqlite3.dll")]
		public static extern Result sqlite3_bind_int64(IntPtr hStmt, int n, long value);

		[DllImport("sqlite3.dll")]
		public static extern Result sqlite3_bind_null(IntPtr hStmt, int n);

		[DllImport("sqlite3.dll")]
		public static extern Result sqlite3_bind_text(IntPtr hStmt, int n, byte[] value, int length, IntPtr freetype);

		[DllImport("sqlite3.dll")]
		public static extern long sqlite3_last_insert_rowid(IntPtr hDb);

		[DllImport("sqlite3.dll")]
		public static extern Result sqlite3_reset(IntPtr hStmt);

		[DllImport("sqlite3.dll")]
		public static extern Result sqlite3_clear_bindings(IntPtr hStmt);

		public static byte[] StringToUTF8ByteArray(string sText)
		{
			int num = Encoding.UTF8.GetByteCount(sText) + 1;
			byte[] array = new byte[num];
			num = Encoding.UTF8.GetBytes(sText, 0, sText.Length, array, 0);
			array[num] = 0;
			return array;
		}

		public static string PtrToString(IntPtr ptr)
		{
			return Marshal.PtrToStringAnsi(ptr);
		}

		public void CloseDatabase()
		{
			if (hDb != IntPtr.Zero)
			{
				sqlite3_close(hDb);
				hDb = IntPtr.Zero;
			}
		}

		public SQLiteStatement Prepare(string SQL)
		{
			if (hDb == IntPtr.Zero)
			{
				throw new AmazonClientException("Database closed: " + dbPath);
			}
			return new SQLiteStatement(this, SQL);
		}

		public void OpenDatabase()
		{
			if (sqlite3_open(dbPath, out hDb) != Result.SQLITE_OK)
			{
				throw new AmazonClientException("Could not open database file: " + dbPath);
			}
		}

		public long LastInsertRowid()
		{
			return sqlite3_last_insert_rowid(hDb);
		}

		public void Exec(string SQL)
		{
			if (hDb == IntPtr.Zero)
			{
				throw new AmazonClientException("SQLite database is not open.");
			}
			IntPtr errorMessage;
			if (exec(hDb, SQL, IntPtr.Zero, IntPtr.Zero, out errorMessage) != Result.SQLITE_OK)
			{
				throw new AmazonClientException("Could not execute SQL statement." + ErrorMsg());
			}
		}

		public string ErrorMsg()
		{
			return PtrToString(sqlite3_errmsg(hDb));
		}
	}
}
