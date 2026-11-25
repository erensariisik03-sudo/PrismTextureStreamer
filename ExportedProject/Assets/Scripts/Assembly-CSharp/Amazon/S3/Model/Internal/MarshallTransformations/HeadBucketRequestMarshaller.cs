using Amazon.Runtime;
using Amazon.Runtime.Internal;
using Amazon.Runtime.Internal.Transform;

namespace Amazon.S3.Model.Internal.MarshallTransformations
{
	internal class HeadBucketRequestMarshaller : IMarshaller<IRequest, AmazonWebServiceRequest>, IMarshaller<IRequest, HeadBucketRequest>
	{
		public IRequest Marshall(AmazonWebServiceRequest input)
		{
			return Marshall((HeadBucketRequest)input);
		}

		public IRequest Marshall(HeadBucketRequest headBucketRequest)
		{
			IRequest request = new DefaultRequest(headBucketRequest, "AmazonS3");
			request.HttpMethod = "HEAD";
			request.ResourcePath = "/" + S3Transforms.ToStringValue(headBucketRequest.BucketName);
			request.UseQueryString = true;
			return request;
		}
	}
}
