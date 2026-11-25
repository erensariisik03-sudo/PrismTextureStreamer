using Amazon.Runtime;
using Amazon.Runtime.Internal;
using Amazon.Runtime.Internal.Transform;

namespace Amazon.S3.Model.Internal.MarshallTransformations
{
	public class GetBucketRequestPaymentRequestMarshaller : IMarshaller<IRequest, AmazonWebServiceRequest>, IMarshaller<IRequest, GetBucketRequestPaymentRequest>
	{
		public IRequest Marshall(AmazonWebServiceRequest input)
		{
			return Marshall((GetBucketRequestPaymentRequest)input);
		}

		public IRequest Marshall(GetBucketRequestPaymentRequest getBucketRequestPaymentRequest)
		{
			IRequest request = new DefaultRequest(getBucketRequestPaymentRequest, "AmazonS3");
			request.HttpMethod = "GET";
			request.ResourcePath = "/" + S3Transforms.ToStringValue(getBucketRequestPaymentRequest.BucketName);
			request.AddSubResource("requestPayment");
			request.UseQueryString = true;
			return request;
		}
	}
}
