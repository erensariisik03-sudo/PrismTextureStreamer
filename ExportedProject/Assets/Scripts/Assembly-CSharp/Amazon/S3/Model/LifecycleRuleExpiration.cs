using System;

namespace Amazon.S3.Model
{
	public class LifecycleRuleExpiration
	{
		private DateTime? date;

		private int? days;

		public DateTime Date
		{
			get
			{
				DateTime? dateTime = date;
				return (!dateTime.HasValue) ? default(DateTime) : dateTime.Value;
			}
			set
			{
				date = value;
			}
		}

		public int Days
		{
			get
			{
				int? num = days;
				return num.HasValue ? num.Value : 0;
			}
			set
			{
				days = value;
			}
		}

		internal bool IsSetDate()
		{
			return date.HasValue;
		}

		internal bool IsSetDays()
		{
			return days.HasValue;
		}
	}
}
