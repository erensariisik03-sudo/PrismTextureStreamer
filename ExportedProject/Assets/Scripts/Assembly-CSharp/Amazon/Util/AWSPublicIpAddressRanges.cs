using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using Amazon.Runtime;
using ThirdParty.Json.LitJson;

namespace Amazon.Util
{
	public class AWSPublicIpAddressRanges
	{
		public const string AmazonServiceKey = "AMAZON";

		public const string EC2ServiceKey = "EC2";

		public const string CloudFrontServiceKey = "CLOUDFRONT";

		public const string Route53ServiceKey = "ROUTE53";

		public const string Route53HealthChecksServiceKey = "ROUTE53_HEALTHCHECKS";

		public const string GlobalRegionIdentifier = "GLOBAL";

		private static readonly Uri IpAddressRangeEndpoint = new Uri("https://ip-ranges.amazonaws.com/ip-ranges.json");

		public IEnumerable<string> ServiceKeys
		{
			get
			{
				HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
				foreach (AWSPublicIpAddressRange allAddressRange in AllAddressRanges)
				{
					hashSet.Add(allAddressRange.Service);
				}
				return hashSet;
			}
		}

		public DateTime CreateDate { get; private set; }

		public IEnumerable<AWSPublicIpAddressRange> AllAddressRanges { get; private set; }

		private AWSPublicIpAddressRanges()
		{
		}

		public IEnumerable<AWSPublicIpAddressRange> AddressRangesByServiceKey(string serviceKey)
		{
			if (!AllAddressRanges.Any())
			{
				throw new InvalidOperationException("No address range data has been loaded and parsed.");
			}
			return AllAddressRanges.Where((AWSPublicIpAddressRange ar) => ar.Service.Equals(serviceKey, StringComparison.OrdinalIgnoreCase)).ToList();
		}

		public IEnumerable<AWSPublicIpAddressRange> AddressRangesByRegion(string regionIdentifier)
		{
			if (!AllAddressRanges.Any())
			{
				throw new InvalidOperationException("No address range data has been loaded and parsed.");
			}
			return AllAddressRanges.Where((AWSPublicIpAddressRange ar) => ar.Region.Equals(regionIdentifier, StringComparison.OrdinalIgnoreCase)).ToList();
		}

		public static AWSPublicIpAddressRanges Load()
		{
			int num = 0;
			while (num < 3)
			{
				try
				{
					HttpWebRequest httpWebRequest = WebRequest.Create(IpAddressRangeEndpoint) as HttpWebRequest;
					IAsyncResult asyncResult = httpWebRequest.BeginGetResponse(null, null);
					HttpWebResponse httpWebResponse = httpWebRequest.EndGetResponse(asyncResult) as HttpWebResponse;
					using (httpWebResponse)
					{
						using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
						{
							return Parse(streamReader.ReadToEnd());
						}
					}
				}
				catch (WebException innerException)
				{
					num++;
					if (num == 3)
					{
						throw new AmazonServiceException(string.Format(CultureInfo.InvariantCulture, "Error downloading public IP address ranges file from {0}.", IpAddressRangeEndpoint), innerException);
					}
				}
				int val = (int)(Math.Pow(4.0, num) * 100.0);
				val = Math.Min(val, 30000);
				AWSSDKUtils.Sleep(val);
			}
			return null;
		}

		private static AWSPublicIpAddressRanges Parse(string fileContent)
		{
			try
			{
				AWSPublicIpAddressRanges aWSPublicIpAddressRanges = new AWSPublicIpAddressRanges();
				JsonData jsonData = JsonMapper.ToObject(new JsonReader(fileContent));
				DateTime? dateTime = null;
				try
				{
					string s = (string)jsonData["createDate"];
					dateTime = DateTime.ParseExact(s, "yyyy-MM-dd-HH-mm-ss", null);
				}
				catch (FormatException)
				{
				}
				catch (ArgumentNullException)
				{
				}
				aWSPublicIpAddressRanges.CreateDate = dateTime.GetValueOrDefault(DateTime.Now.ToUniversalTime());
				List<AWSPublicIpAddressRange> list = (List<AWSPublicIpAddressRange>)(aWSPublicIpAddressRanges.AllAddressRanges = new List<AWSPublicIpAddressRange>());
				JsonData jsonData2 = jsonData["prefixes"];
				if (!jsonData2.IsArray)
				{
					throw new InvalidDataException("Expected array content for prefixes key.");
				}
				list.AddRange(from JsonData range in jsonData2
					select new AWSPublicIpAddressRange
					{
						IpPrefix = (string)range["ip_prefix"],
						Region = (string)range["region"],
						Service = (string)range["service"]
					});
				return aWSPublicIpAddressRanges;
			}
			catch (Exception innerException)
			{
				throw new InvalidDataException("IP address ranges content in unexpected/invalid format.", innerException);
			}
		}
	}
}
