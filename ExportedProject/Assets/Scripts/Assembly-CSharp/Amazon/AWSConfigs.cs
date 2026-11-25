using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Xml.Linq;
using Amazon.Runtime.Internal;
using Amazon.Runtime.Internal.Util;
using Amazon.Util;
using Amazon.Util.Internal;
using UnityEngine;

namespace Amazon
{
	public static class AWSConfigs
	{
		public const string AWSRegionKey = "AWSRegion";

		public const string AWSProfileNameKey = "AWSProfileName";

		public const string AWSProfilesLocationKey = "AWSProfilesLocation";

		public const string LoggingKey = "AWSLogging";

		public const string ResponseLoggingKey = "AWSResponseLogging";

		public const string LogMetricsKey = "AWSLogMetrics";

		public const string EndpointDefinitionKey = "AWSEndpointDefinition";

		public const string UseSdkCacheKey = "AWSCache";

		internal const string LoggingDestinationProperty = "LogTo";

		private const string CONFIG_FILE = "awsconfig";

		private static char[] validSeparators = new char[2] { ' ', ',' };

		internal static string _awsRegion = GetConfig("AWSRegion");

		internal static LoggingOptions _logging = GetLoggingSetting();

		internal static ResponseLoggingOption _responseLogging = GetConfigEnum<ResponseLoggingOption>("AWSResponseLogging");

		internal static bool _logMetrics = GetConfigBool("AWSLogMetrics");

		internal static string _endpointDefinition = GetConfig("AWSEndpointDefinition");

		internal static string _awsProfileName = GetConfig("AWSProfileName");

		internal static string _awsAccountsLocation = GetConfig("AWSProfilesLocation");

		internal static bool _useSdkCache = GetConfigBool("AWSCache", true);

		private static RootConfig _rootConfig = new RootConfig();

		private static List<string> standardConfigs = new List<string> { "region", "logging", "correctForClockSkew" };

		private static Dictionary<string, List<TraceListener>> _traceListeners = new Dictionary<string, List<TraceListener>>(StringComparer.OrdinalIgnoreCase);

		private static XDocument xmlDoc = LoadConfigFromResource();

		private static object _lock = new object();

		public static bool CorrectForClockSkew
		{
			get
			{
				return _rootConfig.CorrectForClockSkew;
			}
			set
			{
				_rootConfig.CorrectForClockSkew = value;
			}
		}

		public static TimeSpan ClockOffset { get; internal set; }

		public static string AWSRegion
		{
			get
			{
				return _rootConfig.Region;
			}
			set
			{
				_rootConfig.Region = value;
			}
		}

		public static string AWSProfileName
		{
			get
			{
				return _rootConfig.ProfileName;
			}
			set
			{
				_rootConfig.ProfileName = value;
			}
		}

		public static string AWSProfilesLocation
		{
			get
			{
				return _rootConfig.ProfilesLocation;
			}
			set
			{
				_rootConfig.ProfilesLocation = value;
			}
		}

		[Obsolete("This property is obsolete. Use LoggingConfig.LogTo instead.")]
		public static LoggingOptions Logging
		{
			get
			{
				return _rootConfig.Logging.LogTo;
			}
			set
			{
				_rootConfig.Logging.LogTo = value;
			}
		}

		[Obsolete("This property is obsolete. Use LoggingConfig.LogResponses instead.")]
		public static ResponseLoggingOption ResponseLogging
		{
			get
			{
				return _rootConfig.Logging.LogResponses;
			}
			set
			{
				_rootConfig.Logging.LogResponses = value;
			}
		}

		[Obsolete("This property is obsolete. Use LoggingConfig.LogMetrics instead.")]
		public static bool LogMetrics
		{
			get
			{
				return _rootConfig.Logging.LogMetrics;
			}
			set
			{
				_rootConfig.Logging.LogMetrics = value;
			}
		}

		public static string EndpointDefinition
		{
			get
			{
				return _rootConfig.EndpointDefinition;
			}
			set
			{
				_rootConfig.EndpointDefinition = value;
			}
		}

		public static bool UseSdkCache
		{
			get
			{
				return _rootConfig.UseSdkCache;
			}
			set
			{
				_rootConfig.UseSdkCache = value;
			}
		}

		public static LoggingConfig LoggingConfig
		{
			get
			{
				return _rootConfig.Logging;
			}
		}

		public static ProxyConfig ProxyConfig
		{
			get
			{
				return _rootConfig.Proxy;
			}
		}

		public static RegionEndpoint RegionEndpoint
		{
			get
			{
				return _rootConfig.RegionEndpoint;
			}
			set
			{
				_rootConfig.RegionEndpoint = value;
			}
		}

