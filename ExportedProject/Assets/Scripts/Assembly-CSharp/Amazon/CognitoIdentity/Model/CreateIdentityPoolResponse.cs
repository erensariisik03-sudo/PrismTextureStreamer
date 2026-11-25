using System.Collections.Generic;
using Amazon.Runtime;

namespace Amazon.CognitoIdentity.Model
{
	public class CreateIdentityPoolResponse : AmazonWebServiceResponse
	{
		private bool? _allowUnauthenticatedIdentities;

		private string _developerProviderName;

		private string _identityPoolId;

		private string _identityPoolName;

		private List<string> _openIdConnectProviderARNs = new List<string>();

		private Dictionary<string, string> _supportedLoginProviders = new Dictionary<string, string>();

		public bool AllowUnauthenticatedIdentities
		{
			get
			{
				return _allowUnauthenticatedIdentities == true;
			}
			set
			{
				_allowUnauthenticatedIdentities = value;
			}
		}

		public string DeveloperProviderName
		{
			get
			{
				return _developerProviderName;
			}
			set
			{
				_developerProviderName = value;
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

		public string IdentityPoolName
		{
			get
			{
				return _identityPoolName;
			}
			set
			{
				_identityPoolName = value;
			}
		}

		public List<string> OpenIdConnectProviderARNs
		{
			get
			{
				return _openIdConnectProviderARNs;
			}
			set
			{
				_openIdConnectProviderARNs = value;
			}
		}

		public Dictionary<string, string> SupportedLoginProviders
		{
			get
			{
				return _supportedLoginProviders;
			}
			set
			{
				_supportedLoginProviders = value;
			}
		}

		internal bool IsSetAllowUnauthenticatedIdentities()
		{
			return _allowUnauthenticatedIdentities.HasValue;
		}

		internal bool IsSetDeveloperProviderName()
		{
			return _developerProviderName != null;
		}

		internal bool IsSetIdentityPoolId()
		{
			return _identityPoolId != null;
		}

		internal bool IsSetIdentityPoolName()
		{
			return _identityPoolName != null;
		}

		internal bool IsSetOpenIdConnectProviderARNs()
		{
			return _openIdConnectProviderARNs != null && _openIdConnectProviderARNs.Count > 0;
		}

		internal bool IsSetSupportedLoginProviders()
		{
			return _supportedLoginProviders != null && _supportedLoginProviders.Count > 0;
		}
	}
}
