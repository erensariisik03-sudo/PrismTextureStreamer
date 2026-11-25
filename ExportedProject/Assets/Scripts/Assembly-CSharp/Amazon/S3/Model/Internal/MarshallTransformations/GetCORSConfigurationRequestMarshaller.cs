using Amazon.Runtime;
using Amazon.Runtime.Internal;
using Amazon.Runtime.Internal.Transform;

namespace Amazon.S3.Model.Internal.MarshallTransformations
{
	public class GetCORSConfigurationRequestMarshaller : IMarshaller<IRequest, AmazonWebServiceRequest>, IMarshaller<IRequest, GetCORSConfigurationRequest>
	{
		public IRequest Marshall(AmazonWebServiceRequest input)
		{
			return Marshall((GetCORSConfigurationRequest)input);
		}

		public IRequest Marshall(GetCORSConfigurationRequest getCORSConfigurationRequest)
		{
			IRequest request = new DefaultRequest(getCORSConfigurationRequest, "AmazonS3");
			request.Suppress404Exceptions = true;
			request.HttpMethod = "GET";
			request.ResourcePath = "/" + S3Transforms.ToStringValue(getCORSConfigurationRequest.BucketName);
			request.AddSubResource("cors");
			request.UseQueryString = true;
			return request;
		}
	}
}
