using System.Collections.Generic;
using Amazon.Runtime;

namespace Amazon.S3.Model
{
	public class DeleteObjectsResponse : AmazonWebServiceResponse
	{
		private List<DeletedObject> deleted = new List<DeletedObject>();

		private List<DeleteError> errors = new List<DeleteError>();

		public List<DeletedObject> DeletedObjects
		{
			get
			{
				return deleted;
			}
			set
			{
				deleted = value;
			}
		}

		public List<DeleteError> DeleteErrors
		{
			get
			{
				return errors;
			}
			set
			{
				errors = value;
			}
		}

		internal bool IsSetDeletedObjects()
		{
			return deleted.Count > 0;
		}

		internal bool IsSetDeleteErrors()
		{
			return errors.Count > 0;
		}
	}
}
