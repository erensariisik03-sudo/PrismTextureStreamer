using System;
using System.Net;
using System.Text;
using Amazon.Runtime.Internal.Transform;
using Amazon.Runtime.Internal.Util;

namespace Amazon.Runtime.Internal
{
	public class UnityHttpErrorResponseExceptionHandler<T> : ExceptionHandler<UnityHttpErrorResponseException> where T : AmazonServiceException, new()
	{
		public UnityHttpErrorResponseExceptionHandler(ILogger logger)
			: base(logger)
		{
		}

		public override bool HandleException(IExecutionContext executionContext, UnityHttpErrorResponseException exception)
		{
			IRequestContext requestContext = executionContext.RequestContext;
			IWebResponseData response = exception.Response;
			UnityWebResponseData unityWebResponseData = (UnityWebResponseData)response.ResponseBody;
			if (!unityWebResponseData.IsResponseBodyPresent)
			{
				LogCurlRequest(exception.Request);
				ErrorResponse errorResponse = new ErrorResponse();
				if (response.IsHeaderPresent("x-amz-request-id"))
				{
					errorResponse.RequestId = response.GetHeaderValue("x-amz-request-id");
					requestContext.Metrics.AddProperty(Metric.AWSRequestID, response.GetHeaderValue("x-amz-request-id"));
				}
				if (response.IsHeaderPresent("x-amzn-ErrorType"))
				{
					errorResponse.Code = response.GetHeaderValue("x-amzn-ErrorType");
				}
				if (response.IsHeaderPresent("x-amzn-ErrorMessage"))
				{
					errorResponse.Message = response.GetHeaderValue("x-amzn-ErrorMessage");
				}
				if (response.IsHeaderPresent("x-amz-id-2"))
				{
					requestContext.Metrics.AddProperty(Metric.AmzId2, response.GetHeaderValue("x-amz-id-2"));
				}
				requestContext.Metrics.AddProperty(Metric.StatusCode, response.StatusCode);
				Exception ex = null;
				ISimplifiedErrorUnmarshaller simplifiedErrorUnmarshaller = requestContext.Unmarshaller as ISimplifiedErrorUnmarshaller;
				if (simplifiedErrorUnmarshaller != null)
				{
					ex = simplifiedErrorUnmarshaller.UnmarshallException(response, errorResponse, exception);
					LogErrorMessage(ex, errorResponse);
					throw ex;
				}
				AmazonServiceException ex2 = new AmazonServiceException();
				ex2.RequestId = errorResponse.RequestId;
				ex2.ErrorCode = errorResponse.Code;
				ex2.StatusCode = response.StatusCode;
				throw ex2;
			}
			if (HandleSuppressed404(executionContext, response))
			{
				return false;
			}
			requestContext.Metrics.AddProperty(Metric.StatusCode, response.StatusCode);
			AmazonServiceException ex3 = null;
			try
			{
				using (response.ResponseBody)
				{
					ResponseUnmarshaller unmarshaller = requestContext.Unmarshaller;
					bool readEntireResponse = true;
					UnmarshallerContext unmarshallerContext = unmarshaller.CreateContext(response, readEntireResponse, response.ResponseBody.OpenResponse(), requestContext.Metrics);
					try
					{
						ex3 = unmarshaller.UnmarshallException(unmarshallerContext, exception, response.StatusCode);
					}
					catch (Exception ex4)
					{
						if (ex4 is AmazonServiceException || ex4 is AmazonClientException)
						{
							throw;
						}
						string headerValue = response.GetHeaderValue("x-amzn-RequestId");
						string responseBody = unmarshallerContext.ResponseBody;
						throw new AmazonUnmarshallingException(headerValue, null, responseBody, ex4);
					}
					requestContext.Metrics.AddProperty(Metric.AWSRequestID, ex3.RequestId);
					requestContext.Metrics.AddProperty(Metric.AWSErrorCode, ex3.ErrorCode);
					if (requestContext.ClientConfig.LogResponse || AWSConfigs.LoggingConfig.LogResponses != ResponseLoggingOption.Never)
					{
						base.Logger.Error(ex3, "Received error response: [{0}]", unmarshallerContext.ResponseBody);
					}
				}
			}
			catch (Exception exception2)
			{
				base.Logger.Error(exception2, "Failed to unmarshall a service error response.");
				throw;
			}
			throw ex3;
		}

		private void LogErrorMessage(Exception exception, ErrorResponse errorResponse)
		{
			if (!string.IsNullOrEmpty(errorResponse.Code))
			{
				if (!string.IsNullOrEmpty(errorResponse.Message))
				{
					base.Logger.Error(exception, "Recieved Exception of type {0}, Message {1}, request id {2}", errorResponse.Code, errorResponse.Message, errorResponse.RequestId);
				}
				else
				{
					base.Logger.Error(exception, "Recieved Exception of type {0}, request id {1}", errorResponse.Code, errorResponse.RequestId);
				}
			}
		}

		private void LogCurlRequest(UnityWebRequest request)
		{
			string text = "curl " + ((!(request.Method == "GET")) ? "-X POST " : "-G ");
			foreach (string key in request.Headers.Keys)
			{
				string text2 = text;
				text = text2 + " -H \"" + key + ": " + request.Headers[key] + "\" ";
			}
			if (request.RequestContent != null)
			{
				text = text + " -d '" + Encoding.Default.GetString(request.RequestContent) + "' ";
			}
			text = text + " " + request.RequestUri;
			base.Logger.DebugFormat("{0}", text);
		}

		private bool HandleSuppressed404(IExecutionContext executionContext, IWebResponseData httpErrorResponse)
		{
			IRequestContext requestContext = executionContext.RequestContext;
			IResponseContext responseContext = executionContext.ResponseContext;
			if (httpErrorResponse != null && httpErrorResponse.StatusCode == HttpStatusCode.NotFound && requestContext.Request.Suppress404Exceptions)
			{
				using (httpErrorResponse.ResponseBody)
				{
					ResponseUnmarshaller unmarshaller = requestContext.Unmarshaller;
					bool readEntireResponse = requestContext.ClientConfig.LogResponse || requestContext.ClientConfig.ReadEntireResponse || AWSConfigs.LoggingConfig.LogResponses != ResponseLoggingOption.Never;
					UnmarshallerContext input = unmarshaller.CreateContext(httpErrorResponse, readEntireResponse, httpErrorResponse.ResponseBody.OpenResponse(), requestContext.Metrics);
					try
					{
						responseContext.Response = unmarshaller.Unmarshall(input);
						responseContext.Response.ContentLength = httpErrorResponse.ContentLength;
						responseContext.Response.HttpStatusCode = httpErrorResponse.StatusCode;
						return true;
					}
					catch (Exception exception)
					{
						base.Logger.Debug(exception, "Failed to unmarshall 404 response when it was supressed.");
					}
				}
			}
			return false;
		}
	}
}
