using System;
using System.Globalization;
using System.Net;
using Amazon.Util;

namespace Amazon.Runtime
{
	public abstract class ClientConfig
	{
		internal static readonly TimeSpan InfiniteTimeout = TimeSpan.FromMilliseconds(-1.0);

		public static readonly TimeSpan MaxTimeout = TimeSpan.FromMilliseconds(2147483647.0);

		private RegionEndpoint regionEndpoint = GetDefaultRegionEndpoint();

		private bool useHttp;

		private string serviceURL;

		private string authRegion;

		private string authServiceName;

		private string signatureVersion = "2";

		private SigningAlgorithm signatureMethod = SigningAlgorithm.HmacSHA256;

		private int maxErrorRetry = 4;

		private bool readEntireResponse;

		private bool logResponse;

		private int bufferSize = 8192;

		private long progressUpdateInterval = 102400L;

		private bool resignRetries;

		private ICredentials proxyCredentials;

		private bool logMetrics = AWSConfigs.LoggingConfig.LogMetrics;

		private bool disableLogging;

		private TimeSpan? timeout;

		private bool allowAutoRedirect = true;

		private string proxyHost;

		private int proxyPort = -1;

		private int? connectionLimit;

		private int? maxIdleTime;

		private bool useNagleAlgorithm;

		private TimeSpan? readWriteTimeout;

		public abstract string ServiceVersion { get; }

		public SigningAlgorithm SignatureMethod
		{
			get
			{
				return signatureMethod;
			}
			set
			{
				signatureMethod = value;
			}
		}

		public string SignatureVersion
		{
			get
			{
				return signatureVersion;
			}
			set
			{
				signatureVersion = value;
			}
		}

		public abstract string UserAgent { get; }

		public RegionEndpoint RegionEndpoint
		{
			get
			{
				return regionEndpoint;
			}
			set
			{
				serviceURL = null;
				regionEndpoint = value;
				if (regionEndpoint != null)
				{
					RegionEndpoint.Endpoint endpointForService = regionEndpoint.GetEndpointForService(RegionEndpointServiceName);
					if (endpointForService != null && endpointForService.SignatureVersionOverride != null)
					{
						SignatureVersion = endpointForService.SignatureVersionOverride;
					}
				}
			}
		}

		public abstract string RegionEndpointServiceName { get; }

		public string ServiceURL
		{
			get
			{
				return serviceURL;
			}
			set
			{
				regionEndpoint = null;
				serviceURL = value;
			}
		}

		public bool UseHttp
		{
			get
			{
				return useHttp;
			}
			set
			{
				useHttp = value;
			}
		}

		public string AuthenticationRegion
		{
			get
			{
				return authRegion;
			}
			set
			{
				authRegion = value;
			}
		}

		public string AuthenticationServiceName
		{
			get
			{
				return authServiceName;
			}
			set
			{
				authServiceName = value;
			}
		}

		public int MaxErrorRetry
		{
			get
			{
				return maxErrorRetry;
			}
			set
			{
				maxErrorRetry = value;
			}
		}

		public bool LogResponse
		{
			get
			{
				return logResponse;
			}
			set
			{
				logResponse = value;
			}
		}

		public bool ReadEntireResponse
		{
			get
			{
				return readEntireResponse;
			}
			set
			{
				readEntireResponse = value;
			}
		}

		public int BufferSize
		{
			get
			{
				return bufferSize;
			}
			set
			{
				bufferSize = value;
			}
		}

		public long ProgressUpdateInterval
		{
			get
			{
				return progressUpdateInterval;
			}
			set
			{
				progressUpdateInterval = value;
			}
		}

		public bool ResignRetries
		{
			get
			{
				return resignRetries;
			}
			set
			{
				resignRetries = value;
			}
		}

		protected internal bool AllowAutoRedirect
		{
			get
			{
				return allowAutoRedirect;
			}
			set
			{
				allowAutoRedirect = value;
			}
		}

		public bool LogMetrics
		{
			get
			{
				return logMetrics;
			}
			set
			{
				logMetrics = value;
			}
		}

		public bool DisableLogging
		{
			get
			{
				return disableLogging;
			}
			set
			{
				disableLogging = value;
			}
		}

