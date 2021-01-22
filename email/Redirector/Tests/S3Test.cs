namespace Redirector.Tests
{
	class S3Test
	{
		public static void Test()
		{
			var s3 = new S3();
			s3.GetFile("AMAZON_SES_SETUP_NOTIFICATION");
			s3.DeleteFile("AMAZON_SES_SETUP_NOTIFICATION");
		}
	}
}
