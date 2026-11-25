using System;
using System.Net;
using Amazon.Runtime;

namespace Amazon.CognitoIdentity.Model
{
	public class InvalidParameterException : AmazonCognitoIdentityException
	{
		public InvalidParameterException(string message)
			: base(message)
		{
		}

		public InvalidParameterException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		public InvalidParameterException(Exception innerException)
			: base(innerException)
		{
		}

		public InvalidParameterException(string message, Exception innerException, ErrorType errorType, string errorCode, string requestId, HttpStatusCode statusCode)
			: base(message, innerException, errorType, errorCode, requestId, statusCode)
		{
		}

		public InvalidParameterException(string message, ErrorType errorType, string errorCode, string requestId, HttpStatusCode statusCode)
			: base(message, errorType, errorCode, requestId, statusCode)
		{
		}
	}
}
