# Running customized container at Lambda

[This is the guide was used](https://docs.aws.amazon.com/lambda/latest/dg/csharp-image.html#csharp-image-clients)

I followed the guide to create a project and compared my project to the Lambda one.

The normal programa looks like:

```csharp
	internal class Program
	{
		static void Main(string[] args)
		{
			/* YOUR CODE */
		}
	}
```

But lambda program is like:

```csharp
	public class Function
	{
		private static async Task Main(string[] args)
		{
			Func<string, ILambdaContext, string> handler = FunctionHandler;
			await LambdaBootstrapBuilder.Create(handler, new DefaultLambdaJsonSerializer())
				.Build()
				.RunAsync();
		}

		public static string FunctionHandler(string input, ILambdaContext context)
		{
			/* YOUR CODE */

			return input.ToUpper();
		}
	}
```

Plus, add these libs to your project:

- Amazon.Lambda.Core
- Amazon.Lambda.RuntimeSupport
- Amazon.Lambda.Serialization.SystemTextJson

And you are good to go! Not need to install the lambda project template or anyhing else!
