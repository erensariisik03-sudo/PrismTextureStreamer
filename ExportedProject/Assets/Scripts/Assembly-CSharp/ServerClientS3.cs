using System;
using System.Collections;
using System.IO;
using Amazon;
using Amazon.CognitoIdentity;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using UnityEngine;
using UnityEngine.UI;

public class ServerClientS3 : MonoBehaviour
{
	private string imageurl = "s3-eu-west-1.amazonaws.com/axesinmotion/picturePublic.jpg";

	public Texture2D thumbnail;

	public Image image;

	public void button()
	{
		Debug.Log("pulsado");
	}

	private void Start()
	{
	}

	public void GetListBuckets()
	{
		string data = null;
		string identityPoolId = "eu-west-1:3f9ef3d5-1db0-44d1-8e2b-5b147afa991f";
		RegionEndpoint eUWest = RegionEndpoint.EUWest1;
		CognitoAWSCredentials credentials = new CognitoAWSCredentials(identityPoolId, eUWest);
		AmazonS3Client amazonS3Client = new AmazonS3Client(credentials, eUWest);
		data = "Fetching all the Buckets";
		amazonS3Client.ListBucketsAsync(new ListBucketsRequest(), (AmazonServiceResult<ListBucketsRequest, ListBucketsResponse> responseObject) =>
		{
			data += "\n";
			if (responseObject.Exception == null)
			{
				data += "Got Response \nPrinting now \n";
				responseObject.Response.Buckets.ForEach((S3Bucket s3b) =>
				{
					data += string.Format("bucket = {0}, created date = {1} \n", s3b.BucketName, s3b.CreationDate);
				});
			}
			else
			{
				data += "Got Exception \n";
			}
		});
		Debug.Log("RESULTADO:\n" + data);
	}

	private void GetListObjects()
	{
		string data = null;
		string text = "extremecardrivingsimulator";
		string identityPoolId = "eu-west-1:523f8efe-c91f-41fb-b817-40bc859f9447";
		RegionEndpoint eUWest = RegionEndpoint.EUWest1;
		CognitoAWSCredentials credentials = new CognitoAWSCredentials(identityPoolId, eUWest);
		AmazonS3Client amazonS3Client = new AmazonS3Client(credentials, eUWest);
		data = "Fetching all the Objects from " + text;
		ListObjectsRequest listObjectsRequest = new ListObjectsRequest();
		listObjectsRequest.BucketName = text;
		ListObjectsRequest request = listObjectsRequest;
		amazonS3Client.ListObjectsAsync(request, (AmazonServiceResult<ListObjectsRequest, ListObjectsResponse> responseObject) =>
		{
			AsyncCallback(responseObject);
			data += "\n";
			if (responseObject.Exception == null)
			{
				data += "Got Response \nPrinting now \n";
				responseObject.Response.S3Objects.ForEach((S3Object o) =>
				{
					data += string.Format("{0}\n", o.Key);
				});
			}
			else
			{
				data += "Got Exception \n";
			}
		});
	}

	private void AsyncCallback(AmazonServiceResult<ListObjectsRequest, ListObjectsResponse> responseObject)
	{
		string data = string.Empty;
		if (responseObject.Exception == null)
		{
			data += "Got Response \nPrinting now \n";
			responseObject.Response.S3Objects.ForEach((S3Object o) =>
			{
				data += string.Format("{0}\n", o.Key);
			});
		}
		else
		{
			data += "Got Exception \n";
		}
		Debug.LogWarning("RESULTADO:\n" + data);
	}

	public void LoadJson(string ID, string bucket, string file, Func<string, string> Callback)
	{
		string data = null;
		RegionEndpoint eUWest = RegionEndpoint.EUWest1;
		CognitoAWSCredentials credentials = new CognitoAWSCredentials(ID, eUWest);
		AmazonS3Client amazonS3Client = new AmazonS3Client(credentials, eUWest);
		amazonS3Client.GetObjectAsync(bucket, file, (AmazonServiceResult<GetObjectRequest, GetObjectResponse> responseObj) =>
		{
			GetObjectResponse response = responseObj.Response;
			if (response != null && response.ResponseStream != null)
			{
				using (StreamReader streamReader = new StreamReader(response.ResponseStream))
				{
					data = streamReader.ReadToEnd();
					Callback(data);
				}
			}
		});
	}

	private string AsyncCallback(string s)
	{
		Debug.Log(s);
		return s;
	}

	public void DownloadImage()
	{
		StartCoroutine(coDownloadImage(imageurl));
	}

	private IEnumerator coDownloadImage(string imageUrl)
	{
		WWW www = new WWW(imageUrl);
		yield return www;
		thumbnail = new Texture2D(www.texture.width, www.texture.height, TextureFormat.DXT1, false);
		www.LoadImageIntoTexture(thumbnail);
		www.Dispose();
		www = null;
		Sprite sp = Sprite.Create(thumbnail, new Rect(0f, 0f, thumbnail.width, thumbnail.height), new Vector2(0.5f, 0.5f));
		image.sprite = sp;
	}
}
