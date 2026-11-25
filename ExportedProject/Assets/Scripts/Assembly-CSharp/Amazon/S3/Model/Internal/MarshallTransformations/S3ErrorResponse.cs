using System;
using Amazon.Runtime.Internal;

namespace Amazon.S3.Model.Internal.MarshallTransformations
{
	public class S3ErrorResponse : ErrorResponse
	{
		public string Resource { get; set; }

		public string Id2 { get; set; }

		public Exception ParsingException { get; set; }
	}
}
