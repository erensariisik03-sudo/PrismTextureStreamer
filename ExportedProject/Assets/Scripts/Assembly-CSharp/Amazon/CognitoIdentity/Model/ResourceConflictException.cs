using System;
using System.Net;
using Amazon.Runtime;

namespace Amazon.CognitoIdentity.Model
{
	public class ResourceConflictException : AmazonCognitoIdentityException
	{
		public ResourceConflictException(string message)
			: base(message)
		{
		}

		public ResourceConflictException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		public ResourceConflictException(Exception innerException)
			: base(innerException)
		{
		}

		public ResourceConflictException(string message, Exception innerException, ErrorType errorType, string errorCode, string requestId, HttpStatusCode statusCode)
			: base(message, innerException, errorType, errorCode, requestId, statusCode)
		{
		}

		public ResourceConflictException(string message, ErrorType errorType, string errorCode, string requestId, HttpStatusCode statusCode)
			: base(message, errorType, errorCode, requestId, statusCode)
		{
		}
	}
}
