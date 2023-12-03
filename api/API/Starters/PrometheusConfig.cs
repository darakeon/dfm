using System;
using Microsoft.AspNetCore.Builder;
using Prometheus;
using Prometheus.DotNetRuntime;

namespace DFM.API.Starters
{
	public class PrometheusConfig
	{
		public static void Start()
		{
			var builder = DotNetRuntimeStatsBuilder.Default();
			builder = DotNetRuntimeStatsBuilder.Customize()
				.WithContentionStats(CaptureLevel.Informational)
				.WithGcStats(CaptureLevel.Verbose)
				.WithThreadPoolStats(CaptureLevel.Informational)
				.WithExceptionStats(CaptureLevel.Errors)
				.WithJitStats();

			builder.RecycleCollectorsEvery(new TimeSpan(0, 20, 0));

			var collector = builder.StartCollecting();
		}

		public static void UseMetrics(IApplicationBuilder app)
		{
			app.UseHttpMetrics();
			app.UseMetricServer();
		}
	}
}
