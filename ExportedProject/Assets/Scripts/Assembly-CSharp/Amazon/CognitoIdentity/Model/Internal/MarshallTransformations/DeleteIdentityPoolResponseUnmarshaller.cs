using System;
using System.Net;
using Amazon.Runtime;
using Amazon.Runtime.Internal;
using Amazon.Runtime.Internal.Transform;

namespace Amazon.CognitoIdentity.Model.Internal.MarshallTransformations
{
	public class DeleteIdentityPoolResponseUnmarshaller : JsonResponseUnmarshaller, ISimplifiedErrorUnmarshaller
	{
		private static DeleteIdentityPoolResponseUnmarshaller _instance = new DeleteIdentityPoolResponseUnmarshaller();

		public static DeleteIdentityPoolResponseUnmarshaller Instance
		{
			get
			{
				return _instance;
			}
		}

		public override AmazonWebServiceResponse Unmarshall(JsonUnmarshallerContext context)
		{
			return new DeleteIdentityPoolResponse();
		}

		public override AmazonServiceException UnmarshallException(JsonUnmarshallerContext context, Exception innerException, HttpStatusCode statusCode)
		{
			ErrorResponse errorResponse = JsonErrorResponseUnmarshaller.GetInstance().Unmarshall(context);
			if (errorResponse.Code != null && errorResponse.Code.Equals("InternalErrorException"))
			{
				return new InternalErrorException(errorResponse.Message, innerException, errorResponse.Type, errorResponse.Code, errorResponse.RequestId, statusCode);
			}
			if (errorResponse.Code != null && errorResponse.Code.Equals("InvalidParameterException"))
			{
				return new InvalidParameterException(errorResponse.Message, innerException, errorResponse.Type, errorResponse.Code, errorResponse.RequestId, statusCode);
			}
			if (errorResponse.Code != null && errorResponse.Code.Equals("NotAuthorizedException"))
			{
				return new NotAuthorizedException(errorResponse.Message, innerException, errorResponse.Type, errorResponse.Code, errorResponse.RequestId, statusCode);
			}
			if (errorResponse.Code != null && errorResponse.Code.Equals("ResourceNotFoundException"))
			{
				return new ResourceNotFoundException(errorResponse.Message, innerException, errorResponse.Type, errorResponse.Code, errorResponse.RequestId, statusCode);
			}
			if (errorResponse.Code != null && errorResponse.Code.Equals("TooManyRequestsException"))
			{
				return new TooManyRequestsException(errorResponse.Message, innerException, errorResponse.Type, errorResponse.Code, errorResponse.RequestId, statusCode);
			}
			return new AmazonCognitoIdentityException(errorResponse.Message, innerException, errorResponse.Type, errorResponse.Code, errorResponse.RequestId, statusCode);
		}

		public AmazonServiceException UnmarshallException(IWebResponseData response, ErrorResponse errorResponse, Exception innerException)
		{
			if (!string.IsNullOrEmpty(errorResponse.Code) && errorResponse.Code.StartsWith("InternalErrorException", StringComparison.OrdinalIgnoreCase))
			{
				string message = ((!string.IsNullOrEmpty(errorResponse.Message)) ? errorResponse.Message : ResponseUnmarshaller.GetDefaultErrorMessage<AmazonCognitoIdentityException>());
				return new InternalErrorException(message, innerException, ErrorType.Unknown, errorResponse.Code, errorResponse.RequestId, response.StatusCode);
			}
			if (!string.IsNullOrEmpty(errorResponse.Code) && errorResponse.Code.StartsWith("InvalidParameterException", StringComparison.OrdinalIgnoreCase))
			{
				string message2 = ((!string.IsNullOrEmpty(errorResponse.Message)) ? errorResponse.Message : ResponseUnmarshaller.GetDefaultErrorMessage<AmazonCognitoIdentityException>());
				return new InvalidParameterException(message2, innerException, ErrorType.Unknown, errorResponse.Code, errorResponse.RequestId, response.StatusCode);
			}
			if (!string.IsNullOrEmpty(errorResponse.Code) && errorResponse.Code.StartsWith("NotAuthorizedException", StringComparison.OrdinalIgnoreCase))
			{
				string message3 = ((!string.IsNullOrEmpty(errorResponse.Message)) ? errorResponse.Message : ResponseUnmarshaller.GetDefaultErrorMessage<AmazonCognitoIdentityException>());
				return new NotAuthorizedException(message3, innerException, ErrorType.Unknown, errorResponse.Code, errorResponse.RequestId, response.StatusCode);
			}
			if (!string.IsNullOrEmpty(errorResponse.Code) && errorResponse.Code.StartsWith("ResourceNotFoundException", StringComparison.OrdinalIgnoreCase))
			{
				string message4 = ((!string.IsNullOrEmpty(errorResponse.Message)) ? errorResponse.Message : ResponseUnmarshaller.GetDefaultErrorMessage<AmazonCognitoIdentityException>());
				return new ResourceNotFoundException(message4, innerException, ErrorType.Unknown, errorResponse.Code, errorResponse.RequestId, response.StatusCode);
			}
			if (!string.IsNullOrEmpty(errorResponse.Code) && errorResponse.Code.StartsWith("TooManyRequestsException", StringComparison.OrdinalIgnoreCase))
			{
				string message5 = ((!string.IsNullOrEmpty(errorResponse.Message)) ? errorResponse.Message : ResponseUnmarshaller.GetDefaultErrorMessage<AmazonCognitoIdentityException>());
				return new TooManyRequestsException(message5, innerException, ErrorType.Unknown, errorResponse.Code, errorResponse.RequestId, response.StatusCode);
			}
			return new AmazonCognitoIdentityException(ResponseUnmarshaller.GetDefaultErrorMessage<AmazonCognitoIdentityException>(), innerException, ErrorType.Unknown, errorResponse.Code, errorResponse.RequestId, response.StatusCode);
		}

		internal static DeleteIdentityPoolResponseUnmarshaller GetInstance()
		{
			return _instance;
		}
	}
}