		public ICredentials ProxyCredentials
		{
			get
			{
				if (proxyCredentials == null && (!string.IsNullOrEmpty(AWSConfigs.ProxyConfig.Username) || !string.IsNullOrEmpty(AWSConfigs.ProxyConfig.Password)))
				{
					return new NetworkCredential(AWSConfigs.ProxyConfig.Username, AWSConfigs.ProxyConfig.Password ?? string.Empty);
				}
				return proxyCredentials;
			}
			set
			{
				proxyCredentials = value;
			}
		}

		public TimeSpan? Timeout
		{
			get
			{
				return timeout;
			}
			set
			{
				ValidateTimeout(value);
				timeout = value;
			}
		}

		public string ProxyHost
		{
			get
			{
				if (string.IsNullOrEmpty(proxyHost))
				{
					return AWSConfigs.ProxyConfig.Host;
				}
				return proxyHost;
			}
			set
			{
				proxyHost = value;
			}
		}

		public int ProxyPort
		{
			get
			{
				if (proxyPort <= 0)
				{
					return AWSConfigs.ProxyConfig.Port.GetValueOrDefault();
				}
				return proxyPort;
			}
			set
			{
				proxyPort = value;
			}
		}

		public int MaxIdleTime
		{
			get
			{
				return AWSSDKUtils.GetMaxIdleTime(maxIdleTime);
			}
			set
			{
				maxIdleTime = value;
			}
		}

		public int ConnectionLimit
		{
			get
			{
				return AWSSDKUtils.GetConnectionLimit(connectionLimit);
			}
			set
			{
				connectionLimit = value;
			}
		}

		public bool UseNagleAlgorithm
		{
			get
			{
				return useNagleAlgorithm;
			}
			set
			{
				useNagleAlgorithm = value;
			}
		}

		public TimeSpan? ReadWriteTimeout
		{
			get
			{
				return readWriteTimeout;
			}
			set
			{
				ValidateTimeout(value);
				readWriteTimeout = value;
			}
		}

		public ClientConfig()
		{
			Initialize();
		}

		public string DetermineServiceURL()
		{
			if (ServiceURL != null)
			{
				return ServiceURL;
			}
			return GetUrl(RegionEndpoint, RegionEndpointServiceName, UseHttp);
		}

		internal static string GetUrl(RegionEndpoint regionEndpoint, string regionEndpointServiceName, bool useHttp)
		{
			RegionEndpoint.Endpoint endpointForService = regionEndpoint.GetEndpointForService(regionEndpointServiceName);
			return new Uri(string.Format(CultureInfo.InvariantCulture, "{0}{1}", (!useHttp) ? "https://" : "http://", endpointForService.Hostname)).AbsoluteUri;
		}

		protected virtual void Initialize()
		{
		}

		public void SetUseNagleIfAvailable(bool useNagle)
		{
		}

		public virtual void Validate()
		{
			if (RegionEndpoint == null && string.IsNullOrEmpty(ServiceURL))
			{
				throw new AmazonClientException("No RegionEndpoint or ServiceURL configured");
			}
		}

		public static void ValidateTimeout(TimeSpan? timeout)
		{
			if (!timeout.HasValue)
			{
				throw new ArgumentNullException("timeout");
			}
			if ((timeout.GetValueOrDefault() != InfiniteTimeout || !timeout.HasValue) && ((timeout.HasValue && timeout.Value <= TimeSpan.Zero) || (timeout.HasValue && timeout.Value > MaxTimeout)))
			{
				throw new ArgumentOutOfRangeException("timeout");
			}
		}

		public static TimeSpan? GetTimeoutValue(TimeSpan? clientTimeout, TimeSpan? requestTimeout)
		{
			return requestTimeout ?? clientTimeout ?? ((TimeSpan?)null);
		}

		private static RegionEndpoint GetDefaultRegionEndpoint()
		{
			string aWSRegion = AWSConfigs.AWSRegion;
			if (!string.IsNullOrEmpty(aWSRegion))
			{
				RegionEndpoint bySystemName = RegionEndpoint.GetBySystemName(aWSRegion);
				if (bySystemName == null)
				{
					throw new ArgumentException("Region {0} specified in the app.config is not a valid region name", aWSRegion);
				}
				return bySystemName;
			}
			return null;
		}
	}
}
