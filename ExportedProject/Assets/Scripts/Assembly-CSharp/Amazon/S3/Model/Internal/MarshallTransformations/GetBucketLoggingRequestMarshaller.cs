using Amazon.Runtime;
using Amazon.Runtime.Internal;
using Amazon.Runtime.Internal.Transform;

namespace Amazon.S3.Model.Internal.MarshallTransformations
{
	public class GetBucketLoggingRequestMarshaller : IMarshaller<IRequest, AmazonWebServiceRequest>, IMarshaller<IRequest, GetBucketLoggingRequest>
	{
		public IRequest Marshall(AmazonWebServiceRequest input)
		{
			return Marshall((GetBucketLoggingRequest)input);
		}

		public IRequest Marshall(GetBucketLoggingRequest getBucketLoggingRequest)
		{
			IRequest request = new DefaultRequest(getBucketLoggingRequest, "AmazonS3");
			request.Suppress404Exceptions = true;
			request.HttpMethod = "GET";
			request.ResourcePath = "/" + S3Transforms.ToStringValue(getBucketLoggingRequest.BucketName);
			request.AddSubResource("logging");
			request.UseQueryString = true;
			return request;
		}
	}
}
