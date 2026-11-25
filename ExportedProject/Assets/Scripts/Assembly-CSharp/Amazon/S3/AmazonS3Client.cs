using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using Amazon.Runtime;
using Amazon.Runtime.Internal;
using Amazon.Runtime.Internal.Auth;
using Amazon.Runtime.Internal.Transform;
using Amazon.S3.Internal;
using Amazon.S3.Model;
using Amazon.S3.Model.Internal.MarshallTransformations;
using Amazon.S3.Util;

namespace Amazon.S3
{
	public class AmazonS3Client : AmazonUnityServiceClient, IDisposable, IAmazonS3
	{
		public AmazonS3Client(AWSCredentials credentials)
			: this(credentials, new AmazonS3Config())
		{
		}

		public AmazonS3Client(AWSCredentials credentials, RegionEndpoint region)
			: this(credentials, new AmazonS3Config
			{
				RegionEndpoint = region
			})
		{
		}

		public AmazonS3Client(AWSCredentials credentials, AmazonS3Config clientConfig)
			: base(credentials, clientConfig)
		{
		}

		public AmazonS3Client(string awsAccessKeyId, string awsSecretAccessKey)
			: this(awsAccessKeyId, awsSecretAccessKey, new AmazonS3Config())
		{
		}

		public AmazonS3Client(string awsAccessKeyId, string awsSecretAccessKey, RegionEndpoint region)
			: this(awsAccessKeyId, awsSecretAccessKey, new AmazonS3Config
			{
				RegionEndpoint = region
			})
		{
		}

		public AmazonS3Client(string awsAccessKeyId, string awsSecretAccessKey, AmazonS3Config clientConfig)
			: base(awsAccessKeyId, awsSecretAccessKey, clientConfig)
		{
		}

		public AmazonS3Client(string awsAccessKeyId, string awsSecretAccessKey, string awsSessionToken)
			: this(awsAccessKeyId, awsSecretAccessKey, awsSessionToken, new AmazonS3Config())
		{
		}

		public AmazonS3Client(string awsAccessKeyId, string awsSecretAccessKey, string awsSessionToken, RegionEndpoint region)
			: this(awsAccessKeyId, awsSecretAccessKey, awsSessionToken, new AmazonS3Config
			{
				RegionEndpoint = region
			})
		{
		}

		public AmazonS3Client(string awsAccessKeyId, string awsSecretAccessKey, string awsSessionToken, AmazonS3Config clientConfig)
			: base(awsAccessKeyId, awsSecretAccessKey, awsSessionToken, clientConfig)
		{
		}

		public void PostObjectAsync(PostObjectRequest request, AmazonServiceCallback<PostObjectRequest, PostObjectResponse> callback, AsyncOptions options = null)
		{
			options = ((options != null) ? options : new AsyncOptions());
			Action<AmazonWebServiceRequest, AmazonWebServiceResponse, Exception, AsyncOptions> callbackHelper = (AmazonWebServiceRequest req, AmazonWebServiceResponse res, Exception ex, AsyncOptions ao) =>
			{
				AmazonServiceResult<PostObjectRequest, PostObjectResponse> responseObject = new AmazonServiceResult<PostObjectRequest, PostObjectResponse>((PostObjectRequest)req, (PostObjectResponse)res, ex, ao.State);
				if (callback != null)
				{
					callback(responseObject);
				}
			};
			ThreadPool.QueueUserWorkItem(delegate
			{
				if (request.SignedPolicy == null)
				{
					CreateSignedPolicy(request);
				}
				PostObject(request, options, callbackHelper);
			});
		}

		private void CreateSignedPolicy(PostObjectRequest request)
		{
			if (request.ContentType == null)
			{
				int num = request.Key.LastIndexOf('.');
				string text = null;
				if (num != -1)
				{
					text = request.Key.Substring(num, request.Key.Length - num);
					request.ContentType = AmazonS3Util.MimeTypeFromExtension(text);
				}
				else
				{
					request.ContentType = "application/octet-stream";
				}
			}
			string text2 = null;
			int num2 = request.Key.LastIndexOf('/');
			text2 = ((num2 != -1) ? ("{\"expiration\": \"" + DateTime.UtcNow.AddHours(24.0).ToString("yyyy-MM-ddTHH:mm:ssZ") + "\",\"conditions\": [{\"bucket\": \"" + request.Bucket + "\"},[\"starts-with\", \"$key\", \"" + request.Key.Substring(0, num2) + "/\"],{\"acl\": \"private\"},[\"eq\", \"$Content-Type\", \"" + request.ContentType + "\"]]}") : ("{\"expiration\": \"" + DateTime.UtcNow.AddHours(24.0).ToString("yyyy-MM-ddTHH:mm:ssZ") + "\",\"conditions\": [{\"bucket\": \"" + request.Bucket + "\"},[\"starts-with\", \"$key\", \"\"],{\"acl\": \"private\"},[\"eq\", \"$Content-Type\", \"" + request.ContentType + "\"]]}"));
			request.SignedPolicy = S3PostUploadSignedPolicy.GetSignedPolicy(text2, base.Credentials);
		}

