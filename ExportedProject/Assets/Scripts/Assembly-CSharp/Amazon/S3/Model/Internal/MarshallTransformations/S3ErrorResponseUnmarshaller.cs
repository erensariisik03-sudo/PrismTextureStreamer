using System;
using System.Net;
using Amazon.Runtime;
using Amazon.Runtime.Internal.Transform;

namespace Amazon.S3.Model.Internal.MarshallTransformations
{
	public class S3ErrorResponseUnmarshaller : IUnmarshaller<S3ErrorResponse, XmlUnmarshallerContext>
	{
		private static S3ErrorResponseUnmarshaller _instance;

		public static S3ErrorResponseUnmarshaller Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new S3ErrorResponseUnmarshaller();
				}
				return _instance;
			}
		}

		public S3ErrorResponse Unmarshall(XmlUnmarshallerContext context)
		{
			S3ErrorResponse s3ErrorResponse = new S3ErrorResponse();
			HttpStatusCode statusCode = context.ResponseData.StatusCode;
			s3ErrorResponse.Code = statusCode.ToString();
			if (context.ResponseData.IsHeaderPresent("x-amz-request-id"))
			{
				s3ErrorResponse.RequestId = context.ResponseData.GetHeaderValue("x-amz-request-id");
			}
			if (context.ResponseData.IsHeaderPresent("x-amz-id-2"))
			{
				s3ErrorResponse.Id2 = context.ResponseData.GetHeaderValue("x-amz-id-2");
			}
			if (statusCode >= HttpStatusCode.InternalServerError)
			{
				s3ErrorResponse.Type = ErrorType.Receiver;
			}
			else if (statusCode >= HttpStatusCode.BadRequest)
			{
				s3ErrorResponse.Type = ErrorType.Sender;
			}
			else
			{
				s3ErrorResponse.Type = ErrorType.Unknown;
			}
			string text = null;
			if (context.ResponseData.IsHeaderPresent("Content-Length"))
			{
				text = context.ResponseData.GetHeaderValue("Content-Length");
			}
			long result;
			if (string.IsNullOrEmpty(text) || !long.TryParse(text, out result))
			{
				result = -1L;
			}
			if (result < 0)
			{
				try
				{
					result = context.Stream.Length;
				}
				catch
				{
					result = -1L;
				}
			}
			if (context.Stream.CanRead && result != 0L)
			{
				try
				{
					while (context.Read())
					{
						if (context.IsStartElement)
						{
							if (context.TestExpression("Error/Code"))
							{
								s3ErrorResponse.Code = StringUnmarshaller.GetInstance().Unmarshall(context);
							}
							else if (context.TestExpression("Error/Message"))
							{
								s3ErrorResponse.Message = StringUnmarshaller.GetInstance().Unmarshall(context);
							}
							else if (context.TestExpression("Error/Resource"))
							{
								s3ErrorResponse.Resource = StringUnmarshaller.GetInstance().Unmarshall(context);
							}
							else if (context.TestExpression("Error/RequestId"))
							{
								s3ErrorResponse.RequestId = StringUnmarshaller.GetInstance().Unmarshall(context);
							}
							else if (context.TestExpression("Error/HostId"))
							{
								s3ErrorResponse.Id2 = StringUnmarshaller.GetInstance().Unmarshall(context);
							}
						}
					}
				}
				catch (Exception parsingException)
				{
					s3ErrorResponse.ParsingException = parsingException;
				}
			}
			return s3ErrorResponse;
		}
	}
}
