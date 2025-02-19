using ParseTenable.DTO;
using ParseTenable.Extensions;
using System.Globalization;

namespace ParseTenable.PrepareData
{
	internal static class Eol
	{
		/// <summary>
		/// Creates all the EOL related variables
		/// </summary>
		/// <param name="eol"></param>
		/// <param name="eolDate"></param>
		/// <param name="eolSupported"></param>
		/// <param name="daysUntilEOLString"></param>
		public static void Handle(OSSupportDate? eol, string unkownString, out string eolDate, out string eolSupported, out string daysUntilEOLString)
		{
			eolDate = string.Empty;
			daysUntilEOLString = string.Empty;
			eolSupported = unkownString;

			TimeSpan? daysUntilEOL = null;

			if (eol != null)
			{
				eolDate = eol.SupportDate;

				if (eol.SupportDate == null)
				{
					eolDate = unkownString;
				}
				else
				{
					var eolDateFormat = DateTime.ParseExact(eolDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);
					daysUntilEOL = DateTime.ParseExact(eolDate, "dd-MM-yyyy", CultureInfo.InvariantCulture) - DateTime.Now;
					eolSupported = (DateTime.Now <= eolDateFormat).ToSN();
				}
			}

			if (daysUntilEOL != null)
			{
				var daysTimeSpan = (TimeSpan)daysUntilEOL;
				daysUntilEOLString = daysTimeSpan.Days.ToString();
			}
		}
	}
}