		private void PostObject(PostObjectRequest request, AsyncOptions options, Action<AmazonWebServiceRequest, AmazonWebServiceResponse, Exception, AsyncOptions> callbackHelper)
		{
			string text = ((!request.Region.Equals(RegionEndpoint.USEast1)) ? ("s3-" + request.Region.SystemName) : "s3");
			IDictionary<string, string> dictionary = new Dictionary<string, string>();
			string uriString = ((request.Bucket.IndexOf('.') <= -1) ? string.Format(CultureInfo.InvariantCulture, "https://{0}.{1}.amazonaws.com", request.Bucket, text) : string.Format(CultureInfo.InvariantCulture, "https://{0}.amazonaws.com/{1}/", text, request.Bucket));
			Uri requestUri = new Uri(uriString);
			UnityWebRequest unityWebRequest = new UnityWebRequest(requestUri);
			string text2 = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Replace('=', 'z');
			dictionary["Content-Type"] = string.Format(CultureInfo.InvariantCulture, "multipart/form-data; boundary={0}", text2);
			dictionary["User-Agent"] = "User-Agent";
			unityWebRequest.Method = "POST";
			using (MemoryStream memoryStream = new MemoryStream())
			{
				request.WriteFormData(text2, memoryStream);
				byte[] bytes = Encoding.UTF8.GetBytes(string.Format(CultureInfo.InvariantCulture, "--{0}\r\nContent-Disposition: form-data; name=\"file\"\r\n\r\n", text2));
				memoryStream.Write(bytes, 0, bytes.Length);
				using (Stream stream = ((request.Path != null) ? File.OpenRead(request.Path) : request.InputStream))
				{
					byte[] buffer = new byte[1024];
					int count;
					while ((count = stream.Read(buffer, 0, 1024)) > 0)
					{
						memoryStream.Write(buffer, 0, count);
					}
				}
				byte[] bytes2 = Encoding.UTF8.GetBytes(string.Format(CultureInfo.InvariantCulture, "\r\n--{0}--", text2));
				memoryStream.Write(bytes2, 0, bytes2.Length);
				unityWebRequest.WriteToRequestBody(null, memoryStream.ToArray(), dictionary);
			}
			AsyncRequestContext asyncRequestContext = new AsyncRequestContext(base.Config.LogMetrics);
			asyncRequestContext.ClientConfig = base.Config;
			asyncRequestContext.OriginalRequest = request;
			asyncRequestContext.Action = callbackHelper;
			asyncRequestContext.AsyncOptions = options;
			asyncRequestContext.IsAsync = true;
			AsyncExecutionContext asyncExecutionContext = new AsyncExecutionContext(asyncRequestContext, new AsyncResponseContext());
			unityWebRequest.SetRequestHeaders(dictionary);
			asyncExecutionContext.RuntimeState = unityWebRequest;
			asyncExecutionContext.ResponseContext.AsyncResult = new RuntimeAsyncResult(asyncExecutionContext.RequestContext.Callback, asyncExecutionContext.RequestContext.State);
			asyncExecutionContext.ResponseContext.AsyncResult.AsyncOptions = asyncExecutionContext.RequestContext.AsyncOptions;
			asyncExecutionContext.ResponseContext.AsyncResult.Action = asyncExecutionContext.RequestContext.Action;
			asyncExecutionContext.ResponseContext.AsyncResult.Request = asyncExecutionContext.RequestContext.OriginalRequest;
			unityWebRequest.BeginGetResponse(ProcessPostResponse, asyncExecutionContext);
		}

		private void ProcessPostResponse(IAsyncResult result)
		{
			IAsyncExecutionContext asyncExecutionContext = null;
			UnityWebRequest unityWebRequest = null;
			try
			{
				asyncExecutionContext = result.AsyncState as IAsyncExecutionContext;
				unityWebRequest = asyncExecutionContext.RuntimeState as UnityWebRequest;
				IWebResponseData httpResponse = unityWebRequest.EndGetResponse(result);
				asyncExecutionContext.ResponseContext.HttpResponse = httpResponse;
			}
			catch (Exception exception)
			{
				asyncExecutionContext.ResponseContext.AsyncResult.Exception = exception;
			}
			finally
			{
				unityWebRequest.Dispose();
			}
			PostResponseHelper(result);
		}

		private void PostResponseHelper(IAsyncResult result)
		{
			IAsyncExecutionContext asyncExecutionContext = result.AsyncState as IAsyncExecutionContext;
			IWebResponseData httpResponse = asyncExecutionContext.ResponseContext.HttpResponse;
			PostObjectResponse postObjectResponse = new PostObjectResponse();
			postObjectResponse.HttpStatusCode = httpResponse.StatusCode;
			postObjectResponse.ContentLength = httpResponse.ContentLength;
			if (httpResponse.IsHeaderPresent("x-amz-request-id"))
			{
				postObjectResponse.RequestId = httpResponse.GetHeaderValue("x-amz-request-id");
			}
			if (httpResponse.IsHeaderPresent("x-amz-id-2"))
			{
				postObjectResponse.HostId = httpResponse.GetHeaderValue("x-amz-id-2");
			}
			PostObjectRequest request = asyncExecutionContext.RequestContext.OriginalRequest as PostObjectRequest;
			RuntimeAsyncResult asyncResult = asyncExecutionContext.ResponseContext.AsyncResult;
			asyncResult.Request = request;
			asyncResult.Response = postObjectResponse;
			asyncResult.Exception = asyncExecutionContext.ResponseContext.AsyncResult.Exception;
			asyncResult.Action = asyncExecutionContext.RequestContext.Action;
			asyncResult.InvokeCallback();
		}

