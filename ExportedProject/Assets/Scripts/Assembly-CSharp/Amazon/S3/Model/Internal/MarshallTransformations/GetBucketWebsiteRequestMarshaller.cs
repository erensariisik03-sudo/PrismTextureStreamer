using Amazon.Runtime;
using Amazon.Runtime.Internal;
using Amazon.Runtime.Internal.Transform;

namespace Amazon.S3.Model.Internal.MarshallTransformations
{
	public class GetBucketWebsiteRequestMarshaller : IMarshaller<IRequest, AmazonWebServiceRequest>, IMarshaller<IRequest, GetBucketWebsiteRequest>
	{
		public IRequest Marshall(AmazonWebServiceRequest input)
		{
			return Marshall((GetBucketWebsiteRequest)input);
		}

		public IRequest Marshall(GetBucketWebsiteRequest getBucketWebsiteRequest)
		{
			IRequest request = new DefaultRequest(getBucketWebsiteRequest, "AmazonS3");
			request.Suppress404Exceptions = true;
			request.HttpMethod = "GET";
			request.ResourcePath = "/" + S3Transforms.ToStringValue(getBucketWebsiteRequest.BucketName);
			request.AddSubResource("website");
			request.UseQueryString = true;
			return request;
		}
	}
}
