using Amazon.Runtime;
using Amazon.Runtime.Internal;
using Amazon.Runtime.Internal.Transform;

namespace Amazon.S3.Model.Internal.MarshallTransformations
{
	public class GetBucketPolicyRequestMarshaller : IMarshaller<IRequest, AmazonWebServiceRequest>, IMarshaller<IRequest, GetBucketPolicyRequest>
	{
		public IRequest Marshall(AmazonWebServiceRequest input)
		{
			return Marshall((GetBucketPolicyRequest)input);
		}

		public IRequest Marshall(GetBucketPolicyRequest getBucketPolicyRequest)
		{
			IRequest request = new DefaultRequest(getBucketPolicyRequest, "AmazonS3");
			request.Suppress404Exceptions = true;
			request.HttpMethod = "GET";
			request.ResourcePath = "/" + S3Transforms.ToStringValue(getBucketPolicyRequest.BucketName);
			request.AddSubResource("policy");
			request.UseQueryString = true;
			return request;
		}
	}
}
