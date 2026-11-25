using System;
using AxesInMotion.SocialPlugin.Adapters;

namespace AxesInMotion.SocialPlugin
{
	public class SocialFactory
	{
		public static ISocial Create(SocialType socialType)
		{
			switch (socialType)
			{
			case SocialType.Facebook:
				return new FacebookAdapter();
			case SocialType.GooglePlay:
				return new GooglePlayAdapter();
			default:
				throw new NotImplementedException();
			}
		}
	}
}
