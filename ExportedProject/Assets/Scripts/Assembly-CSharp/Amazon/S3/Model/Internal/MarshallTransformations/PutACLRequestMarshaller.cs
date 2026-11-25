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
	public class PutACLRequestMarshaller : IMarshaller<IRequest, AmazonWebServiceRequest>, IMarshaller<IRequest, PutACLRequest>
	{
		public IRequest Marshall(AmazonWebServiceRequest input)
		{
			return Marshall((PutACLRequest)input);
		}

		public IRequest Marshall(PutACLRequest putObjectAclRequest)
		{
			IRequest request = new DefaultRequest(putObjectAclRequest, "AmazonS3");
			request.HttpMethod = "PUT";
			if (putObjectAclRequest.IsSetCannedACL())
			{
				request.Headers.Add("x-amz-acl", S3Transforms.ToStringValue(putObjectAclRequest.CannedACL));
			}
			request.ResourcePath = string.Format(CultureInfo.InvariantCulture, "/{0}/{1}", S3Transforms.ToStringValue(putObjectAclRequest.BucketName), S3Transforms.ToStringValue(putObjectAclRequest.Key));
			request.AddSubResource("acl");
			if (putObjectAclRequest.IsSetVersionId())
			{
				request.AddSubResource("versionId", S3Transforms.ToStringValue(putObjectAclRequest.VersionId));
			}
			StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
			using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings
			{
				Encoding = Encoding.UTF8,
				OmitXmlDeclaration = true
			}))
			{
				S3AccessControlList accessControlList = putObjectAclRequest.AccessControlList;
				if (accessControlList != null)
				{
					xmlWriter.WriteStartElement("AccessControlPolicy", string.Empty);
					List<S3Grant> grants = accessControlList.Grants;
					if (grants != null && grants.Count > 0)
					{
						xmlWriter.WriteStartElement("AccessControlList", string.Empty);
						foreach (S3Grant item in grants)
						{
							xmlWriter.WriteStartElement("Grant", string.Empty);
							if (item != null)
							{
								S3Grantee grantee = item.Grantee;
								if (grantee != null)
								{
									xmlWriter.WriteStartElement("Grantee", string.Empty);
									if (grantee.IsSetType())
									{
										xmlWriter.WriteAttributeString("xsi", "type", "http://www.w3.org/2001/XMLSchema-instance", grantee.Type.ToString());
									}
									if (grantee.IsSetDisplayName())
									{
										xmlWriter.WriteElementString("DisplayName", string.Empty, S3Transforms.ToXmlStringValue(grantee.DisplayName));
									}
									if (grantee.IsSetEmailAddress())
									{
										xmlWriter.WriteElementString("EmailAddress", string.Empty, S3Transforms.ToXmlStringValue(grantee.EmailAddress));
									}
									if (grantee.IsSetCanonicalUser())
									{
										xmlWriter.WriteElementString("ID", string.Empty, S3Transforms.ToXmlStringValue(grantee.CanonicalUser));
									}
									if (grantee.IsSetURI())
									{
										xmlWriter.WriteElementString("URI", string.Empty, S3Transforms.ToXmlStringValue(grantee.URI));
									}
									xmlWriter.WriteEndElement();
								}
								if (item.IsSetPermission())
								{
									xmlWriter.WriteElementString("Permission", string.Empty, S3Transforms.ToXmlStringValue(item.Permission));
								}
							}
							xmlWriter.WriteEndElement();
						}
						xmlWriter.WriteEndElement();
						Owner owner = accessControlList.Owner;
						if (owner != null)
						{
							xmlWriter.WriteStartElement("Owner", string.Empty);
							if (owner.IsSetDisplayName())
							{
								xmlWriter.WriteElementString("DisplayName", string.Empty, S3Transforms.ToXmlStringValue(owner.DisplayName));
							}
							if (owner.IsSetId())
							{
								xmlWriter.WriteElementString("ID", string.Empty, S3Transforms.ToXmlStringValue(owner.Id));
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
