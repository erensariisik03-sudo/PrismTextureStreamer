using System;
using Amazon.Util.Storage.Internal;
using UnityEngine;

namespace Amazon.Util.Internal.PlatformServices
{
	public class NetworkReachability : INetworkReachability
	{
		public NetworkStatus NetworkStatus
		{
			get
			{
				switch (NetworkInfo.GetReachability())
				{
				case UnityEngine.NetworkReachability.ReachableViaCarrierDataNetwork:
					return NetworkStatus.ReachableViaCarrierDataNetwork;
				case UnityEngine.NetworkReachability.ReachableViaLocalAreaNetwork:
					return NetworkStatus.ReachableViaWiFiNetwork;
				default:
					return NetworkStatus.NotReachable;
				}
			}
		}

		public event EventHandler<NetworkStatusEventArgs> NetworkReachabilityChanged
		{
			add
			{
				throw new NotImplementedException("This functionality is not implemented in the portable version of this assembly. You should reference the AWSSDK.Core NuGet package from your main application project in order to reference the platform-specific implementation.");
			}
			remove
			{
				throw new NotImplementedException("This functionality is not implemented in the portable version of this assembly. You should reference the AWSSDK.Core NuGet package from your main application project in order to reference the platform-specific implementation.");
			}
		}
	}
}
