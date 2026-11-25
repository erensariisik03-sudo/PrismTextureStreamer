using System;
using System.Net;
using Amazon.Runtime;

namespace Amazon.SecurityToken.Model
{
	public class MalformedPolicyDocumentException : AmazonSecurityTokenServiceException
	{
		public MalformedPolicyDocumentException(string message)
			: base(message)
		{
		}

		public MalformedPolicyDocumentException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		public MalformedPolicyDocumentException(Exception innerException)
			: base(innerException)
		{
		}

		public MalformedPolicyDocumentException(string message, Exception innerException, ErrorType errorType, string errorCode, string requestId, HttpStatusCode statusCode)
			: base(message, innerException, errorType, errorCode, requestId, statusCode)
		{
		}

		public MalformedPolicyDocumentException(string message, ErrorType errorType, string errorCode, string requestId, HttpStatusCode statusCode)
			: base(message, errorType, errorCode, requestId, statusCode)
		{
		}
	}
}
