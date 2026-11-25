using Amazon.Runtime;
using Amazon.Runtime.Internal;
using Amazon.Runtime.Internal.Transform;

namespace Amazon.S3.Model.Internal.MarshallTransformations
{
	public class GetBucketTaggingRequestMarshaller : IMarshaller<IRequest, AmazonWebServiceRequest>, IMarshaller<IRequest, GetBucketTaggingRequest>
	{
		public IRequest Marshall(AmazonWebServiceRequest input)
		{
			return Marshall((GetBucketTaggingRequest)input);
		}

		public IRequest Marshall(GetBucketTaggingRequest getBucketTaggingRequest)
		{
			IRequest request = new DefaultRequest(getBucketTaggingRequest, "AmazonS3");
			request.Suppress404Exceptions = true;
			request.HttpMethod = "GET";
			request.ResourcePath = "/" + S3Transforms.ToStringValue(getBucketTaggingRequest.BucketName);
			request.AddSubResource("tagging");
			request.UseQueryString = true;
			return request;
		}
	}
}
