using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using Amazon.Util;

namespace Amazon.Runtime.Internal
{
	public class DefaultRetryPolicy : RetryPolicy
	{
		private int _maxBackoffInMilliseconds = (int)TimeSpan.FromSeconds(30.0).TotalMilliseconds;

		private ICollection<WebExceptionStatus> _webExceptionStatusesToRetryOn = new HashSet<WebExceptionStatus>
		{
			WebExceptionStatus.ConnectFailure,
			WebExceptionStatus.ConnectionClosed,
			WebExceptionStatus.KeepAliveFailure,
			WebExceptionStatus.NameResolutionFailure,
			WebExceptionStatus.ReceiveFailure
		};

		private ICollection<string> _errorCodesToRetryOn = new HashSet<string> { "Throttling", "ProvisionedThroughputExceededException", "RequestTimeout" };

		public int MaxBackoffInMilliseconds
		{
			get
			{
				return _maxBackoffInMilliseconds;
			}
			set
			{
				_maxBackoffInMilliseconds = value;
			}
		}

		public ICollection<string> ErrorCodesToRetryOn
		{
			get
			{
				return _errorCodesToRetryOn;
			}
		}

		public ICollection<WebExceptionStatus> WebExceptionStatusesToRetryOn
		{
			get
			{
				return _webExceptionStatusesToRetryOn;
			}
		}

		public DefaultRetryPolicy(int maxRetries)
		{
			base.MaxRetries = maxRetries;
		}

		public override bool CanRetry(IExecutionContext executionContext)
		{
			return executionContext.RequestContext.Request.IsRequestStreamRewindable();
		}

		public override bool RetryForException(IExecutionContext executionContext, Exception exception)
		{
			if (exception is IOException)
			{
				if (IsInnerException<ThreadAbortException>(exception))
				{
					return false;
				}
				return true;
			}
			AmazonServiceException ex = exception as AmazonServiceException;
			if (ex != null)
			{
				if (ex.StatusCode == HttpStatusCode.InternalServerError || ex.StatusCode == HttpStatusCode.ServiceUnavailable)
				{
					return true;
				}
				if (ex.StatusCode == HttpStatusCode.BadRequest || ex.StatusCode == HttpStatusCode.ServiceUnavailable)
				{
					string errorCode = ex.ErrorCode;
					if (ErrorCodesToRetryOn.Contains(errorCode))
					{
						return true;
					}
				}
				WebException inner;
				if (IsInnerException<WebException>(exception, out inner) && WebExceptionStatusesToRetryOn.Contains(inner.Status))
				{
					return true;
				}
			}
			return false;
		}

		public override bool RetryLimitReached(IExecutionContext executionContext)
		{
			return executionContext.RequestContext.Retries >= base.MaxRetries;
		}

		public override void WaitBeforeRetry(IExecutionContext executionContext)
		{
			WaitBeforeRetry(executionContext.RequestContext.Retries, MaxBackoffInMilliseconds);
		}

		public static void WaitBeforeRetry(int retries, int maxBackoffInMilliseconds)
		{
			int val = (int)(Math.Pow(4.0, retries) * 100.0);
			val = Math.Min(val, maxBackoffInMilliseconds);
			AWSSDKUtils.Sleep(val);
		}

		protected static bool IsInnerException<T>(Exception exception) where T : Exception
		{
			T inner;
			return IsInnerException<T>(exception, out inner);
		}

		protected static bool IsInnerException<T>(Exception exception, out T inner) where T : Exception
		{
			inner = (T)null;
			Exception ex = exception;
			while (ex.InnerException != null)
			{
				inner = ex.InnerException as T;
				if (inner != null)
				{
					return true;
				}
				ex = ex.InnerException;
			}
			return false;
		}
	}
}
