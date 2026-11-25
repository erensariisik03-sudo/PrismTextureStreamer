using Amazon.Runtime;
using Amazon.Util.Internal;

namespace Amazon.S3
{
	public class AmazonS3Config : ClientConfig
	{
		private bool forcePathStyle;

		private static readonly string UserAgentString = InternalSDKUtils.BuildUserAgentString("2.1.0.0");

		private string _userAgent = UserAgentString;

		public bool ForcePathStyle
		{
			get
			{
				return forcePathStyle;
			}
			set
			{
				forcePathStyle = value;
			}
		}

		public override string RegionEndpointServiceName
		{
			get
			{
				return "s3";
			}
		}

		public override string ServiceVersion
		{
			get
			{
				return "2006-03-01";
			}
		}

		public override string UserAgent
		{
			get
			{
				return _userAgent;
			}
		}

		public AmazonS3Config()
		{
			base.AuthenticationServiceName = "s3";
		}

		protected override void Initialize()
		{
			base.AllowAutoRedirect = false;
		}
	}
}
