using Amazon.Runtime;
using Amazon.Runtime.Internal.Transform;

namespace Amazon.S3.Model.Internal.MarshallTransformations
{
	internal class RestoreObjectResponseUnmarshaller : S3ReponseUnmarshaller
	{
		private static RestoreObjectResponseUnmarshaller _instance;

		public static RestoreObjectResponseUnmarshaller Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new RestoreObjectResponseUnmarshaller();
				}
				return _instance;
			}
		}

		public override AmazonWebServiceResponse Unmarshall(XmlUnmarshallerContext context)
		{
			return new RestoreObjectResponse();
		}
	}
}
