using System;
using Amazon.Runtime;
using Amazon.Runtime.Internal;
using Amazon.S3.Model;
using Amazon.S3.Util;

namespace Amazon.S3.Internal
{
	public class AmazonS3KmsHandler : PipelineHandler
	{
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
			IRequest request = executionContext.RequestContext.Request;
			EvaluateIfSigV4Required(request);
		}

		internal static void EvaluateIfSigV4Required(IRequest request)
		{
			if (request.OriginalRequest is GetObjectRequest && AmazonS3Uri.IsAmazonS3Endpoint(request.Endpoint))
			{
				RegionEndpoint region = new AmazonS3Uri(request.Endpoint.OriginalString).Region;
				if (region != RegionEndpoint.USEast1)
				{
					request.UseSigV4 = true;
				}
			}
		}
	}
}
