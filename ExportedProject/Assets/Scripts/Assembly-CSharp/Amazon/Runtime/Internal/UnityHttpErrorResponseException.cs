using System;
using Amazon.Runtime.Internal.Transform;

namespace Amazon.Runtime.Internal
{
	public class UnityHttpErrorResponseException : Exception
	{
		public IWebResponseData Response { get; set; }

		public UnityWebRequest Request { get; private set; }

		public UnityHttpErrorResponseException(UnityWebRequest request)
		{
			Request = request;
			Response = request.Response;
		}

		public UnityHttpErrorResponseException(string message, UnityWebRequest request)
			: base(message)
		{
			Request = request;
			Response = request.Response;
		}

		public UnityHttpErrorResponseException(string message, Exception innerException, UnityWebRequest request)
			: base(message, innerException)
		{
			Request = request;
			Response = request.Response;
		}
	}
}
