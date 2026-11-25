using Amazon.Runtime;
using Amazon.Runtime.Internal;
using Amazon.Runtime.Internal.Transform;

namespace Amazon.S3.Model.Internal.MarshallTransformations
{
	public class DeleteLifecycleConfigurationRequestMarshaller : IMarshaller<IRequest, AmazonWebServiceRequest>, IMarshaller<IRequest, DeleteLifecycleConfigurationRequest>
	{
		public IRequest Marshall(AmazonWebServiceRequest input)
		{
			return Marshall((DeleteLifecycleConfigurationRequest)input);
		}

		public IRequest Marshall(DeleteLifecycleConfigurationRequest deleteLifecycleConfigurationRequest)
		{
			IRequest request = new DefaultRequest(deleteLifecycleConfigurationRequest, "AmazonS3");
			request.HttpMethod = "DELETE";
			request.ResourcePath = "/" + S3Transforms.ToStringValue(deleteLifecycleConfigurationRequest.BucketName);
			request.AddSubResource("lifecycle");
			request.UseQueryString = true;
			return request;
		}
	}
}
