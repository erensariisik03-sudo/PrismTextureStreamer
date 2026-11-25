using System.Collections.Generic;

namespace Amazon.S3.Model
{
	public class CloudFunctionConfiguration
	{
		private List<EventType> _events;

		public string Id { get; set; }

		public List<EventType> Events
		{
			get
			{
				if (_events == null)
				{
					_events = new List<EventType>();
				}
				return _events;
			}
			set
			{
				_events = value;
			}
		}

		public string CloudFunction { get; set; }

		public string InvocationRole { get; set; }

		internal bool IsSetId()
		{
			return Id != null;
		}

		internal bool IsSetEvents()
		{
			return _events != null && _events.Count > 0;
		}

		internal bool IsSetCloudFunction()
		{
			return CloudFunction != null;
		}

		internal bool IsSetInvocationRole()
		{
			return InvocationRole != null;
		}
	}
}
