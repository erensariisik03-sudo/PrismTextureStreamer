using System;

namespace Amazon.Runtime.Internal
{
	public sealed class UnityWebRequestFactory : IDisposable, IHttpRequestFactory<string>
	{
		public IHttpRequest<string> CreateHttpRequest(Uri requestUri)
		{
			return new UnityWebRequest(requestUri);
		}

		public void Dispose()
		{
		}
	}
}
