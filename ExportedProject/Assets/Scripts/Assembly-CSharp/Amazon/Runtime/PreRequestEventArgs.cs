using System;

namespace Amazon.Runtime
{
	public class PreRequestEventArgs : EventArgs
	{
		public AmazonWebServiceRequest Request { get; protected set; }

		protected PreRequestEventArgs()
		{
		}

		internal static PreRequestEventArgs Create(AmazonWebServiceRequest request)
		{
			PreRequestEventArgs e = new PreRequestEventArgs();
			e.Request = request;
			return e;
		}
	}
}
