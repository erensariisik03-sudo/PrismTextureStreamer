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
	public class PutBucketReplicationRequestMarshaller : IMarshaller<IRequest, AmazonWebServiceRequest>, IMarshaller<IRequest, PutBucketReplicationRequest>
	{
		public IRequest Marshall(AmazonWebServiceRequest input)
		{
			return Marshall((PutBucketReplicationRequest)input);
		}

		public IRequest Marshall(PutBucketReplicationRequest putBucketreplicationRequest)
		{
			IRequest request = new DefaultRequest(putBucketreplicationRequest, "AmazonS3");
			request.HttpMethod = "PUT";
			request.ResourcePath = "/" + S3Transforms.ToStringValue(putBucketreplicationRequest.BucketName);
			request.AddSubResource("replication");
			StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
			using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings
			{
				Encoding = Encoding.UTF8,
				OmitXmlDeclaration = true
			}))
			{
				ReplicationConfiguration configuration = putBucketreplicationRequest.Configuration;
				if (configuration != null)
				{
					xmlWriter.WriteStartElement("ReplicationConfiguration", string.Empty);
					if (configuration.Role != null)
					{
						xmlWriter.WriteElementString("Role", string.Empty, S3Transforms.ToXmlStringValue(configuration.Role));
					}
					if (configuration.Rules != null)
					{
						foreach (ReplicationRule rule in configuration.Rules)
						{
							xmlWriter.WriteStartElement("Rule");
							if (rule.IsSetId())
							{
								xmlWriter.WriteElementString("ID", string.Empty, S3Transforms.ToXmlStringValue(rule.Id));
							}
							if (rule.IsSetPrefix())
							{
								xmlWriter.WriteElementString("Prefix", string.Empty, S3Transforms.ToXmlStringValue(rule.Prefix));
							}
							else
							{
								xmlWriter.WriteElementString("Prefix", string.Empty, S3Transforms.ToXmlStringValue(string.Empty));
							}
							if (rule.IsSetStatus())
							{
								xmlWriter.WriteElementString("Status", string.Empty, S3Transforms.ToXmlStringValue(rule.Status.ToString()));
							}
							if (rule.IsSetDestination())
							{
								xmlWriter.WriteStartElement("Destination", string.Empty);
								if (rule.Destination.IsSetBucketArn())
								{
									xmlWriter.WriteElementString("Bucket", string.Empty, rule.Destination.BucketArn);
								}
								if (rule.Destination.IsSetStorageClass())
								{
									xmlWriter.WriteElementString("StorageClass", string.Empty, rule.Destination.StorageClass);
								}
								xmlWriter.WriteEndElement();
							}
							xmlWriter.WriteEndElement();
						}
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
