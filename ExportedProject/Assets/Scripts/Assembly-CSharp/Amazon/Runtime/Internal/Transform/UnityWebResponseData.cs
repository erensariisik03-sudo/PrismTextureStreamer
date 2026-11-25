using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Amazon.Runtime.Internal.Util;
using UnityEngine;

namespace Amazon.Runtime.Internal.Transform
{
	public sealed class UnityWebResponseData : IDisposable, IWebResponseData, IHttpResponseBody
	{
		private Dictionary<string, string> _headers;

		private Stream _responseStream;

		private byte[] _responseBody;

		private ILogger _logger;

		public long ContentLength { get; private set; }

		public string ContentType { get; private set; }

		public HttpStatusCode StatusCode { get; private set; }

		public bool IsSuccessStatusCode { get; private set; }

		public IHttpResponseBody ResponseBody
		{
			get
			{
				return this;
			}
		}

		public bool IsResponseBodyPresent
		{
			get
			{
				return _responseBody != null && _responseBody.Length > 0;
			}
		}

		public UnityWebResponseData(WWW wwwRequest)
		{
			_logger = Logger.GetLogger(GetType());
			_headers = wwwRequest.responseHeaders;
			try
			{
				_responseBody = wwwRequest.bytes;
			}
			catch (Exception)
			{
				_logger.DebugFormat("setting response body to null");
				_responseBody = null;
			}
			if (wwwRequest.error == null)
			{
				_logger.DebugFormat("recieved successful response");
			}
			else
			{
				_logger.DebugFormat("recieved error response");
				_logger.DebugFormat("recieved = {0}", wwwRequest.error);
			}
			if ((_responseBody != null && _responseBody.Length > 0) || (_responseBody.Length == 0 && wwwRequest.error == null))
			{
				_responseStream = new MemoryStream(_responseBody);
			}
			ContentLength = wwwRequest.bytesDownloaded;
			string value = null;
			_headers.TryGetValue("Content-Type".ToUpperInvariant(), out value);
			ContentType = value;
			try
			{
				int result;
				if (string.IsNullOrEmpty(wwwRequest.error))
				{
					string value2 = string.Empty;
					_headers.TryGetValue("Status".ToUpperInvariant(), out value2);
					if (!string.IsNullOrEmpty(value2))
					{
						StatusCode = (HttpStatusCode)(int)Enum.Parse(typeof(HttpStatusCode), value2.Substring(9, 3).Trim());
					}
					else
					{
						StatusCode = (HttpStatusCode)0;
					}
				}
				else if (int.TryParse(wwwRequest.error.Substring(0, 3), out result))
				{
					StatusCode = (HttpStatusCode)(int)Enum.Parse(typeof(HttpStatusCode), wwwRequest.error.Substring(3).Replace(" ", string.Empty).Replace(":", string.Empty)
						.Trim(), true);
				}
				else
				{
					StatusCode = (HttpStatusCode)0;
				}
			}
			catch
			{
				StatusCode = (HttpStatusCode)0;
			}
			_logger.DebugFormat("Status = {0}", StatusCode);
			IsSuccessStatusCode = wwwRequest.error == null;
		}

		public string[] GetHeaderNames()
		{
			return _headers.Keys.ToArray();
		}

		public bool IsHeaderPresent(string headerName)
		{
			return _headers.ContainsKey(headerName.ToUpperInvariant());
		}

		public string GetHeaderValue(string headerName)
		{
			string value = null;
			_headers.TryGetValue(headerName.ToUpperInvariant(), out value);
			return value;
		}

		public Stream OpenResponse()
		{
			return _responseStream;
		}

		public void Dispose()
		{
			if (_responseStream != null)
			{
				_responseStream.Dispose();
				_responseStream = null;
			}
		}
	}
}
