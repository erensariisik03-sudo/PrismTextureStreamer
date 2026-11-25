using System.Globalization;
using Amazon.Runtime;
using Amazon.Runtime.Internal;
using Amazon.Runtime.Internal.Transform;

namespace Amazon.S3.Model.Internal.MarshallTransformations
{
	public class AbortMultipartUploadRequestMarshaller : IMarshaller<IRequest, AmazonWebServiceRequest>, IMarshaller<IRequest, AbortMultipartUploadRequest>
	{
		public IRequest Marshall(AmazonWebServiceRequest input)
		{
			return Marshall((AbortMultipartUploadRequest)input);
		}

		public IRequest Marshall(AbortMultipartUploadRequest abortMultipartUploadRequest)
		{
			IRequest request = new DefaultRequest(abortMultipartUploadRequest, "AmazonS3");
			request.HttpMethod = "DELETE";
			request.ResourcePath = string.Format(CultureInfo.InvariantCulture, "/{0}/{1}", S3Transforms.ToStringValue(abortMultipartUploadRequest.BucketName), S3Transforms.ToStringValue(abortMultipartUploadRequest.Key));
			request.AddSubResource("uploadId", S3Transforms.ToStringValue(abortMultipartUploadRequest.UploadId));
			request.UseQueryString = true;
			return request;
		}
	}
}
