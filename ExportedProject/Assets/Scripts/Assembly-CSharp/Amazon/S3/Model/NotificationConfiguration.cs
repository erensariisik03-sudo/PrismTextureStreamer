using System.Collections.Generic;

namespace Amazon.S3.Model
{
	public abstract class NotificationConfiguration
	{
		private List<EventType> _events;

		private Filter filter;

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

		public Filter Filter
		{
			get
			{
				return filter;
			}
			set
			{
				filter = value;
			}
		}

		internal bool IsSetEvents()
		{
			return _events != null && _events.Count > 0;
		}

		internal bool IsSetFilter()
		{
			return filter != null;
		}
	}
}
