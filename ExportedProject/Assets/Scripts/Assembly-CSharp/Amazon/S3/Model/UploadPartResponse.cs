using Amazon.Runtime;

namespace Amazon.S3.Model
{
	public class UploadPartResponse : AmazonWebServiceResponse
	{
		private ServerSideEncryptionMethod serverSideEncryption;

		private string eTag;

		private int partNumber;

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

		public int PartNumber
		{
			get
			{
				return partNumber;
			}
			set
			{
				partNumber = value;
			}
		}

		internal bool IsSetETag()
		{
			return eTag != null;
		}
	}
}
