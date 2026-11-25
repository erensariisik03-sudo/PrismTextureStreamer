using Amazon.Runtime;
using Amazon.Runtime.Internal;
using Amazon.Runtime.Internal.Transform;

namespace Amazon.S3.Model.Internal.MarshallTransformations
{
	public class DeleteBucketWebsiteRequestMarshaller : IMarshaller<IRequest, AmazonWebServiceRequest>, IMarshaller<IRequest, DeleteBucketWebsiteRequest>
	{
		public IRequest Marshall(AmazonWebServiceRequest input)
		{
			return Marshall((DeleteBucketWebsiteRequest)input);
		}

		public IRequest Marshall(DeleteBucketWebsiteRequest deleteBucketWebsiteRequest)
		{
			IRequest request = new DefaultRequest(deleteBucketWebsiteRequest, "AmazonS3");
			request.HttpMethod = "DELETE";
			request.ResourcePath = "/" + S3Transforms.ToStringValue(deleteBucketWebsiteRequest.BucketName);
			request.AddSubResource("website");
			request.UseQueryString = true;
			return request;
		}
	}
}
