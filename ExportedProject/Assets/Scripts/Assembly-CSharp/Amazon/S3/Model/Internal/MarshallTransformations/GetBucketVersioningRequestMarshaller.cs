using Amazon.Runtime;
using Amazon.Runtime.Internal;
using Amazon.Runtime.Internal.Transform;

namespace Amazon.S3.Model.Internal.MarshallTransformations
{
	public class GetBucketVersioningRequestMarshaller : IMarshaller<IRequest, AmazonWebServiceRequest>, IMarshaller<IRequest, GetBucketVersioningRequest>
	{
		public IRequest Marshall(AmazonWebServiceRequest input)
		{
			return Marshall((GetBucketVersioningRequest)input);
		}

		public IRequest Marshall(GetBucketVersioningRequest getBucketVersioningRequest)
		{
			IRequest request = new DefaultRequest(getBucketVersioningRequest, "AmazonS3");
			request.HttpMethod = "GET";
			request.ResourcePath = "/" + S3Transforms.ToStringValue(getBucketVersioningRequest.BucketName);
			request.AddSubResource("versioning");
			request.UseQueryString = true;
			return request;
		}
	}
}
