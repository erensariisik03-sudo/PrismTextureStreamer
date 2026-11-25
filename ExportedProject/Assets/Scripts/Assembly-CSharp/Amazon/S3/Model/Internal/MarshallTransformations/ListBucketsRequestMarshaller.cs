using Amazon.Runtime;
using Amazon.Runtime.Internal;
using Amazon.Runtime.Internal.Transform;

namespace Amazon.S3.Model.Internal.MarshallTransformations
{
	public class ListBucketsRequestMarshaller : IMarshaller<IRequest, AmazonWebServiceRequest>, IMarshaller<IRequest, ListBucketsRequest>
	{
		public IRequest Marshall(AmazonWebServiceRequest input)
		{
			return Marshall((ListBucketsRequest)input);
		}

		public IRequest Marshall(ListBucketsRequest listBucketsRequest)
		{
			IRequest request = new DefaultRequest(listBucketsRequest, "AmazonS3");
			request.HttpMethod = "GET";
			request.ResourcePath = "/";
			request.UseQueryString = true;
			return request;
		}
	}
}
