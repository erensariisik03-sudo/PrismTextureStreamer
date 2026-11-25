using System;
using System.IO;

namespace Amazon.Runtime.Internal.Transform
{
	public class MemoryStreamUnmarshaller : IUnmarshaller<MemoryStream, XmlUnmarshallerContext>, IUnmarshaller<MemoryStream, JsonUnmarshallerContext>
	{
		private static MemoryStreamUnmarshaller _instance = new MemoryStreamUnmarshaller();

		public static MemoryStreamUnmarshaller Instance
		{
			get
			{
				return _instance;
			}
		}

		private MemoryStreamUnmarshaller()
		{
		}

		public static MemoryStreamUnmarshaller GetInstance()
		{
			return Instance;
		}

		public MemoryStream Unmarshall(XmlUnmarshallerContext context)
		{
			byte[] buffer = Convert.FromBase64String(context.ReadText());
			return new MemoryStream(buffer);
		}

		public MemoryStream Unmarshall(JsonUnmarshallerContext context)
		{
			context.Read();
			byte[] buffer = Convert.FromBase64String(context.ReadText());
			return new MemoryStream(buffer);
		}
	}
}
