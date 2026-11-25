using System;
using System.Net;
using Amazon.Runtime;

namespace Amazon.CognitoIdentity.Model
{
	public class ExternalServiceException : AmazonCognitoIdentityException
	{
		public ExternalServiceException(string message)
			: base(message)
		{
		}

		public ExternalServiceException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		public ExternalServiceException(Exception innerException)
			: base(innerException)
		{
		}

		public ExternalServiceException(string message, Exception innerException, ErrorType errorType, string errorCode, string requestId, HttpStatusCode statusCode)
			: base(message, innerException, errorType, errorCode, requestId, statusCode)
		{
		}

		public ExternalServiceException(string message, ErrorType errorType, string errorCode, string requestId, HttpStatusCode statusCode)
			: base(message, errorType, errorCode, requestId, statusCode)
		{
		}
	}
}
