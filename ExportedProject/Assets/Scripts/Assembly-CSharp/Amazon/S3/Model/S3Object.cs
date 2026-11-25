using System;

namespace Amazon.S3.Model
{
	public class S3Object
	{
		private string eTag;

		private string key;

		private DateTime? lastModified;

		private Owner owner;

		private long? size;

		private S3StorageClass storageClass;

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

		public Owner Owner
		{
			get
			{
				return owner;
			}
			set
			{
				owner = value;
			}
		}

		public long Size
		{
			get
			{
				long? num = size;
				return (!num.HasValue) ? 0 : num.Value;
			}
			set
			{
				size = value;
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

		internal bool IsSetETag()
		{
			return eTag != null;
		}

		internal bool IsSetKey()
		{
			return key != null;
		}

		internal bool IsSetLastModified()
		{
			return lastModified.HasValue;
		}

		internal bool IsSetOwner()
		{
			return owner != null;
		}

		internal bool IsSetSize()
		{
			return size.HasValue;
		}

		internal bool IsSetStorageClass()
		{
			return storageClass != null;
		}
	}
}
