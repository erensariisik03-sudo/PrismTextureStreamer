namespace Amazon.S3.Model
{
	public class S3BucketVersioningConfig
	{
		private VersionStatus status = "Off";

		private bool? enableMfaDelete;

		public VersionStatus Status
		{
			get
			{
				return status;
			}
			set
			{
				status = value;
			}
		}

		public bool EnableMfaDelete
		{
			get
			{
				return enableMfaDelete == true;
			}
			set
			{
				enableMfaDelete = value;
			}
		}

		internal bool IsSetStatus()
		{
			return status != null && status != VersionStatus.Off;
		}

		internal bool IsSetEnableMfaDelete()
		{
			return enableMfaDelete.HasValue;
		}
	}
}
