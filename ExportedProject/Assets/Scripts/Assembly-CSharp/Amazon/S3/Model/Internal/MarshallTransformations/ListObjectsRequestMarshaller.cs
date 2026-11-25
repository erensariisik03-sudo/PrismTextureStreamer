using Amazon.Runtime;
using Amazon.Runtime.Internal;
using Amazon.Runtime.Internal.Transform;

namespace Amazon.S3.Model.Internal.MarshallTransformations
{
	public class ListObjectsRequestMarshaller : IMarshaller<IRequest, AmazonWebServiceRequest>, IMarshaller<IRequest, ListObjectsRequest>
	{
		public IRequest Marshall(AmazonWebServiceRequest input)
		{
			return Marshall((ListObjectsRequest)input);
		}

		public IRequest Marshall(ListObjectsRequest listObjectsRequest)
		{
			IRequest request = new DefaultRequest(listObjectsRequest, "AmazonS3");
			request.HttpMethod = "GET";
			request.ResourcePath = "/" + S3Transforms.ToStringValue(listObjectsRequest.BucketName);
			if (listObjectsRequest.IsSetDelimiter())
			{
				request.Parameters.Add("delimiter", S3Transforms.ToStringValue(listObjectsRequest.Delimiter));
			}
			if (listObjectsRequest.IsSetMarker())
			{
				request.Parameters.Add("marker", S3Transforms.ToStringValue(listObjectsRequest.Marker));
			}
			if (listObjectsRequest.IsSetMaxKeys())
			{
				request.Parameters.Add("max-keys", S3Transforms.ToStringValue(listObjectsRequest.MaxKeys));
			}
			if (listObjectsRequest.IsSetPrefix())
			{
				request.Parameters.Add("prefix", S3Transforms.ToStringValue(listObjectsRequest.Prefix));
			}
			if (listObjectsRequest.IsSetEncoding())
			{
				request.Parameters.Add("encoding-type", S3Transforms.ToStringValue(listObjectsRequest.Encoding));
			}
			request.UseQueryString = true;
			return request;
		}
	}
}
