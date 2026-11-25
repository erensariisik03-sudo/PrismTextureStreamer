using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using Amazon.Runtime.Internal.Util;
using ThirdParty.Json.LitJson;

namespace Amazon.Runtime
{
	public class InstanceProfileAWSCredentials : RefreshingAWSCredentials
	{
		private class SecurityBase
		{
			public string Code { get; set; }

			public string Message { get; set; }

			public DateTime LastUpdated { get; set; }
		}

		private class SecurityInfo : SecurityBase
		{
			public string InstanceProfileArn { get; set; }

			public string InstanceProfileId { get; set; }
		}

		private class SecurityCredentials : SecurityBase
		{
			public string Type { get; set; }

			public string AccessKeyId { get; set; }

			public string SecretAccessKey { get; set; }

			public string Token { get; set; }

			public DateTime Expiration { get; set; }
		}

		private static TimeSpan _preemptExpiryTime = TimeSpan.FromMinutes(15.0);

		private CredentialsRefreshState _currentRefreshState;

		private static TimeSpan _refreshAttemptPeriod = TimeSpan.FromHours(1.0);

		private static string[] AliasSeparators = new string[1] { "<br/>" };

		private static string Server = "http://169.254.169.254";

		private static string RolesPath = "/latest/meta-data/iam/security-credentials/";

		private static string InfoPath = "/latest/meta-data/iam/info";

		private static string SuccessCode = "Success";

		public string Role { get; set; }

		private static Uri RolesUri
		{
			get
			{
				return new Uri(Server + RolesPath);
			}
		}

		private Uri CurrentRoleUri
		{
			get
			{
				return new Uri(Server + RolesPath + Role);
			}
		}

		private static Uri InfoUri
		{
			get
			{
				return new Uri(Server + InfoPath);
			}
		}

		public InstanceProfileAWSCredentials(string role)
		{
			Role = role;
			base.PreemptExpiryTime = _preemptExpiryTime;
		}

		public InstanceProfileAWSCredentials()
			: this(GetFirstRole())
		{
		}

		protected override CredentialsRefreshState GenerateNewCredentials()
		{
			CredentialsRefreshState credentialsRefreshState = null;
			try
			{
				credentialsRefreshState = GetRefreshState();
			}
			catch (Exception ex)
			{
				Logger logger = Logger.GetLogger(typeof(InstanceProfileAWSCredentials));
				logger.InfoFormat("Error getting credentials from Instance Profile service: {0}", ex);
			}
			if (credentialsRefreshState != null)
			{
				_currentRefreshState = credentialsRefreshState;
			}
			if (_currentRefreshState == null)
			{
				_currentRefreshState = GetRefreshState();
			}
			return GetEarlyRefreshState(_currentRefreshState);
		}

		public static IEnumerable<string> GetAvailableRoles()
		{
			string allAliases = GetContents(RolesUri);
			if (string.IsNullOrEmpty(allAliases))
			{
				yield break;
			}
			string[] parts = allAliases.Split(AliasSeparators, StringSplitOptions.RemoveEmptyEntries);
			string[] array = parts;
			foreach (string part in array)
			{
				string trim = part.Trim();
				if (!string.IsNullOrEmpty(trim))
				{
					yield return trim;
				}
			}
		}

		private CredentialsRefreshState GetEarlyRefreshState(CredentialsRefreshState state)
		{
			DateTime expiration = DateTime.Now + _refreshAttemptPeriod + base.PreemptExpiryTime;
			if (expiration.ToUniversalTime() > state.Expiration.ToUniversalTime())
			{
				expiration = state.Expiration;
			}
			CredentialsRefreshState credentialsRefreshState = new CredentialsRefreshState();
			credentialsRefreshState.Credentials = state.Credentials.Copy();
			credentialsRefreshState.Expiration = expiration;
			return credentialsRefreshState;
		}

		private CredentialsRefreshState GetRefreshState()
		{
			SecurityInfo serviceInfo = GetServiceInfo();
			if (!string.IsNullOrEmpty(serviceInfo.Message))
			{
				throw new AmazonServiceException(string.Format(CultureInfo.InvariantCulture, "Unable to retrieve credentials. Message = \"{0}\".", serviceInfo.Message));
			}
			SecurityCredentials roleCredentials = GetRoleCredentials();
			CredentialsRefreshState credentialsRefreshState = new CredentialsRefreshState();
			credentialsRefreshState.Credentials = new ImmutableCredentials(roleCredentials.AccessKeyId, roleCredentials.SecretAccessKey, roleCredentials.Token);
			credentialsRefreshState.Expiration = roleCredentials.Expiration;
			return credentialsRefreshState;
		}

		private static SecurityInfo GetServiceInfo()
		{
			string contents = GetContents(InfoUri);
			SecurityInfo securityInfo = JsonMapper.ToObject<SecurityInfo>(contents);
			ValidateResponse(securityInfo);
			return securityInfo;
		}

		private SecurityCredentials GetRoleCredentials()
		{
			string contents = GetContents(CurrentRoleUri);
			SecurityCredentials securityCredentials = JsonMapper.ToObject<SecurityCredentials>(contents);
			ValidateResponse(securityCredentials);
			return securityCredentials;
		}

		private static void ValidateResponse(SecurityBase response)
		{
			if (!string.Equals(response.Code, SuccessCode, StringComparison.OrdinalIgnoreCase))
			{
				throw new AmazonServiceException(string.Format(CultureInfo.InvariantCulture, "Unable to retrieve credentials. Code = \"{0}\". Message = \"{1}\".", response.Code, response.Message));
			}
		}

		private static string GetContents(Uri uri)
		{
			try
			{
				HttpWebRequest httpWebRequest = WebRequest.Create(uri) as HttpWebRequest;
				IAsyncResult asyncResult = httpWebRequest.BeginGetResponse(null, null);
				using (HttpWebResponse httpWebResponse = httpWebRequest.EndGetResponse(asyncResult) as HttpWebResponse)
				{
					using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
					{
						return streamReader.ReadToEnd();
					}
				}
			}
			catch (WebException)
			{
				throw new AmazonServiceException("Unable to reach credentials server");
			}
		}

		private static string GetFirstRole()
		{
			IEnumerable<string> availableRoles = GetAvailableRoles();
			using (IEnumerator<string> enumerator = availableRoles.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					return enumerator.Current;
				}
			}
			throw new InvalidOperationException("No roles found");
		}
	}
}
