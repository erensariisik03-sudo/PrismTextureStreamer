using System.Collections.Generic;
using System.Xml.Serialization;
using Amazon.Runtime;

namespace Amazon.S3.Model
{
	public class DeleteObjectsRequest : AmazonWebServiceRequest
	{
		private string bucketName;

		private List<KeyVersion> objects = new List<KeyVersion>();

		private bool? quiet;

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

		public List<KeyVersion> Objects
		{
			get
			{
				if (objects == null)
				{
					objects = new List<KeyVersion>();
				}
				return objects;
			}
			set
			{
				objects = value;
			}
		}

		public bool Quiet
		{
			get
			{
				bool? flag = quiet;
				return flag.HasValue && flag.Value;
			}
			set
			{
				quiet = value;
			}
		}

		[XmlIgnore]
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

		public void AddKey(string key)
		{
			AddKey(new KeyVersion
			{
				Key = key
			});
		}

		public void AddKey(string key, string version)
		{
			AddKey(new KeyVersion
			{
				Key = key,
				VersionId = version
			});
		}

		private void AddKey(KeyVersion keyVersion)
		{
			if (Objects == null)
			{
				Objects = new List<KeyVersion>();
			}
			Objects.Add(keyVersion);
		}

		internal bool IsSetBucketName()
		{
			return bucketName != null;
		}

		internal bool IsSetObjects()
		{
			return objects.Count > 0;
		}

		internal bool IsSetQuiet()
		{
			return quiet.HasValue;
		}

		internal bool IsSetMfaCodes()
		{
			return mfaCodes != null && !string.IsNullOrEmpty(MfaCodes.SerialNumber) && !string.IsNullOrEmpty(MfaCodes.AuthenticationValue);
		}
	}
}
