using System;
using System.Collections;
using System.Threading;
using Amazon.Runtime.Internal.Transform;
using Amazon.Runtime.Internal.Util;
using UnityEngine;

namespace Amazon.Runtime.Internal
{
	public class UnityMainThreadDispatcher : MonoBehaviour
	{
		public class NetworkStatusRefreshed : EventArgs
		{
			public NetworkReachability NetworkReachability;

			public NetworkStatusRefreshed(NetworkReachability reachability)
			{
				NetworkReachability = reachability;
			}
		}

		private Logger _logger;

		public static event EventHandler<NetworkStatusRefreshed> OnRefresh;

		public void Awake()
		{
			_logger = Logger.GetLogger(GetType());
			InvokeRepeating("ProcessRequests", 0.1f, 0.1f);
		}

		private void ProcessRequests()
		{
			UnityWebRequest unityWebRequest = UnityRequestQueue.Instance.DequeueRequest();
			if (unityWebRequest != null)
			{
				StartCoroutine(InvokeRequest(unityWebRequest));
			}
			RuntimeAsyncResult runtimeAsyncResult = UnityRequestQueue.Instance.DequeueCallback();
			if (runtimeAsyncResult != null && runtimeAsyncResult.Action != null)
			{
				try
				{
					runtimeAsyncResult.Action(runtimeAsyncResult.Request, runtimeAsyncResult.Response, runtimeAsyncResult.Exception, runtimeAsyncResult.AsyncOptions);
				}
				catch (Exception exception)
				{
					_logger.Error(exception, "An unhandled exception was thrown from the callback method {0}.", runtimeAsyncResult.Request.ToString());
				}
			}
			Action action = UnityRequestQueue.Instance.DequeueMainThreadOperation();
			if (action != null)
			{
				try
				{
					action();
				}
				catch (Exception exception2)
				{
					_logger.Error(exception2, "An unhandled exception was thrown from the callback method");
				}
			}
			NetworkReachability internetReachability = Application.internetReachability;
			if (OnRefresh != null)
			{
				OnRefresh(this, new NetworkStatusRefreshed(internetReachability));
			}
		}

		private IEnumerator InvokeRequest(UnityWebRequest request)
		{
			request.WwwRequest = new WWW(request.RequestUri.AbsoluteUri, request.RequestContent, request.Headers);
			yield return request.WwwRequest;
			request.Response = new UnityWebResponseData(request.WwwRequest);
			if (request.IsSync)
			{
				if (!request.Response.IsSuccessStatusCode)
				{
					request.Exception = new UnityHttpErrorResponseException(request);
				}
				request.WaitHandle.Set();
				yield break;
			}
			if (!request.Response.IsSuccessStatusCode)
			{
				request.Exception = new UnityHttpErrorResponseException(request);
			}
			UnityWebRequest request2 = default(UnityWebRequest);
			ThreadPool.QueueUserWorkItem((object state) =>
			{
				try
				{
					request2.Callback(request2.AsyncResult);
				}
				catch (Exception exception)
				{
					_logger.Error(exception, "An exception was thrown from the callback method executed fromUnityMainThreadDispatcher.InvokeRequest method.");
				}
			});
		}
	}
}
