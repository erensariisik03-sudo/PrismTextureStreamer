using System.Globalization;
using System.Net;
using Amazon.Runtime.Internal.Util;

namespace Amazon.Runtime.Internal
{
	public class WebExceptionHandler : ExceptionHandler<WebException>
	{
		public WebExceptionHandler(ILogger logger)
			: base(logger)
		{
		}

		public override bool HandleException(IExecutionContext executionContext, WebException exception)
		{
			IRequestContext requestContext = executionContext.RequestContext;
			HttpWebResponse httpWebResponse = exception.Response as HttpWebResponse;
			if (httpWebResponse != null)
			{
				requestContext.Metrics.AddProperty(Metric.StatusCode, httpWebResponse.StatusCode);
			}
			string message = string.Format(CultureInfo.InvariantCulture, "A WebException with status {0} was thrown.", exception.Status);
			throw new AmazonServiceException(message, exception);
		}
	}
}
