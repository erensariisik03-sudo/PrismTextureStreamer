using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace Amazon.Runtime.Internal.Util
{
	public static class StringUtils
	{
		private static readonly Encoding UTF_8 = Encoding.UTF8;

		public static string FromString(string value)
		{
			return value;
		}

		public static string FromString(ConstantClass value)
		{
			return (!(value == null)) ? value.Intern().Value : string.Empty;
		}

		public static string FromMemoryStream(MemoryStream value)
		{
			return Convert.ToBase64String(value.ToArray());
		}

		public static string FromInt(int value)
		{
			return value.ToString(CultureInfo.InvariantCulture);
		}

		public static string FromInt(int? value)
		{
			if (value.HasValue)
			{
				return value.Value.ToString(CultureInfo.InvariantCulture);
			}
			return null;
		}

		public static string FromLong(long value)
		{
			return value.ToString(CultureInfo.InvariantCulture);
		}

		public static string FromBool(bool value)
		{
			return value.ToString(CultureInfo.InvariantCulture).ToLowerInvariant();
		}

		public static string FromDateTime(DateTime value)
		{
			return value.ToString("yyyy-MM-dd\\THH:mm:ss.fff\\Z", CultureInfo.InvariantCulture);
		}

		public static string FromDouble(double value)
		{
			return value.ToString(CultureInfo.InvariantCulture);
		}

		public static long Utf8ByteLength(string theString)
		{
			if (theString == null)
			{
				return 0L;
			}
			return UTF_8.GetByteCount(theString);
		}
	}
}
