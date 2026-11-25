using System.Globalization;
using Amazon.Runtime;
using Amazon.Runtime.Internal;
using Amazon.Runtime.Internal.Transform;

namespace Amazon.S3.Model.Internal.MarshallTransformations
{
	public class GetObjectTorrentRequestMarshaller : IMarshaller<IRequest, AmazonWebServiceRequest>, IMarshaller<IRequest, GetObjectTorrentRequest>
	{
		public IRequest Marshall(AmazonWebServiceRequest input)
		{
			return Marshall((GetObjectTorrentRequest)input);
		}

		public IRequest Marshall(GetObjectTorrentRequest getObjectTorrentRequest)
		{
			IRequest request = new DefaultRequest(getObjectTorrentRequest, "AmazonS3");
			request.HttpMethod = "GET";
			request.ResourcePath = string.Format(CultureInfo.InvariantCulture, "/{0}/{1}", S3Transforms.ToStringValue(getObjectTorrentRequest.BucketName), S3Transforms.ToStringValue(getObjectTorrentRequest.Key));
			request.AddSubResource("torrent");
			request.UseQueryString = true;
			return request;
		}
	}
}
