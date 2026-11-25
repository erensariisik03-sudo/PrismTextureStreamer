using Amazon.Runtime;

namespace Amazon.S3.Model
{
	public class DeleteObjectRequest : AmazonWebServiceRequest
	{
		private string bucketName;

		private string key;

		private string versionId;

		private MfaCodes mfaCodes;

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

		public string Key
		{
			get
			{
				return key;
			}
			set
			{
				key = value;
			}
		}

		public string VersionId
		{
			get
			{
				return versionId;
			}
			set
			{
				versionId = value;
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

		internal bool IsSetBucketName()
		{
			return bucketName != null;
		}

		internal bool IsSetKey()
		{
			return key != null;
		}

		internal bool IsSetVersionId()
		{
			return !string.IsNullOrEmpty(versionId);
		}

		internal bool IsSetMfaCodes()
		{
			return mfaCodes != null && !string.IsNullOrEmpty(MfaCodes.SerialNumber) && !string.IsNullOrEmpty(MfaCodes.AuthenticationValue);
		}
	}
}
