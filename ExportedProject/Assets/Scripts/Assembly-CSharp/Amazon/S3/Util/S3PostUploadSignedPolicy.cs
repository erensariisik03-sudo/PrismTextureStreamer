using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using Amazon.Runtime;
using Amazon.Util;
using ThirdParty.Json.LitJson;

namespace Amazon.S3.Util
{
	[XmlRoot(IsNullable = false)]
	public class S3PostUploadSignedPolicy
	{
		private static string KEY_POLICY = "policy";

		private static string KEY_SIGNATURE = "signature";

		private static string KEY_ACCESSKEY = "access_key";

		public string Policy { get; set; }

		public string Signature { get; set; }

		public string AccessKeyId { get; set; }

		public string SecurityToken { get; set; }

		public static S3PostUploadSignedPolicy GetSignedPolicy(string policy, AWSCredentials credentials)
		{
			ImmutableCredentials credentials2 = credentials.GetCredentials();
			byte[] inArray = ((!credentials2.UseToken) ? Encoding.UTF8.GetBytes(policy.Trim()) : addTokenToPolicy(policy, credentials2.Token));
			string text = Convert.ToBase64String(inArray);
			string signature = CryptoUtilFactory.CryptoInstance.HMACSign(Encoding.UTF8.GetBytes(text), credentials2.SecretKey, SigningAlgorithm.HmacSHA1);
			S3PostUploadSignedPolicy s3PostUploadSignedPolicy = new S3PostUploadSignedPolicy();
			s3PostUploadSignedPolicy.Policy = text;
			s3PostUploadSignedPolicy.Signature = signature;
			s3PostUploadSignedPolicy.AccessKeyId = credentials2.AccessKey;
			s3PostUploadSignedPolicy.SecurityToken = credentials2.Token;
			return s3PostUploadSignedPolicy;
		}

		private static byte[] addTokenToPolicy(string policy, string token)
		{
			JsonData jsonData = JsonMapper.ToObject(new JsonReader(policy));
			bool flag = false;
			JsonData jsonData2 = jsonData["conditions"];
			if (jsonData2 != null && jsonData2.IsArray)
			{
				for (int i = 0; i < jsonData2.Count; i++)
				{
					JsonData jsonData3 = jsonData2[i];
					if (jsonData3.IsObject && jsonData3[S3Constants.PostFormDataSecurityToken] != null)
					{
						jsonData3[S3Constants.PostFormDataSecurityToken] = token;
						flag = true;
					}
				}
				if (!flag)
				{
					JsonData jsonData4 = new JsonData();
					jsonData4.SetJsonType(JsonType.Object);
					jsonData4[S3Constants.PostFormDataSecurityToken] = token;
					jsonData2.Add(jsonData4);
				}
			}
			return Encoding.UTF8.GetBytes(JsonMapper.ToJson(jsonData).Trim());
		}

		public string GetReadablePolicy()
		{
			return Encoding.UTF8.GetString(Convert.FromBase64String(Policy));
		}

		public string ToJson()
		{
			JsonData jsonData = new JsonData();
			jsonData[KEY_POLICY] = Policy;
			jsonData[KEY_SIGNATURE] = Signature;
			jsonData[KEY_ACCESSKEY] = AccessKeyId;
			return JsonMapper.ToJson(jsonData);
		}

		public string ToXml()
		{
			StringBuilder stringBuilder = new StringBuilder(1024);
			XmlSerializer xmlSerializer = new XmlSerializer(GetType());
			using (StringWriter textWriter = new StringWriter(stringBuilder, CultureInfo.InvariantCulture))
			{
				xmlSerializer.Serialize(textWriter, this);
			}
			return stringBuilder.ToString();
		}

		public static S3PostUploadSignedPolicy GetSignedPolicyFromJson(string policyJson)
		{
			JsonData jsonData;
			try
			{
				jsonData = JsonMapper.ToObject(policyJson);
			}
			catch (Exception innerException)
			{
				throw new ArgumentException("Invalid JSON document", innerException);
			}
			if (jsonData[KEY_POLICY] == null || !jsonData[KEY_POLICY].IsString)
			{
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "JSON document requires '{0}' field"), KEY_POLICY);
			}
			if (jsonData[KEY_SIGNATURE] == null || !jsonData[KEY_SIGNATURE].IsString)
			{
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "JSON document requires '{0}' field"), KEY_SIGNATURE);
			}
			if (jsonData[KEY_ACCESSKEY] == null || !jsonData[KEY_ACCESSKEY].IsString)
			{
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "JSON document requires '{0}' field"), KEY_ACCESSKEY);
			}
			S3PostUploadSignedPolicy s3PostUploadSignedPolicy = new S3PostUploadSignedPolicy();
			s3PostUploadSignedPolicy.Policy = jsonData[KEY_POLICY].ToString();
			s3PostUploadSignedPolicy.Signature = jsonData[KEY_SIGNATURE].ToString();
			s3PostUploadSignedPolicy.AccessKeyId = jsonData[KEY_ACCESSKEY].ToString();
			return s3PostUploadSignedPolicy;
		}

		public static S3PostUploadSignedPolicy GetSignedPolicyFromXml(string policyXml)
		{
			StringReader textReader = new StringReader(policyXml);
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(S3PostUploadSignedPolicy));
			S3PostUploadSignedPolicy s3PostUploadSignedPolicy;
			try
			{
				s3PostUploadSignedPolicy = xmlSerializer.Deserialize(textReader) as S3PostUploadSignedPolicy;
			}
			catch (Exception innerException)
			{
				throw new ArgumentException("Could not parse XML", innerException);
			}
			if (string.IsNullOrEmpty(s3PostUploadSignedPolicy.AccessKeyId))
			{
				throw new ArgumentException("XML Document requries 'AccessKeyId' field");
			}
			if (string.IsNullOrEmpty(s3PostUploadSignedPolicy.Policy))
			{
				throw new ArgumentException("XML Document requries 'Policy' field");
			}
			if (string.IsNullOrEmpty(s3PostUploadSignedPolicy.Signature))
			{
				throw new ArgumentException("XML Document requries 'Signature' field");
			}
			return s3PostUploadSignedPolicy;
		}
	}
}
