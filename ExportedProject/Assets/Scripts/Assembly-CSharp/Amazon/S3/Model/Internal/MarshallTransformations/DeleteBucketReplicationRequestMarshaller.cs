using Amazon.Runtime;
using Amazon.Runtime.Internal;
using Amazon.Runtime.Internal.Transform;

namespace Amazon.S3.Model.Internal.MarshallTransformations
{
	public class DeleteBucketReplicationRequestMarshaller : IMarshaller<IRequest, AmazonWebServiceRequest>, IMarshaller<IRequest, DeleteBucketReplicationRequest>
	{
		public IRequest Marshall(AmazonWebServiceRequest input)
		{
			return Marshall((DeleteBucketReplicationRequest)input);
		}

		public IRequest Marshall(DeleteBucketReplicationRequest deleteBucketReplicationRequest)
		{
			IRequest request = new DefaultRequest(deleteBucketReplicationRequest, "AmazonS3");
			request.HttpMethod = "DELETE";
			request.ResourcePath = "/" + S3Transforms.ToStringValue(deleteBucketReplicationRequest.BucketName);
			request.AddSubResource("replication");
			request.UseQueryString = true;
			return request;
		}
	}
}
