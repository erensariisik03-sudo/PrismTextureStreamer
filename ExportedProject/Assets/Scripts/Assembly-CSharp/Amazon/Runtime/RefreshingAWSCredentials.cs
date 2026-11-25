using System;
using System.Globalization;
using Amazon.Runtime.Internal.Util;

namespace Amazon.Runtime
{
	public abstract class RefreshingAWSCredentials : AWSCredentials
	{
		public class CredentialsRefreshState
		{
			public ImmutableCredentials Credentials { get; set; }

			public DateTime Expiration { get; set; }

			public CredentialsRefreshState()
			{
			}

			public CredentialsRefreshState(ImmutableCredentials credentials, DateTime expiration)
			{
				Credentials = credentials;
				Expiration = expiration;
			}
		}

		protected CredentialsRefreshState _currentState;

		private object _refreshLock = new object();

		private TimeSpan _preemptExpiryTime = TimeSpan.FromMinutes(0.0);

		public TimeSpan PreemptExpiryTime
		{
			get
			{
				return _preemptExpiryTime;
			}
			set
			{
				if (value < TimeSpan.Zero)
				{
					throw new ArgumentOutOfRangeException("value", "PreemptExpiryTime cannot be negative");
				}
				_preemptExpiryTime = value;
			}
		}

		private bool ShouldUpdate
		{
			get
			{
				if (_currentState == null)
				{
					return true;
				}
				DateTime utcNow = DateTime.UtcNow;
				DateTime dateTime = _currentState.Expiration.ToUniversalTime();
				return utcNow > dateTime;
			}
		}

		public override ImmutableCredentials GetCredentials()
		{
			lock (_refreshLock)
			{
				if (ShouldUpdate)
				{
					_currentState = GenerateNewCredentials();
					UpdateToGeneratedCredentials(_currentState);
				}
				return _currentState.Credentials.Copy();
			}
		}

		private void UpdateToGeneratedCredentials(CredentialsRefreshState state)
		{
			if (ShouldUpdate)
			{
				string message = string.Format(CultureInfo.InvariantCulture, "The retrieved credentials have already expired: Now = {0}, Credentials expiration = {1}", DateTime.Now, state.Expiration);
				throw new AmazonClientException(message);
			}
			state.Expiration -= PreemptExpiryTime;
			if (ShouldUpdate)
			{
				Logger logger = Logger.GetLogger(typeof(RefreshingAWSCredentials));
				logger.InfoFormat("The preempt expiry time is set too high: Current time = {0}, Credentials expiry time = {1}, Preempt expiry time = {2}.", DateTime.Now, _currentState.Expiration, PreemptExpiryTime);
			}
		}

		protected virtual CredentialsRefreshState GenerateNewCredentials()
		{
			throw new NotImplementedException();
		}

		public virtual void ClearCredentials()
		{
			_currentState = null;
		}
	}
}
