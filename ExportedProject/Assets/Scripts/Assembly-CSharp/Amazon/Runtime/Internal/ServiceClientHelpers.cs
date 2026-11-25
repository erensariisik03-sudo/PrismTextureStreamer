using System;
using System.Globalization;
using System.Reflection;
using Amazon.Util.Internal;

namespace Amazon.Runtime.Internal
{
	public static class ServiceClientHelpers
	{
		public const string S3_ASSEMBLY_NAME = "AWSSDK.S3";

		public const string S3_SERVICE_CLASS_NAME = "Amazon.S3.AmazonS3Client";

		public static TClient CreateServiceFromAnother<TClient, TConfig>(AmazonServiceClient originalServiceClient) where TClient : AmazonServiceClient where TConfig : ClientConfig, new()
		{
			AWSCredentials credentials = originalServiceClient.Credentials;
			TConfig val = originalServiceClient.CloneConfig<TConfig>();
			ITypeInfo typeInfo = TypeFactory.GetTypeInfo(typeof(TClient));
			ConstructorInfo constructor = typeInfo.GetConstructor(new ITypeInfo[2]
			{
				TypeFactory.GetTypeInfo(typeof(AWSCredentials)),
				TypeFactory.GetTypeInfo(val.GetType())
			});
			return constructor.Invoke(new object[2] { credentials, val }) as TClient;
		}

		public static TClient CreateServiceFromAssembly<TClient>(string assemblyName, string serviceClientClassName, RegionEndpoint region) where TClient : class
		{
			ITypeInfo typeInfo = LoadServiceClientType(assemblyName, serviceClientClassName);
			ConstructorInfo constructor = typeInfo.GetConstructor(new ITypeInfo[1] { TypeFactory.GetTypeInfo(typeof(RegionEndpoint)) });
			return constructor.Invoke(new object[1] { region }) as TClient;
		}

		public static TClient CreateServiceFromAssembly<TClient>(string assemblyName, string serviceClientClassName, AWSCredentials credentials, RegionEndpoint region) where TClient : class
		{
			ITypeInfo typeInfo = LoadServiceClientType(assemblyName, serviceClientClassName);
			ConstructorInfo constructor = typeInfo.GetConstructor(new ITypeInfo[2]
			{
				TypeFactory.GetTypeInfo(typeof(AWSCredentials)),
				TypeFactory.GetTypeInfo(typeof(RegionEndpoint))
			});
			return constructor.Invoke(new object[2] { credentials, region }) as TClient;
		}

		public static TClient CreateServiceFromAssembly<TClient>(string assemblyName, string serviceClientClassName, AmazonServiceClient originalServiceClient) where TClient : class
		{
			ITypeInfo typeInfo = LoadServiceClientType(assemblyName, serviceClientClassName);
			ClientConfig clientConfig = CreateServiceConfig(assemblyName, serviceClientClassName);
			originalServiceClient.CloneConfig(clientConfig);
			ConstructorInfo constructor = typeInfo.GetConstructor(new ITypeInfo[2]
			{
				TypeFactory.GetTypeInfo(typeof(AWSCredentials)),
				TypeFactory.GetTypeInfo(clientConfig.GetType())
			});
			return constructor.Invoke(new object[2] { originalServiceClient.Credentials, clientConfig }) as TClient;
		}

		private static ITypeInfo LoadServiceClientType(string assemblyName, string serviceClientClassName)
		{
			Assembly assembly = Assembly.Load(new AssemblyName(assemblyName));
			if (assembly == null)
			{
				throw new AmazonClientException(string.Format(CultureInfo.InvariantCulture, "Failed to find service client {0}. Be sure to include a reference to {1}.", serviceClientClassName, assemblyName));
			}
			Type type = assembly.GetType(serviceClientClassName);
			return TypeFactory.GetTypeInfo(type);
		}

		private static ClientConfig CreateServiceConfig(string assemblyName, string serviceClientClassName)
		{
			Assembly assembly = Assembly.Load(new AssemblyName(assemblyName));
			if (assembly == null)
			{
				throw new AmazonClientException(string.Format(CultureInfo.InvariantCulture, "Failed to find service client {0}. Be sure to include a reference to {1}.", serviceClientClassName, assemblyName));
			}
			Type type = assembly.GetType(serviceClientClassName.Replace("Client", "Config"));
			ConstructorInfo constructor = TypeFactory.GetTypeInfo(type).GetConstructor(new ITypeInfo[0]);
			object obj = constructor.Invoke(new object[0]);
			return obj as ClientConfig;
		}
	}
}
