using System.Collections.Generic;

namespace Amazon.CognitoIdentity.Model
{
	public class UnlinkIdentityRequest : AmazonCognitoIdentityRequest
	{
		private string _identityId;

		private Dictionary<string, string> _logins = new Dictionary<string, string>();

		private List<string> _loginsToRemove = new List<string>();

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

		public Dictionary<string, string> Logins
		{
			get
			{
				return _logins;
			}
			set
			{
				_logins = value;
			}
		}

		public List<string> LoginsToRemove
		{
			get
			{
				return _loginsToRemove;
			}
			set
			{
				_loginsToRemove = value;
			}
		}

		internal bool IsSetIdentityId()
		{
			return _identityId != null;
		}

		internal bool IsSetLogins()
		{
			return _logins != null && _logins.Count > 0;
		}

		internal bool IsSetLoginsToRemove()
		{
			return _loginsToRemove != null && _loginsToRemove.Count > 0;
		}
	}
}
