using System.Linq;

namespace Amazon.Runtime.Internal.Util
{
	public static class Hashing
	{
		public static int Hash(params object[] value)
		{
			int[] hashes = value.Select((object v) => (v != null) ? v.GetHashCode() : 0).ToArray();
			return CombineHashes(hashes);
		}

		public static int CombineHashes(params int[] hashes)
		{
			int num = 0;
			foreach (int b in hashes)
			{
				num = CombineHashesInternal(num, b);
			}
			return num;
		}

		private static int CombineHashesInternal(int a, int b)
		{
			return ((a << 5) + a) ^ b;
		}
	}
}
