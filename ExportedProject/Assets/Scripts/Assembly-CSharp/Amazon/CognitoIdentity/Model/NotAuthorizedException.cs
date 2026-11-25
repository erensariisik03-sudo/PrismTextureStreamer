using System;
using System.Net;
using Amazon.Runtime;

namespace Amazon.CognitoIdentity.Model
{
	public class NotAuthorizedException : AmazonCognitoIdentityException
	{
		public NotAuthorizedException(string message)
			: base(message)
		{
		}

		public NotAuthorizedException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		public NotAuthorizedException(Exception innerException)
			: base(innerException)
		{
		}

		public NotAuthorizedException(string message, Exception innerException, ErrorType errorType, string errorCode, string requestId, HttpStatusCode statusCode)
			: base(message, innerException, errorType, errorCode, requestId, statusCode)
		{
		}

		public NotAuthorizedException(string message, ErrorType errorType, string errorCode, string requestId, HttpStatusCode statusCode)
			: base(message, errorType, errorCode, requestId, statusCode)
		{
		}
	}
}