		protected override AbstractAWSSigner CreateSigner()
		{
			return new S3Signer();
		}

		protected override void CustomizeRuntimePipeline(RuntimePipeline pipeline)
		{
			pipeline.ReplaceHandler<ErrorHandler>(new ErrorHandler<AmazonS3Exception>(base.Logger));
			pipeline.AddHandlerBefore<Marshaller>(new AmazonS3PreMarshallHandler());
			pipeline.AddHandlerAfter<Marshaller>(new AmazonS3PostMarshallHandler());
			pipeline.AddHandlerAfter<EndpointResolver>(new AmazonS3KmsHandler());
			pipeline.AddHandlerBefore<Unmarshaller>(new AmazonS3ResponseHandler());
			pipeline.AddHandlerAfter<ErrorCallbackHandler>(new AmazonS3ExceptionHandler());
			pipeline.AddHandlerAfter<Unmarshaller>(new AmazonS3RedirectHandler());
			pipeline.ReplaceHandler<RetryHandler>(new RetryHandler(new AmazonS3RetryPolicy(base.Config.MaxErrorRetry)));
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}

		public void DeleteObjectsAsync(DeleteObjectsRequest request, AmazonServiceCallback<DeleteObjectsRequest, DeleteObjectsResponse> callback, AsyncOptions options = null)
		{
			options = ((options != null) ? options : new AsyncOptions());
			DeleteObjectsRequestMarshaller marshaller = new DeleteObjectsRequestMarshaller();
			DeleteObjectsResponseUnmarshaller instance = DeleteObjectsResponseUnmarshaller.Instance;
			Action<AmazonWebServiceRequest, AmazonWebServiceResponse, Exception, AsyncOptions> callbackHelper = null;
			if (callback != null)
			{
				callbackHelper = (AmazonWebServiceRequest req, AmazonWebServiceResponse res, Exception ex, AsyncOptions ao) =>
				{
					AmazonServiceResult<DeleteObjectsRequest, DeleteObjectsResponse> responseObject = new AmazonServiceResult<DeleteObjectsRequest, DeleteObjectsResponse>((DeleteObjectsRequest)req, (DeleteObjectsResponse)res, ex, ao.State);
					callback(responseObject);
				};
			}
			BeginInvoke(request, marshaller, instance, options, callbackHelper);
		}

		public void GetACLAsync(string bucketName, AmazonServiceCallback<GetACLRequest, GetACLResponse> callback, AsyncOptions options = null)
		{
			GetACLRequest getACLRequest = new GetACLRequest();
			getACLRequest.BucketName = bucketName;
			GetACLAsync(getACLRequest, callback, options);
		}

		public void GetACLAsync(GetACLRequest request, AmazonServiceCallback<GetACLRequest, GetACLResponse> callback, AsyncOptions options = null)
		{
			options = ((options != null) ? options : new AsyncOptions());
			GetACLRequestMarshaller marshaller = new GetACLRequestMarshaller();
			GetACLResponseUnmarshaller instance = GetACLResponseUnmarshaller.Instance;
			Action<AmazonWebServiceRequest, AmazonWebServiceResponse, Exception, AsyncOptions> callbackHelper = null;
			if (callback != null)
			{
				callbackHelper = (AmazonWebServiceRequest req, AmazonWebServiceResponse res, Exception ex, AsyncOptions ao) =>
				{
					AmazonServiceResult<GetACLRequest, GetACLResponse> responseObject = new AmazonServiceResult<GetACLRequest, GetACLResponse>((GetACLRequest)req, (GetACLResponse)res, ex, ao.State);
					callback(responseObject);
				};
			}
			BeginInvoke(request, marshaller, instance, options, callbackHelper);
		}

		public void GetBucketLocationAsync(string bucketName, AmazonServiceCallback<GetBucketLocationRequest, GetBucketLocationResponse> callback, AsyncOptions options = null)
		{
			GetBucketLocationRequest getBucketLocationRequest = new GetBucketLocationRequest();
			getBucketLocationRequest.BucketName = bucketName;
			GetBucketLocationAsync(getBucketLocationRequest, callback, options);
		}

		public void GetBucketLocationAsync(GetBucketLocationRequest request, AmazonServiceCallback<GetBucketLocationRequest, GetBucketLocationResponse> callback, AsyncOptions options = null)
		{
			options = ((options != null) ? options : new AsyncOptions());
			GetBucketLocationRequestMarshaller marshaller = new GetBucketLocationRequestMarshaller();
			GetBucketLocationResponseUnmarshaller instance = GetBucketLocationResponseUnmarshaller.Instance;
			Action<AmazonWebServiceRequest, AmazonWebServiceResponse, Exception, AsyncOptions> callbackHelper = null;
			if (callback != null)
			{
				callbackHelper = (AmazonWebServiceRequest req, AmazonWebServiceResponse res, Exception ex, AsyncOptions ao) =>
				{
					AmazonServiceResult<GetBucketLocationRequest, GetBucketLocationResponse> responseObject = new AmazonServiceResult<GetBucketLocationRequest, GetBucketLocationResponse>((GetBucketLocationRequest)req, (GetBucketLocationResponse)res, ex, ao.State);
					callback(responseObject);
				};
			}
			BeginInvoke(request, marshaller, instance, options, callbackHelper);
		}

