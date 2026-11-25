using System.Collections.Generic;
using Amazon.Runtime;

namespace Amazon.CognitoIdentity.Model
{
	public class GetIdentityPoolRolesResponse : AmazonWebServiceResponse
	{
		private string _identityPoolId;

		private Dictionary<string, string> _roles = new Dictionary<string, string>();

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

		public Dictionary<string, string> Roles
		{
			get
			{
				return _roles;
			}
			set
			{
				_roles = value;
			}
		}

		internal bool IsSetIdentityPoolId()
		{
			return _identityPoolId != null;
		}

		internal bool IsSetRoles()
		{
			return _roles != null && _roles.Count > 0;
		}
	}
}
