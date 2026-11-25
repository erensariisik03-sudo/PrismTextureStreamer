using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using Amazon.Runtime.Internal.Auth;
using Amazon.Runtime.Internal.Transform;
using Amazon.Runtime.Internal.Util;

namespace Amazon.Runtime.Internal
{
	public class AsyncResult : IDisposable, IRequestData, IAsyncResult
	{
		public class AsyncRequestState
		{
			internal HttpWebRequest WebRequest { get; private set; }

			internal byte[] RequestData { get; private set; }

			internal Stream RequestStream { get; private set; }

			internal bool GetRequestStreamCallbackCalled { get; set; }

			internal bool GetResponseCallbackCalled { get; set; }

			public AsyncRequestState(HttpWebRequest webRequest, byte[] requestData, Stream requestStream)
			{
				WebRequest = webRequest;
				RequestData = requestData;
				RequestStream = requestStream;
			}
		}

		private ManualResetEvent _waitHandle;

		private object _lockObj;

		private Stopwatch _stopWatch;

		private bool _callbackInvoked;

		private bool _disposed;

		private Guid id = Guid.NewGuid();

		internal Exception Exception { get; set; }

		public int RetriesAttempt { get; set; }

		internal AsyncRequestState RequestState { get; set; }

		internal object FinalResponse { get; set; }

		public ResponseUnmarshaller Unmarshaller { get; private set; }

		public IRequest Request { get; private set; }

		internal AsyncCallback Callback { get; private set; }

		public AbstractAWSSigner Signer { get; private set; }

		internal object State { get; private set; }

		internal string RequestName { get; private set; }

		public RequestMetrics Metrics { get; private set; }

		public bool CompletedSynchronously { get; private set; }

		public bool IsCompleted { get; private set; }

		public object AsyncState
		{
			get
			{
				return State;
			}
		}

		public WaitHandle AsyncWaitHandle
		{
			get
			{
				if (_waitHandle != null)
				{
					return _waitHandle;
				}
				lock (_lockObj)
				{
					if (_waitHandle == null)
					{
						_waitHandle = new ManualResetEvent(IsCompleted);
					}
				}
				return _waitHandle;
			}
		}

		internal Guid Id
		{
			get
			{
				return id;
			}
		}

		internal AsyncResult(IRequest request, AsyncCallback callback, object state, bool completeSynchronized, AbstractAWSSigner signer, ResponseUnmarshaller unmarshaller)
		{
			Request = request;
			Callback = callback;
			State = state;
			CompletedSynchronously = completeSynchronized;
			Signer = signer;
			Unmarshaller = unmarshaller;
			RequestName = request.OriginalRequest.GetType().Name;
			Metrics = new RequestMetrics();
			_lockObj = new object();
			_stopWatch = Stopwatch.StartNew();
			_stopWatch.Start();
		}

		internal void SetCompletedSynchronously(bool completedSynchronously)
		{
			CompletedSynchronously = completedSynchronously;
		}

		internal void SignalWaitHandle()
		{
			IsCompleted = true;
			if (_waitHandle != null)
			{
				_waitHandle.Set();
			}
		}

		internal void HandleException(Exception exception)
		{
			Exception = exception;
			Metrics.AddProperty(Metric.Exception, exception);
			InvokeCallback();
		}

		internal void InvokeCallback()
		{
			SignalWaitHandle();
			if (!_callbackInvoked && Callback != null)
			{
				_callbackInvoked = true;
				Callback(this);
			}
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				if (disposing && _waitHandle != null)
				{
					_waitHandle.Close();
					_waitHandle = null;
				}
				_disposed = true;
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}
