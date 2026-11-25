using Amazon.Runtime.Internal.Transform;

namespace Amazon.S3.Model.Internal.MarshallTransformations
{
	internal class RulesItemUnmarshaller : IUnmarshaller<LifecycleRule, XmlUnmarshallerContext>, IUnmarshaller<LifecycleRule, JsonUnmarshallerContext>
	{
		private static RulesItemUnmarshaller _instance;

		public static RulesItemUnmarshaller Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new RulesItemUnmarshaller();
				}
				return _instance;
			}
		}

		public LifecycleRule Unmarshall(XmlUnmarshallerContext context)
		{
			LifecycleRule lifecycleRule = new LifecycleRule();
			int currentDepth = context.CurrentDepth;
			int num = currentDepth + 1;
			if (context.IsStartOfDocument)
			{
				num += 2;
			}
			while (context.Read())
			{
				if (context.IsStartElement || context.IsAttribute)
				{
					if (context.TestExpression("Expiration", num))
					{
						lifecycleRule.Expiration = ExpirationUnmarshaller.Instance.Unmarshall(context);
					}
					else if (context.TestExpression("ID", num))
					{
						lifecycleRule.Id = StringUnmarshaller.GetInstance().Unmarshall(context);
					}
					else if (context.TestExpression("Prefix", num))
					{
						lifecycleRule.Prefix = StringUnmarshaller.GetInstance().Unmarshall(context);
					}
					else if (context.TestExpression("Status", num))
					{
						lifecycleRule.Status = StringUnmarshaller.GetInstance().Unmarshall(context);
					}
					else if (context.TestExpression("Transition", num))
					{
						lifecycleRule.Transitions.Add(TransitionUnmarshaller.Instance.Unmarshall(context));
					}
					else if (context.TestExpression("NoncurrentVersionTransition", num))
					{
						lifecycleRule.NoncurrentVersionTransitions.Add(LifecycleRuleNoncurrentVersionTransitionUnmarshaller.Instance.Unmarshall(context));
					}
					else if (context.TestExpression("NoncurrentVersionExpiration", num))
					{
						lifecycleRule.NoncurrentVersionExpiration = LifecycleRuleNoncurrentVersionExpirationUnmarshaller.Instance.Unmarshall(context);
					}
				}
				else if (context.IsEndElement && context.CurrentDepth < currentDepth)
				{
					return lifecycleRule;
				}
			}
			return lifecycleRule;
		}

		public LifecycleRule Unmarshall(JsonUnmarshallerContext context)
		{
			return null;
		}
	}
}