		public void GetBucketLoggingAsync(string bucketName, AmazonServiceCallback<GetBucketLoggingRequest, GetBucketLoggingResponse> callback, AsyncOptions options = null)
		{
			GetBucketLoggingRequest getBucketLoggingRequest = new GetBucketLoggingRequest();
			getBucketLoggingRequest.BucketName = bucketName;
			GetBucketLoggingAsync(getBucketLoggingRequest, callback, options);
		}

		public void GetBucketLoggingAsync(GetBucketLoggingRequest request, AmazonServiceCallback<GetBucketLoggingRequest, GetBucketLoggingResponse> callback, AsyncOptions options = null)
		{
			options = ((options != null) ? options : new AsyncOptions());
			GetBucketLoggingRequestMarshaller marshaller = new GetBucketLoggingRequestMarshaller();
			GetBucketLoggingResponseUnmarshaller instance = GetBucketLoggingResponseUnmarshaller.Instance;
			Action<AmazonWebServiceRequest, AmazonWebServiceResponse, Exception, AsyncOptions> callbackHelper = null;
			if (callback != null)
			{
				callbackHelper = (AmazonWebServiceRequest req, AmazonWebServiceResponse res, Exception ex, AsyncOptions ao) =>
				{
					AmazonServiceResult<GetBucketLoggingRequest, GetBucketLoggingResponse> responseObject = new AmazonServiceResult<GetBucketLoggingRequest, GetBucketLoggingResponse>((GetBucketLoggingRequest)req, (GetBucketLoggingResponse)res, ex, ao.State);
					callback(responseObject);
				};
			}
			BeginInvoke(request, marshaller, instance, options, callbackHelper);
		}

		public void GetBucketNotificationAsync(string bucketName, AmazonServiceCallback<GetBucketNotificationRequest, GetBucketNotificationResponse> callback, AsyncOptions options = null)
		{
			GetBucketNotificationRequest getBucketNotificationRequest = new GetBucketNotificationRequest();
			getBucketNotificationRequest.BucketName = bucketName;
			GetBucketNotificationAsync(getBucketNotificationRequest, callback, options);
		}

		public void GetBucketNotificationAsync(GetBucketNotificationRequest request, AmazonServiceCallback<GetBucketNotificationRequest, GetBucketNotificationResponse> callback, AsyncOptions options = null)
		{
			options = ((options != null) ? options : new AsyncOptions());
			GetBucketNotificationRequestMarshaller marshaller = new GetBucketNotificationRequestMarshaller();
			GetBucketNotificationResponseUnmarshaller instance = GetBucketNotificationResponseUnmarshaller.Instance;
			Action<AmazonWebServiceRequest, AmazonWebServiceResponse, Exception, AsyncOptions> callbackHelper = null;
			if (callback != null)
			{
				callbackHelper = (AmazonWebServiceRequest req, AmazonWebServiceResponse res, Exception ex, AsyncOptions ao) =>
				{
					AmazonServiceResult<GetBucketNotificationRequest, GetBucketNotificationResponse> responseObject = new AmazonServiceResult<GetBucketNotificationRequest, GetBucketNotificationResponse>((GetBucketNotificationRequest)req, (GetBucketNotificationResponse)res, ex, ao.State);
					callback(responseObject);
				};
			}
			BeginInvoke(request, marshaller, instance, options, callbackHelper);
		}

		public void GetBucketPolicyAsync(string bucketName, AmazonServiceCallback<GetBucketPolicyRequest, GetBucketPolicyResponse> callback, AsyncOptions options = null)
		{
			GetBucketPolicyRequest getBucketPolicyRequest = new GetBucketPolicyRequest();
			getBucketPolicyRequest.BucketName = bucketName;
			GetBucketPolicyAsync(getBucketPolicyRequest, callback, options);
		}

		public void GetBucketPolicyAsync(GetBucketPolicyRequest request, AmazonServiceCallback<GetBucketPolicyRequest, GetBucketPolicyResponse> callback, AsyncOptions options = null)
		{
			options = ((options != null) ? options : new AsyncOptions());
			GetBucketPolicyRequestMarshaller marshaller = new GetBucketPolicyRequestMarshaller();
			GetBucketPolicyResponseUnmarshaller instance = GetBucketPolicyResponseUnmarshaller.Instance;
			Action<AmazonWebServiceRequest, AmazonWebServiceResponse, Exception, AsyncOptions> callbackHelper = null;
			if (callback != null)
			{
				callbackHelper = (AmazonWebServiceRequest req, AmazonWebServiceResponse res, Exception ex, AsyncOptions ao) =>
				{
					AmazonServiceResult<GetBucketPolicyRequest, GetBucketPolicyResponse> responseObject = new AmazonServiceResult<GetBucketPolicyRequest, GetBucketPolicyResponse>((GetBucketPolicyRequest)req, (GetBucketPolicyResponse)res, ex, ao.State);
					callback(responseObject);
				};
			}
			BeginInvoke(request, marshaller, instance, options, callbackHelper);
		}

		public void GetBucketRequestPaymentAsync(string bucketName, AmazonServiceCallback<GetBucketRequestPaymentRequest, GetBucketRequestPaymentResponse> callback, AsyncOptions options = null)
		{
			GetBucketRequestPaymentRequest getBucketRequestPaymentRequest = new GetBucketRequestPaymentRequest();
			getBucketRequestPaymentRequest.BucketName = bucketName;
			GetBucketRequestPaymentAsync(getBucketRequestPaymentRequest, callback, options);
		}

