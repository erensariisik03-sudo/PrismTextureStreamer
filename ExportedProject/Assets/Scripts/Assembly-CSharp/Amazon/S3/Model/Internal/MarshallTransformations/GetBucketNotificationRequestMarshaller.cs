using Amazon.Runtime;
using Amazon.Runtime.Internal;
using Amazon.Runtime.Internal.Transform;

namespace Amazon.S3.Model.Internal.MarshallTransformations
{
	public class GetBucketNotificationRequestMarshaller : IMarshaller<IRequest, AmazonWebServiceRequest>, IMarshaller<IRequest, GetBucketNotificationRequest>
	{
		public IRequest Marshall(AmazonWebServiceRequest input)
		{
			return Marshall((GetBucketNotificationRequest)input);
		}

		public IRequest Marshall(GetBucketNotificationRequest getBucketNotificationRequest)
		{
			IRequest request = new DefaultRequest(getBucketNotificationRequest, "AmazonS3");
			request.HttpMethod = "GET";
			request.ResourcePath = "/" + S3Transforms.ToStringValue(getBucketNotificationRequest.BucketName);
			request.AddSubResource("notification");
			request.UseQueryString = true;
			return request;
		}
	}
}
