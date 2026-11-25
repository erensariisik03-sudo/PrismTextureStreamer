using System;
using System.Net;
using Amazon.Runtime;
using Amazon.Runtime.Internal.Transform;

namespace Amazon.SecurityToken
{
	public class AmazonSecurityTokenServiceException : AmazonServiceException
	{
		public AmazonSecurityTokenServiceException()
			: base(ResponseUnmarshaller.GetDefaultErrorMessage<AmazonSecurityTokenServiceException>())
		{
		}

		public AmazonSecurityTokenServiceException(string message)
			: base(message)
		{
		}

		public AmazonSecurityTokenServiceException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		public AmazonSecurityTokenServiceException(Exception innerException)
			: base(innerException.Message, innerException)
		{
		}

		public AmazonSecurityTokenServiceException(string message, ErrorType errorType, string errorCode, string requestId, HttpStatusCode statusCode)
			: base(message, errorType, errorCode, requestId, statusCode)
		{
		}

		public AmazonSecurityTokenServiceException(string message, Exception innerException, ErrorType errorType, string errorCode, string requestId, HttpStatusCode statusCode)
			: base(message, innerException, errorType, errorCode, requestId, statusCode)
		{
		}
	}
}
