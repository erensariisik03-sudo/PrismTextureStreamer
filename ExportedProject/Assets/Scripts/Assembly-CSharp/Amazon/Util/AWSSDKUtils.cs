using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using Amazon.Runtime.Internal.Util;

namespace Amazon.Util
{
	public static class AWSSDKUtils
	{
		internal const string DefaultRegion = "us-east-1";

		internal const string DefaultGovRegion = "us-gov-west-1";

		internal const int DefaultMaxRetry = 3;

		private const int DefaultConnectionLimit = 50;

		private const int DefaultMaxIdleTime = 50000;

		public const int DefaultBufferSize = 8192;

		public const long DefaultProgressUpdateInterval = 102400L;

		public const string UserAgentHeader = "User-Agent";

		public const string ValidUrlCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";

		public const string ValidUrlCharactersRFC1738 = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.";

		public const string UrlEncodedContent = "application/x-www-form-urlencoded; charset=utf-8";

		public const string GMTDateFormat = "ddd, dd MMM yyyy HH:mm:ss \\G\\M\\T";

		public const string ISO8601DateFormat = "yyyy-MM-dd\\THH:mm:ss.fff\\Z";

		public const string ISO8601DateFormatNoMS = "yyyy-MM-dd\\THH:mm:ss\\Z";

		public const string ISO8601BasicDateTimeFormat = "yyyyMMddTHHmmssZ";

		public const string ISO8601BasicDateFormat = "yyyyMMdd";

		public const string RFC822DateFormat = "ddd, dd MMM yyyy HH:mm:ss \\G\\M\\T";

		private const int _defaultDefaultConnectionLimit = 2;

		private const int _defaultMaxIdleTime = 100000;

		public static readonly DateTime EPOCH_START = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		internal static Dictionary<int, string> RFCEncodingSchemes = new Dictionary<int, string>
		{
			{ 3986, "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~" },
			{ 1738, "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_." }
		};

		private static string ValidPathCharacters = DetermineValidPathCharacters();

		private static BackgroundInvoker _dispatcher;

		private static readonly object _preserveStackTraceLookupLock = new object();

		private static bool _preserveStackTraceLookup = false;

		private static MethodInfo _preserveStackTrace;

		private static BackgroundInvoker Dispatcher
		{
			get
			{
				if (_dispatcher == null)
				{
					_dispatcher = new BackgroundInvoker();
				}
				return _dispatcher;
			}
		}

		public static string FormattedCurrentTimestampGMT
		{
			get
			{
				DateTime correctedUtcNow = CorrectedUtcNow;
				return new DateTime(correctedUtcNow.Year, correctedUtcNow.Month, correctedUtcNow.Day, correctedUtcNow.Hour, correctedUtcNow.Minute, correctedUtcNow.Second, correctedUtcNow.Millisecond, DateTimeKind.Local).ToString("ddd, dd MMM yyyy HH:mm:ss \\G\\M\\T", CultureInfo.InvariantCulture);
			}
		}

		public static string FormattedCurrentTimestampISO8601
		{
			get
			{
				return GetFormattedTimestampISO8601(0);
			}
		}

		public static string FormattedCurrentTimestampRFC822
		{
			get
			{
				return GetFormattedTimestampRFC822(0);
			}
		}

		public static DateTime CorrectedUtcNow
		{
			get
			{
				DateTime utcNow = DateTime.UtcNow;
				if (AWSConfigs.CorrectForClockSkew)
				{
					utcNow += AWSConfigs.ClockOffset;
				}
				return utcNow;
			}
		}

