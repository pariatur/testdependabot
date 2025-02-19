using OfficeOpenXml;
using OfficeOpenXml.Style;
using ParseTenable.DTO;
using ParseTenable.Excel;
using ParseTenable.Extensions;
using ParseTenable.PrepareData;
using System.Drawing;
using static ParseTenable.PrepareData.Severities;

namespace ParseTenable
{
	/// <summary>
	/// Main routine
	/// </summary>
	internal class Parser
	{
		// Input from files
		List<TenableJSON> _tenableVulnerabilities;
		List<OSSupportDate> _osSupport;
		List<OSRealVersion> _osRealVersion;
		List<OSReplaceByAD> _osReplaceByAD;
		List<DTO.ExceptionServer> _exceptionServers;
		List<OSManualnput> _osManualInput;
		List<Assets> _assets;
		string _outputFilePath;
		string _outputFilePrefix;
		string _eolUnkown;

		// Excel related variables
		ExcelPackage _excelFile;
		ExcelWorksheet _worksheetVulnerabilities;
		ExcelWorksheet _worksheetAssets;
		List<List<string>> _contentVulnerabilities;
		List<List<string>> _contentAssets;

		// Excel related constants
		const int _vulnerabilityRowSeverity = 9;
		const int _vulnerabilityRowEOLDays = 7;
		const int _assetRowEOLDays = 6;
		readonly int[] _vulnerabilityRowIsNumber = new int[] { 3, 7 };
		readonly int[] _assetRowIsNumber = new int[] { 6, 8, 9, 10, 11 };

		// Colors for painting cells
		Color _colorSeverityCritical;
		Color _colorSeverityHigh;
		Color _colorSeverityMedium;
		Color _colorSeverityLow;
		Color _colorSeverityForeBright;
		Color _colorSeverityForeDark;
		Color _colorEOLPossitive = Color.Blue;
		Color _colorEOLNegative = Color.Red;

		public void Read()
		{
			var configurationFile = Data.Read.Configuration();
			_tenableVulnerabilities = Data.Read.TenableVulnerabilities(configurationFile.TenableVulnerabilities);
			_osSupport = Data.Read.OperatingSystemSupport(configurationFile.OSSupportDates);
			_osRealVersion = Data.Read.OperatingSystemReplaceByName(configurationFile.OSRealVersion);
			_osReplaceByAD = Data.Read.ActiveDirectoryDeviceList(configurationFile.DeviceList);
			_exceptionServers = Data.Read.ExceptionServers(configurationFile.ExceptionServers);
			_osManualInput = Data.Read.OSManualnputs(configurationFile.OSManualInput);
			_assets = Data.Read.Assets(configurationFile.TenableAssets);
			_outputFilePath = configurationFile.OutputFolder;
			_outputFilePrefix = configurationFile.OutputFileSufix;
			_eolUnkown = configurationFile.UnknownOperatingSystem;

			_colorSeverityCritical = ColorTranslator.FromHtml(configurationFile.Colors.Severity.Critical);
			_colorSeverityHigh = ColorTranslator.FromHtml(configurationFile.Colors.Severity.High);
			_colorSeverityMedium = ColorTranslator.FromHtml(configurationFile.Colors.Severity.Medium);
			_colorSeverityLow = ColorTranslator.FromHtml(configurationFile.Colors.Severity.Low);
			_colorSeverityForeBright = ColorTranslator.FromHtml(configurationFile.Colors.Severity.ForeBright);
			_colorSeverityForeDark = ColorTranslator.FromHtml(configurationFile.Colors.Severity.ForeDark);
			_colorEOLPossitive = ColorTranslator.FromHtml(configurationFile.Colors.Eol.Positive);
			_colorEOLNegative = ColorTranslator.FromHtml(configurationFile.Colors.Eol.Negative);
		}

