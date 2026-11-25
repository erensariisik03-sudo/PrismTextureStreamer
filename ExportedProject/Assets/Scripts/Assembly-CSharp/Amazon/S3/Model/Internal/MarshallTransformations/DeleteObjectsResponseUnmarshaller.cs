using Amazon.Runtime;
using Amazon.Runtime.Internal.Transform;

namespace Amazon.S3.Model.Internal.MarshallTransformations
{
	internal class DeleteObjectsResponseUnmarshaller : S3ReponseUnmarshaller
	{
		private static DeleteObjectsResponseUnmarshaller _instance;

		public static DeleteObjectsResponseUnmarshaller Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new DeleteObjectsResponseUnmarshaller();
				}
				return _instance;
			}
		}

		public override AmazonWebServiceResponse Unmarshall(XmlUnmarshallerContext context)
		{
			DeleteObjectsResponse deleteObjectsResponse = new DeleteObjectsResponse();
			while (context.Read())
			{
				if (context.IsStartElement)
				{
					UnmarshallResult(context, deleteObjectsResponse);
				}
			}
			return deleteObjectsResponse;
		}

		private static void UnmarshallResult(XmlUnmarshallerContext context, DeleteObjectsResponse response)
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
					if (context.TestExpression("Deleted", num))
					{
						response.DeletedObjects.Add(DeletedObjectUnmarshaller.Instance.Unmarshall(context));
					}
					else if (context.TestExpression("Error", num))
					{
						response.DeleteErrors.Add(ErrorsItemUnmarshaller.Instance.Unmarshall(context));
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
