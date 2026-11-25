using System;
using System.Collections.Generic;
using Amazon.Runtime.Internal;

namespace Amazon.Runtime
{
	public class WebServiceRequestEventArgs : RequestEventArgs
	{
		public IDictionary<string, string> Headers { get; protected set; }

		public IDictionary<string, string> Parameters { get; protected set; }

		public string ServiceName { get; protected set; }

		public Uri Endpoint { get; protected set; }

		public AmazonWebServiceRequest Request { get; protected set; }

		[Obsolete("OriginalRequest property has been deprecated in favor of the Request property")]
		public AmazonWebServiceRequest OriginalRequest
		{
			get
			{
				return Request;
			}
		}

		protected WebServiceRequestEventArgs()
		{
		}

		internal static WebServiceRequestEventArgs Create(IRequest request)
		{
			WebServiceRequestEventArgs e = new WebServiceRequestEventArgs();
			e.Headers = request.Headers;
			e.Parameters = request.Parameters;
			e.ServiceName = request.ServiceName;
			e.Request = request.OriginalRequest;
			e.Endpoint = request.Endpoint;
			return e;
		}
	}
}
