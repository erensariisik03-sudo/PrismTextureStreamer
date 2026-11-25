using System;
using System.Net;
using Amazon.Runtime;
using Amazon.Runtime.Internal;
using Amazon.Runtime.Internal.Transform;

namespace Amazon.CognitoIdentity.Model.Internal.MarshallTransformations
{
	public class CreateIdentityPoolResponseUnmarshaller : JsonResponseUnmarshaller, ISimplifiedErrorUnmarshaller
	{
		private static CreateIdentityPoolResponseUnmarshaller _instance = new CreateIdentityPoolResponseUnmarshaller();

		public static CreateIdentityPoolResponseUnmarshaller Instance
		{
			get
			{
				return _instance;
			}
		}

		public override AmazonWebServiceResponse Unmarshall(JsonUnmarshallerContext context)
		{
			CreateIdentityPoolResponse createIdentityPoolResponse = new CreateIdentityPoolResponse();
			context.Read();
			int currentDepth = context.CurrentDepth;
			while (context.ReadAtDepth(currentDepth))
			{
				if (context.TestExpression("AllowUnauthenticatedIdentities", currentDepth))
				{
					BoolUnmarshaller instance = BoolUnmarshaller.Instance;
					createIdentityPoolResponse.AllowUnauthenticatedIdentities = instance.Unmarshall(context);
				}
				else if (context.TestExpression("DeveloperProviderName", currentDepth))
				{
					StringUnmarshaller instance2 = StringUnmarshaller.Instance;
					createIdentityPoolResponse.DeveloperProviderName = instance2.Unmarshall(context);
				}
				else if (context.TestExpression("IdentityPoolId", currentDepth))
				{
					StringUnmarshaller instance3 = StringUnmarshaller.Instance;
					createIdentityPoolResponse.IdentityPoolId = instance3.Unmarshall(context);
				}
				else if (context.TestExpression("IdentityPoolName", currentDepth))
				{
					StringUnmarshaller instance4 = StringUnmarshaller.Instance;
					createIdentityPoolResponse.IdentityPoolName = instance4.Unmarshall(context);
				}
				else if (context.TestExpression("OpenIdConnectProviderARNs", currentDepth))
				{
					ListUnmarshaller<string, StringUnmarshaller> listUnmarshaller = new ListUnmarshaller<string, StringUnmarshaller>(StringUnmarshaller.Instance);
					createIdentityPoolResponse.OpenIdConnectProviderARNs = listUnmarshaller.Unmarshall(context);
				}
				else if (context.TestExpression("SupportedLoginProviders", currentDepth))
				{
					DictionaryUnmarshaller<string, string, StringUnmarshaller, StringUnmarshaller> dictionaryUnmarshaller = new DictionaryUnmarshaller<string, string, StringUnmarshaller, StringUnmarshaller>(StringUnmarshaller.Instance, StringUnmarshaller.Instance);
					createIdentityPoolResponse.SupportedLoginProviders = dictionaryUnmarshaller.Unmarshall(context);
				}
			}
			return createIdentityPoolResponse;
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
			if (errorResponse.Code != null && errorResponse.Code.Equals("LimitExceededException"))
			{
				return new LimitExceededException(errorResponse.Message, innerException, errorResponse.Type, errorResponse.Code, errorResponse.RequestId, statusCode);
			}
			if (errorResponse.Code != null && errorResponse.Code.Equals("NotAuthorizedException"))
			{
				return new NotAuthorizedException(errorResponse.Message, innerException, errorResponse.Type, errorResponse.Code, errorResponse.RequestId, statusCode);
			}
			if (errorResponse.Code != null && errorResponse.Code.Equals("ResourceConflictException"))
			{
				return new ResourceConflictException(errorResponse.Message, innerException, errorResponse.Type, errorResponse.Code, errorResponse.RequestId, statusCode);
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
			if (!string.IsNullOrEmpty(errorResponse.Code) && errorResponse.Code.StartsWith("LimitExceededException", StringComparison.OrdinalIgnoreCase))
			{
				string message3 = ((!string.IsNullOrEmpty(errorResponse.Message)) ? errorResponse.Message : ResponseUnmarshaller.GetDefaultErrorMessage<AmazonCognitoIdentityException>());
				return new LimitExceededException(message3, innerException, ErrorType.Unknown, errorResponse.Code, errorResponse.RequestId, response.StatusCode);
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
			if (!string.IsNullOrEmpty(errorResponse.Code) && errorResponse.Code.StartsWith("TooManyRequestsException", StringComparison.OrdinalIgnoreCase))
			{
				string message6 = ((!string.IsNullOrEmpty(errorResponse.Message)) ? errorResponse.Message : ResponseUnmarshaller.GetDefaultErrorMessage<AmazonCognitoIdentityException>());
				return new TooManyRequestsException(message6, innerException, ErrorType.Unknown, errorResponse.Code, errorResponse.RequestId, response.StatusCode);
			}
			return new AmazonCognitoIdentityException(ResponseUnmarshaller.GetDefaultErrorMessage<AmazonCognitoIdentityException>(), innerException, ErrorType.Unknown, errorResponse.Code, errorResponse.RequestId, response.StatusCode);
		}

		internal static CreateIdentityPoolResponseUnmarshaller GetInstance()
		{
			return _instance;
		}
	}
}
