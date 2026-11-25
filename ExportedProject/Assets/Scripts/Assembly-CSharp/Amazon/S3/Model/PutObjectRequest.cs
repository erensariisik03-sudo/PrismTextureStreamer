using System;
using System.IO;
using Amazon.Runtime;
using Amazon.Runtime.Internal;

namespace Amazon.S3.Model
{
	public class PutObjectRequest : PutWithACLRequest
	{
		private S3CannedACL cannedACL;

		private string bucketName;

		private string key;

		private S3StorageClass storageClass;

		private string websiteRedirectLocation;

		private HeadersCollection headersCollection = new HeadersCollection();

		private MetadataCollection metadataCollection = new MetadataCollection();

		private ServerSideEncryptionMethod serverSideEncryption;

		private ServerSideEncryptionCustomerMethod serverSideCustomerEncryption;

		private string serverSideEncryptionCustomerProvidedKey;

		private string serverSideEncryptionCustomerProvidedKeyMD5;

		private string serverSideEncryptionKeyManagementServiceKeyId;

		private Stream inputStream;

		private string filePath;

		private string contentBody;

		private bool autoCloseStream = true;

		private bool autoResetStreamPosition = true;

		private string md5Digest;

		public S3CannedACL CannedACL
		{
			get
			{
				return cannedACL;
			}
			set
			{
				cannedACL = value;
			}
		}

		public Stream InputStream
		{
			get
			{
				return inputStream;
			}
			set
			{
				inputStream = value;
			}
		}

		public string FilePath
		{
			get
			{
				return filePath;
			}
			set
			{
				filePath = value;
			}
		}

		public string ContentBody
		{
			get
			{
				return contentBody;
			}
			set
			{
				contentBody = value;
			}
		}

		public bool AutoCloseStream
		{
			get
			{
				return autoCloseStream;
			}
			set
			{
				autoCloseStream = value;
			}
		}

		public bool AutoResetStreamPosition
		{
			get
			{
				return autoResetStreamPosition;
			}
			set
			{
				autoResetStreamPosition = value;
			}
		}

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

		public HeadersCollection Headers
		{
			get
			{
				if (headersCollection == null)
				{
					headersCollection = new HeadersCollection();
				}
				return headersCollection;
			}
			internal set
			{
				headersCollection = value;
			}
		}

		public MetadataCollection Metadata
		{
			get
			{
				if (metadataCollection == null)
				{
					metadataCollection = new MetadataCollection();
				}
				return metadataCollection;
			}
			internal set
			{
				metadataCollection = value;
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

		public S3StorageClass StorageClass
		{
			get
			{
				return storageClass;
			}
			set
			{
				storageClass = value;
			}
		}

		public string WebsiteRedirectLocation
		{
			get
			{
				return websiteRedirectLocation;
			}
			set
			{
				websiteRedirectLocation = value;
			}
		}

		public EventHandler<StreamTransferProgressArgs> StreamTransferProgress
		{
			get
			{
				return ((IAmazonWebServiceRequest)this).StreamUploadProgressCallback;
			}
			set
			{
				((IAmazonWebServiceRequest)this).StreamUploadProgressCallback = value;
			}
		}

		public string ContentType
		{
			get
			{
				return Headers.ContentType;
			}
			set
			{
				Headers.ContentType = value;
			}
		}

		public string MD5Digest
		{
			get
			{
				return md5Digest;
			}
			set
			{
				md5Digest = value;
			}
		}

		protected override bool IncludeSHA256Header
		{
			get
			{
				return false;
			}
		}

		protected override bool Expect100Continue
		{
			get
			{
				return true;
			}
		}

		internal bool IsSetCannedACL()
		{
			return cannedACL != null && cannedACL != S3CannedACL.NoACL;
		}

		internal bool IsSetInputStream()
		{
			return inputStream != null;
		}

		internal bool IsSetBucket()
		{
			return bucketName != null;
		}

		internal bool IsSetKey()
		{
			return key != null;
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

		internal bool IsSetStorageClass()
		{
			return storageClass != null;
		}

		internal bool IsSetWebsiteRedirectLocation()
		{
			return websiteRedirectLocation != null;
		}

		internal bool IsSetMD5Digest()
		{
			return !string.IsNullOrEmpty(md5Digest);
		}

		internal void SetupForFilePath()
		{
		}
	}
}