		public void GetBucketRequestPaymentAsync(GetBucketRequestPaymentRequest request, AmazonServiceCallback<GetBucketRequestPaymentRequest, GetBucketRequestPaymentResponse> callback, AsyncOptions options = null)
		{
			options = ((options != null) ? options : new AsyncOptions());
			GetBucketRequestPaymentRequestMarshaller marshaller = new GetBucketRequestPaymentRequestMarshaller();
			GetBucketRequestPaymentResponseUnmarshaller instance = GetBucketRequestPaymentResponseUnmarshaller.Instance;
			Action<AmazonWebServiceRequest, AmazonWebServiceResponse, Exception, AsyncOptions> callbackHelper = null;
			if (callback != null)
			{
				callbackHelper = (AmazonWebServiceRequest req, AmazonWebServiceResponse res, Exception ex, AsyncOptions ao) =>
				{
					AmazonServiceResult<GetBucketRequestPaymentRequest, GetBucketRequestPaymentResponse> responseObject = new AmazonServiceResult<GetBucketRequestPaymentRequest, GetBucketRequestPaymentResponse>((GetBucketRequestPaymentRequest)req, (GetBucketRequestPaymentResponse)res, ex, ao.State);
					callback(responseObject);
				};
			}
			BeginInvoke(request, marshaller, instance, options, callbackHelper);
		}

		public void GetBucketTaggingAsync(GetBucketTaggingRequest request, AmazonServiceCallback<GetBucketTaggingRequest, GetBucketTaggingResponse> callback, AsyncOptions options = null)
		{
			options = ((options != null) ? options : new AsyncOptions());
			GetBucketTaggingRequestMarshaller marshaller = new GetBucketTaggingRequestMarshaller();
			GetBucketTaggingResponseUnmarshaller instance = GetBucketTaggingResponseUnmarshaller.Instance;
			Action<AmazonWebServiceRequest, AmazonWebServiceResponse, Exception, AsyncOptions> callbackHelper = null;
			if (callback != null)
			{
				callbackHelper = (AmazonWebServiceRequest req, AmazonWebServiceResponse res, Exception ex, AsyncOptions ao) =>
				{
					AmazonServiceResult<GetBucketTaggingRequest, GetBucketTaggingResponse> responseObject = new AmazonServiceResult<GetBucketTaggingRequest, GetBucketTaggingResponse>((GetBucketTaggingRequest)req, (GetBucketTaggingResponse)res, ex, ao.State);
					callback(responseObject);
				};
			}
			BeginInvoke(request, marshaller, instance, options, callbackHelper);
		}

		public void GetBucketVersioningAsync(string bucketName, AmazonServiceCallback<GetBucketVersioningRequest, GetBucketVersioningResponse> callback, AsyncOptions options = null)
		{
			GetBucketVersioningRequest getBucketVersioningRequest = new GetBucketVersioningRequest();
			getBucketVersioningRequest.BucketName = bucketName;
			GetBucketVersioningAsync(getBucketVersioningRequest, callback, options);
		}

		public void GetBucketVersioningAsync(GetBucketVersioningRequest request, AmazonServiceCallback<GetBucketVersioningRequest, GetBucketVersioningResponse> callback, AsyncOptions options = null)
		{
			options = ((options != null) ? options : new AsyncOptions());
			GetBucketVersioningRequestMarshaller marshaller = new GetBucketVersioningRequestMarshaller();
			GetBucketVersioningResponseUnmarshaller instance = GetBucketVersioningResponseUnmarshaller.Instance;
			Action<AmazonWebServiceRequest, AmazonWebServiceResponse, Exception, AsyncOptions> callbackHelper = null;
			if (callback != null)
			{
				callbackHelper = (AmazonWebServiceRequest req, AmazonWebServiceResponse res, Exception ex, AsyncOptions ao) =>
				{
					AmazonServiceResult<GetBucketVersioningRequest, GetBucketVersioningResponse> responseObject = new AmazonServiceResult<GetBucketVersioningRequest, GetBucketVersioningResponse>((GetBucketVersioningRequest)req, (GetBucketVersioningResponse)res, ex, ao.State);
					callback(responseObject);
				};
			}
			BeginInvoke(request, marshaller, instance, options, callbackHelper);
		}

		public void GetBucketWebsiteAsync(string bucketName, AmazonServiceCallback<GetBucketWebsiteRequest, GetBucketWebsiteResponse> callback, AsyncOptions options = null)
		{
			GetBucketWebsiteRequest getBucketWebsiteRequest = new GetBucketWebsiteRequest();
			getBucketWebsiteRequest.BucketName = bucketName;
			GetBucketWebsiteAsync(getBucketWebsiteRequest, callback, options);
		}

		public void GetBucketWebsiteAsync(GetBucketWebsiteRequest request, AmazonServiceCallback<GetBucketWebsiteRequest, GetBucketWebsiteResponse> callback, AsyncOptions options = null)
		{
			options = ((options != null) ? options : new AsyncOptions());
			GetBucketWebsiteRequestMarshaller marshaller = new GetBucketWebsiteRequestMarshaller();
			GetBucketWebsiteResponseUnmarshaller instance = GetBucketWebsiteResponseUnmarshaller.Instance;
			Action<AmazonWebServiceRequest, AmazonWebServiceResponse, Exception, AsyncOptions> callbackHelper = null;
			if (callback != null)
			{
				callbackHelper = (AmazonWebServiceRequest req, AmazonWebServiceResponse res, Exception ex, AsyncOptions ao) =>
				{
					AmazonServiceResult<GetBucketWebsiteRequest, GetBucketWebsiteResponse> responseObject = new AmazonServiceResult<GetBucketWebsiteRequest, GetBucketWebsiteResponse>((GetBucketWebsiteRequest)req, (GetBucketWebsiteResponse)res, ex, ao.State);
					callback(responseObject);
				};
			}
			BeginInvoke(request, marshaller, instance, options, callbackHelper);
		}

