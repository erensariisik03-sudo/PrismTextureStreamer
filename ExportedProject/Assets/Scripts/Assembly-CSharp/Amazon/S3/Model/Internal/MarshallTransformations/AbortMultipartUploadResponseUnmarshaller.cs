using Amazon.Runtime;
using Amazon.Runtime.Internal.Transform;

namespace Amazon.S3.Model.Internal.MarshallTransformations
{
	internal class AbortMultipartUploadResponseUnmarshaller : S3ReponseUnmarshaller
	{
		private static AbortMultipartUploadResponseUnmarshaller _instance;

		public static AbortMultipartUploadResponseUnmarshaller Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new AbortMultipartUploadResponseUnmarshaller();
				}
				return _instance;
			}
		}

		public override AmazonWebServiceResponse Unmarshall(XmlUnmarshallerContext context)
		{
			return new AbortMultipartUploadResponse();
		}
	}
}
