using Amazon.Runtime;
using Amazon.Runtime.Internal;
using Amazon.Runtime.Internal.Transform;

namespace Amazon.S3.Model.Internal.MarshallTransformations
{
	public class DeleteCORSConfigurationRequestMarshaller : IMarshaller<IRequest, AmazonWebServiceRequest>, IMarshaller<IRequest, DeleteCORSConfigurationRequest>
	{
		public IRequest Marshall(AmazonWebServiceRequest input)
		{
			return Marshall((DeleteCORSConfigurationRequest)input);
		}

		public IRequest Marshall(DeleteCORSConfigurationRequest deleteCORSConfigurationRequest)
		{
			IRequest request = new DefaultRequest(deleteCORSConfigurationRequest, "AmazonS3");
			request.HttpMethod = "DELETE";
			request.ResourcePath = "/" + S3Transforms.ToStringValue(deleteCORSConfigurationRequest.BucketName);
			request.AddSubResource("cors");
			request.UseQueryString = true;
			return request;
		}
	}
}
