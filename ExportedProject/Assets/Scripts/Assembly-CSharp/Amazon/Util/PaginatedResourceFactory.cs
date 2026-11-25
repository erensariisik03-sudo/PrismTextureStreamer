using System;
using System.Collections.Generic;
using System.Reflection;
using Amazon.Util.Internal;

namespace Amazon.Util
{
	public static class PaginatedResourceFactory
	{
		public static object Create<TItemType, TRequestType, TResponseType>(PaginatedResourceInfo pri)
		{
			pri.Verify();
			return Create<TItemType, TRequestType, TResponseType>(pri.Client, pri.MethodName, pri.Request, pri.TokenRequestPropertyPath, pri.TokenResponsePropertyPath, pri.ItemListPropertyPath);
		}

		private static PaginatedResource<ItemType> Create<ItemType, TRequestType, TResponseType>(object client, string methodName, object request, string tokenRequestPropertyPath, string tokenResponsePropertyPath, string itemListPropertyPath)
		{
			ITypeInfo typeInfo = TypeFactory.GetTypeInfo(client.GetType());
			MethodInfo fetcherMethod = typeInfo.GetMethod(methodName, new ITypeInfo[1] { TypeFactory.GetTypeInfo(typeof(TRequestType)) });
			Func<TRequestType, TResponseType> call = (TRequestType req) => (TResponseType)fetcherMethod.Invoke(client, new object[1] { req });
			return Create<ItemType, TRequestType, TResponseType>(call, (TRequestType)request, tokenRequestPropertyPath, tokenResponsePropertyPath, itemListPropertyPath);
		}

		private static PaginatedResource<ItemType> Create<ItemType, TRequestType, TResponseType>(Func<TRequestType, TResponseType> call, TRequestType request, string tokenRequestPropertyPath, string tokenResponsePropertyPath, string itemListPropertyPath)
		{
			Func<string, Marker<ItemType>> fetcher = (string token) =>
			{
				SetPropertyValueAtPath(request, tokenRequestPropertyPath, token);
				TResponseType val = call(request);
				string propertyValueFromPath = GetPropertyValueFromPath<string>(val, tokenResponsePropertyPath);
				List<ItemType> propertyValueFromPath2 = GetPropertyValueFromPath<List<ItemType>>(val, itemListPropertyPath);
				return new Marker<ItemType>(propertyValueFromPath2, propertyValueFromPath);
			};
			return new PaginatedResource<ItemType>(fetcher);
		}

		private static void SetPropertyValueAtPath(object instance, string path, string value)
		{
			string[] array = path.Split('.');
			object obj = instance;
			Type type = instance.GetType();
			PropertyInfo propertyInfo = null;
			int i;
			for (i = 0; i < array.Length - 1; i++)
			{
				string name = array[i];
				propertyInfo = TypeFactory.GetTypeInfo(type).GetProperty(name);
				obj = propertyInfo.GetValue(obj, null);
				type = propertyInfo.PropertyType;
			}
			propertyInfo = TypeFactory.GetTypeInfo(type).GetProperty(array[i]);
			propertyInfo.SetValue(obj, value, null);
		}

		private static T GetPropertyValueFromPath<T>(object instance, string path)
		{
			string[] array = path.Split('.');
			object obj = instance;
			Type type = instance.GetType();
			PropertyInfo propertyInfo = null;
			string[] array2 = array;
			foreach (string name in array2)
			{
				propertyInfo = TypeFactory.GetTypeInfo(type).GetProperty(name);
				obj = propertyInfo.GetValue(obj, null);
				type = propertyInfo.PropertyType;
			}
			return (T)obj;
		}

		internal static Type GetPropertyTypeFromPath(Type start, string path)
		{
			string[] array = path.Split('.');
			Type type = start;
			PropertyInfo propertyInfo = null;
			string[] array2 = array;
			foreach (string name in array2)
			{
				propertyInfo = TypeFactory.GetTypeInfo(type).GetProperty(name);
				type = propertyInfo.PropertyType;
			}
			return type;
		}

		private static Type GetFuncType<T, U>()
		{
			return typeof(Func<T, U>);
		}

		internal static T Cast<T>(object o)
		{
			return (T)o;
		}
	}
}
