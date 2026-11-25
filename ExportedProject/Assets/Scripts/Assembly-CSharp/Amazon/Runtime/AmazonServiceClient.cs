using System;
using System.Collections.Generic;
using System.Text;
using Amazon.Runtime.Internal;
using Amazon.Runtime.Internal.Auth;
using Amazon.Runtime.Internal.Transform;
using Amazon.Runtime.Internal.Util;
using Amazon.Util;

namespace Amazon.Runtime
{
	public abstract class AmazonServiceClient : IDisposable
	{
		private bool _disposed;

		private Logger _logger;

		protected RuntimePipeline RuntimePipeline { get; set; }

		protected internal AWSCredentials Credentials { get; private set; }

		public ClientConfig Config { get; private set; }

		protected ILogger Logger
		{
			get
			{
				return _logger;
			}
		}

		protected virtual bool SupportResponseLogging
		{
			get
			{
				return true;
			}
		}

		protected AbstractAWSSigner Signer { get; private set; }

		internal event PreRequestEventHandler BeforeMarshallingEvent;

		public event RequestEventHandler BeforeRequestEvent;

		public event ResponseEventHandler AfterResponseEvent;

		public event ExceptionEventHandler ExceptionEvent;

		protected AmazonServiceClient(AWSCredentials credentials, ClientConfig config)
		{
			if (config.DisableLogging)
			{
				_logger = Amazon.Runtime.Internal.Util.Logger.EmptyLogger;
			}
			else
			{
				_logger = Amazon.Runtime.Internal.Util.Logger.GetLogger(GetType());
			}
			config.Validate();
			Config = config;
			Credentials = credentials;
			Signer = CreateSigner();
			Initialize();
			BuildRuntimePipeline();
		}

		protected AmazonServiceClient(string awsAccessKeyId, string awsSecretAccessKey, string awsSessionToken, ClientConfig config)
			: this(new SessionAWSCredentials(awsAccessKeyId, awsSecretAccessKey, awsSessionToken), config)
		{
		}

		protected AmazonServiceClient(string awsAccessKeyId, string awsSecretAccessKey, ClientConfig config)
			: this(new BasicAWSCredentials(awsAccessKeyId, awsSecretAccessKey), config)
		{
		}

		protected virtual void Initialize()
		{
		}

		protected TResponse Invoke<TRequest, TResponse>(TRequest request, IMarshaller<IRequest, AmazonWebServiceRequest> marshaller, ResponseUnmarshaller unmarshaller) where TRequest : AmazonWebServiceRequest where TResponse : AmazonWebServiceResponse
		{
			ThrowIfDisposed();
			RequestContext requestContext = new RequestContext(Config.LogMetrics);
			requestContext.ClientConfig = Config;
			requestContext.Marshaller = marshaller;
			requestContext.OriginalRequest = request;
			requestContext.Signer = Signer;
			requestContext.Unmarshaller = unmarshaller;
			requestContext.IsAsync = false;
			ExecutionContext executionContext = new ExecutionContext(requestContext, new ResponseContext());
			return (TResponse)RuntimePipeline.InvokeSync(executionContext).Response;
		}

		protected IAsyncResult BeginInvoke<TRequest>(TRequest request, IMarshaller<IRequest, AmazonWebServiceRequest> marshaller, ResponseUnmarshaller unmarshaller, AsyncCallback callback, object state) where TRequest : AmazonWebServiceRequest
		{
			ThrowIfDisposed();
			AsyncRequestContext asyncRequestContext = new AsyncRequestContext(Config.LogMetrics);
			asyncRequestContext.ClientConfig = Config;
			asyncRequestContext.Marshaller = marshaller;
			asyncRequestContext.OriginalRequest = request;
			asyncRequestContext.Signer = Signer;
			asyncRequestContext.Unmarshaller = unmarshaller;
			asyncRequestContext.Callback = callback;
			asyncRequestContext.State = state;
			asyncRequestContext.IsAsync = true;
			AsyncExecutionContext executionContext = new AsyncExecutionContext(asyncRequestContext, new AsyncResponseContext());
			return RuntimePipeline.InvokeAsync(executionContext);
		}

