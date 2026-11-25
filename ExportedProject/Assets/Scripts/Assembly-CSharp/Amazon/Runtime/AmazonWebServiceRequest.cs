using System;
using System.Collections.Generic;
using Amazon.Runtime.Internal;

namespace Amazon.Runtime
{
	public abstract class AmazonWebServiceRequest : IAmazonWebServiceRequest
	{
		private Dictionary<string, object> requestState;

		EventHandler<StreamTransferProgressArgs> IAmazonWebServiceRequest.StreamUploadProgressCallback { get; set; }

		Dictionary<string, object> IAmazonWebServiceRequest.RequestState
		{
			get
			{
				if (requestState == null)
				{
					requestState = new Dictionary<string, object>();
				}
				return requestState;
			}
		}

		bool IAmazonWebServiceRequest.UseSigV4 { get; set; }

		protected virtual bool Expect100Continue
		{
			get
			{
				return false;
			}
		}

		protected virtual bool IncludeSHA256Header
		{
			get
			{
				return true;
			}
		}

		internal event RequestEventHandler BeforeRequestEvent;

		void IAmazonWebServiceRequest.AddBeforeRequestHandler(RequestEventHandler handler)
		{
			BeforeRequestEvent = (RequestEventHandler)Delegate.Combine(BeforeRequestEvent, handler);
		}

		void IAmazonWebServiceRequest.RemoveBeforeRequestHandler(RequestEventHandler handler)
		{
			BeforeRequestEvent = (RequestEventHandler)Delegate.Remove(BeforeRequestEvent, handler);
		}

		internal void FireBeforeRequestEvent(object sender, RequestEventArgs args)
		{
			if (BeforeRequestEvent != null)
			{
				BeforeRequestEvent(sender, args);
			}
		}

		internal bool GetExpect100Continue()
		{
			return Expect100Continue;
		}

		internal bool GetIncludeSHA256Header()
		{
			return IncludeSHA256Header;
		}
	}
}
