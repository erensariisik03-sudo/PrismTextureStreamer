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
	public class PutLifecycleConfigurationRequestMarshaller : IMarshaller<IRequest, AmazonWebServiceRequest>, IMarshaller<IRequest, PutLifecycleConfigurationRequest>
	{
		public IRequest Marshall(AmazonWebServiceRequest input)
		{
			return Marshall((PutLifecycleConfigurationRequest)input);
		}

		public IRequest Marshall(PutLifecycleConfigurationRequest putLifecycleConfigurationRequest)
		{
			IRequest request = new DefaultRequest(putLifecycleConfigurationRequest, "AmazonS3");
			request.HttpMethod = "PUT";
			request.ResourcePath = "/" + S3Transforms.ToStringValue(putLifecycleConfigurationRequest.BucketName);
			request.AddSubResource("lifecycle");
			StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
			using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings
			{
				Encoding = Encoding.UTF8,
				OmitXmlDeclaration = true
			}))
			{
				LifecycleConfiguration configuration = putLifecycleConfigurationRequest.Configuration;
				if (configuration != null)
				{
					xmlWriter.WriteStartElement("LifecycleConfiguration", string.Empty);
					if (configuration != null)
					{
						List<LifecycleRule> rules = configuration.Rules;
						if (rules != null && rules.Count > 0)
						{
							foreach (LifecycleRule item in rules)
							{
								xmlWriter.WriteStartElement("Rule", string.Empty);
								if (item != null)
								{
									LifecycleRuleExpiration expiration = item.Expiration;
									if (expiration != null)
									{
										xmlWriter.WriteStartElement("Expiration", string.Empty);
										if (expiration.IsSetDate())
										{
											xmlWriter.WriteElementString("Date", string.Empty, S3Transforms.ToXmlStringValue(expiration.Date));
										}
										if (expiration.IsSetDays())
										{
											xmlWriter.WriteElementString("Days", string.Empty, S3Transforms.ToXmlStringValue(expiration.Days));
										}
										xmlWriter.WriteEndElement();
									}
									List<LifecycleTransition> transitions = item.Transitions;
									if (transitions != null && transitions.Count > 0)
									{
										foreach (LifecycleTransition item2 in transitions)
										{
											if (item2 != null)
											{
												xmlWriter.WriteStartElement("Transition", string.Empty);
												if (item2.IsSetDate())
												{
													xmlWriter.WriteElementString("Date", string.Empty, S3Transforms.ToXmlStringValue(item2.Date));
												}
												if (item2.IsSetDays())
												{
													xmlWriter.WriteElementString("Days", string.Empty, S3Transforms.ToXmlStringValue(item2.Days));
												}
												if (item2.IsSetStorageClass())
												{
													xmlWriter.WriteElementString("StorageClass", string.Empty, S3Transforms.ToXmlStringValue(item2.StorageClass));
												}
												xmlWriter.WriteEndElement();
											}
										}
									}
									LifecycleRuleNoncurrentVersionExpiration noncurrentVersionExpiration = item.NoncurrentVersionExpiration;
									if (noncurrentVersionExpiration != null)
									{
										xmlWriter.WriteStartElement("NoncurrentVersionExpiration", string.Empty);
										if (noncurrentVersionExpiration.IsSetNoncurrentDays())
										{
											xmlWriter.WriteElementString("NoncurrentDays", string.Empty, S3Transforms.ToXmlStringValue(noncurrentVersionExpiration.NoncurrentDays));
										}
										xmlWriter.WriteEndElement();
									}
									List<LifecycleRuleNoncurrentVersionTransition> noncurrentVersionTransitions = item.NoncurrentVersionTransitions;
									if (noncurrentVersionTransitions != null && noncurrentVersionTransitions.Count > 0)
									{
										foreach (LifecycleRuleNoncurrentVersionTransition item3 in noncurrentVersionTransitions)
										{
											if (item3 != null)
											{
												xmlWriter.WriteStartElement("NoncurrentVersionTransition", string.Empty);
												if (item3.IsSetNoncurrentDays())
												{
													xmlWriter.WriteElementString("NoncurrentDays", string.Empty, S3Transforms.ToXmlStringValue(item3.NoncurrentDays));
												}
												if (item3.IsSetStorageClass())
												{
													xmlWriter.WriteElementString("StorageClass", string.Empty, S3Transforms.ToXmlStringValue(item3.StorageClass));
												}
												xmlWriter.WriteEndElement();
											}
										}
									}
								}
								if (item.IsSetId())
								{
									xmlWriter.WriteElementString("ID", string.Empty, S3Transforms.ToXmlStringValue(item.Id));
								}
								if (item.IsSetPrefix())
								{
									xmlWriter.WriteElementString("Prefix", string.Empty, S3Transforms.ToXmlStringValue(item.Prefix));
								}
								if (item.IsSetStatus())
								{
									xmlWriter.WriteElementString("Status", string.Empty, S3Transforms.ToXmlStringValue(item.Status));
								}
								else
								{
									xmlWriter.WriteElementString("Status", string.Empty, "Disabled");
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
