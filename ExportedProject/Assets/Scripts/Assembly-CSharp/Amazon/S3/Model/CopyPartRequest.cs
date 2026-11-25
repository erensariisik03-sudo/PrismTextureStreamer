using System;
using System.Collections.Generic;
using Amazon.Runtime;

namespace Amazon.S3.Model
{
	public class CopyPartRequest : AmazonWebServiceRequest
	{
		private string srcBucket;

		private string srcKey;

		private string srcVersionId;

		private string dstBucket;

		private string dstKey;

		private string uploadId;

		private List<string> etagsToMatch;

		private List<string> etagsToNotMatch;

		private DateTime? modifiedSinceDate;

		private DateTime? unmodifiedSinceDate;

		private int? partNumber;

		private long? firstByte;

		private long? lastByte;

		private ServerSideEncryptionMethod serverSideEncryption;

		private ServerSideEncryptionCustomerMethod serverSideCustomerEncryption;

		private string serverSideEncryptionCustomerProvidedKey;

		private string serverSideEncryptionCustomerProvidedKeyMD5;

		private string serverSideEncryptionKeyManagementServiceKeyId;

		private ServerSideEncryptionCustomerMethod copySourceServerSideCustomerEncryption;

		private string copySourceServerSideEncryptionCustomerProvidedKey;

		private string copySourceServerSideEncryptionCustomerProvidedKeyMD5;

		public string SourceBucket
		{
			get
			{
				return srcBucket;
			}
			set
			{
				srcBucket = value;
			}
		}

		public string SourceKey
		{
			get
			{
				return srcKey;
			}
			set
			{
				srcKey = value;
			}
		}

		public string SourceVersionId
		{
			get
			{
				return srcVersionId;
			}
			set
			{
				srcVersionId = value;
			}
		}

		public string DestinationBucket
		{
			get
			{
				return dstBucket;
			}
			set
			{
				dstBucket = value;
			}
		}

		public string DestinationKey
		{
			get
			{
				return dstKey;
			}
			set
			{
				dstKey = value;
			}
		}

		public string UploadId
		{
			get
			{
				return uploadId;
			}
			set
			{
				uploadId = value;
			}
		}

		public List<string> ETagToMatch
		{
			get
			{
				if (etagsToMatch == null)
				{
					etagsToMatch = new List<string>();
				}
				return etagsToMatch;
			}
			set
			{
				etagsToMatch = value;
			}
		}

		public List<string> ETagsToNotMatch
		{
			get
			{
				if (etagsToNotMatch == null)
				{
					etagsToNotMatch = new List<string>();
				}
				return etagsToNotMatch;
			}
			set
			{
				etagsToNotMatch = value;
			}
		}

		public DateTime ModifiedSinceDate
		{
			get
			{
				return modifiedSinceDate.GetValueOrDefault();
			}
			set
			{
				modifiedSinceDate = value;
			}
		}

		public DateTime UnmodifiedSinceDate
		{
			get
			{
				return unmodifiedSinceDate.GetValueOrDefault();
			}
			set
			{
				unmodifiedSinceDate = value;
			}
		}

		public int PartNumber
		{
			get
			{
				return partNumber.GetValueOrDefault();
			}
			set
			{
				partNumber = value;
			}
		}

		public long FirstByte
		{
			get
			{
				return firstByte.GetValueOrDefault();
			}
			set
			{
				firstByte = value;
			}
		}

		public long LastByte
		{
			get
			{
				return lastByte.GetValueOrDefault();
			}
			set
			{
				lastByte = value;
			}
		}

		public ServerSideEncryptionMethod ServerSideEncryptionMethod
		{
			get
			{
				return serverSideEncryption;
			}
			set
			{
				serverSideEncryption = value;
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

		public string ServerSideEncryptionKeyManagementServiceKeyId
		{
			get
			{
				return serverSideEncryptionKeyManagementServiceKeyId;
			}
			set
			{
				serverSideEncryptionKeyManagementServiceKeyId = value;
			}
		}

		public ServerSideEncryptionCustomerMethod CopySourceServerSideEncryptionCustomerMethod
		{
			get
			{
				return copySourceServerSideCustomerEncryption;
			}
			set
			{
				copySourceServerSideCustomerEncryption = value;
			}
		}

		public string CopySourceServerSideEncryptionCustomerProvidedKey
		{
			get
			{
				return copySourceServerSideEncryptionCustomerProvidedKey;
			}
			set
			{
				copySourceServerSideEncryptionCustomerProvidedKey = value;
			}
		}

		public string CopySourceServerSideEncryptionCustomerProvidedKeyMD5
		{
			get
			{
				return copySourceServerSideEncryptionCustomerProvidedKeyMD5;
			}
			set
			{
				copySourceServerSideEncryptionCustomerProvidedKeyMD5 = value;
			}
		}

		internal bool IsSetSourceBucket()
		{
			return !string.IsNullOrEmpty(srcBucket);
		}

		internal bool IsSetSourceKey()
		{
			return !string.IsNullOrEmpty(srcKey);
		}

		internal bool IsSetSourceVersionId()
		{
			return !string.IsNullOrEmpty(srcVersionId);
		}

		internal bool IsSetDestinationBucket()
		{
			return !string.IsNullOrEmpty(dstBucket);
		}

		internal bool IsSetDestinationKey()
		{
			return !string.IsNullOrEmpty(dstKey);
		}

		internal bool IsSetUploadId()
		{
			return !string.IsNullOrEmpty(uploadId);
		}

		internal bool IsSetETagToMatch()
		{
			return etagsToMatch != null && etagsToMatch.Count > 0;
		}

		internal bool IsSetETagToNotMatch()
		{
			return etagsToNotMatch != null && etagsToNotMatch.Count > 0;
		}

		internal bool IsSetModifiedSinceDate()
		{
			return modifiedSinceDate.HasValue;
		}

		internal bool IsSetUnmodifiedSinceDate()
		{
			return unmodifiedSinceDate.HasValue;
		}

		internal bool IsSetPartNumber()
		{
			return partNumber.HasValue;
		}

		internal bool IsSetFirstByte()
		{
			return firstByte.HasValue;
		}

		internal bool IsSetLastByte()
		{
			return lastByte.HasValue;
		}

		internal bool IsSetServerSideEncryptionMethod()
		{
			return serverSideEncryption != null && serverSideEncryption != ServerSideEncryptionMethod.None;
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

		internal bool IsSetServerSideEncryptionKeyManagementServiceKeyId()
		{
			return !string.IsNullOrEmpty(serverSideEncryptionKeyManagementServiceKeyId);
		}

		internal bool IsSetCopySourceServerSideEncryptionCustomerMethod()
		{
			return copySourceServerSideCustomerEncryption != null && copySourceServerSideCustomerEncryption != ServerSideEncryptionCustomerMethod.None;
		}

		internal bool IsSetCopySourceServerSideEncryptionCustomerProvidedKey()
		{
			return !string.IsNullOrEmpty(copySourceServerSideEncryptionCustomerProvidedKey);
		}

		internal bool IsSetCopySourceServerSideEncryptionCustomerProvidedKeyMD5()
		{
			return !string.IsNullOrEmpty(copySourceServerSideEncryptionCustomerProvidedKeyMD5);
		}
	}
}
