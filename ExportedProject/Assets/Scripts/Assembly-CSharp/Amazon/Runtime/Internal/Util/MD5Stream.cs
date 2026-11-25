using System.IO;

namespace Amazon.Runtime.Internal.Util
{
	public class MD5Stream : HashStream<HashingWrapperMD5>
	{
		public MD5Stream(Stream baseStream, byte[] expectedHash, long expectedLength)
			: base(baseStream, expectedHash, expectedLength)
		{
		}
	}
}
