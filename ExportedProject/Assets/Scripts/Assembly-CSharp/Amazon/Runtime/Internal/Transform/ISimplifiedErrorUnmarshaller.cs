using System;

namespace Amazon.Runtime.Internal.Transform
{
	public interface ISimplifiedErrorUnmarshaller
	{
		AmazonServiceException UnmarshallException(IWebResponseData response, ErrorResponse errorResponse, Exception innerException);
	}
}