		public void GetCORSConfigurationAsync(string bucketName, AmazonServiceCallback<GetCORSConfigurationRequest, GetCORSConfigurationResponse> callback, AsyncOptions options = null)
		{
			GetCORSConfigurationRequest getCORSConfigurationRequest = new GetCORSConfigurationRequest();
			getCORSConfigurationRequest.BucketName = bucketName;
			GetCORSConfigurationAsync(getCORSConfigurationRequest, callback, options);
		}

		public void GetCORSConfigurationAsync(GetCORSConfigurationRequest request, AmazonServiceCallback<GetCORSConfigurationRequest, GetCORSConfigurationResponse> callback, AsyncOptions options = null)
		{
			options = ((options != null) ? options : new AsyncOptions());
			GetCORSConfigurationRequestMarshaller marshaller = new GetCORSConfigurationRequestMarshaller();
			GetCORSConfigurationResponseUnmarshaller instance = GetCORSConfigurationResponseUnmarshaller.Instance;
			Action<AmazonWebServiceRequest, AmazonWebServiceResponse, Exception, AsyncOptions> callbackHelper = null;
			if (callback != null)
			{
				callbackHelper = (AmazonWebServiceRequest req, AmazonWebServiceResponse res, Exception ex, AsyncOptions ao) =>
				{
					AmazonServiceResult<GetCORSConfigurationRequest, GetCORSConfigurationResponse> responseObject = new AmazonServiceResult<GetCORSConfigurationRequest, GetCORSConfigurationResponse>((GetCORSConfigurationRequest)req, (GetCORSConfigurationResponse)res, ex, ao.State);
					callback(responseObject);
				};
			}
			BeginInvoke(request, marshaller, instance, options, callbackHelper);
		}

		public void GetLifecycleConfigurationAsync(string bucketName, AmazonServiceCallback<GetLifecycleConfigurationRequest, GetLifecycleConfigurationResponse> callback, AsyncOptions options = null)
		{
			GetLifecycleConfigurationRequest getLifecycleConfigurationRequest = new GetLifecycleConfigurationRequest();
			getLifecycleConfigurationRequest.BucketName = bucketName;
			GetLifecycleConfigurationAsync(getLifecycleConfigurationRequest, callback, options);
		}

		public void GetLifecycleConfigurationAsync(GetLifecycleConfigurationRequest request, AmazonServiceCallback<GetLifecycleConfigurationRequest, GetLifecycleConfigurationResponse> callback, AsyncOptions options = null)
		{
			options = ((options != null) ? options : new AsyncOptions());
			GetLifecycleConfigurationRequestMarshaller marshaller = new GetLifecycleConfigurationRequestMarshaller();
			GetLifecycleConfigurationResponseUnmarshaller instance = GetLifecycleConfigurationResponseUnmarshaller.Instance;
			Action<AmazonWebServiceRequest, AmazonWebServiceResponse, Exception, AsyncOptions> callbackHelper = null;
			if (callback != null)
			{
				callbackHelper = (AmazonWebServiceRequest req, AmazonWebServiceResponse res, Exception ex, AsyncOptions ao) =>
				{
					AmazonServiceResult<GetLifecycleConfigurationRequest, GetLifecycleConfigurationResponse> responseObject = new AmazonServiceResult<GetLifecycleConfigurationRequest, GetLifecycleConfigurationResponse>((GetLifecycleConfigurationRequest)req, (GetLifecycleConfigurationResponse)res, ex, ao.State);
					callback(responseObject);
				};
			}
			BeginInvoke(request, marshaller, instance, options, callbackHelper);
		}

		public void GetObjectAsync(string bucketName, string key, AmazonServiceCallback<GetObjectRequest, GetObjectResponse> callback, AsyncOptions options = null)
		{
			GetObjectRequest getObjectRequest = new GetObjectRequest();
			getObjectRequest.BucketName = bucketName;
			getObjectRequest.Key = key;
			GetObjectAsync(getObjectRequest, callback, options);
		}

		public void GetObjectAsync(string bucketName, string key, string versionId, AmazonServiceCallback<GetObjectRequest, GetObjectResponse> callback, AsyncOptions options = null)
		{
			GetObjectRequest getObjectRequest = new GetObjectRequest();
			getObjectRequest.BucketName = bucketName;
			getObjectRequest.Key = key;
			getObjectRequest.VersionId = versionId;
			GetObjectAsync(getObjectRequest, callback, options);
		}

