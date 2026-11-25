using Amazon.Runtime;

namespace Amazon.S3.Model
{
	public class ListObjectsRequest : AmazonWebServiceRequest
	{
		private string bucketName;

		private string delimiter;

		private string marker;

		private int? maxKeys;

		private string prefix;

		private EncodingType encoding;

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

		public string Delimiter
		{
			get
			{
				return delimiter;
			}
			set
			{
				delimiter = value;
			}
		}

		public string Marker
		{
			get
			{
				return marker;
			}
			set
			{
				marker = value;
			}
		}

		public int MaxKeys
		{
			get
			{
				int? num = maxKeys;
				return num.HasValue ? num.Value : 0;
			}
			set
			{
				maxKeys = value;
			}
		}

		public string Prefix
		{
			get
			{
				return prefix;
			}
			set
			{
				prefix = value;
			}
		}

		public EncodingType Encoding
		{
			get
			{
				return encoding;
			}
			set
			{
				encoding = value;
			}
		}

		internal bool IsSetBucketName()
		{
			return bucketName != null;
		}

		internal bool IsSetDelimiter()
		{
			return delimiter != null;
		}

		internal bool IsSetMarker()
		{
			return marker != null;
		}

		internal bool IsSetMaxKeys()
		{
			return maxKeys.HasValue;
		}

		internal bool IsSetPrefix()
		{
			return prefix != null;
		}

		internal bool IsSetEncoding()
		{
			return encoding != null;
		}
	}
}
