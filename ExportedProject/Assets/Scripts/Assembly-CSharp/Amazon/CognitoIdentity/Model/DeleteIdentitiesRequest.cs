using System.Collections.Generic;

namespace Amazon.CognitoIdentity.Model
{
	public class DeleteIdentitiesRequest : AmazonCognitoIdentityRequest
	{
		private List<string> _identityIdsToDelete = new List<string>();

		public List<string> IdentityIdsToDelete
		{
			get
			{
				return _identityIdsToDelete;
			}
			set
			{
				_identityIdsToDelete = value;
			}
		}

		internal bool IsSetIdentityIdsToDelete()
		{
			return _identityIdsToDelete != null && _identityIdsToDelete.Count > 0;
		}
	}
}