		/// <summary>
		/// Prepares the vulnerabilities data
		/// </summary>
		public void PrepareVulnerabilities()
		{
			_contentVulnerabilities = new List<List<string>>();
			var header = new List<string>
			{
				"Nombre",
				"IP",
				"Puerto",
				"Sistema operativo",
				"SO soportado",
				"SO EOL",
				"Días EOL",
				"Servidor con excepción",
				"Riesgo",
				"ID",
				"Tipo",
				"Vulnerabilidad",
				"Resumen",
				"Solución",
			};

			_contentVulnerabilities.Add(header);

			foreach (var item in _tenableVulnerabilities)
			{
				var certificado = false;
				if (item.definition == null)
				{
					throw new Exception("No existe apartado 'definition' para el item ID:" + item.id);
				}

                if (item.definition.synopsis == null)
                {
                    throw new Exception("No existe apartado 'synopsis' para el item ID:" + item.id);
                }

                var synopsisLowerCase = item.definition.synopsis.ToLower();
				var solutionLowerCase = "";
				if (!string.IsNullOrEmpty(item.definition.solution))
				{
					solutionLowerCase = item.definition.solution.ToLower();
				}

				var nameLowerCase = item.definition.name.ToLower();
				var actualización =
					   synopsisLowerCase.Contains("update")
					|| nameLowerCase.Contains("fix pack")
					|| solutionLowerCase.Contains("fix pack")
					|| solutionLowerCase.Contains("upgrade")
					|| solutionLowerCase.Contains("upgrading")
					|| solutionLowerCase.Contains("update")
					|| solutionLowerCase.Contains("apply the appropriate patch");

				if (!actualización)
				{
					certificado =
						   synopsisLowerCase.Contains("ssl")
						|| synopsisLowerCase.Contains("tls")
						|| synopsisLowerCase.Contains("ciphers")
						|| nameLowerCase.Contains("ssl")
						|| nameLowerCase.Contains("tls")
						|| nameLowerCase.Contains("ciphers");
				}

				var actualizaciónOS =
					   nameLowerCase.Contains("unsupported windows os (remote)")
					   || nameLowerCase.Contains("windows 7")
					   || nameLowerCase.Contains("unix operating system unsupported version detection");

				if (actualizaciónOS)
				{
					actualización = false;
				}

				var otro = !actualización && !certificado && !actualizaciónOS;

				var tipo = string.Empty;

				if (actualización)
				{
					tipo = "Actualización";
				}
				else if (actualizaciónOS)
				{
					tipo = "Actualización OS";
				}
				else if (certificado)
				{
					tipo = "Certificado";
				}
				else if (otro)
				{
					tipo = "Otro";
				}

				var realOS = ReplaceOS.OS(item.asset.name, item.asset.operating_system, _osReplaceByAD, _osRealVersion, _osManualInput);

				// Add EOL
				var eol = _osSupport.FirstOrDefault(x => x.OperatingSystem == realOS);
				Eol.Handle(eol, _eolUnkown, out var eolDate, out var eolSupported, out var daysUntilEOLString);

				// Add server exception value
				var exceptionServerValue = PrepareData.ExceptionServer.ServerHasException(item.asset.display_ipv4_address, _exceptionServers);

				var test = item.definition.solution == null ? "Tenable doesn't declare a solution for this, probable it's an outdated plugin.": item.definition.solution;
				// Create the row
				var line = new List<string>
				{
					item.asset.name,
					item.asset.display_ipv4_address,
					item.port.ToString(),
					realOS,
					eolSupported,
					eolDate,
					daysUntilEOLString,
					exceptionServerValue.ToSN(),
					item.definition.severity.ToString(),
					item.id,
					tipo,
					item.definition.name,
					item.definition.synopsis,
					test.Replace("\n","")
				};

				_contentVulnerabilities.Add(line);
			}
		}

		/// <summary>
		/// Prepares the assets data
		/// </summary>
		public void PrepareAssets()
		{
			_contentAssets = new List<List<string>>();
			var header = new List<string>
			{
				"Nombre",
				"IP",
				"Sistema operativo",
				"SO soportado",
				"SO EOL",
				"Días EOL",
				"Servidor con excepción",
				"Vulnerabilidades: Bajo",
				"Vulnerabilidades: Medio",
				"Vulnerabilidades: Alto",
				"Vulnerabilidades: Crítico"
			};

			_contentAssets.Add(header);
			foreach (var item in _assets)
			{
				var realOS = ReplaceOS.OS(item.Name, item.OperatingSystem, _osReplaceByAD, _osRealVersion, _osManualInput);

				// Add EOL
				var eol = _osSupport.FirstOrDefault(x => x.OperatingSystem == realOS);
				Eol.Handle(eol, _eolUnkown, out var eolDate, out var eolSupported, out var daysUntilEOLString);

				// Add server exception value
				var exceptionServerValue = PrepareData.ExceptionServer.ServerHasException(item.IPv4, _exceptionServers);

				var bajo = GetVulnerabilitiesBySeverity(item.Name, Severity.Bajo, _tenableVulnerabilities);
				var medio = GetVulnerabilitiesBySeverity(item.Name, Severity.Medio, _tenableVulnerabilities);
				var alto = GetVulnerabilitiesBySeverity(item.Name, Severity.Alto, _tenableVulnerabilities);
				var crítico = GetVulnerabilitiesBySeverity(item.Name, Severity.Crítico, _tenableVulnerabilities);

				// Create the row
				var line = new List<string>
				{
					item.Name,
					item.IPv4,
					realOS,
					eolSupported,
					eolDate,
					daysUntilEOLString,
					exceptionServerValue.ToSN(),
					bajo.ToString(),
					medio.ToString(),
					alto.ToString(),
					crítico.ToString()
				};

				_contentAssets.Add(line);
			}
		}