		protected static TResponse EndInvoke<TResponse>(IAsyncResult result) where TResponse : AmazonWebServiceResponse
		{
			if (result == null)
			{
				throw new ArgumentNullException("result", "Parameter result cannot be null.");
			}
			RuntimeAsyncResult runtimeAsyncResult = result as RuntimeAsyncResult;
			if (runtimeAsyncResult == null)
			{
				throw new ArgumentOutOfRangeException("result", "Parameter result is not of type RuntimeAsyncResult.");
			}
			using (runtimeAsyncResult)
			{
				if (!runtimeAsyncResult.IsCompleted)
				{
					runtimeAsyncResult.AsyncWaitHandle.WaitOne();
				}
				if (runtimeAsyncResult.Exception != null)
				{
					AWSSDKUtils.PreserveStackTrace(runtimeAsyncResult.Exception);
					throw runtimeAsyncResult.Exception;
				}
				return (TResponse)runtimeAsyncResult.Response;
			}
		}

		protected void ProcessPreRequestHandlers(IExecutionContext executionContext)
		{
			if (BeforeMarshallingEvent != null)
			{
				PreRequestEventArgs e = PreRequestEventArgs.Create(executionContext.RequestContext.OriginalRequest);
				BeforeMarshallingEvent(this, e);
			}
		}

		protected void ProcessRequestHandlers(IExecutionContext executionContext)
		{
			IRequest request = executionContext.RequestContext.Request;
			WebServiceRequestEventArgs e = WebServiceRequestEventArgs.Create(request);
			if (request.OriginalRequest != null)
			{
				request.OriginalRequest.FireBeforeRequestEvent(this, e);
			}
			if (BeforeRequestEvent != null)
			{
				BeforeRequestEvent(this, e);
			}
		}

		protected void ProcessResponseHandlers(IExecutionContext executionContext)
		{
			if (AfterResponseEvent != null)
			{
				WebServiceResponseEventArgs e = WebServiceResponseEventArgs.Create(executionContext.ResponseContext.Response, executionContext.RequestContext.Request, executionContext.ResponseContext.HttpResponse);
				AfterResponseEvent(this, e);
			}
		}

		protected virtual void ProcessExceptionHandlers(IExecutionContext executionContext, Exception exception)
		{
			if (ExceptionEvent != null)
			{
				WebServiceExceptionEventArgs e = WebServiceExceptionEventArgs.Create(exception, executionContext.RequestContext.Request);
				ExceptionEvent(this, e);
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!_disposed && disposing)
			{
				if (RuntimePipeline != null)
				{
					RuntimePipeline.Dispose();
				}
				_disposed = true;
			}
		}

		protected virtual void ThrowIfDisposed()
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
		}

		protected abstract AbstractAWSSigner CreateSigner();

		protected virtual void CustomizeRuntimePipeline(RuntimePipeline pipeline)
		{
		}

		protected virtual void BuildRuntimePipeline()
		{
		}

		public static Uri ComposeUrl(IRequest iRequest)
		{
			Uri endpoint = iRequest.Endpoint;
			string text = iRequest.ResourcePath;
			if (text == null)
			{
				text = string.Empty;
			}
			else if (text.StartsWith("/", StringComparison.Ordinal))
			{
				text = text.Substring(1);
			}
			string arg = "?";
			StringBuilder stringBuilder = new StringBuilder();
			if (iRequest.SubResources.Count > 0)
			{
				foreach (KeyValuePair<string, string> subResource in iRequest.SubResources)
				{
					stringBuilder.AppendFormat("{0}{1}", arg, subResource.Key);
					if (subResource.Value != null)
					{
						stringBuilder.AppendFormat("={0}", subResource.Value);
					}
					arg = "&";
				}
			}
			if (iRequest.UseQueryString && iRequest.Parameters.Count > 0)
			{
				string parametersAsString = AWSSDKUtils.GetParametersAsString(iRequest.Parameters);
				stringBuilder.AppendFormat("{0}{1}", arg, parametersAsString);
			}
			string text2 = AWSSDKUtils.UrlEncode(text, true) + stringBuilder;
			Uri uri = new Uri(endpoint.AbsoluteUri + text2);
			DontUnescapePathDotsAndSlashes(uri);
			return uri;
		}

		private static void DontUnescapePathDotsAndSlashes(Uri uri)
		{
		}

		internal C CloneConfig<C>() where C : ClientConfig, new()
		{
			C val = new C();
			CloneConfig(val);
			return val;
		}

		internal void CloneConfig(ClientConfig newConfig)
		{
			if (!string.IsNullOrEmpty(Config.ServiceURL))
			{
				string systemName = AWSSDKUtils.DetermineRegion(Config.ServiceURL);
				RegionEndpoint bySystemName = RegionEndpoint.GetBySystemName(systemName);
				newConfig.RegionEndpoint = bySystemName;
			}
			else
			{
				newConfig.RegionEndpoint = Config.RegionEndpoint;
			}
			newConfig.UseHttp = Config.UseHttp;
			newConfig.ProxyCredentials = Config.ProxyCredentials;
		}
	}
}
