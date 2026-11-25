using System;
using Amazon.Runtime.Internal.Util;

namespace Amazon.Runtime.Internal
{
	public abstract class ExceptionHandler<T> : IExceptionHandler, IExceptionHandler<T> where T : Exception
	{
		private ILogger _logger;

		protected ILogger Logger
		{
			get
			{
				return _logger;
			}
		}

		protected ExceptionHandler(ILogger logger)
		{
			_logger = logger;
		}

		public bool Handle(IExecutionContext executionContext, Exception exception)
		{
			return HandleException(executionContext, exception as T);
		}

		public abstract bool HandleException(IExecutionContext executionContext, T exception);
	}
}
