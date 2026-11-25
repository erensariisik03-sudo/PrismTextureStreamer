using Amazon.Runtime;
using Amazon.Runtime.Internal;
using Amazon.Runtime.Internal.Transform;

namespace Amazon.S3.Model.Internal.MarshallTransformations
{
	public class DeleteBucketPolicyRequestMarshaller : IMarshaller<IRequest, AmazonWebServiceRequest>, IMarshaller<IRequest, DeleteBucketPolicyRequest>
	{
		public IRequest Marshall(AmazonWebServiceRequest input)
		{
			return Marshall((DeleteBucketPolicyRequest)input);
		}

		public IRequest Marshall(DeleteBucketPolicyRequest deleteBucketPolicyRequest)
		{
			IRequest request = new DefaultRequest(deleteBucketPolicyRequest, "AmazonS3");
			request.HttpMethod = "DELETE";
			request.ResourcePath = "/" + S3Transforms.ToStringValue(deleteBucketPolicyRequest.BucketName);
			request.AddSubResource("policy");
			request.UseQueryString = true;
			return request;
		}
	}
}
