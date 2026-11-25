using Amazon.Runtime;

namespace Amazon.S3.Model
{
	public class RestoreObjectRequest : AmazonWebServiceRequest
	{
		private string bucketName;

		private string key;

		private int? days;

		private string versionId;

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

		public int Days
		{
			get
			{
				int? num = days;
				return num.HasValue ? num.Value : 0;
			}
			set
			{
				days = value;
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

		internal bool IsSetBucketName()
		{
			return bucketName != null;
		}

		internal bool IsSetKey()
		{
			return key != null;
		}

		internal bool IsSetDays()
		{
			return days.HasValue;
		}

		internal bool IsSetVersionId()
		{
			return versionId != null;
		}
	}
}