		public void GetObjectAsync(GetObjectRequest request, AmazonServiceCallback<GetObjectRequest, GetObjectResponse> callback, AsyncOptions options = null)
		{
			options = ((options != null) ? options : new AsyncOptions());
			GetObjectRequestMarshaller marshaller = new GetObjectRequestMarshaller();
			GetObjectResponseUnmarshaller instance = GetObjectResponseUnmarshaller.Instance;
			Action<AmazonWebServiceRequest, AmazonWebServiceResponse, Exception, AsyncOptions> callbackHelper = null;
			if (callback != null)
			{
				callbackHelper = (AmazonWebServiceRequest req, AmazonWebServiceResponse res, Exception ex, AsyncOptions ao) =>
				{
					AmazonServiceResult<GetObjectRequest, GetObjectResponse> responseObject = new AmazonServiceResult<GetObjectRequest, GetObjectResponse>((GetObjectRequest)req, (GetObjectResponse)res, ex, ao.State);
					callback(responseObject);
				};
			}
			BeginInvoke(request, marshaller, instance, options, callbackHelper);
		}

		public void GetObjectTorrentAsync(string bucketName, string key, AmazonServiceCallback<GetObjectTorrentRequest, GetObjectTorrentResponse> callback, AsyncOptions options = null)
		{
			GetObjectTorrentRequest getObjectTorrentRequest = new GetObjectTorrentRequest();
			getObjectTorrentRequest.BucketName = bucketName;
			getObjectTorrentRequest.Key = key;
			GetObjectTorrentAsync(getObjectTorrentRequest, callback, options);
		}

		public void GetObjectTorrentAsync(GetObjectTorrentRequest request, AmazonServiceCallback<GetObjectTorrentRequest, GetObjectTorrentResponse> callback, AsyncOptions options = null)
		{
			options = ((options != null) ? options : new AsyncOptions());
			GetObjectTorrentRequestMarshaller marshaller = new GetObjectTorrentRequestMarshaller();
			GetObjectTorrentResponseUnmarshaller instance = GetObjectTorrentResponseUnmarshaller.Instance;
			Action<AmazonWebServiceRequest, AmazonWebServiceResponse, Exception, AsyncOptions> callbackHelper = null;
			if (callback != null)
			{
				callbackHelper = (AmazonWebServiceRequest req, AmazonWebServiceResponse res, Exception ex, AsyncOptions ao) =>
				{
					AmazonServiceResult<GetObjectTorrentRequest, GetObjectTorrentResponse> responseObject = new AmazonServiceResult<GetObjectTorrentRequest, GetObjectTorrentResponse>((GetObjectTorrentRequest)req, (GetObjectTorrentResponse)res, ex, ao.State);
					callback(responseObject);
				};
			}
			BeginInvoke(request, marshaller, instance, options, callbackHelper);
		}

		public void ListBucketsAsync(AmazonServiceCallback<ListBucketsRequest, ListBucketsResponse> callback, AsyncOptions options = null)
		{
			ListBucketsAsync(new ListBucketsRequest(), callback, options);
		}

		public void ListBucketsAsync(ListBucketsRequest request, AmazonServiceCallback<ListBucketsRequest, ListBucketsResponse> callback, AsyncOptions options = null)
		{
			options = ((options != null) ? options : new AsyncOptions());
			ListBucketsRequestMarshaller marshaller = new ListBucketsRequestMarshaller();
			ListBucketsResponseUnmarshaller instance = ListBucketsResponseUnmarshaller.Instance;
			Action<AmazonWebServiceRequest, AmazonWebServiceResponse, Exception, AsyncOptions> callbackHelper = null;
			if (callback != null)
			{
				callbackHelper = (AmazonWebServiceRequest req, AmazonWebServiceResponse res, Exception ex, AsyncOptions ao) =>
				{
					AmazonServiceResult<ListBucketsRequest, ListBucketsResponse> responseObject = new AmazonServiceResult<ListBucketsRequest, ListBucketsResponse>((ListBucketsRequest)req, (ListBucketsResponse)res, ex, ao.State);
					callback(responseObject);
				};
			}
			BeginInvoke(request, marshaller, instance, options, callbackHelper);
		}

		public void ListObjectsAsync(string bucketName, AmazonServiceCallback<ListObjectsRequest, ListObjectsResponse> callback, AsyncOptions options = null)
		{
			ListObjectsRequest listObjectsRequest = new ListObjectsRequest();
			listObjectsRequest.BucketName = bucketName;
			ListObjectsAsync(listObjectsRequest, callback, options);
		}

		public void ListObjectsAsync(string bucketName, string prefix, AmazonServiceCallback<ListObjectsRequest, ListObjectsResponse> callback, AsyncOptions options = null)
		{
			ListObjectsRequest listObjectsRequest = new ListObjectsRequest();
			listObjectsRequest.BucketName = bucketName;
			listObjectsRequest.Prefix = prefix;
			ListObjectsAsync(listObjectsRequest, callback, options);
		}

		public void ListObjectsAsync(ListObjectsRequest request, AmazonServiceCallback<ListObjectsRequest, ListObjectsResponse> callback, AsyncOptions options = null)
		{
			options = ((options != null) ? options : new AsyncOptions());
			ListObjectsRequestMarshaller marshaller = new ListObjectsRequestMarshaller();
			ListObjectsResponseUnmarshaller instance = ListObjectsResponseUnmarshaller.Instance;
			Action<AmazonWebServiceRequest, AmazonWebServiceResponse, Exception, AsyncOptions> callbackHelper = null;
			if (callback != null)
			{
				callbackHelper = (AmazonWebServiceRequest req, AmazonWebServiceResponse res, Exception ex, AsyncOptions ao) =>
				{
					AmazonServiceResult<ListObjectsRequest, ListObjectsResponse> responseObject = new AmazonServiceResult<ListObjectsRequest, ListObjectsResponse>((ListObjectsRequest)req, (ListObjectsResponse)res, ex, ao.State);
					callback(responseObject);
				};
			}
			BeginInvoke(request, marshaller, instance, options, callbackHelper);
		}

