using System;
using System.Net;
using Amazon.Runtime;

namespace Amazon.SecurityToken.Model
{
	public class IDPRejectedClaimException : AmazonSecurityTokenServiceException
	{
		public IDPRejectedClaimException(string message)
			: base(message)
		{
		}

		public IDPRejectedClaimException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		public IDPRejectedClaimException(Exception innerException)
			: base(innerException)
		{
		}

		public IDPRejectedClaimException(string message, Exception innerException, ErrorType errorType, string errorCode, string requestId, HttpStatusCode statusCode)
			: base(message, innerException, errorType, errorCode, requestId, statusCode)
		{
		}

		public IDPRejectedClaimException(string message, ErrorType errorType, string errorCode, string requestId, HttpStatusCode statusCode)
			: base(message, errorType, errorCode, requestId, statusCode)
		{
		}
	}
}
