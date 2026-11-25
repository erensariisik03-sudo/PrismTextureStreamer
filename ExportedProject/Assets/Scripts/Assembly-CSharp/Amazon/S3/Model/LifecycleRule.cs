using System;
using System.Collections.Generic;

namespace Amazon.S3.Model
{
	public class LifecycleRule
	{
		private string id;

		private string prefix;

		private LifecycleRuleExpiration expiration;

		private LifecycleRuleStatus status = LifecycleRuleStatus.Disabled;

		private LifecycleTransition transition;

		private LifecycleRuleNoncurrentVersionTransition noncurrentVersionTransition;

		private LifecycleRuleNoncurrentVersionExpiration noncurrentVersionExpiration;

		private List<LifecycleTransition> transitions;

		private List<LifecycleRuleNoncurrentVersionTransition> noncurrentVersionTransitions;

		public LifecycleRuleExpiration Expiration
		{
			get
			{
				return expiration;
			}
			set
			{
				expiration = value;
			}
		}

		public string Id
		{
			get
			{
				return id;
			}
			set
			{
				id = value;
			}
		}

		public string Prefix
		{
			get
			{
				return prefix;
			}
			set
			{
				prefix = value;
			}
		}

		public LifecycleRuleStatus Status
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

		[Obsolete("The Transition property is now obsolete in favor the Transitions property.")]
		public LifecycleTransition Transition
		{
			get
			{
				if (!IsSetTransitions())
				{
					return null;
				}
				return Transitions[0];
			}
			set
			{
				if (Transitions.Count == 0)
				{
					Transitions.Add(value);
				}
				else
				{
					Transitions[0] = value;
				}
			}
		}

		public LifecycleRuleNoncurrentVersionExpiration NoncurrentVersionExpiration
		{
			get
			{
				return noncurrentVersionExpiration;
			}
			set
			{
				noncurrentVersionExpiration = value;
			}
		}

		[Obsolete("The NoncurrentVersionTransition property is now obsolete in favor the NoncurrentVersionTransitions property.")]
		public LifecycleRuleNoncurrentVersionTransition NoncurrentVersionTransition
		{
			get
			{
				if (!IsSetNoncurrentVersionTransitions())
				{
					return null;
				}
				return NoncurrentVersionTransitions[0];
			}
			set
			{
				if (NoncurrentVersionTransitions.Count == 0)
				{
					NoncurrentVersionTransitions.Add(value);
				}
				else
				{
					NoncurrentVersionTransitions[0] = value;
				}
			}
		}

		public List<LifecycleTransition> Transitions
		{
			get
			{
				if (transitions == null)
				{
					transitions = new List<LifecycleTransition>();
				}
				return transitions;
			}
			set
			{
				transitions = value;
			}
		}

		public List<LifecycleRuleNoncurrentVersionTransition> NoncurrentVersionTransitions
		{
			get
			{
				if (noncurrentVersionTransitions == null)
				{
					noncurrentVersionTransitions = new List<LifecycleRuleNoncurrentVersionTransition>();
				}
				return noncurrentVersionTransitions;
			}
			set
			{
				noncurrentVersionTransitions = value;
			}
		}

		internal bool IsSetExpiration()
		{
			return expiration != null;
		}

		internal bool IsSetId()
		{
			return id != null;
		}

		internal bool IsSetPrefix()
		{
			return prefix != null;
		}

		internal bool IsSetStatus()
		{
			return status != null;
		}

		internal bool IsSetTransition()
		{
			return transition != null;
		}

		internal bool IsSetNoncurrentVersionExpiration()
		{
			return noncurrentVersionExpiration != null;
		}

		internal bool IsSetNoncurrentVersionTransition()
		{
			return noncurrentVersionTransition != null;
		}

		internal bool IsSetTransitions()
		{
			return transitions != null && transitions.Count > 0;
		}

		internal bool IsSetNoncurrentVersionTransitions()
		{
			return noncurrentVersionTransitions != null && noncurrentVersionTransitions.Count > 0;
		}
	}
}
