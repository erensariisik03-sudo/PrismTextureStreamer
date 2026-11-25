using System;

namespace Amazon.Runtime.Internal.Util
{
	public class Timing : IMetricsTiming
	{
		private long startTime;

		private long endTime;

		public bool IsFinished { get; private set; }

		public long ElapsedTicks
		{
			get
			{
				return IsFinished ? (endTime - startTime) : 0;
			}
		}

		public TimeSpan ElapsedTime
		{
			get
			{
				return TimeSpan.FromTicks(ElapsedTicks);
			}
		}

		public Timing()
		{
			startTime = (endTime = 0L);
			IsFinished = true;
		}

		public Timing(long currentTime)
		{
			startTime = currentTime;
			IsFinished = false;
		}

		public void Stop(long currentTime)
		{
			endTime = currentTime;
			IsFinished = true;
		}
	}
}
