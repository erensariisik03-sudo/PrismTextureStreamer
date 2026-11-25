using System.Xml.Linq;
using Amazon.Util.Internal;

namespace Amazon
{
	public static class AWSConfigsS3
	{
		private const string s3Key = "s3";

		public const string S3UseSignatureVersion4Key = "AWS.S3.UseSignatureVersion4";

		public static bool UseSignatureVersion4 { get; set; }

		static AWSConfigsS3()
		{
			string config = AWSConfigs.GetConfig("AWS.S3.UseSignatureVersion4");
			if (!string.IsNullOrEmpty(config))
			{
				UseSignatureVersion4 = bool.Parse(config);
			}
			RootConfig rootConfig = new RootConfig();
			XElement serviceSection = rootConfig.GetServiceSection("s3");
			if (serviceSection != null)
			{
				V4ClientSectionRoot v4ClientSectionRoot = new V4ClientSectionRoot(serviceSection);
				if (v4ClientSectionRoot.S3 != null)
				{
					Configure(v4ClientSectionRoot.S3);
				}
			}
		}

		internal static void Configure(V4ClientSection section)
		{
			if (section.ElementInformation.IsPresent)
			{
				UseSignatureVersion4 = section.UseSignatureVersion4 == true;
			}
		}
	}
}
