using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Amazon.Runtime.Internal.Transform;
using UnityEngine;

namespace Amazon.Runtime.Internal
{
	public sealed class UnityWebRequest : IDisposable, IHttpRequest<string>
	{
		public Uri RequestUri { get; private set; }

		public WWW WwwRequest { get; internal set; }

		public byte[] RequestContent { get; private set; }

		public Dictionary<string, string> Headers { get; private set; }

		public AsyncCallback Callback { get; private set; }

		public IAsyncResult AsyncResult { get; private set; }

		public ManualResetEvent WaitHandle { get; private set; }

		public bool IsSync { get; set; }

		public UnityWebResponseData Response { get; internal set; }

		public Exception Exception { get; internal set; }

		public string Method { get; set; }

		public UnityWebRequest(Uri requestUri)
		{
			RequestUri = requestUri;
			Headers = new Dictionary<string, string>();
		}

		public void ConfigureRequest(IRequestContext requestContext)
		{
		}

		public void SetRequestHeaders(IDictionary<string, string> headers)
		{
			foreach (KeyValuePair<string, string> header in headers)
			{
				Headers.Add(header);
			}
		}

		public string GetRequestContent()
		{
			return string.Empty;
		}

		public IWebResponseData GetResponse()
		{
			if (UnityInitializer.IsMainThread())
			{
				throw new Exception("Network on game thread");
			}
			IsSync = true;
			WaitHandle = new ManualResetEvent(false);
			try
			{
				UnityRequestQueue.Instance.EnqueueRequest(this);
				WaitHandle.WaitOne();
				if (Exception != null)
				{
					throw Exception;
				}
				return Response;
			}
			finally
			{
				WaitHandle.Close();
			}
		}

		public void WriteToRequestBody(string requestContent, Stream contentStream, IDictionary<string, string> contentHeaders, IRequestContext requestContext)
		{
			byte[] array = new byte[8192];
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int count;
				while ((count = contentStream.Read(array, 0, array.Length)) > 0)
				{
					memoryStream.Write(array, 0, count);
				}
				RequestContent = memoryStream.ToArray();
			}
		}

		public void WriteToRequestBody(string requestContent, byte[] content, IDictionary<string, string> contentHeaders)
		{
			RequestContent = content;
		}

		public void Abort()
		{
		}

		public IAsyncResult BeginGetRequestContent(AsyncCallback callback, object state)
		{
			SimpleAsyncResult simpleAsyncResult = new SimpleAsyncResult(state);
			callback(simpleAsyncResult);
			return simpleAsyncResult;
		}

		public string EndGetRequestContent(IAsyncResult asyncResult)
		{
			return string.Empty;
		}

		public IAsyncResult BeginGetResponse(AsyncCallback callback, object state)
		{
			Callback = callback;
			AsyncResult = new SimpleAsyncResult(state);
			UnityRequestQueue.Instance.EnqueueRequest(this);
			return AsyncResult;
		}

		public IWebResponseData EndGetResponse(IAsyncResult asyncResult)
		{
			if (Exception != null)
			{
				throw Exception;
			}
			return Response;
		}

		public void Dispose()
		{
		}
	}
}
