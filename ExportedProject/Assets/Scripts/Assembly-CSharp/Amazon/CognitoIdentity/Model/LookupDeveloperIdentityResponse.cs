using System.Collections.Generic;
using Amazon.Runtime;

namespace Amazon.CognitoIdentity.Model
{
	public class LookupDeveloperIdentityResponse : AmazonWebServiceResponse
	{
		private List<string> _developerUserIdentifierList = new List<string>();

		private string _identityId;

		private string _nextToken;

		public List<string> DeveloperUserIdentifierList
		{
			get
			{
				return _developerUserIdentifierList;
			}
			set
			{
				_developerUserIdentifierList = value;
			}
		}

		public string IdentityId
		{
			get
			{
				return _identityId;
			}
			set
			{
				_identityId = value;
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

		internal bool IsSetDeveloperUserIdentifierList()
		{
			return _developerUserIdentifierList != null && _developerUserIdentifierList.Count > 0;
		}

		internal bool IsSetIdentityId()
		{
			return _identityId != null;
		}

		internal bool IsSetNextToken()
		{
			return _nextToken != null;
		}
	}
}
