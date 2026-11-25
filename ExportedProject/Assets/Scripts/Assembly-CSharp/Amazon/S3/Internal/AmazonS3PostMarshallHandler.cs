using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Amazon.Runtime;
using Amazon.Runtime.Internal;

namespace Amazon.S3.Internal
{
	public class AmazonS3PostMarshallHandler : PipelineHandler
	{
		private static HashSet<string> sseKeyHeaders = new HashSet<string> { "x-amz-server-side-encryption-customer-key", "x-amz-server-side-encryption-aws-kms-key-id" };

		private static char[] separators = new char[2] { '/', '?' };

		private static Regex bucketValidationRegex = new Regex("^[A-Za-z0-9._\\-]+$");

		private static Regex dnsValidationRegex1 = new Regex("^[a-z0-9][a-z0-9.-]+[a-z0-9]$");

		private static Regex dnsValidationRegex2 = new Regex("(\\d+\\.){3}\\d+");

		private static string[] invalidPatterns = new string[3] { "..", "-.", ".-" };

		public override void InvokeSync(IExecutionContext executionContext)
		{
			PreInvoke(executionContext);
			base.InvokeSync(executionContext);
		}

		public override IAsyncResult InvokeAsync(IAsyncExecutionContext executionContext)
		{
			PreInvoke(ExecutionContext.CreateFromAsyncContext(executionContext));
			return base.InvokeAsync(executionContext);
		}

		protected virtual void PreInvoke(IExecutionContext executionContext)
		{
			ProcessRequestHandlers(executionContext);
		}

		public static void ProcessRequestHandlers(IExecutionContext executionContext)
		{
			IRequest request = executionContext.RequestContext.Request;
			ClientConfig clientConfig = executionContext.RequestContext.ClientConfig;
			string value;
			if (request.Headers.TryGetValue("x-amz-server-side-encryption", out value) && string.Equals(value, ServerSideEncryptionMethod.AWSKMS.Value, StringComparison.Ordinal))
			{
				request.UseSigV4 = true;
			}
			string bucketName = GetBucketName(request.ResourcePath);
			if (string.IsNullOrEmpty(bucketName))
			{
				return;
			}
			AmazonS3Config amazonS3Config = clientConfig as AmazonS3Config;
			if (amazonS3Config == null)
			{
				throw new AmazonClientException("Current config object is not of type AmazonS3Config");
			}
			bool flag = IsDnsCompatibleBucketName(bucketName);
			UriBuilder uriBuilder = new UriBuilder(EndpointResolver.DetermineEndpoint(amazonS3Config, request));
			bool flag2 = string.Equals(uriBuilder.Scheme, "http", StringComparison.OrdinalIgnoreCase);
			if (!amazonS3Config.ForcePathStyle && flag && (flag2 || bucketName.IndexOf('.') < 0))
			{
				uriBuilder.Host = bucketName + "." + uriBuilder.Host;
				request.Endpoint = uriBuilder.Uri;
				string text = request.ResourcePath;
				string text2 = "/" + bucketName;
				if (text.IndexOf(text2, StringComparison.Ordinal) == 0)
				{
					text = text.Substring(text2.Length);
				}
				request.ResourcePath = text;
				request.CanonicalResourcePrefix = text2;
			}
			if (flag2)
			{
				ValidateHttpsOnlyHeaders(request);
			}
		}

		private static void ValidateHttpsOnlyHeaders(IRequest request)
		{
			ValidateSseKeyHeaders(request);
			ValidateSseHeaderValue(request);
		}

		private static void ValidateSseHeaderValue(IRequest request)
		{
			string value;
			if (request.Headers.TryGetValue("x-amz-server-side-encryption", out value) && string.Equals(value, ServerSideEncryptionMethod.AWSKMS))
			{
				throw new AmazonClientException("Request specifying Server Side Encryption with AWS KMS managed keys can only be transmitted over HTTPS");
			}
		}

		private static void ValidateSseKeyHeaders(IRequest request)
		{
			string[] array = (from kvp in request.Headers
				where !string.IsNullOrEmpty(kvp.Value) && sseKeyHeaders.Contains(kvp.Key)
				select kvp.Key).ToArray();
			if (array.Length > 0)
			{
				string message = string.Format(CultureInfo.InvariantCulture, "Request contains headers which can only be transmitted over HTTPS: {0}", string.Join(", ", array));
				throw new AmazonClientException(message);
			}
		}

		internal static string GetBucketName(string resourcePath)
		{
			resourcePath = resourcePath.Trim().Trim(separators);
			string[] array = resourcePath.Split(separators, 2);
			return array[0];
		}

		public static bool IsValidBucketName(string bucketName)
		{
			if (string.IsNullOrEmpty(bucketName))
			{
				return false;
			}
			if (bucketName.Length < 3 || bucketName.Length > 255)
			{
				return false;
			}
			if (bucketName.IndexOf('\n') >= 0)
			{
				return false;
			}
			if (!bucketValidationRegex.IsMatch(bucketName))
			{
				return false;
			}
			return true;
		}

		public static bool IsDnsCompatibleBucketName(string bucketName)
		{
			if (!IsValidBucketName(bucketName))
			{
				return false;
			}
			if (bucketName.Length > 63)
			{
				return false;
			}
			if (!dnsValidationRegex1.IsMatch(bucketName))
			{
				return false;
			}
			if (dnsValidationRegex2.IsMatch(bucketName))
			{
				return false;
			}
			if (StringContainsAny(bucketName, invalidPatterns, StringComparison.Ordinal))
			{
				return false;
			}
			return true;
		}

		private static bool StringContainsAny(string toCheck, string[] values, StringComparison stringComparison)
		{
			foreach (string value in values)
			{
				if (toCheck.IndexOf(value, stringComparison) >= 0)
				{
					return true;
				}
			}
			return false;
		}
	}
}