		public static string ApplicationName
		{
			get
			{
				return _rootConfig.ApplicationName;
			}
			set
			{
				_rootConfig.ApplicationName = value;
			}
		}

		internal static event PropertyChangedEventHandler PropertyChanged;

		private static LoggingOptions GetLoggingSetting()
		{
			string config = GetConfig("AWSLogging");
			if (string.IsNullOrEmpty(config))
			{
				return LoggingOptions.None;
			}
			string[] array = config.Split(validSeparators, StringSplitOptions.RemoveEmptyEntries);
			if (array == null || array.Length == 0)
			{
				return LoggingOptions.None;
			}
			LoggingOptions loggingOptions = LoggingOptions.None;
			string[] array2 = array;
			foreach (string value in array2)
			{
				LoggingOptions loggingOptions2 = ParseEnum<LoggingOptions>(value);
				loggingOptions |= loggingOptions2;
			}
			return loggingOptions;
		}

		internal static void OnPropertyChanged(string name)
		{
			PropertyChangedEventHandler propertyChanged = PropertyChanged;
			if (propertyChanged != null)
			{
				propertyChanged(null, new PropertyChangedEventArgs(name));
			}
		}

		private static bool GetConfigBool(string name, bool defaultValue = false)
		{
			string config = GetConfig(name);
			bool result;
			if (bool.TryParse(config, out result))
			{
				return result;
			}
			return defaultValue;
		}

		private static T GetConfigEnum<T>(string name)
		{
			ITypeInfo typeInfo = TypeFactory.GetTypeInfo(typeof(T));
			if (!typeInfo.IsEnum)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Type {0} must be enum", typeInfo.FullName));
			}
			string config = GetConfig(name);
			if (string.IsNullOrEmpty(config))
			{
				return default(T);
			}
			return ParseEnum<T>(config);
		}

