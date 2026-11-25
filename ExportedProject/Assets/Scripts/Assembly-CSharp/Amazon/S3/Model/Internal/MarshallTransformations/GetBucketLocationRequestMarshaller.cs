using Amazon.Runtime;
using Amazon.Runtime.Internal;
using Amazon.Runtime.Internal.Transform;

namespace Amazon.S3.Model.Internal.MarshallTransformations
{
	public class GetBucketLocationRequestMarshaller : IMarshaller<IRequest, AmazonWebServiceRequest>, IMarshaller<IRequest, GetBucketLocationRequest>
	{
		public IRequest Marshall(AmazonWebServiceRequest input)
		{
			return Marshall((GetBucketLocationRequest)input);
		}

		public IRequest Marshall(GetBucketLocationRequest getBucketLocationRequest)
		{
			IRequest request = new DefaultRequest(getBucketLocationRequest, "AmazonS3");
			request.HttpMethod = "GET";
			request.ResourcePath = "/" + S3Transforms.ToStringValue(getBucketLocationRequest.BucketName);
			request.AddSubResource("location");
			request.UseQueryString = true;
			return request;
		}
	}
}
