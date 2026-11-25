using System;
using Amazon.Runtime;

namespace Amazon.S3
{
	public sealed class S3Region : ConstantClass
	{
		public static readonly S3Region US = new S3Region(string.Empty);

		public static readonly S3Region EU = new S3Region("EU");

		public static readonly S3Region EUC1 = new S3Region("eu-central-1");

		public static readonly S3Region USW1 = new S3Region("us-west-1");

		public static readonly S3Region USW2 = new S3Region("us-west-2");

		public static readonly S3Region APS1 = new S3Region("ap-southeast-1");

		public static readonly S3Region APS2 = new S3Region("ap-southeast-2");

		public static readonly S3Region APN1 = new S3Region("ap-northeast-1");

		public static readonly S3Region SAE1 = new S3Region("sa-east-1");

		[Obsolete("This constant is obsolete. Usags of this property should be migrated to the USW1 constant")]
		public static readonly S3Region SFO = new S3Region("us-west-1");

		public S3Region(string value)
			: base(value)
		{
		}

		public static S3Region FindValue(string value)
		{
			if (value == null)
			{
				return US;
			}
			return ConstantClass.FindValue<S3Region>(value);
		}

		public static implicit operator S3Region(string value)
		{
			return FindValue(value);
		}
	}
}
