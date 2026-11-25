using System.Collections.Generic;
using Amazon.Runtime;

namespace Amazon.S3.Model
{
	public class PutBucketNotificationRequest : AmazonWebServiceRequest
	{
		public string BucketName { get; set; }

		public List<TopicConfiguration> TopicConfigurations { get; set; }

		public List<QueueConfiguration> QueueConfigurations { get; set; }

		public List<LambdaFunctionConfiguration> LambdaFunctionConfigurations { get; set; }

		internal bool IsSetBucketName()
		{
			return BucketName != null;
		}

		internal bool IsSetTopicConfigurations()
		{
			return TopicConfigurations != null && TopicConfigurations.Count > 0;
		}

		internal bool IsSetQueueConfigurations()
		{
			return QueueConfigurations != null && QueueConfigurations.Count > 0;
		}

		internal bool IsSetLambdaFunctionConfigurations()
		{
			return LambdaFunctionConfigurations != null && LambdaFunctionConfigurations.Count > 0;
		}
	}
}
