namespace Amazon.Runtime.Internal
{
	public class AsyncResponseContext : ResponseContext, IResponseContext, IAsyncResponseContext
	{
		public RuntimeAsyncResult AsyncResult { get; set; }
	}
}