		public void ListVersionsAsync(string bucketName, AmazonServiceCallback<ListVersionsRequest, ListVersionsResponse> callback, AsyncOptions options = null)
		{
			ListVersionsRequest listVersionsRequest = new ListVersionsRequest();
			listVersionsRequest.BucketName = bucketName;
			ListVersionsAsync(listVersionsRequest, callback, options);
		}

		public void ListVersionsAsync(string bucketName, string prefix, AmazonServiceCallback<ListVersionsRequest, ListVersionsResponse> callback, AsyncOptions options = null)
		{
			ListVersionsRequest listVersionsRequest = new ListVersionsRequest();
			listVersionsRequest.BucketName = bucketName;
			listVersionsRequest.Prefix = prefix;
			ListVersionsAsync(listVersionsRequest, callback, options);
		}

		public void ListVersionsAsync(ListVersionsRequest request, AmazonServiceCallback<ListVersionsRequest, ListVersionsResponse> callback, AsyncOptions options = null)
		{
			options = ((options != null) ? options : new AsyncOptions());
			ListVersionsRequestMarshaller marshaller = new ListVersionsRequestMarshaller();
			ListVersionsResponseUnmarshaller instance = ListVersionsResponseUnmarshaller.Instance;
			Action<AmazonWebServiceRequest, AmazonWebServiceResponse, Exception, AsyncOptions> callbackHelper = null;
			if (callback != null)
			{
				callbackHelper = (AmazonWebServiceRequest req, AmazonWebServiceResponse res, Exception ex, AsyncOptions ao) =>
				{
					AmazonServiceResult<ListVersionsRequest, ListVersionsResponse> responseObject = new AmazonServiceResult<ListVersionsRequest, ListVersionsResponse>((ListVersionsRequest)req, (ListVersionsResponse)res, ex, ao.State);
					callback(responseObject);
				};
			}
			BeginInvoke(request, marshaller, instance, options, callbackHelper);
		}

		public void RestoreObjectAsync(string bucketName, string key, AmazonServiceCallback<RestoreObjectRequest, RestoreObjectResponse> callback, AsyncOptions options = null)
		{
			RestoreObjectRequest restoreObjectRequest = new RestoreObjectRequest();
			restoreObjectRequest.BucketName = bucketName;
			restoreObjectRequest.Key = key;
			RestoreObjectAsync(restoreObjectRequest, callback, options);
		}

		public void RestoreObjectAsync(string bucketName, string key, int days, AmazonServiceCallback<RestoreObjectRequest, RestoreObjectResponse> callback, AsyncOptions options = null)
		{
			RestoreObjectRequest restoreObjectRequest = new RestoreObjectRequest();
			restoreObjectRequest.BucketName = bucketName;
			restoreObjectRequest.Key = key;
			restoreObjectRequest.Days = days;
			RestoreObjectAsync(restoreObjectRequest, callback, options);
		}

		public void RestoreObjectAsync(string bucketName, string key, string versionId, AmazonServiceCallback<RestoreObjectRequest, RestoreObjectResponse> callback, AsyncOptions options = null)
		{
			RestoreObjectRequest restoreObjectRequest = new RestoreObjectRequest();
			restoreObjectRequest.BucketName = bucketName;
			restoreObjectRequest.Key = key;
			restoreObjectRequest.VersionId = versionId;
			RestoreObjectAsync(restoreObjectRequest, callback, options);
		}

		public void RestoreObjectAsync(string bucketName, string key, string versionId, int days, AmazonServiceCallback<RestoreObjectRequest, RestoreObjectResponse> callback, AsyncOptions options = null)
		{
			RestoreObjectRequest restoreObjectRequest = new RestoreObjectRequest();
			restoreObjectRequest.BucketName = bucketName;
			restoreObjectRequest.Key = key;
			restoreObjectRequest.VersionId = versionId;
			restoreObjectRequest.Days = days;
			RestoreObjectAsync(restoreObjectRequest, callback, options);
		}

		public void RestoreObjectAsync(RestoreObjectRequest request, AmazonServiceCallback<RestoreObjectRequest, RestoreObjectResponse> callback, AsyncOptions options = null)
		{
			options = ((options != null) ? options : new AsyncOptions());
			RestoreObjectRequestMarshaller marshaller = new RestoreObjectRequestMarshaller();
			RestoreObjectResponseUnmarshaller instance = RestoreObjectResponseUnmarshaller.Instance;
			Action<AmazonWebServiceRequest, AmazonWebServiceResponse, Exception, AsyncOptions> callbackHelper = null;
			if (callback != null)
			{
				callbackHelper = (AmazonWebServiceRequest req, AmazonWebServiceResponse res, Exception ex, AsyncOptions ao) =>
				{
					AmazonServiceResult<RestoreObjectRequest, RestoreObjectResponse> responseObject = new AmazonServiceResult<RestoreObjectRequest, RestoreObjectResponse>((RestoreObjectRequest)req, (RestoreObjectResponse)res, ex, ao.State);
					callback(responseObject);
				};
			}
			BeginInvoke(request, marshaller, instance, options, callbackHelper);
		}
	}
}
