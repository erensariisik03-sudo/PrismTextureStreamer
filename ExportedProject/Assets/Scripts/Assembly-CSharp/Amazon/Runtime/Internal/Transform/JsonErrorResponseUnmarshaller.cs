using System;

namespace Amazon.Runtime.Internal.Transform
{
	public class JsonErrorResponseUnmarshaller : IUnmarshaller<ErrorResponse, JsonUnmarshallerContext>
	{
		private static JsonErrorResponseUnmarshaller instance;

		public ErrorResponse Unmarshall(JsonUnmarshallerContext context)
		{
			ErrorResponse errorResponse = new ErrorResponse();
			if (context.Peek() == 60)
			{
				ErrorResponseUnmarshaller errorResponseUnmarshaller = new ErrorResponseUnmarshaller();
				XmlUnmarshallerContext context2 = new XmlUnmarshallerContext(context.Stream, false, null);
				errorResponse = errorResponseUnmarshaller.Unmarshall(context2);
			}
			else
			{
				while (context.Read())
				{
					if (context.TestExpression("__type"))
					{
						string text = StringUnmarshaller.GetInstance().Unmarshall(context);
						errorResponse.Code = text.Substring(text.LastIndexOf("#", StringComparison.Ordinal) + 1);
						if (Enum.IsDefined(typeof(ErrorType), text))
						{
							errorResponse.Type = (ErrorType)(int)Enum.Parse(typeof(ErrorType), text, true);
						}
						else
						{
							errorResponse.Type = ErrorType.Unknown;
						}
					}
					else if (context.TestExpression("code"))
					{
						errorResponse.Code = StringUnmarshaller.GetInstance().Unmarshall(context);
					}
					else if (context.TestExpression("message"))
					{
						errorResponse.Message = StringUnmarshaller.GetInstance().Unmarshall(context);
					}
				}
			}
			return errorResponse;
		}

		public static JsonErrorResponseUnmarshaller GetInstance()
		{
			if (instance == null)
			{
				instance = new JsonErrorResponseUnmarshaller();
			}
			return instance;
		}
	}
}
