namespace Amazon.S3.Model
{
	public class S3ObjectVersion : S3Object
	{
		private bool? isLatest;

		private string versionId;

		private bool isDeleteMarker;

		public bool IsLatest
		{
			get
			{
				bool? flag = isLatest;
				return flag.HasValue && flag.Value;
			}
			set
			{
				isLatest = value;
			}
		}

		public string VersionId
		{
			get
			{
				return versionId;
			}
			set
			{
				versionId = value;
			}
		}

		public bool IsDeleteMarker
		{
			get
			{
				return isDeleteMarker;
			}
			set
			{
				isDeleteMarker = value;
			}
		}
	}
}
