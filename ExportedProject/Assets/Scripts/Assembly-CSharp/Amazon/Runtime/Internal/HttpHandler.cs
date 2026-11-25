using System;
using System.Globalization;
using System.IO;
using System.Text;
using Amazon.Runtime.Internal.Transform;
using Amazon.Runtime.Internal.Util;
using Amazon.Util;

namespace Amazon.Runtime.Internal
{
	public class HttpHandler<TRequestContent> : PipelineHandler, IDisposable
	{
		private bool _disposed;

		private IHttpRequestFactory<TRequestContent> _requestFactory;

		public object CallbackSender { get; private set; }

		public HttpHandler(IHttpRequestFactory<TRequestContent> requestFactory, object callbackSender)
		{
			_requestFactory = requestFactory;
			CallbackSender = callbackSender;
		}

		public override void InvokeSync(IExecutionContext executionContext)
		{
			IHttpRequest<TRequestContent> httpRequest = null;
			try
			{
				SetMetrics(executionContext.RequestContext);
				IRequest request = executionContext.RequestContext.Request;
				httpRequest = CreateWebRequest(executionContext.RequestContext);
				httpRequest.SetRequestHeaders(request.Headers);
				using (executionContext.RequestContext.Metrics.StartEvent(Metric.HttpRequestTime))
				{
					if (request.HasRequestBody())
					{
						try
						{
							TRequestContent requestContent = httpRequest.GetRequestContent();
							WriteContentToRequestBody(requestContent, httpRequest, executionContext.RequestContext);
						}
						catch
						{
							CompleteFailedRequest(httpRequest);
							throw;
						}
					}
					executionContext.ResponseContext.HttpResponse = httpRequest.GetResponse();
				}
			}
			finally
			{
				if (httpRequest != null)
				{
					httpRequest.Dispose();
				}
			}
		}

		private static void CompleteFailedRequest(IHttpRequest<TRequestContent> httpRequest)
		{
			try
			{
				IWebResponseData webResponseData = null;
				try
				{
					webResponseData = httpRequest.GetResponse();
				}
				catch
				{
				}
				finally
				{
					if (webResponseData != null && webResponseData.ResponseBody != null)
					{
						webResponseData.ResponseBody.Dispose();
					}
				}
			}
			catch
			{
			}
		}

		public override IAsyncResult InvokeAsync(IAsyncExecutionContext executionContext)
		{
			IHttpRequest<TRequestContent> httpRequest = null;
			try
			{
				SetMetrics(executionContext.RequestContext);
				httpRequest = (IHttpRequest<TRequestContent>)(executionContext.RuntimeState = CreateWebRequest(executionContext.RequestContext));
				IRequest request = executionContext.RequestContext.Request;
				if (executionContext.RequestContext.Retries == 0)
				{
					executionContext.ResponseContext.AsyncResult = new RuntimeAsyncResult(executionContext.RequestContext.Callback, executionContext.RequestContext.State);
					executionContext.ResponseContext.AsyncResult.AsyncOptions = executionContext.RequestContext.AsyncOptions;
					executionContext.ResponseContext.AsyncResult.Action = executionContext.RequestContext.Action;
					executionContext.ResponseContext.AsyncResult.Request = executionContext.RequestContext.OriginalRequest;
				}
				httpRequest.SetRequestHeaders(executionContext.RequestContext.Request.Headers);
				executionContext.RequestContext.Metrics.StartEvent(Metric.HttpRequestTime);
				if (request.HasRequestBody())
				{
					httpRequest.BeginGetRequestContent(GetRequestStreamCallback, executionContext);
				}
				else
				{
					httpRequest.BeginGetResponse(GetResponseCallback, executionContext);
				}
				return executionContext.ResponseContext.AsyncResult;
			}
			catch (Exception exception)
			{
				if (executionContext.ResponseContext.AsyncResult != null)
				{
					executionContext.ResponseContext.AsyncResult.Dispose();
					executionContext.ResponseContext.AsyncResult = null;
				}
				if (httpRequest != null)
				{
					httpRequest.Dispose();
				}
				Logger.Error(exception, "An exception occured while initiating an asynchronous HTTP request.");
				throw;
			}
		}

		private void GetRequestStreamCallback(IAsyncResult result)
		{
			IAsyncExecutionContext asyncExecutionContext = null;
			IHttpRequest<TRequestContent> httpRequest = null;
			try
			{
				asyncExecutionContext = result.AsyncState as IAsyncExecutionContext;
				httpRequest = asyncExecutionContext.RuntimeState as IHttpRequest<TRequestContent>;
				TRequestContent requestContent = httpRequest.EndGetRequestContent(result);
				WriteContentToRequestBody(requestContent, httpRequest, asyncExecutionContext.RequestContext);
				httpRequest.BeginGetResponse(GetResponseCallback, asyncExecutionContext);
			}
			catch (Exception exception)
			{
				httpRequest.Dispose();
				asyncExecutionContext.ResponseContext.AsyncResult.Exception = exception;
				base.InvokeAsyncCallback(asyncExecutionContext);
			}
		}

