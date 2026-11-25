using Amazon.Runtime;

namespace Amazon.S3.Model
{
	public class PutBucketVersioningRequest : AmazonWebServiceRequest
	{
		private string bucketName;

		private MfaCodes mfaCodes;

		private S3BucketVersioningConfig config;

		public string BucketName
		{
			get
			{
				return bucketName;
			}
			set
			{
				bucketName = value;
			}
		}

		public MfaCodes MfaCodes
		{
			get
			{
				return mfaCodes;
			}
			set
			{
				mfaCodes = value;
			}
		}

		public S3BucketVersioningConfig VersioningConfig
		{
			get
			{
				if (config == null)
				{
					config = new S3BucketVersioningConfig();
				}
				return config;
			}
			set
			{
				config = value;
			}
		}

		internal bool IsSetBucketName()
		{
			return bucketName != null;
		}

		internal bool IsSetMfaCodes()
		{
			return mfaCodes != null && !string.IsNullOrEmpty(MfaCodes.SerialNumber) && !string.IsNullOrEmpty(MfaCodes.AuthenticationValue);
		}

		internal bool IsSetVersioningConfiguration()
		{
			return config != null;
		}
	}
}
