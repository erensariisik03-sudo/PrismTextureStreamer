using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using Amazon.Runtime.Internal;
using Amazon.Util;
using ThirdParty.Json.LitJson;
using UnityEngine;

namespace Amazon
{
	public class RegionEndpoint
	{
		public class Endpoint
		{
			public string Hostname { get; private set; }

			public string AuthRegion { get; private set; }

			public string SignatureVersionOverride { get; private set; }

			internal Endpoint(string hostname, string authregion, string signatureVersionOverride)
			{
				Hostname = hostname;
				AuthRegion = authregion;
				SignatureVersionOverride = signatureVersionOverride;
			}

			public override string ToString()
			{
				return Hostname;
			}
		}

		private const string REGIONS_FILE = "endpoints";

		private const string REGIONS_CUSTOMIZATIONS_FILE = "endpoints.customizations";

		private const int MAX_DOWNLOAD_RETRIES = 3;

		private const string DEFAULT_RULE = "*/*";

		private static Dictionary<string, JsonData> _documentEndpoints;

		private static bool loaded = false;

		private static readonly object LOCK_OBJECT = new object();

		private static Dictionary<string, RegionEndpoint> hashBySystemName = new Dictionary<string, RegionEndpoint>(StringComparer.OrdinalIgnoreCase);

		public static readonly RegionEndpoint USEast1 = NewEndpoint("us-east-1", "US East (Virginia)");

		public static readonly RegionEndpoint USWest1 = NewEndpoint("us-west-1", "US West (N. California)");

		public static readonly RegionEndpoint USWest2 = NewEndpoint("us-west-2", "US West (Oregon)");

		public static readonly RegionEndpoint EUWest1 = NewEndpoint("eu-west-1", "EU West (Ireland)");

		public static readonly RegionEndpoint EUCentral1 = NewEndpoint("eu-central-1", "EU Central (Frankfurt)");

		public static readonly RegionEndpoint APNortheast1 = NewEndpoint("ap-northeast-1", "Asia Pacific (Tokyo)");

		public static readonly RegionEndpoint APSoutheast1 = NewEndpoint("ap-southeast-1", "Asia Pacific (Singapore)");

		public static readonly RegionEndpoint APSoutheast2 = NewEndpoint("ap-southeast-2", "Asia Pacific (Sydney)");

		public static readonly RegionEndpoint SAEast1 = NewEndpoint("sa-east-1", "South America (Sao Paulo)");

		public static readonly RegionEndpoint USGovCloudWest1 = NewEndpoint("us-gov-west-1", "US GovCloud West (Oregon)");

		public static readonly RegionEndpoint CNNorth1 = NewEndpoint("cn-north-1", "China (Beijing)");

		public static IEnumerable<RegionEndpoint> EnumerableAllRegions
		{
			get
			{
				if (!loaded)
				{
					LoadEndpointDefinitions();
				}
				return hashBySystemName.Values;
			}
		}

		public string SystemName { get; private set; }

		public string DisplayName { get; private set; }

		private RegionEndpoint(string systemName, string displayName)
		{
			SystemName = systemName;
			DisplayName = displayName;
		}

		public Endpoint GetEndpointForService(string serviceName)
		{
			if (!loaded)
			{
				LoadEndpointDefinitions();
			}
			JsonData endpointRule = GetEndpointRule(serviceName);
			string text = endpointRule["endpoint"].ToString();
			string text2 = text.Replace("{region}", SystemName).Replace("{service}", serviceName);
			string signatureVersionOverride = null;
			if (endpointRule["signature-version"] != null)
			{
				signatureVersionOverride = endpointRule["signature-version"].ToString();
			}
			string text3 = ((endpointRule["auth-region"] == null) ? AWSSDKUtils.DetermineRegion(text2) : endpointRule["auth-region"].ToString());
			return new Endpoint(text2, string.Equals(text3, SystemName) ? null : text3, signatureVersionOverride);
		}

		private JsonData GetEndpointRule(string serviceName)
		{
			JsonData value = null;
			if (_documentEndpoints.TryGetValue(string.Format(CultureInfo.InvariantCulture, "{0}/{1}", SystemName, serviceName), out value))
			{
				return value;
			}
			if (_documentEndpoints.TryGetValue(string.Format(CultureInfo.InvariantCulture, "{0}/*", SystemName), out value))
			{
				return value;
			}
			if (_documentEndpoints.TryGetValue(string.Format(CultureInfo.InvariantCulture, "*/{0}", serviceName), out value))
			{
				return value;
			}
			return _documentEndpoints["*/*"];
		}

		private static RegionEndpoint NewEndpoint(string systemName, string displayName)
		{
			RegionEndpoint regionEndpoint = new RegionEndpoint(systemName, displayName);
			hashBySystemName.Add(regionEndpoint.SystemName, regionEndpoint);
			return regionEndpoint;
		}

		public static RegionEndpoint GetBySystemName(string systemName)
		{
			if (!loaded)
			{
				LoadEndpointDefinitions();
			}
			RegionEndpoint value = null;
			if (!hashBySystemName.TryGetValue(systemName, out value))
			{
				if (systemName.StartsWith("cn-", StringComparison.Ordinal))
				{
					return NewEndpoint(systemName, "China (Unknown)");
				}
				return NewEndpoint(systemName, "Unknown");
			}
			return value;
		}

		private static void LoadEndpointDefinitions()
		{
			lock (LOCK_OBJECT)
			{
				if (!loaded)
				{
					_documentEndpoints = new Dictionary<string, JsonData>();
					string endpointDefinition = AWSConfigs.EndpointDefinition;
					if (string.IsNullOrEmpty(endpointDefinition))
					{
						LoadEndpointDefinitionsFromEmbeddedResource();
					}
					loaded = true;
				}
			}
		}

		private static void ReadEndpointFile(Stream stream)
		{
			using (StreamReader reader = new StreamReader(stream))
			{
				JsonData jsonData = JsonMapper.ToObject(reader);
				JsonData jsonData2 = jsonData["endpoints"];
				foreach (string propertyName in jsonData2.PropertyNames)
				{
					_documentEndpoints[propertyName] = jsonData2[propertyName];
				}
			}
		}

		private static void LoadEndpointDefinitionsFromEmbeddedResource()
		{
			Action action = () =>
			{
				TextAsset textAsset = Resources.Load("endpoints") as TextAsset;
				using (Stream stream = new MemoryStream(textAsset.bytes))
				{
					using (new StreamReader(stream))
					{
						ReadEndpointFile(stream);
					}
				}
				textAsset = Resources.Load("endpoints.customizations") as TextAsset;
				using (Stream stream2 = new MemoryStream(textAsset.bytes))
				{
					using (new StreamReader(stream2))
					{
						ReadEndpointFile(stream2);
					}
				}
			};
			if (UnityInitializer.IsMainThread())
			{
				action();
				return;
			}
			ManualResetEvent e = new ManualResetEvent(false);
			UnityRequestQueue.Instance.ExecuteOnMainThread(() =>
			{
				action();
				e.Set();
			});
			e.WaitOne();
		}

		public static void UnloadEndpointDefinitions()
		{
			lock (LOCK_OBJECT)
			{
				_documentEndpoints.Clear();
				loaded = false;
			}
		}

		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "{0} ({1})", DisplayName, SystemName);
		}
	}
}
