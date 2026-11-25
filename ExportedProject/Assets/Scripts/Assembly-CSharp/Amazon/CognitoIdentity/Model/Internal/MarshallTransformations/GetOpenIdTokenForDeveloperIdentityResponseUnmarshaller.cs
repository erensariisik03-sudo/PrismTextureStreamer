using System;
using System.Net;
using Amazon.Runtime;
using Amazon.Runtime.Internal;
using Amazon.Runtime.Internal.Transform;

namespace Amazon.CognitoIdentity.Model.Internal.MarshallTransformations
{
	public class GetOpenIdTokenForDeveloperIdentityResponseUnmarshaller : JsonResponseUnmarshaller, ISimplifiedErrorUnmarshaller
	{
		private static GetOpenIdTokenForDeveloperIdentityResponseUnmarshaller _instance = new GetOpenIdTokenForDeveloperIdentityResponseUnmarshaller();

		public static GetOpenIdTokenForDeveloperIdentityResponseUnmarshaller Instance
		{
			get
			{
				return _instance;
			}
		}

		public override AmazonWebServiceResponse Unmarshall(JsonUnmarshallerContext context)
		{
			GetOpenIdTokenForDeveloperIdentityResponse getOpenIdTokenForDeveloperIdentityResponse = new GetOpenIdTokenForDeveloperIdentityResponse();
			context.Read();
			int currentDepth = context.CurrentDepth;
			while (context.ReadAtDepth(currentDepth))
			{
				if (context.TestExpression("IdentityId", currentDepth))
				{
					StringUnmarshaller instance = StringUnmarshaller.Instance;
					getOpenIdTokenForDeveloperIdentityResponse.IdentityId = instance.Unmarshall(context);
				}
				else if (context.TestExpression("Token", currentDepth))
				{
					StringUnmarshaller instance2 = StringUnmarshaller.Instance;
					getOpenIdTokenForDeveloperIdentityResponse.Token = instance2.Unmarshall(context);
				}
			}
			return getOpenIdTokenForDeveloperIdentityResponse;
		}

		public override AmazonServiceException UnmarshallException(JsonUnmarshallerContext context, Exception innerException, HttpStatusCode statusCode)
		{
			ErrorResponse errorResponse = JsonErrorResponseUnmarshaller.GetInstance().Unmarshall(context);
			if (errorResponse.Code != null && errorResponse.Code.Equals("DeveloperUserAlreadyRegisteredException"))
			{
				return new DeveloperUserAlreadyRegisteredException(errorResponse.Message, innerException, errorResponse.Type, errorResponse.Code, errorResponse.RequestId, statusCode);
			}
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
			if (errorResponse.Code != null && errorResponse.Code.Equals("ResourceConflictException"))
			{
				return new ResourceConflictException(errorResponse.Message, innerException, errorResponse.Type, errorResponse.Code, errorResponse.RequestId, statusCode);
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
			if (!string.IsNullOrEmpty(errorResponse.Code) && errorResponse.Code.StartsWith("DeveloperUserAlreadyRegisteredException", StringComparison.OrdinalIgnoreCase))
			{
				string message = ((!string.IsNullOrEmpty(errorResponse.Message)) ? errorResponse.Message : ResponseUnmarshaller.GetDefaultErrorMessage<AmazonCognitoIdentityException>());
				return new DeveloperUserAlreadyRegisteredException(message, innerException, ErrorType.Unknown, errorResponse.Code, errorResponse.RequestId, response.StatusCode);
			}
			if (!string.IsNullOrEmpty(errorResponse.Code) && errorResponse.Code.StartsWith("InternalErrorException", StringComparison.OrdinalIgnoreCase))
			{
				string message2 = ((!string.IsNullOrEmpty(errorResponse.Message)) ? errorResponse.Message : ResponseUnmarshaller.GetDefaultErrorMessage<AmazonCognitoIdentityException>());
				return new InternalErrorException(message2, innerException, ErrorType.Unknown, errorResponse.Code, errorResponse.RequestId, response.StatusCode);
			}
			if (!string.IsNullOrEmpty(errorResponse.Code) && errorResponse.Code.StartsWith("InvalidParameterException", StringComparison.OrdinalIgnoreCase))
			{
				string message3 = ((!string.IsNullOrEmpty(errorResponse.Message)) ? errorResponse.Message : ResponseUnmarshaller.GetDefaultErrorMessage<AmazonCognitoIdentityException>());
				return new InvalidParameterException(message3, innerException, ErrorType.Unknown, errorResponse.Code, errorResponse.RequestId, response.StatusCode);
			}
			if (!string.IsNullOrEmpty(errorResponse.Code) && errorResponse.Code.StartsWith("NotAuthorizedException", StringComparison.OrdinalIgnoreCase))
			{
				string message4 = ((!string.IsNullOrEmpty(errorResponse.Message)) ? errorResponse.Message : ResponseUnmarshaller.GetDefaultErrorMessage<AmazonCognitoIdentityException>());
				return new NotAuthorizedException(message4, innerException, ErrorType.Unknown, errorResponse.Code, errorResponse.RequestId, response.StatusCode);
			}
			if (!string.IsNullOrEmpty(errorResponse.Code) && errorResponse.Code.StartsWith("ResourceConflictException", StringComparison.OrdinalIgnoreCase))
			{
				string message5 = ((!string.IsNullOrEmpty(errorResponse.Message)) ? errorResponse.Message : ResponseUnmarshaller.GetDefaultErrorMessage<AmazonCognitoIdentityException>());
				return new ResourceConflictException(message5, innerException, ErrorType.Unknown, errorResponse.Code, errorResponse.RequestId, response.StatusCode);
			}
			if (!string.IsNullOrEmpty(errorResponse.Code) && errorResponse.Code.StartsWith("ResourceNotFoundException", StringComparison.OrdinalIgnoreCase))
			{
				string message6 = ((!string.IsNullOrEmpty(errorResponse.Message)) ? errorResponse.Message : ResponseUnmarshaller.GetDefaultErrorMessage<AmazonCognitoIdentityException>());
				return new ResourceNotFoundException(message6, innerException, ErrorType.Unknown, errorResponse.Code, errorResponse.RequestId, response.StatusCode);
			}
			if (!string.IsNullOrEmpty(errorResponse.Code) && errorResponse.Code.StartsWith("TooManyRequestsException", StringComparison.OrdinalIgnoreCase))
			{
				string message7 = ((!string.IsNullOrEmpty(errorResponse.Message)) ? errorResponse.Message : ResponseUnmarshaller.GetDefaultErrorMessage<AmazonCognitoIdentityException>());
				return new TooManyRequestsException(message7, innerException, ErrorType.Unknown, errorResponse.Code, errorResponse.RequestId, response.StatusCode);
			}
			return new AmazonCognitoIdentityException(ResponseUnmarshaller.GetDefaultErrorMessage<AmazonCognitoIdentityException>(), innerException, ErrorType.Unknown, errorResponse.Code, errorResponse.RequestId, response.StatusCode);
		}

		internal static GetOpenIdTokenForDeveloperIdentityResponseUnmarshaller GetInstance()
		{
			return _instance;
		}
	}
}