		private void GetResponseCallback(IAsyncResult result)
		{
			IAsyncExecutionContext asyncExecutionContext = null;
			IHttpRequest<TRequestContent> httpRequest = null;
			try
			{
				asyncExecutionContext = result.AsyncState as IAsyncExecutionContext;
				httpRequest = asyncExecutionContext.RuntimeState as IHttpRequest<TRequestContent>;
				IWebResponseData httpResponse = httpRequest.EndGetResponse(result);
				asyncExecutionContext.ResponseContext.HttpResponse = httpResponse;
			}
			catch (Exception exception)
			{
				asyncExecutionContext.ResponseContext.AsyncResult.Exception = exception;
			}
			finally
			{
				asyncExecutionContext.RequestContext.Metrics.StopEvent(Metric.HttpRequestTime);
				httpRequest.Dispose();
				base.InvokeAsyncCallback(asyncExecutionContext);
			}
		}

		private static void SetMetrics(IRequestContext requestContext)
		{
			requestContext.Metrics.AddProperty(Metric.ServiceName, requestContext.Request.ServiceName);
			requestContext.Metrics.AddProperty(Metric.ServiceEndpoint, requestContext.Request.Endpoint);
			requestContext.Metrics.AddProperty(Metric.MethodName, requestContext.Request.RequestName);
		}

		private void WriteContentToRequestBody(TRequestContent requestContent, IHttpRequest<TRequestContent> httpRequest, IRequestContext requestContext)
		{
			IRequest request = requestContext.Request;
			if (request.ContentStream == null)
			{
				byte[] content = request.Content;
				requestContext.Metrics.AddProperty(Metric.RequestSize, content.Length);
				httpRequest.WriteToRequestBody(requestContent, content, requestContext.Request.Headers);
				return;
			}
			Stream stream = request.ContentStream;
			EventHandler<StreamTransferProgressArgs> streamUploadProgressCallback = ((IAmazonWebServiceRequest)request.OriginalRequest).StreamUploadProgressCallback;
			if (streamUploadProgressCallback != null)
			{
				EventStream eventStream = new EventStream(stream, true);
				StreamReadTracker streamReadTracker = new StreamReadTracker(CallbackSender, streamUploadProgressCallback, stream.Length, requestContext.ClientConfig.ProgressUpdateInterval);
				eventStream.OnRead += streamReadTracker.ReadProgress;
				stream = eventStream;
			}
			Stream contentStream = ((!request.UseChunkEncoding || request.AWS4SignerResult == null) ? stream : new ChunkedUploadWrapperStream(stream, requestContext.ClientConfig.BufferSize, request.AWS4SignerResult));
			httpRequest.WriteToRequestBody(requestContent, contentStream, requestContext.Request.Headers, requestContext);
		}

		protected virtual IHttpRequest<TRequestContent> CreateWebRequest(IRequestContext requestContext)
		{
			IRequest request = requestContext.Request;
			Uri requestUri = AmazonServiceClient.ComposeUrl(request);
			IHttpRequest<TRequestContent> httpRequest = _requestFactory.CreateHttpRequest(requestUri);
			httpRequest.ConfigureRequest(requestContext);
			httpRequest.Method = request.HttpMethod;
			if (request.MayContainRequestBody())
			{
				byte[] array = request.Content;
				if (request.SetContentFromParameters || (array == null && request.ContentStream == null))
				{
					string parametersAsString = AWSSDKUtils.GetParametersAsString(request.Parameters);
					array = (request.Content = Encoding.UTF8.GetBytes(parametersAsString));
					request.SetContentFromParameters = true;
				}
				if (array != null)
				{
					request.Headers["Content-Length"] = array.Length.ToString(CultureInfo.InvariantCulture);
				}
				else if (request.ContentStream != null && !request.Headers.ContainsKey("Content-Length"))
				{
					request.Headers["Content-Length"] = request.ContentStream.Length.ToString(CultureInfo.InvariantCulture);
				}
			}
			else if (request.UseQueryString && (request.HttpMethod == "POST" || request.HttpMethod == "PUT"))
			{
				request.Content = new byte[0];
			}
			if (requestContext.Unmarshaller is JsonResponseUnmarshaller)
			{
				request.Headers["Accept"] = "application/json";
			}
			return httpRequest;
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
				if (_requestFactory != null)
				{
					_requestFactory.Dispose();
				}
				_disposed = true;
			}
		}
	}
}
