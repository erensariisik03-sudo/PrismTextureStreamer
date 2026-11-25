using Amazon.Runtime;
using Amazon.Runtime.Internal;
using Amazon.Runtime.Internal.Transform;

namespace Amazon.S3.Model.Internal.MarshallTransformations
{
	public class GetLifecycleConfigurationRequestMarshaller : IMarshaller<IRequest, AmazonWebServiceRequest>, IMarshaller<IRequest, GetLifecycleConfigurationRequest>
	{
		public IRequest Marshall(AmazonWebServiceRequest input)
		{
			return Marshall((GetLifecycleConfigurationRequest)input);
		}

		public IRequest Marshall(GetLifecycleConfigurationRequest getLifecycleConfiguration)
		{
			IRequest request = new DefaultRequest(getLifecycleConfiguration, "AmazonS3");
			request.Suppress404Exceptions = true;
			request.HttpMethod = "GET";
			request.ResourcePath = "/" + S3Transforms.ToStringValue(getLifecycleConfiguration.BucketName);
			request.AddSubResource("lifecycle");
			request.UseQueryString = true;
			return request;
		}
	}
}
