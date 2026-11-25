using Amazon.Runtime;
using Amazon.Runtime.Internal;
using Amazon.Runtime.Internal.Transform;

namespace Amazon.S3.Model.Internal.MarshallTransformations
{
	public class GetBucketReplicationRequestMarshaller : IMarshaller<IRequest, AmazonWebServiceRequest>, IMarshaller<IRequest, GetBucketReplicationRequest>
	{
		public IRequest Marshall(AmazonWebServiceRequest input)
		{
			return Marshall((GetBucketReplicationRequest)input);
		}

		public IRequest Marshall(GetBucketReplicationRequest getBucketReplicationConfigurationRequest)
		{
			IRequest request = new DefaultRequest(getBucketReplicationConfigurationRequest, "AmazonS3");
			request.Suppress404Exceptions = true;
			request.HttpMethod = "GET";
			request.ResourcePath = "/" + S3Transforms.ToStringValue(getBucketReplicationConfigurationRequest.BucketName);
			request.AddSubResource("replication");
			request.UseQueryString = true;
			return request;
		}
	}
}
