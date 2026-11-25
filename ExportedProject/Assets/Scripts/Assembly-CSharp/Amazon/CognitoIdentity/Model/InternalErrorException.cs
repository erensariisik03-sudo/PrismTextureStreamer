using System;
using System.Net;
using Amazon.Runtime;

namespace Amazon.CognitoIdentity.Model
{
	public class InternalErrorException : AmazonCognitoIdentityException
	{
		public InternalErrorException(string message)
			: base(message)
		{
		}

		public InternalErrorException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		public InternalErrorException(Exception innerException)
			: base(innerException)
		{
		}

		public InternalErrorException(string message, Exception innerException, ErrorType errorType, string errorCode, string requestId, HttpStatusCode statusCode)
			: base(message, innerException, errorType, errorCode, requestId, statusCode)
		{
		}

		public InternalErrorException(string message, ErrorType errorType, string errorCode, string requestId, HttpStatusCode statusCode)
			: base(message, errorType, errorCode, requestId, statusCode)
		{
		}
	}
}
