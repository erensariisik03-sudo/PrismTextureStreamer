using System.IO;
using System.Text;
using Amazon.Runtime;
using Amazon.Runtime.Internal;
using Amazon.Runtime.Internal.Transform;

namespace Amazon.S3.Model.Internal.MarshallTransformations
{
	public class PutBucketPolicyRequestMarshaller : IMarshaller<IRequest, AmazonWebServiceRequest>, IMarshaller<IRequest, PutBucketPolicyRequest>
	{
		public IRequest Marshall(AmazonWebServiceRequest input)
		{
			return Marshall((PutBucketPolicyRequest)input);
		}

		public IRequest Marshall(PutBucketPolicyRequest putBucketPolicyRequest)
		{
			IRequest request = new DefaultRequest(putBucketPolicyRequest, "AmazonS3");
			request.HttpMethod = "PUT";
			if (putBucketPolicyRequest.IsSetContentMD5())
			{
				request.Headers.Add("Content-MD5", S3Transforms.ToStringValue(putBucketPolicyRequest.ContentMD5));
			}
			if (!request.Headers.ContainsKey("Content-Type"))
			{
				request.Headers.Add("Content-Type", "text/plain");
			}
			request.ResourcePath = "/" + S3Transforms.ToStringValue(putBucketPolicyRequest.BucketName);
			request.AddSubResource("policy");
			request.ContentStream = new MemoryStream(Encoding.UTF8.GetBytes(putBucketPolicyRequest.Policy));
			return request;
		}
	}
}
