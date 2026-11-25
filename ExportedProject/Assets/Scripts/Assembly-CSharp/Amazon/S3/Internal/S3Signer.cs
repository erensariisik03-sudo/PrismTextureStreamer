using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amazon.Runtime;
using Amazon.Runtime.Internal;
using Amazon.Runtime.Internal.Auth;
using Amazon.Runtime.Internal.Util;
using Amazon.Util;

namespace Amazon.S3.Internal
{
	public class S3Signer : AbstractAWSSigner
	{
		private readonly bool _useSigV4;

		private static readonly HashSet<string> SignableParameters = new HashSet<string>(new string[6] { "response-content-type", "response-content-language", "response-expires", "response-cache-control", "response-content-disposition", "response-content-encoding" }, StringComparer.OrdinalIgnoreCase);

		public override ClientProtocol Protocol
		{
			get
			{
				return ClientProtocol.RestProtocol;
			}
		}

		public S3Signer()
		{
			_useSigV4 = AWSConfigsS3.UseSignatureVersion4;
		}

		public override void Sign(IRequest request, ClientConfig clientConfig, RequestMetrics metrics, string awsAccessKeyId, string awsSecretAccessKey)
		{
			AbstractAWSSigner abstractAWSSigner = SelectSigner(this, _useSigV4, request, clientConfig);
			AWS4Signer aWS4Signer = abstractAWSSigner as AWS4Signer;
			if (aWS4Signer != null)
			{
				AWS4SigningResult aWS4SigningResult = aWS4Signer.SignRequest(request, clientConfig, metrics, awsAccessKeyId, awsSecretAccessKey);
				request.Headers["Authorization"] = aWS4SigningResult.ForAuthorizationHeader;
				if (request.UseChunkEncoding)
				{
					request.AWS4SignerResult = aWS4SigningResult;
				}
			}
			else
			{
				SignRequest(request, metrics, awsAccessKeyId, awsSecretAccessKey);
			}
		}

		internal static void SignRequest(IRequest request, RequestMetrics metrics, string awsAccessKeyId, string awsSecretAccessKey)
		{
			request.Headers["X-Amz-Date"] = AWSSDKUtils.FormattedCurrentTimestampRFC822;
			string text = BuildStringToSign(request);
			metrics.AddProperty(Metric.StringToSign, text);
			string text2 = CryptoUtilFactory.CryptoInstance.HMACSign(text, awsSecretAccessKey, SigningAlgorithm.HmacSHA1);
			string value = "AWS " + awsAccessKeyId + ":" + text2;
			request.Headers["Authorization"] = value;
		}

		private static string BuildStringToSign(IRequest request)
		{
			StringBuilder stringBuilder = new StringBuilder(string.Empty, 256);
			stringBuilder.Append(request.HttpMethod);
			stringBuilder.Append("\n");
			IDictionary<string, string> headers = request.Headers;
			IDictionary<string, string> parameters = request.Parameters;
			if (headers != null)
			{
				string text = null;
				if (headers.ContainsKey("Content-MD5") && !string.IsNullOrEmpty(text = headers["Content-MD5"]))
				{
					stringBuilder.Append(text);
				}
				stringBuilder.Append("\n");
				if (parameters.ContainsKey("ContentType"))
				{
					stringBuilder.Append(parameters["ContentType"]);
				}
				else if (headers.ContainsKey("Content-Type"))
				{
					stringBuilder.Append(headers["Content-Type"]);
				}
				stringBuilder.Append("\n");
			}
			else
			{
				stringBuilder.Append("\n\n");
			}
			if (parameters.ContainsKey("Expires"))
			{
				stringBuilder.Append(parameters["Expires"]);
				if (headers != null)
				{
					headers.Remove("X-Amz-Date");
				}
			}
			stringBuilder.Append("\n");
			stringBuilder.Append(BuildCanonicalizedHeaders(headers));
			string value = BuildCanonicalizedResource(request);
			if (!string.IsNullOrEmpty(value))
			{
				stringBuilder.Append(value);
			}
			return stringBuilder.ToString();
		}

		private static string BuildCanonicalizedHeaders(IDictionary<string, string> headers)
		{
			StringBuilder stringBuilder = new StringBuilder(256);
			foreach (string item in headers.Keys.OrderBy((string x) => x, StringComparer.OrdinalIgnoreCase))
			{
				string text = item.ToLowerInvariant();
				if (text.StartsWith("x-amz-", StringComparison.Ordinal))
				{
					stringBuilder.Append(text + ":" + headers[item] + "\n");
				}
			}
			return stringBuilder.ToString();
		}

		private static string BuildCanonicalizedResource(IRequest request)
		{
			StringBuilder stringBuilder = new StringBuilder(request.CanonicalResourcePrefix);
			stringBuilder.Append(string.IsNullOrEmpty(request.ResourcePath) ? "/" : AWSSDKUtils.UrlEncode(request.ResourcePath, true));
			Dictionary<string, string> dictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
			if (request.SubResources.Count > 0)
			{
				foreach (KeyValuePair<string, string> subResource in request.SubResources)
				{
					dictionary.Add(subResource.Key, subResource.Value);
				}
			}
			if (request.Parameters.Count > 0)
			{
				List<KeyValuePair<string, string>> list = request.Parameters.Where((KeyValuePair<string, string> kvp) => SignableParameters.Contains(kvp.Key) && kvp.Value != null).ToList();
				foreach (KeyValuePair<string, string> item in list)
				{
					dictionary.Add(item.Key, item.Value);
				}
			}
			string arg = "?";
			foreach (KeyValuePair<string, string> item2 in dictionary.OrderBy((KeyValuePair<string, string> r) => r.Key))
			{
				stringBuilder.AppendFormat("{0}{1}", arg, item2.Key);
				if (item2.Value != null)
				{
					stringBuilder.AppendFormat("={0}", item2.Value);
				}
				arg = "&";
			}
			return stringBuilder.ToString();
		}
	}
}