		internal static bool IsIL2CPP
		{
			get
			{
				Type type = Type.GetType("Mono.Runtime");
				if (type != null)
				{
					MethodInfo method = type.GetMethod("GetDisplayName", BindingFlags.Static | BindingFlags.NonPublic);
					if (method != null)
					{
						string text = null;
						try
						{
							text = method.Invoke(null, null).ToString();
						}
						catch (Exception)
						{
							return false;
						}
						if (text != null && text.ToUpper().Contains("IL2CPP"))
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		private static string DetermineValidPathCharacters()
		{
			StringBuilder stringBuilder = new StringBuilder();
			string text = "/:'()!*[]";
			for (int i = 0; i < text.Length; i++)
			{
				char c = text[i];
				string text2 = Uri.EscapeUriString(c.ToString());
				if (text2.Length == 1 && text2[0] == c)
				{
					stringBuilder.Append(c);
				}
			}
			return stringBuilder.ToString();
		}

		public static string GetExtension(string path)
		{
			if (path == null)
			{
				return null;
			}
			int length = path.Length;
			int num = length;
			while (--num >= 0)
			{
				char c = path[num];
				if (c == '.')
				{
					if (num != length - 1)
					{
						return path.Substring(num, length - num);
					}
					return string.Empty;
				}
				if (IsPathSeparator(c))
				{
					break;
				}
			}
			return string.Empty;
		}

		private static bool IsPathSeparator(char ch)
		{
			return ch == '\\' || ch == '/' || ch == ':';
		}

		internal static string CalculateStringToSignV2(IDictionary<string, string> parameters, string serviceUrl)
		{
			StringBuilder stringBuilder = new StringBuilder("POST\n", 512);
			IDictionary<string, string> dictionary = new SortedDictionary<string, string>(parameters, StringComparer.Ordinal);
			Uri uri = new Uri(serviceUrl);
			stringBuilder.Append(uri.Host);
			stringBuilder.Append("\n");
			string text = uri.AbsolutePath;
			if (text == null || text.Length == 0)
			{
				text = "/";
			}
			stringBuilder.Append(UrlEncode(text, true));
			stringBuilder.Append("\n");
			foreach (KeyValuePair<string, string> item in dictionary)
			{
				if (item.Value != null)
				{
					stringBuilder.Append(UrlEncode(item.Key, false));
					stringBuilder.Append("=");
					stringBuilder.Append(UrlEncode(item.Value, false));
					stringBuilder.Append("&");
				}
			}
			string text2 = stringBuilder.ToString();
			return text2.Remove(text2.Length - 1);
		}

		internal static string GetParametersAsString(IDictionary<string, string> parameters)
		{
			string[] array = new string[parameters.Keys.Count];
			parameters.Keys.CopyTo(array, 0);
			Array.Sort(array);
			StringBuilder stringBuilder = new StringBuilder(512);
			string[] array2 = array;
			foreach (string text in array2)
			{
				string text2 = parameters[text];
				if (text2 != null)
				{
					stringBuilder.Append(text);
					stringBuilder.Append('=');
					stringBuilder.Append(UrlEncode(text2, false));
					stringBuilder.Append('&');
				}
			}
			string text3 = stringBuilder.ToString();
			if (text3.Length == 0)
			{
				return string.Empty;
			}
			return text3.Remove(text3.Length - 1);
		}

		public static string Join(List<string> strings)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = true;
			foreach (string @string in strings)
			{
				if (!flag)
				{
					stringBuilder.Append(", ");
				}
				stringBuilder.Append(@string);
				flag = false;
			}
			return stringBuilder.ToString();
		}

		public static string DetermineRegion(string url)
		{
			int num = url.IndexOf("//", StringComparison.Ordinal);
			if (num >= 0)
			{
				url = url.Substring(num + 2);
			}
			if (url.EndsWith("/", StringComparison.Ordinal))
			{
				url = url.Substring(0, url.Length - 1);
			}
			int num2 = url.IndexOf(".amazonaws.com", StringComparison.Ordinal);
			if (num2 < 0)
			{
				return "us-east-1";
			}
			string text = url.Substring(0, num2);
			int num3 = url.IndexOf(".cloudsearch.amazonaws.com", StringComparison.Ordinal);
			if (num3 > 0)
			{
				text = url.Substring(0, num3);
			}
			int num4 = text.IndexOf("queue", StringComparison.Ordinal);
			if (num4 == 0)
			{
				return "us-east-1";
			}
			if (num4 > 0)
			{
				return text.Substring(0, num4 - 1);
			}
			if (text.StartsWith("s3-", StringComparison.Ordinal))
			{
				text = "s3." + text.Substring(3);
			}
			int num5 = text.LastIndexOf('.');
			if (num5 == -1)
			{
				return "us-east-1";
			}
			string text2 = text.Substring(num5 + 1);
			if (text2.Equals("external-1"))
			{
				return RegionEndpoint.USEast1.SystemName;
			}
			if (string.Equals(text2, "us-gov", StringComparison.Ordinal))
			{
				return "us-gov-west-1";
			}
			return text2;
		}

		public static string DetermineService(string url)
		{
			int num = url.IndexOf("//", StringComparison.Ordinal);
			if (num >= 0)
			{
				url = url.Substring(num + 2);
			}
			string[] array = url.Split(new char[1] { '.' }, StringSplitOptions.RemoveEmptyEntries);
			if (array == null || array.Length == 0)
			{
				return string.Empty;
			}
			string text = array[0];
			int num2 = text.IndexOf('-');
			string text2 = ((num2 >= 0) ? text.Substring(0, num2) : text);
			if (text2.Equals("queue"))
			{
				return "sqs";
			}
			return text2;
		}

		public static DateTime ConvertFromUnixEpochSeconds(int seconds)
		{
			DateTime dateTime = new DateTime((long)seconds * 10000000L + EPOCH_START.Ticks, DateTimeKind.Utc);
			return dateTime.ToLocalTime();
		}

		public static int ConvertToUnixEpochSeconds(DateTime dateTime)
		{
			return (int)ConvertToUnixEpochMilliSeconds(dateTime);
		}

		public static double ConvertToUnixEpochMilliSeconds(DateTime dateTime)
		{
			TimeSpan timeSpan = new TimeSpan(dateTime.ToUniversalTime().Ticks - EPOCH_START.Ticks);
			return Math.Round(timeSpan.TotalMilliseconds, 0) / 1000.0;
		}

		internal static string ToHex(byte[] data, bool lowercase)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < data.Length; i++)
			{
				stringBuilder.Append(data[i].ToString((!lowercase) ? "X2" : "x2", CultureInfo.InvariantCulture));
			}
			return stringBuilder.ToString();
		}

		public static void InvokeInBackground<T>(EventHandler<T> handler, T args, object sender) where T : EventArgs
		{
			if (handler == null)
			{
				return;
			}
			Delegate[] invocationList = handler.GetInvocationList();
			Delegate[] array = invocationList;
			foreach (Delegate obj in array)
			{
				EventHandler<T> eventHandler = (EventHandler<T>)obj;
				if (eventHandler != null)
				{
					Dispatcher.Dispatch(() =>
					{
						eventHandler(sender, args);
					});
				}
			}
		}

		public static Dictionary<string, string> ParseQueryParameters(string url)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			if (!string.IsNullOrEmpty(url))
			{
				int num = url.IndexOf('?');
				if (num >= 0)
				{
					string text = url.Substring(num + 1);
					string[] array = text.Split(new char[1] { '&' }, StringSplitOptions.None);
					string[] array2 = array;
					foreach (string text2 in array2)
					{
						if (!string.IsNullOrEmpty(text2))
						{
							string[] array3 = text2.Split(new char[1] { '=' }, 2);
							string key = array3[0];
							string value = ((array3.Length != 1) ? array3[1] : null);
							dictionary[key] = value;
						}
					}
				}
			}
			return dictionary;
		}

