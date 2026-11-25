using System;

namespace Amazon.S3.Model
{
	public class GetObjectResponse : StreamResponse
	{
		private string deleteMarker;

		private string acceptRanges;

		private Expiration expiration;

		private DateTime? restoreExpiration;

		private bool restoreInProgress;

		private DateTime? lastModified;

		private string eTag;

		private int? missingMeta;

		private string versionId;

		private DateTime? expires;

		private string websiteRedirectLocation;

		private ServerSideEncryptionMethod serverSideEncryption;

		private ServerSideEncryptionCustomerMethod serverSideEncryptionCustomerMethod;

		private string serverSideEncryptionKeyManagementServiceKeyId;

		private HeadersCollection headersCollection = new HeadersCollection();

		private MetadataCollection metadataCollection = new MetadataCollection();

		private ReplicationStatus replicationStatus;

		private S3StorageClass storageClass;

		private string bucketName;

		private string key;

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

		public string DeleteMarker
		{
			get
			{
				return deleteMarker;
			}
			set
			{
				deleteMarker = value;
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
		}

		public string AcceptRanges
		{
			get
			{
				return acceptRanges;
			}
			set
			{
				acceptRanges = value;
			}
		}

		public Expiration Expiration
		{
			get
			{
				return expiration;
			}
			set
			{
				expiration = value;
			}
		}

		public DateTime? RestoreExpiration
		{
			get
			{
				return restoreExpiration;
			}
			set
			{
				restoreExpiration = value;
			}
		}

		public bool RestoreInProgress
		{
			get
			{
				return restoreInProgress;
			}
			set
			{
				restoreInProgress = value;
			}
		}

		public DateTime LastModified
		{
			get
			{
				DateTime? dateTime = lastModified;
				return (!dateTime.HasValue) ? default(DateTime) : dateTime.Value;
			}
			set
			{
				lastModified = value;
			}
		}

		public string ETag
		{
			get
			{
				return eTag;
			}
			set
			{
				eTag = value;
			}
		}

		public int MissingMeta
		{
			get
			{
				int? num = missingMeta;
				return num.HasValue ? num.Value : 0;
			}
			set
			{
				missingMeta = value;
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

		public DateTime Expires
		{
			get
			{
				DateTime? dateTime = expires;
				return (!dateTime.HasValue) ? default(DateTime) : dateTime.Value;
			}
			set
			{
				expires = value;
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

		public ReplicationStatus ReplicationStatus
		{
			get
			{
				return replicationStatus;
			}
			set
			{
				replicationStatus = value;
			}
		}

		public ServerSideEncryptionCustomerMethod ServerSideEncryptionCustomerMethod
		{
			get
			{
				if (serverSideEncryptionCustomerMethod == null)
				{
					return ServerSideEncryptionCustomerMethod.None;
				}
				return serverSideEncryptionCustomerMethod;
			}
			set
			{
				serverSideEncryptionCustomerMethod = value;
			}
		}

		internal bool IsSetDeleteMarker()
		{
			return deleteMarker != null;
		}

		internal bool IsSetAcceptRanges()
		{
			return acceptRanges != null;
		}

		internal bool IsSetExpiration()
		{
			return expiration != null;
		}

		internal bool IsSetLastModified()
		{
			return lastModified.HasValue;
		}

		internal bool IsSetETag()
		{
			return eTag != null;
		}

		internal bool IsSetMissingMeta()
		{
			return missingMeta.HasValue;
		}

		internal bool IsSetVersionId()
		{
			return versionId != null;
		}

		internal bool IsSetExpires()
		{
			return expires.HasValue;
		}

		internal bool IsSetWebsiteRedirectLocation()
		{
			return websiteRedirectLocation != null;
		}

		internal bool IsSetServerSideEncryptionMethod()
		{
			return serverSideEncryption != null;
		}

		internal bool IsSetStorageClass()
		{
			return storageClass != null;
		}

		internal bool IsSetServerSideEncryptionKeyManagementServiceKeyId()
		{
			return !string.IsNullOrEmpty(serverSideEncryptionKeyManagementServiceKeyId);
		}

		internal bool IsSetReplicationStatus()
		{
			return ReplicationStatus != null;
		}
	}
}