		/// <summary>
		/// With all the data ready, this creates the excel file
		/// </summary>
		public void ExportToExcel()
		{
			// Set the text and background colors for the first row
			var rowNumberTop = 1;
			var backColor = Color.Black;
			var foreColor = Color.White;

			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
			_excelFile = new ExcelPackage();

			// Vulnerabilities
			_worksheetVulnerabilities = _excelFile.Workbook.Worksheets.Add("Vulnerabilidades");

			for (int i = 0; i < _contentVulnerabilities.Count; i++)
			{
				for (int j = 0; j < _contentVulnerabilities[i].Count; j++)
				{
					var isNumber = _vulnerabilityRowIsNumber.Contains(j + 1);

					if (i > 0 && j == _vulnerabilityRowSeverity - 1)
					{
						Paint.Severities(
							_worksheetVulnerabilities,
							_vulnerabilityRowSeverity,
							i,
							(Severity)Convert.ToInt32(_contentVulnerabilities[i][j]),
							_colorSeverityCritical,
							_colorSeverityHigh,
							_colorSeverityMedium,
							_colorSeverityLow,
							_colorSeverityForeBright,
							_colorSeverityForeDark);

						_worksheetVulnerabilities.Cells[i + 1, j + 1].Value = SeverityNumberToString((Severity)Convert.ToInt32(_contentVulnerabilities[i][j]));
						continue;
					}
					else if (i > 0 && j == _vulnerabilityRowEOLDays - 1)
					{
						Paint.EOLDays(_worksheetVulnerabilities, _vulnerabilityRowEOLDays, i, _contentVulnerabilities[i][j], _colorEOLPossitive, _colorEOLNegative);
					}

					if (i > 0 && isNumber)
					{
						var isInt = int.TryParse(_contentVulnerabilities[i][j], out int intValue);

						if (isInt)
						{
							_worksheetVulnerabilities.Cells[i + 1, j + 1].Value = intValue;
						}
						else
						{
							_worksheetVulnerabilities.Cells[i + 1, j + 1].Value = 0;
						}
					}
					else
					{
						_worksheetVulnerabilities.Cells[i + 1, j + 1].Value = _contentVulnerabilities[i][j];
					}
				}
			}

			//_worksheetVulnerabilities.Column(4).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
			_worksheetVulnerabilities.Cells[1, 1, 1, _worksheetVulnerabilities.Dimension.End.Column].AutoFilter = true;
			_worksheetVulnerabilities.Cells[_worksheetVulnerabilities.Dimension.Address].AutoFitColumns();
			_worksheetVulnerabilities.Row(rowNumberTop).Style.Fill.PatternType = ExcelFillStyle.Solid;
			_worksheetVulnerabilities.Row(rowNumberTop).Style.Fill.BackgroundColor.SetColor(backColor);
			_worksheetVulnerabilities.Row(rowNumberTop).Style.Font.Color.SetColor(foreColor);

			_worksheetVulnerabilities.View.FreezePanes(2, 1);

			PivotTable.Create(_excelFile, _worksheetVulnerabilities, "Vulnerabilidades;Tipo", "Tipo", "Vulnerabilidades por tipo", "B3:B6", "A3:A6");
			PivotTable.Create(_excelFile, _worksheetVulnerabilities, "Vulnerabilidades;Riesgo", "Riesgo", "Vulnerabilidades por riesgo", "B3:B6", "A3:A6");

			// Assets
			_worksheetAssets = _excelFile.Workbook.Worksheets.Add("Servidores");

			for (int i = 0; i < _contentAssets.Count; i++)
			{
				for (int j = 0; j < _contentAssets[i].Count; j++)
				{
					if (i > 0 && j == _assetRowEOLDays - 1)
					{
						Paint.EOLDays(_worksheetAssets, _assetRowEOLDays, i, _contentAssets[i][j], _colorEOLPossitive, _colorEOLNegative);
					}

					if (i > 0 && _assetRowIsNumber.Contains(j + 1))
					{

						var isInt = int.TryParse(_contentAssets[i][j], out int intValue);

						if (isInt)
						{
							_worksheetAssets.Cells[i + 1, j + 1].Value = intValue;
						}
						else
						{
							_worksheetAssets.Cells[i + 1, j + 1].Value = 0;
						}
					}
					else
					{
						_worksheetAssets.Cells[i + 1, j + 1].Value = _contentAssets[i][j];
					}
				}
			}

			// Assets
			_worksheetAssets.Cells[1, 1, 1, _worksheetAssets.Dimension.End.Column].AutoFilter = true;
			_worksheetAssets.Cells[_worksheetAssets.Dimension.Address].AutoFitColumns();
			_worksheetAssets.Row(rowNumberTop).Style.Fill.PatternType = ExcelFillStyle.Solid;
			_worksheetAssets.Row(rowNumberTop).Style.Fill.BackgroundColor.SetColor(backColor);
			_worksheetAssets.Row(rowNumberTop).Style.Font.Color.SetColor(foreColor);

			_worksheetAssets.View.FreezePanes(2, 1);

			PivotTable.Create(_excelFile, _worksheetAssets, "Servidores;Soporte", "SO soportado", "Servidores con soporte", "B3:B4", "A3:A4");

			_excelFile.SaveAs(_outputFilePath + "\\" + _outputFilePrefix + ";" + DateTime.Now.ToShortDateString() + ".xlsx");
		}
	}
}
