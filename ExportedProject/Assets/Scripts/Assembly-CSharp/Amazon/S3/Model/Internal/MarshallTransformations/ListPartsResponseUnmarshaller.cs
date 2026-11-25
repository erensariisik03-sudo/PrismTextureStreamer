using Amazon.Runtime;
using Amazon.Runtime.Internal.Transform;

namespace Amazon.S3.Model.Internal.MarshallTransformations
{
	internal class ListPartsResponseUnmarshaller : S3ReponseUnmarshaller
	{
		private static ListPartsResponseUnmarshaller _instance;

		public static ListPartsResponseUnmarshaller Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new ListPartsResponseUnmarshaller();
				}
				return _instance;
			}
		}

		public override AmazonWebServiceResponse Unmarshall(XmlUnmarshallerContext context)
		{
			ListPartsResponse listPartsResponse = new ListPartsResponse();
			while (context.Read())
			{
				if (context.IsStartElement)
				{
					UnmarshallResult(context, listPartsResponse);
				}
			}
			return listPartsResponse;
		}

		private static void UnmarshallResult(XmlUnmarshallerContext context, ListPartsResponse response)
		{
			int currentDepth = context.CurrentDepth;
			int num = currentDepth + 1;
			if (context.IsStartOfDocument)
			{
				num += 2;
			}
			while (context.Read())
			{
				if (context.IsStartElement || context.IsAttribute)
				{
					if (context.TestExpression("Bucket", num))
					{
						response.BucketName = StringUnmarshaller.GetInstance().Unmarshall(context);
					}
					else if (context.TestExpression("Key", num))
					{
						response.Key = StringUnmarshaller.GetInstance().Unmarshall(context);
					}
					else if (context.TestExpression("UploadId", num))
					{
						response.UploadId = StringUnmarshaller.GetInstance().Unmarshall(context);
					}
					else if (context.TestExpression("PartNumberMarker", num))
					{
						response.PartNumberMarker = IntUnmarshaller.GetInstance().Unmarshall(context);
					}
					else if (context.TestExpression("NextPartNumberMarker", num))
					{
						response.NextPartNumberMarker = IntUnmarshaller.GetInstance().Unmarshall(context);
					}
					else if (context.TestExpression("MaxParts", num))
					{
						response.MaxParts = IntUnmarshaller.GetInstance().Unmarshall(context);
					}
					else if (context.TestExpression("IsTruncated", num))
					{
						response.IsTruncated = BoolUnmarshaller.GetInstance().Unmarshall(context);
					}
					else if (context.TestExpression("Part", num))
					{
						response.Parts.Add(PartDetailUnmarshaller.Instance.Unmarshall(context));
					}
					else if (context.TestExpression("Initiator", num))
					{
						response.Initiator = InitiatorUnmarshaller.Instance.Unmarshall(context);
					}
					else if (context.TestExpression("Owner", num))
					{
						response.Owner = OwnerUnmarshaller.Instance.Unmarshall(context);
					}
					else if (context.TestExpression("StorageClass", num))
					{
						response.StorageClass = StringUnmarshaller.GetInstance().Unmarshall(context);
					}
				}
				else if (context.IsEndElement && context.CurrentDepth < currentDepth)
				{
					break;
				}
			}
		}
	}
}
