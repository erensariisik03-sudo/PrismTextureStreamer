using System;
using System.Net;
using Amazon.Runtime;

namespace Amazon.CognitoIdentity.Model
{
	public class TooManyRequestsException : AmazonCognitoIdentityException
	{
		public TooManyRequestsException(string message)
			: base(message)
		{
		}

		public TooManyRequestsException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		public TooManyRequestsException(Exception innerException)
			: base(innerException)
		{
		}

		public TooManyRequestsException(string message, Exception innerException, ErrorType errorType, string errorCode, string requestId, HttpStatusCode statusCode)
			: base(message, innerException, errorType, errorCode, requestId, statusCode)
		{
		}

		public TooManyRequestsException(string message, ErrorType errorType, string errorCode, string requestId, HttpStatusCode statusCode)
			: base(message, errorType, errorCode, requestId, statusCode)
		{
		}
	}
}
