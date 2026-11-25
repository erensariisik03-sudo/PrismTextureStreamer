using System;
using Amazon.Runtime;

namespace Amazon.S3.Model
{
	public class GetObjectMetadataRequest : AmazonWebServiceRequest
	{
		private string bucketName;

		private DateTime? modifiedSinceDate;

		private DateTime? unmodifiedSinceDate;

		private string etagToMatch;

		private string etagToNotMatch;

		private string key;

		private string versionId;

		private ServerSideEncryptionCustomerMethod serverSideCustomerEncryption;

		private string serverSideEncryptionCustomerProvidedKey;

		private string serverSideEncryptionCustomerProvidedKeyMD5;

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

		public string EtagToMatch
		{
			get
			{
				return etagToMatch;
			}
			set
			{
				etagToMatch = value;
			}
		}

		public DateTime ModifiedSinceDate
		{
			get
			{
				DateTime? dateTime = modifiedSinceDate;
				return (!dateTime.HasValue) ? default(DateTime) : dateTime.Value;
			}
			set
			{
				modifiedSinceDate = value;
			}
		}

		public string EtagToNotMatch
		{
			get
			{
				return etagToNotMatch;
			}
			set
			{
				etagToNotMatch = value;
			}
		}

		public DateTime UnmodifiedSinceDate
		{
			get
			{
				DateTime? dateTime = unmodifiedSinceDate;
				return (!dateTime.HasValue) ? default(DateTime) : dateTime.Value;
			}
			set
			{
				unmodifiedSinceDate = value;
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

		public ServerSideEncryptionCustomerMethod ServerSideEncryptionCustomerMethod
		{
			get
			{
				return serverSideCustomerEncryption;
			}
			set
			{
				serverSideCustomerEncryption = value;
			}
		}

		public string ServerSideEncryptionCustomerProvidedKey
		{
			get
			{
				return serverSideEncryptionCustomerProvidedKey;
			}
			set
			{
				serverSideEncryptionCustomerProvidedKey = value;
			}
		}

		public string ServerSideEncryptionCustomerProvidedKeyMD5
		{
			get
			{
				return serverSideEncryptionCustomerProvidedKeyMD5;
			}
			set
			{
				serverSideEncryptionCustomerProvidedKeyMD5 = value;
			}
		}

		internal bool IsSetBucketName()
		{
			return bucketName != null;
		}

		internal bool IsSetEtagToMatch()
		{
			return etagToMatch != null;
		}

		internal bool IsSetModifiedSinceDate()
		{
			return modifiedSinceDate.HasValue;
		}

		internal bool IsSetEtagToNotMatch()
		{
			return etagToNotMatch != null;
		}

		internal bool IsSetUnmodifiedSinceDate()
		{
			return unmodifiedSinceDate.HasValue;
		}

		internal bool IsSetKey()
		{
			return key != null;
		}

		internal bool IsSetVersionId()
		{
			return versionId != null;
		}

		internal bool IsSetServerSideEncryptionCustomerMethod()
		{
			return serverSideCustomerEncryption != null && serverSideCustomerEncryption != ServerSideEncryptionCustomerMethod.None;
		}

		internal bool IsSetServerSideEncryptionCustomerProvidedKey()
		{
			return !string.IsNullOrEmpty(serverSideEncryptionCustomerProvidedKey);
		}

		internal bool IsSetServerSideEncryptionCustomerProvidedKeyMD5()
		{
			return !string.IsNullOrEmpty(serverSideEncryptionCustomerProvidedKeyMD5);
		}
	}
}
