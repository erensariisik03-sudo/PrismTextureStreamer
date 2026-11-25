using System;
using System.Collections.Generic;
using System.IO;

namespace Amazon.Runtime.SharedInterfaces
{
	public interface ICoreAmazonS3
	{
		string GeneratePreSignedURL(string bucketName, string objectKey, DateTime expiration, IDictionary<string, object> additionalProperties);

		IList<string> GetAllObjectKeys(string bucketName, string prefix, IDictionary<string, object> additionalProperties);

		void Delete(string bucketName, string objectKey, IDictionary<string, object> additionalProperties);

		void Deletes(string bucketName, IEnumerable<string> objectKeys, IDictionary<string, object> additionalProperties);

		void UploadObjectFromStream(string bucketName, string objectKey, Stream stream, IDictionary<string, object> additionalProperties);

		void UploadObjectFromFilePath(string bucketName, string objectKey, string filepath, IDictionary<string, object> additionalProperties);

		void DownloadToFilePath(string bucketName, string objectKey, string filepath, IDictionary<string, object> additionalProperties);

		Stream GetObjectStream(string bucketName, string objectKey, IDictionary<string, object> additionalProperties);

		void MakeObjectPublic(string bucketName, string objectKey, bool enable);

		void EnsureBucketExists(string bucketName);

		bool DoesS3BucketExist(string bucketName);

		IAsyncResult BeginDelete(string bucketName, string objectKey, IDictionary<string, object> additionalProperties, AsyncCallback callback, object state);

		void EndDelete(IAsyncResult result);

		IAsyncResult BeginUploadObjectFromStream(string bucketName, string objectKey, Stream stream, IDictionary<string, object> additionalProperties, AsyncCallback callback, object state);

		void EndUploadObjectFromStream(IAsyncResult result);

		IAsyncResult BeginUploadObjectFromFilePath(string bucketName, string objectKey, string filepath, IDictionary<string, object> additionalProperties, AsyncCallback callback, object state);

		void EndUploadObjectFromFilePath(IAsyncResult result);

		IAsyncResult BeginDownloadToFilePath(string bucketName, string objectKey, string filepath, IDictionary<string, object> additionalProperties, AsyncCallback callback, object state);

		void EndDownloadToFilePath(IAsyncResult result);

		IAsyncResult BeginGetObjectStream(string bucketName, string objectKey, IDictionary<string, object> additionalProperties, AsyncCallback callback, object state);

		Stream EndGetObjectStream(IAsyncResult result);
	}
}
