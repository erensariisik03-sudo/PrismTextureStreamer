using Amazon.Runtime;
using Amazon.Runtime.Internal;
using Amazon.Runtime.Internal.Transform;

namespace Amazon.S3.Model.Internal.MarshallTransformations
{
	public class DeleteBucketTaggingRequestMarshaller : IMarshaller<IRequest, AmazonWebServiceRequest>, IMarshaller<IRequest, DeleteBucketTaggingRequest>
	{
		public IRequest Marshall(AmazonWebServiceRequest input)
		{
			return Marshall((DeleteBucketTaggingRequest)input);
		}

		public IRequest Marshall(DeleteBucketTaggingRequest deleteBucketTaggingRequest)
		{
			IRequest request = new DefaultRequest(deleteBucketTaggingRequest, "AmazonS3");
			request.HttpMethod = "DELETE";
			request.ResourcePath = "/" + S3Transforms.ToStringValue(deleteBucketTaggingRequest.BucketName);
			request.AddSubResource("tagging");
			request.UseQueryString = true;
			return request;
		}
	}
}
