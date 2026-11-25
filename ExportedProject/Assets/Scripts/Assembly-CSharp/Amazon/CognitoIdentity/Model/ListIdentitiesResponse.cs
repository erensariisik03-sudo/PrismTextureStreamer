using System.Collections.Generic;
using Amazon.Runtime;

namespace Amazon.CognitoIdentity.Model
{
	public class ListIdentitiesResponse : AmazonWebServiceResponse
	{
		private List<IdentityDescription> _identities = new List<IdentityDescription>();

		private string _identityPoolId;

		private string _nextToken;

		public List<IdentityDescription> Identities
		{
			get
			{
				return _identities;
			}
			set
			{
				_identities = value;
			}
		}

		public string IdentityPoolId
		{
			get
			{
				return _identityPoolId;
			}
			set
			{
				_identityPoolId = value;
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

		internal bool IsSetIdentities()
		{
			return _identities != null && _identities.Count > 0;
		}

		internal bool IsSetIdentityPoolId()
		{
			return _identityPoolId != null;
		}

		internal bool IsSetNextToken()
		{
			return _nextToken != null;
		}
	}
}
