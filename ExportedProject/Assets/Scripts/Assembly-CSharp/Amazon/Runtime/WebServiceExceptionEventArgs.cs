using System;
using System.Collections.Generic;
using Amazon.Runtime.Internal;

namespace Amazon.Runtime
{
	public class WebServiceExceptionEventArgs : ExceptionEventArgs
	{
		public IDictionary<string, string> Headers { get; protected set; }

		public IDictionary<string, string> Parameters { get; protected set; }

		public string ServiceName { get; protected set; }

		public Uri Endpoint { get; protected set; }

		public AmazonWebServiceRequest Request { get; protected set; }

		public Exception Exception { get; protected set; }

		protected WebServiceExceptionEventArgs()
		{
		}

		internal static WebServiceExceptionEventArgs Create(Exception exception, IRequest request)
		{
			WebServiceExceptionEventArgs e;
			if (request == null)
			{
				e = new WebServiceExceptionEventArgs();
				e.Exception = exception;
				return e;
			}
			e = new WebServiceExceptionEventArgs();
			e.Headers = request.Headers;
			e.Parameters = request.Parameters;
			e.ServiceName = request.ServiceName;
			e.Request = request.OriginalRequest;
			e.Endpoint = request.Endpoint;
			e.Exception = exception;
			return e;
		}
	}
}
