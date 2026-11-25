using System;
using System.Net;
using Amazon.Runtime;
using Amazon.Runtime.Internal.Transform;

namespace Amazon.CognitoIdentity
{
	public class AmazonCognitoIdentityException : AmazonServiceException
	{
		public AmazonCognitoIdentityException()
			: base(ResponseUnmarshaller.GetDefaultErrorMessage<AmazonCognitoIdentityException>())
		{
		}

		public AmazonCognitoIdentityException(string message)
			: base(message)
		{
		}

		public AmazonCognitoIdentityException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		public AmazonCognitoIdentityException(Exception innerException)
			: base(innerException.Message, innerException)
		{
		}

		public AmazonCognitoIdentityException(string message, ErrorType errorType, string errorCode, string requestId, HttpStatusCode statusCode)
			: base(message, errorType, errorCode, requestId, statusCode)
		{
		}

		public AmazonCognitoIdentityException(string message, Exception innerException, ErrorType errorType, string errorCode, string requestId, HttpStatusCode statusCode)
			: base(message, innerException, errorType, errorCode, requestId, statusCode)
		{
		}
	}
}
