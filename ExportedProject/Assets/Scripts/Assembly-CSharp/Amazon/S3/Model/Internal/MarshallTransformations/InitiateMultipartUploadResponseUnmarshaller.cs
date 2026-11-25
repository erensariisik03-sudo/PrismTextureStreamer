using Amazon.Runtime;
using Amazon.Runtime.Internal.Transform;

namespace Amazon.S3.Model.Internal.MarshallTransformations
{
	internal class InitiateMultipartUploadResponseUnmarshaller : S3ReponseUnmarshaller
	{
		private static InitiateMultipartUploadResponseUnmarshaller _instance;

		public static InitiateMultipartUploadResponseUnmarshaller Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new InitiateMultipartUploadResponseUnmarshaller();
				}
				return _instance;
			}
		}

		public override AmazonWebServiceResponse Unmarshall(XmlUnmarshallerContext context)
		{
			InitiateMultipartUploadResponse initiateMultipartUploadResponse = new InitiateMultipartUploadResponse();
			while (context.Read())
			{
				if (context.IsStartElement)
				{
					UnmarshallResult(context, initiateMultipartUploadResponse);
				}
			}
			return initiateMultipartUploadResponse;
		}

		private static void UnmarshallResult(XmlUnmarshallerContext context, InitiateMultipartUploadResponse response)
		{
			int currentDepth = context.CurrentDepth;
			int num = currentDepth + 1;
			if (context.IsStartOfDocument)
			{
				num += 2;
			}
			while (context.Read())
			{
				if (context.IsStartElement || context.IsAttribute)
				{
					if (context.TestExpression("Bucket", num))
					{
						response.BucketName = StringUnmarshaller.GetInstance().Unmarshall(context);
					}
					else if (context.TestExpression("Key", num))
					{
						response.Key = StringUnmarshaller.GetInstance().Unmarshall(context);
					}
					else if (context.TestExpression("UploadId", num))
					{
						response.UploadId = StringUnmarshaller.GetInstance().Unmarshall(context);
					}
				}
				else if (context.IsEndElement && context.CurrentDepth < currentDepth)
				{
					return;
				}
			}
			IWebResponseData responseData = context.ResponseData;
			if (responseData.IsHeaderPresent("x-amz-server-side-encryption"))
			{
				response.ServerSideEncryptionMethod = S3Transforms.ToString(responseData.GetHeaderValue("x-amz-server-side-encryption"));
			}
			if (responseData.IsHeaderPresent("x-amz-server-side-encryption-aws-kms-key-id"))
			{
				response.ServerSideEncryptionKeyManagementServiceKeyId = S3Transforms.ToString(responseData.GetHeaderValue("x-amz-server-side-encryption-aws-kms-key-id"));
			}
		}
	}
}