		internal static bool AreEqual(object[] itemsA, object[] itemsB)
		{
			if (itemsA == null || itemsB == null)
			{
				return itemsA == itemsB;
			}
			if (itemsA.Length != itemsB.Length)
			{
				return false;
			}
			int num = itemsA.Length;
			for (int i = 0; i < num; i++)
			{
				object a = itemsA[i];
				object b = itemsB[i];
				if (!AreEqual(a, b))
				{
					return false;
				}
			}
			return true;
		}

		internal static bool AreEqual(object a, object b)
		{
			if (a == null || b == null)
			{
				return a == b;
			}
			if (object.ReferenceEquals(a, b))
			{
				return true;
			}
			return a.Equals(b);
		}

		public static MemoryStream GenerateMemoryStreamFromString(string s)
		{
			MemoryStream memoryStream = new MemoryStream();
			StreamWriter streamWriter = new StreamWriter(memoryStream);
			streamWriter.Write(s);
			streamWriter.Flush();
			memoryStream.Position = 0L;
			return memoryStream;
		}

		public static void CopyStream(Stream source, Stream destination)
		{
			CopyStream(source, destination, 8192);
		}

		public static void CopyStream(Stream source, Stream destination, int bufferSize)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (destination == null)
			{
				throw new ArgumentNullException("destination");
			}
			if (bufferSize <= 0)
			{
				throw new ArgumentOutOfRangeException("bufferSize");
			}
			byte[] array = new byte[bufferSize];
			int count;
			while ((count = source.Read(array, 0, array.Length)) != 0)
			{
				destination.Write(array, 0, count);
			}
		}

