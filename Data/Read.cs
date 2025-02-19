using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;
using ParseTenable.DTO;

namespace ParseTenable.Data
{
	/// <summary>
	/// Read configuration and input files
	/// </summary>
	internal static class Read
	{
		/// <summary>
		/// Reads the configuration file
		/// </summary>
		/// <returns></returns>
		public static ConfigurationFile Configuration()
		{
			var read = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "Configuration.json");

			var result = JsonConvert.DeserializeObject<ConfigurationFile>(read);

			return result;
		}

		/// <summary>
		/// Reads the Tenable vulnerabilities file
		/// </summary>
		/// <param name="file"></param>
		/// <returns></returns>
		public static List<TenableJSON> TenableVulnerabilities(string file)
		{
			var read = File.ReadAllText(file);

			var result = JsonConvert.DeserializeObject<TenableJSON[]>(read);

			return result.ToList();
		}

		/// <summary>
		/// Reads the EOL of operating systems
		/// </summary>
		/// <param name="file"></param>
		/// <returns></returns>
		public static List<OSSupportDate> OperatingSystemSupport(string file)
		{
			var result = new List<OSSupportDate>();

			using (var reader = new StreamReader(file))
			{
				reader.ReadLine();
				while (!reader.EndOfStream)
				{
					var line = reader.ReadLine();
					var columns = line.Split(';');

					for (int i = 0; i < columns.Length; i++)
					{
						columns[i] = columns[i];
					}

					var date = new OSSupportDate();
					date.OperatingSystem = columns[0];
					date.SupportDate = columns[1];

					result.Add(date);
				}
			}

			return result;
		}

		/// <summary>
		/// Reads the real operating system file
		/// </summary>
		/// <param name="file"></param>
		/// <returns></returns>
		public static List<OSRealVersion> OperatingSystemReplaceByName(string file)
		{
			var result = new List<OSRealVersion>();

			using (var reader = new StreamReader(file))
			{
				reader.ReadLine();
				while (!reader.EndOfStream)
				{
					var line = reader.ReadLine();
					var columns = line.Split(';');

					for (int i = 0; i < columns.Length; i++)
					{
						columns[i] = columns[i];
					}

					var os = new OSRealVersion();
					os.OperatingSystem = columns[0];
					os.OperatingSystemReal = columns[1];

					result.Add(os);
				}
			}

			return result;
		}

		/// <summary>
		/// Reads the Active Directory device list
		/// </summary>
		/// <param name="file"></param>
		/// <returns></returns>
		public static List<OSReplaceByAD> ActiveDirectoryDeviceList(string file)
		{
			var result = new List<OSReplaceByAD>();

			using (var reader = new StreamReader(file))
			{
				reader.ReadLine();
				reader.ReadLine();
				while (!reader.EndOfStream)
				{
					var line = reader.ReadLine();
					var columns = line.Split(',');

					for (int i = 0; i < columns.Length; i++)
					{
						columns[i] = columns[i].Replace("\"", "");
					}

					var device = new OSReplaceByAD();
					device.Name = columns[0];
					device.OperatingSystem = columns[1];
					device.OperatingSystemVersion = columns[2];
					device.Ipv4Address = columns[3];

					result.Add(device);
				}
			}

			return result;
		}

		/// <summary>
		/// Reads the exception servers file
		/// </summary>
		/// <param name="file"></param>
		/// <returns></returns>
		public static List<ExceptionServer> ExceptionServers(string file)
		{
			var result = new List<ExceptionServer>();

			using (var reader = new StreamReader(file))
			{
				reader.ReadLine();
				while (!reader.EndOfStream)
				{
					var line = reader.ReadLine();
					var columns = line.Split(';');

					for (int i = 0; i < columns.Length; i++)
					{
						columns[i] = columns[i];
					}

					var server = new ExceptionServer();
					server.Id = columns[0];
					server.Ticket = columns[1];
					server.Name = columns[2];
					server.Description = columns[3];
					server.Date = Convert.ToDateTime(columns[4]);
					server.Ip = columns[5];

					result.Add(server);
				}
			}

			return result;
		}

		/// <summary>
		/// Reads the manual input file
		/// </summary>
		/// <param name="file"></param>
		/// <returns></returns>
		public static List<OSManualnput> OSManualnputs(string file)
		{
			var result = new List<OSManualnput>();

			using (var reader = new StreamReader(file))
			{
				reader.ReadLine();
				while (!reader.EndOfStream)
				{
					var line = reader.ReadLine();
					var columns = line.Split(';');

					for (int i = 0; i < columns.Length; i++)
					{
						columns[i] = columns[i];
					}

					var server = new OSManualnput();
					server.Name = columns[0];
					server.OperatingSystem = columns[1];

					result.Add(server);
				}
			}

			return result;
		}

		/// <summary>
		/// Reads the Tenable assets file
		/// </summary>
		/// <param name="file"></param>
		/// <returns></returns>
		public static List<Assets> Assets(string file)
		{
			var result = new List<Assets>();

			using (var parser = new TextFieldParser(file))
			{
				parser.TextFieldType = FieldType.Delimited;
				parser.SetDelimiters(",");
				parser.HasFieldsEnclosedInQuotes = true;

				parser.ReadFields();

				while (!parser.EndOfData)
				{
					var columns = parser.ReadFields();

					var server = new Assets();
					server.IPv4 = columns[0];
					server.OperatingSystem = columns[1];
					server.Id = columns[2];
					server.Name = columns[3];
					result.Add(server);
				}
			}

			return result;
		}
	}
}