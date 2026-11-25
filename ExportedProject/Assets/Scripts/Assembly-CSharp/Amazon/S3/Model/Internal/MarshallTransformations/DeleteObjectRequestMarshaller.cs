using System.Globalization;
using Amazon.Runtime;
using Amazon.Runtime.Internal;
using Amazon.Runtime.Internal.Transform;

namespace Amazon.S3.Model.Internal.MarshallTransformations
{
	public class DeleteObjectRequestMarshaller : IMarshaller<IRequest, AmazonWebServiceRequest>, IMarshaller<IRequest, DeleteObjectRequest>
	{
		public IRequest Marshall(AmazonWebServiceRequest input)
		{
			return Marshall((DeleteObjectRequest)input);
		}

		public IRequest Marshall(DeleteObjectRequest deleteObjectRequest)
		{
			IRequest request = new DefaultRequest(deleteObjectRequest, "AmazonS3");
			request.HttpMethod = "DELETE";
			if (deleteObjectRequest.IsSetMfaCodes())
			{
				request.Headers.Add("x-amz-mfa", deleteObjectRequest.MfaCodes.FormattedMfaCodes);
			}
			request.ResourcePath = string.Format(CultureInfo.InvariantCulture, "/{0}/{1}", S3Transforms.ToStringValue(deleteObjectRequest.BucketName), S3Transforms.ToStringValue(deleteObjectRequest.Key));
			if (deleteObjectRequest.IsSetVersionId())
			{
				request.AddSubResource("versionId", S3Transforms.ToStringValue(deleteObjectRequest.VersionId));
			}
			request.UseQueryString = true;
			return request;
		}
	}
}
