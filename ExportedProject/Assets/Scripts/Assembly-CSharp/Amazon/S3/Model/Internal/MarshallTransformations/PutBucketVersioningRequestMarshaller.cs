using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using Amazon.Runtime;
using Amazon.Runtime.Internal;
using Amazon.Runtime.Internal.Transform;
using Amazon.S3.Util;

namespace Amazon.S3.Model.Internal.MarshallTransformations
{
	public class PutBucketVersioningRequestMarshaller : IMarshaller<IRequest, AmazonWebServiceRequest>, IMarshaller<IRequest, PutBucketVersioningRequest>
	{
		public IRequest Marshall(AmazonWebServiceRequest input)
		{
			return Marshall((PutBucketVersioningRequest)input);
		}

		public IRequest Marshall(PutBucketVersioningRequest putBucketVersioningRequest)
		{
			IRequest request = new DefaultRequest(putBucketVersioningRequest, "AmazonS3");
			request.HttpMethod = "PUT";
			if (putBucketVersioningRequest.IsSetMfaCodes())
			{
				request.Headers.Add("x-amz-mfa", putBucketVersioningRequest.MfaCodes.FormattedMfaCodes);
			}
			request.ResourcePath = "/" + S3Transforms.ToStringValue(putBucketVersioningRequest.BucketName);
			request.AddSubResource("versioning");
			StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
			using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings
			{
				Encoding = Encoding.UTF8,
				OmitXmlDeclaration = true
			}))
			{
				S3BucketVersioningConfig versioningConfig = putBucketVersioningRequest.VersioningConfig;
				if (versioningConfig != null)
				{
					xmlWriter.WriteStartElement("VersioningConfiguration", string.Empty);
					if (versioningConfig.IsSetEnableMfaDelete())
					{
						xmlWriter.WriteElementString("MfaDelete", string.Empty, (!versioningConfig.EnableMfaDelete) ? "Disabled" : "Enabled");
					}
					if (versioningConfig.IsSetStatus())
					{
						xmlWriter.WriteElementString("Status", string.Empty, S3Transforms.ToXmlStringValue(versioningConfig.Status));
					}
					xmlWriter.WriteEndElement();
				}
			}
			try
			{
				string text = stringWriter.ToString();
				request.Content = Encoding.UTF8.GetBytes(text);
				request.Headers["Content-Type"] = "application/xml";
				string value = AmazonS3Util.GenerateChecksumForContent(text, true);
				request.Headers["Content-MD5"] = value;
				return request;
			}
			catch (EncoderFallbackException innerException)
			{
				throw new AmazonServiceException("Unable to marshall request to XML", innerException);
			}
		}
	}
}