		public static string GetFormattedTimestampISO8601(int minutesFromNow)
		{
			DateTime dateTime = CorrectedUtcNow.AddMinutes(minutesFromNow);
			return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second, dateTime.Millisecond, DateTimeKind.Local).ToString("yyyy-MM-dd\\THH:mm:ss.fff\\Z", CultureInfo.InvariantCulture);
		}

		public static string GetFormattedTimestampRFC822(int minutesFromNow)
		{
			DateTime dateTime = CorrectedUtcNow.AddMinutes(minutesFromNow);
			return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second, dateTime.Millisecond, DateTimeKind.Local).ToString("ddd, dd MMM yyyy HH:mm:ss \\G\\M\\T", CultureInfo.InvariantCulture);
		}

		public static string UrlEncode(string data, bool path)
		{
			return UrlEncode(3986, data, path);
		}

		public static string UrlEncode(int rfcNumber, string data, bool path)
		{
			StringBuilder stringBuilder = new StringBuilder(data.Length * 2);
			string value;
			if (!RFCEncodingSchemes.TryGetValue(rfcNumber, out value))
			{
				value = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";
			}
			string text = value + ((!path) ? string.Empty : ValidPathCharacters);
			byte[] bytes = Encoding.UTF8.GetBytes(data);
			for (int i = 0; i < bytes.Length; i++)
			{
				char c = (char)bytes[i];
				if (text.IndexOf(c) != -1)
				{
					stringBuilder.Append(c);
					continue;
				}
				stringBuilder.Append("%").Append(string.Format(CultureInfo.InvariantCulture, "{0:X2}", (int)c));
			}
			return stringBuilder.ToString();
		}

		public static void Sleep(TimeSpan ts)
		{
			Sleep((int)ts.TotalMilliseconds);
		}

		public static string BytesToHexString(byte[] value)
		{
			string text = BitConverter.ToString(value);
			return text.Replace("-", string.Empty);
		}

		public static byte[] HexStringToBytes(string hex)
		{
			if (string.IsNullOrEmpty(hex) || hex.Length % 2 == 1)
			{
				throw new ArgumentOutOfRangeException("hex");
			}
			int num = 0;
			byte[] array = new byte[hex.Length / 2];
			for (int i = 0; i < hex.Length; i += 2)
			{
				string value = hex.Substring(i, 2);
				byte b = Convert.ToByte(value, 16);
				array[num] = b;
				num++;
			}
			return array;
		}

		public static void ForceCanonicalPathAndQuery(Uri uri)
		{
			try
			{
				FieldInfo field = typeof(Uri).GetField("m_Flags", BindingFlags.Instance | BindingFlags.NonPublic);
				ulong num = (ulong)field.GetValue(uri);
				num &= 0xFFFFFFFFFFFFFFCFuL;
				field.SetValue(uri, num);
			}
			catch
			{
			}
		}

		public static void PreserveStackTrace(Exception exception)
		{
			if (!_preserveStackTraceLookup)
			{
				lock (_preserveStackTraceLookupLock)
				{
					_preserveStackTraceLookup = true;
					try
					{
						_preserveStackTrace = typeof(Exception).GetMethod("InternalPreserveStackTrace", BindingFlags.Instance | BindingFlags.NonPublic);
					}
					catch
					{
					}
				}
			}
			if (_preserveStackTrace != null)
			{
				_preserveStackTrace.Invoke(exception, null);
			}
		}

		internal static int GetConnectionLimit(int? clientConfigValue)
		{
			if (clientConfigValue.HasValue)
			{
				return clientConfigValue.Value;
			}
			return 50;
		}

		internal static int GetMaxIdleTime(int? clientConfigValue)
		{
			if (clientConfigValue.HasValue)
			{
				return clientConfigValue.Value;
			}
			return 50000;
		}

		public static void Sleep(int ms)
		{
			Thread.Sleep(ms);
		}
	}
}
