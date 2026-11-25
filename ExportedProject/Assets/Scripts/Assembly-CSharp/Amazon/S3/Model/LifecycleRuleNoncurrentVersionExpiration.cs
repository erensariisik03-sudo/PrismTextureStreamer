namespace Amazon.S3.Model
{
	public class LifecycleRuleNoncurrentVersionExpiration
	{
		private int? noncurrentDays;

		public int NoncurrentDays
		{
			get
			{
				int? num = noncurrentDays;
				return num.HasValue ? num.Value : 0;
			}
			set
			{
				noncurrentDays = value;
			}
		}

		internal bool IsSetNoncurrentDays()
		{
			return noncurrentDays.HasValue;
		}
	}
}
