using System;
using System.Collections.Generic;
using Amazon.Runtime.Internal;
using Amazon.Runtime.Internal.Transform;

namespace Amazon.Runtime
{
	public abstract class AmazonUnityServiceClient : AmazonServiceClient
	{
		protected AmazonUnityServiceClient(AWSCredentials credentials, ClientConfig config)
			: base(credentials, config)
		{
		}

		protected AmazonUnityServiceClient(string awsAccessKeyId, string awsSecretAccessKey, string awsSessionToken, ClientConfig config)
			: base(awsAccessKeyId, awsSecretAccessKey, awsSessionToken, config)
		{
		}

		protected AmazonUnityServiceClient(string awsAccessKeyId, string awsSecretAccessKey, ClientConfig config)
			: base(awsAccessKeyId, awsSecretAccessKey, config)
		{
		}

		protected override void CustomizeRuntimePipeline(RuntimePipeline pipeline)
		{
			base.CustomizeRuntimePipeline(pipeline);
		}

		protected IAsyncResult BeginInvoke<TRequest>(TRequest request, IMarshaller<IRequest, AmazonWebServiceRequest> marshaller, ResponseUnmarshaller unmarshaller, AsyncOptions asyncOptions, Action<AmazonWebServiceRequest, AmazonWebServiceResponse, Exception, AsyncOptions> callbackHelper) where TRequest : AmazonWebServiceRequest
		{
			ThrowIfDisposed();
			asyncOptions = asyncOptions ?? new AsyncOptions();
			AsyncRequestContext asyncRequestContext = new AsyncRequestContext(base.Config.LogMetrics);
			asyncRequestContext.ClientConfig = base.Config;
			asyncRequestContext.Marshaller = marshaller;
			asyncRequestContext.OriginalRequest = request;
			asyncRequestContext.Signer = base.Signer;
			asyncRequestContext.Unmarshaller = unmarshaller;
			asyncRequestContext.Action = callbackHelper;
			asyncRequestContext.AsyncOptions = asyncOptions;
			asyncRequestContext.IsAsync = true;
			AsyncExecutionContext executionContext = new AsyncExecutionContext(asyncRequestContext, new AsyncResponseContext());
			return base.RuntimePipeline.InvokeAsync(executionContext);
		}

		protected override void BuildRuntimePipeline()
		{
			UnityWebRequestFactory requestFactory = new UnityWebRequestFactory();
			HttpHandler<string> item = new HttpHandler<string>(requestFactory, this);
			CallbackHandler callbackHandler = new CallbackHandler();
			callbackHandler.OnPreInvoke = base.ProcessPreRequestHandlers;
			CallbackHandler callbackHandler2 = new CallbackHandler();
			callbackHandler2.OnPreInvoke = base.ProcessRequestHandlers;
			CallbackHandler callbackHandler3 = new CallbackHandler();
			callbackHandler3.OnPostInvoke = base.ProcessResponseHandlers;
			ErrorCallbackHandler errorCallbackHandler = new ErrorCallbackHandler();
			errorCallbackHandler.OnError = ProcessExceptionHandlers;
			base.RuntimePipeline = new RuntimePipeline(new List<IPipelineHandler>
			{
				item,
				new Unmarshaller(SupportResponseLogging),
				new ErrorHandler(base.Logger),
				callbackHandler3,
				new Signer(),
				new CredentialsRetriever(base.Credentials),
				new RetryHandler(new DefaultRetryPolicy(base.Config.MaxErrorRetry)),
				callbackHandler2,
				new EndpointResolver(),
				new Marshaller(),
				callbackHandler,
				errorCallbackHandler,
				new MetricsHandler(),
				new ThreadPoolExecutionHandler(10)
			}, base.Logger);
			CustomizeRuntimePipeline(base.RuntimePipeline);
		}
	}
}
