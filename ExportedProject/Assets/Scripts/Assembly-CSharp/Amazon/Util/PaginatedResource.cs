using System;
using System.Collections;
using System.Collections.Generic;

namespace Amazon.Util
{
	internal class PaginatedResource<U> : IEnumerable, IEnumerable<U>
	{
		internal Func<string, Marker<U>> fetcher;

		internal PaginatedResource(Func<string, Marker<U>> fetcher)
		{
			this.fetcher = fetcher;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public IEnumerator<U> GetEnumerator()
		{
			return new PaginationEnumerator<U>(this);
		}
	}
}
