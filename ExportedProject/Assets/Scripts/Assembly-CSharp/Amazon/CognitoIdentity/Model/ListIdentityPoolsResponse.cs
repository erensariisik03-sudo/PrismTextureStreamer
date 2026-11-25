using System.Collections.Generic;
using Amazon.Runtime;

namespace Amazon.CognitoIdentity.Model
{
	public class ListIdentityPoolsResponse : AmazonWebServiceResponse
	{
		private List<IdentityPoolShortDescription> _identityPools = new List<IdentityPoolShortDescription>();

		private string _nextToken;

		public List<IdentityPoolShortDescription> IdentityPools
		{
			get
			{
				return _identityPools;
			}
			set
			{
				_identityPools = value;
			}
		}

		public string NextToken
		{
			get
			{
				return _nextToken;
			}
			set
			{
				_nextToken = value;
			}
		}

		internal bool IsSetIdentityPools()
		{
			return _identityPools != null && _identityPools.Count > 0;
		}

		internal bool IsSetNextToken()
		{
			return _nextToken != null;
		}
	}
}
