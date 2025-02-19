namespace ParseTenable.DTO
{
	internal class ConfigurationFile
	{
		public string TenableVulnerabilities { get; set; }
		public string TenableAssets { get; set; }
		public string OSRealVersion { get; set; }
		public string OSSupportDates { get; set; }
		public string DeviceList { get; set; }
		public string ExceptionServers { get; set; }
		public string OSManualInput { get; set; }
		public string OutputFileSufix { get; set; }
		public string OutputFolder { get; set; }
		public string UnknownOperatingSystem { get; set; }

		public Colors_ Colors { get; set; }

		public class Colors_
		{
			public Severity_ Severity { get; set; }
			public Eol_ Eol { get; set; }
			public class Severity_
			{
				public string Critical { get; set; }
				public string High { get; set; }
				public string Medium { get; set; }
				public string Low { get; set; }
				public string ForeBright { get; set; }
				public string ForeDark { get; set; }

			}
			public class Eol_
			{
				public string Positive { get; set; }
				public string Negative { get; set; }

			}
		}
	}
}
