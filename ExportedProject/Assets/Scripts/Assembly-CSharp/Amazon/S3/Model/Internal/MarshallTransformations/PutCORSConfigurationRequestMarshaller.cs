using System.Collections.Generic;
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
	public class PutCORSConfigurationRequestMarshaller : IMarshaller<IRequest, AmazonWebServiceRequest>, IMarshaller<IRequest, PutCORSConfigurationRequest>
	{
		public IRequest Marshall(AmazonWebServiceRequest input)
		{
			return Marshall((PutCORSConfigurationRequest)input);
		}

		public IRequest Marshall(PutCORSConfigurationRequest putCORSConfigurationRequest)
		{
			IRequest request = new DefaultRequest(putCORSConfigurationRequest, "AmazonS3");
			request.HttpMethod = "PUT";
			request.ResourcePath = "/" + S3Transforms.ToStringValue(putCORSConfigurationRequest.BucketName);
			request.AddSubResource("cors");
			StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
			using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings
			{
				Encoding = Encoding.UTF8,
				OmitXmlDeclaration = true
			}))
			{
				CORSConfiguration configuration = putCORSConfigurationRequest.Configuration;
				if (configuration != null)
				{
					xmlWriter.WriteStartElement("CORSConfiguration", string.Empty);
					if (configuration != null)
					{
						List<CORSRule> rules = configuration.Rules;
						if (rules != null && rules.Count > 0)
						{
							foreach (CORSRule item in rules)
							{
								xmlWriter.WriteStartElement("CORSRule", string.Empty);
								if (item != null)
								{
									List<string> allowedMethods = item.AllowedMethods;
									if (allowedMethods != null && allowedMethods.Count > 0)
									{
										foreach (string item2 in allowedMethods)
										{
											xmlWriter.WriteStartElement("AllowedMethod", string.Empty);
											xmlWriter.WriteValue(item2);
											xmlWriter.WriteEndElement();
										}
									}
								}
								if (item != null)
								{
									List<string> allowedOrigins = item.AllowedOrigins;
									if (allowedOrigins != null && allowedOrigins.Count > 0)
									{
										foreach (string item3 in allowedOrigins)
										{
											xmlWriter.WriteStartElement("AllowedOrigin", string.Empty);
											xmlWriter.WriteValue(item3);
											xmlWriter.WriteEndElement();
										}
									}
								}
								if (item != null)
								{
									List<string> exposeHeaders = item.ExposeHeaders;
									if (exposeHeaders != null && exposeHeaders.Count > 0)
									{
										foreach (string item4 in exposeHeaders)
										{
											xmlWriter.WriteStartElement("ExposeHeader", string.Empty);
											xmlWriter.WriteValue(item4);
											xmlWriter.WriteEndElement();
										}
									}
								}
								if (item != null)
								{
									List<string> allowedHeaders = item.AllowedHeaders;
									if (allowedHeaders != null && allowedHeaders.Count > 0)
									{
										foreach (string item5 in allowedHeaders)
										{
											xmlWriter.WriteStartElement("AllowedHeader", string.Empty);
											xmlWriter.WriteValue(item5);
											xmlWriter.WriteEndElement();
										}
									}
								}
								if (item.IsSetMaxAgeSeconds())
								{
									xmlWriter.WriteElementString("MaxAgeSeconds", string.Empty, S3Transforms.ToXmlStringValue(item.MaxAgeSeconds));
								}
								if (item.IsSetId())
								{
									xmlWriter.WriteElementString("ID", string.Empty, S3Transforms.ToXmlStringValue(item.Id));
								}
								xmlWriter.WriteEndElement();
							}
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