		private static T ParseEnum<T>(string value)
		{
			T result;
			if (TryParseEnum<T>(value, out result))
			{
				return result;
			}
			Type typeFromHandle = typeof(T);
			string format = "Unable to parse value {0} as enum of type {1}. Valid values are: {2}";
			string text = string.Join(", ", Enum.GetNames(typeFromHandle));
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, format, value, typeFromHandle.FullName, text));
		}

		private static bool TryParseEnum<T>(string value, out T result)
		{
			result = default(T);
			if (string.IsNullOrEmpty(value))
			{
				return false;
			}
			try
			{
				T val = (T)Enum.Parse(typeof(T), value, true);
				result = val;
				return true;
			}
			catch (ArgumentException)
			{
				return false;
			}
		}

		public static string GetConfig(string name)
		{
			return null;
		}

		internal static T GetSection<T>(string sectionName) where T : AWSSection, new()
		{
			if (xmlDoc == null)
			{
				lock (_lock)
				{
					if (xmlDoc == null)
					{
						LoadConfigFromResource();
					}
				}
			}
			XElement xElement = xmlDoc.Element(sectionName);
			return new T
			{
				Logging = GetObject<LoggingSection>(xElement, "logging"),
				Region = ((xElement.Attribute("region") != null) ? xElement.Attribute("region").Value : string.Empty),
				CorrectForClockSkew = bool.Parse(xElement.Attribute("correctForClockSkew").Value),
				ServiceSections = GetUnresolvedElements(xElement)
			};
		}

		internal static bool XmlSectionExists(string sectionName)
		{
			return xmlDoc.Element(sectionName) != null;
		}

		public static void AddTraceListener(string source, TraceListener listener)
		{
			if (string.IsNullOrEmpty(source))
			{
				throw new ArgumentException("Source cannot be null or empty", "source");
			}
			if (listener == null)
			{
				throw new ArgumentException("Listener cannot be null", "listener");
			}
			lock (_traceListeners)
			{
				if (!_traceListeners.ContainsKey(source))
				{
					_traceListeners.Add(source, new List<TraceListener>());
				}
				_traceListeners[source].Add(listener);
			}
		}

		public static void RemoveTraceListener(string source, string name)
		{
			if (string.IsNullOrEmpty(source))
			{
				throw new ArgumentException("Source cannot be null or empty", "source");
			}
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentException("Name cannot be null or empty", "name");
			}
			lock (_traceListeners)
			{
				if (_traceListeners.ContainsKey(source))
				{
					foreach (TraceListener item in _traceListeners[source])
					{
						if (item.Name.Equals(name, StringComparison.Ordinal))
						{
							_traceListeners[source].Remove(item);
							break;
						}
					}
				}
			}
			Logger.ClearLoggerCache();
		}

		internal static TraceListener[] TraceListeners(string source)
		{
			lock (_traceListeners)
			{
				List<TraceListener> value;
				if (_traceListeners.TryGetValue(source, out value))
				{
					return value.ToArray();
				}
				return new TraceListener[0];
			}
		}

		private static XDocument LoadConfigFromResource()
		{
			XDocument xDoc = null;
			TextAsset awsEndpoints = null;
			Action action = () =>
			{
				awsEndpoints = Resources.Load("awsconfig") as TextAsset;
				using (Stream stream = new MemoryStream(awsEndpoints.bytes))
				{
					using (StreamReader reader = new StreamReader(stream))
					{
						xDoc = XDocument.Load(reader);
					}
				}
			};
			if (UnityInitializer.IsMainThread())
			{
				action();
			}
			else
			{
				ManualResetEvent e = new ManualResetEvent(false);
				UnityRequestQueue.Instance.ExecuteOnMainThread(() =>
				{
					action();
					e.Set();
				});
				e.WaitOne();
			}
			return xDoc;
		}

		public static T GetObject<T>(XElement rootElement, string propertyName) where T : class, new()
		{
			return (T)GetObject(rootElement, propertyName, typeof(T));
		}

		private static object GetObject(XElement rootElement, string propertyName, Type type)
		{
			object obj = Activator.CreateInstance(type);
			PropertyInfo[] properties = obj.GetType().GetProperties();
			rootElement = ((rootElement.Elements(propertyName).Count() != 0) ? rootElement.Elements(propertyName).First() : rootElement);
			PropertyInfo[] array = properties;
			PropertyInfo propertyInfo;
			for (int i = 0; i < array.Length; i++)
			{
				propertyInfo = array[i];
				if (!propertyInfo.CanWrite)
				{
					continue;
				}
				Type propertyType = propertyInfo.PropertyType;
				XAttribute xAttribute = rootElement.Attributes().SingleOrDefault((XAttribute a) => a.Name.ToString().Equals(propertyInfo.Name, StringComparison.OrdinalIgnoreCase));
				if (xAttribute != null)
				{
					if (propertyType.BaseType.Equals(typeof(Enum)))
					{
						propertyInfo.SetValue(obj, Enum.Parse(propertyType, xAttribute.Value, true), null);
						continue;
					}
					Type underlyingType = Nullable.GetUnderlyingType(propertyType);
					object value = ((underlyingType != null) ? Convert.ChangeType(xAttribute.Value, underlyingType) : ((propertyType != typeof(Type)) ? Convert.ChangeType(xAttribute.Value, propertyType) : Type.GetType(xAttribute.Value, true)));
					propertyInfo.SetValue(obj, value, null);
					continue;
				}
				XElement xElement = rootElement.Elements().SingleOrDefault((XElement e) => e.Name.ToString().Equals(propertyInfo.Name, StringComparison.OrdinalIgnoreCase));
				if (typeof(IList).IsAssignableFrom(propertyType))
				{
					string propertyName2 = ((xElement != null) ? xElement.Name.ToString() : null);
					IEnumerable list = GetList(rootElement, propertyType, propertyName2);
					propertyInfo.SetValue(obj, list, null);
				}
				else if (xElement != null)
				{
					object value2 = GetObject(xElement, xElement.Name.ToString(), propertyType);
					propertyInfo.SetValue(obj, value2, null);
				}
			}
			return obj;
		}

		private static IEnumerable GetList(XElement rootElement, Type listType, string propertyName)
		{
			IList list = (IList)Activator.CreateInstance(listType);
			PropertyInfo property = listType.GetProperty("ItemPropertyName");
			string propertyName2 = (string)property.GetValue(list, null);
			Type propertyType = listType.GetProperty("Item").PropertyType;
			if (!string.IsNullOrEmpty(propertyName))
			{
				rootElement = ((rootElement.Elements(propertyName).Count() != 0) ? rootElement.Elements(propertyName).First() : rootElement);
			}
			foreach (XElement item in rootElement.Elements())
			{
				object value = GetObject(item, propertyName2, propertyType);
				list.Add(value);
			}
			return list;
		}

		private static IDictionary<string, XElement> GetUnresolvedElements(XElement parent)
		{
			IDictionary<string, XElement> dictionary = new Dictionary<string, XElement>();
			foreach (XElement item in parent.Elements())
			{
				if (!standardConfigs.Contains(item.Name.ToString()))
				{
					dictionary.Add(item.Name.ToString(), item);
				}
			}
			return dictionary;
		}
	}
}
