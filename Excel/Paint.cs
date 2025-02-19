using OfficeOpenXml.Style;
using OfficeOpenXml;
using System.Drawing;
using static ParseTenable.PrepareData.Severities;

namespace ParseTenable.Excel
{
    /// <summary>
    /// Paints cells
    /// </summary>
    internal static class Paint
    {
        /// <summary>
        /// Paints a cell
        /// </summary>
        /// <param name="worksheet"></param>
        /// <param name="index"></param>
        /// <param name="row"></param>
        /// <param name="fontColor"></param>
        /// <param name="backColor"></param>
        public static void Cell(ExcelWorksheet worksheet, int index, int row, Color fontColor, Color backColor)
        {
            worksheet.Cells[index + 1, row].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[index + 1, row].Style.Fill.BackgroundColor.SetColor(backColor);
            worksheet.Cells[index + 1, row].Style.Font.Color.SetColor(fontColor);
        }

        /// <summary>
        /// Paints the EOLDays cell
        /// </summary>
        /// <param name="worksheet"></param>
        /// <param name="eolDaysRow"></param>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public static void EOLDays(ExcelWorksheet worksheet, int eolDaysRow, int index, string value, Color possitive, Color negative)
        {
            var backColor = Color.White;
            var fontColor = Color.White;
            var parseBool = int.TryParse(value, out int parseInt);

            if (parseBool)
            {
                if (parseInt > 0)
                {
                    backColor = possitive;
                }
                else
                {
                    backColor = negative;
                }
            }

            Cell(worksheet, index, eolDaysRow, fontColor, backColor);
        }

        /// <summary>
        /// Paints the severity cell
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public static void Severities(ExcelWorksheet worksheet, int row, int index, Severity value, Color critical, Color high, Color medium, Color low, Color foreBright, Color foreDark)
        {
            var backColor = Color.White;
            var fontColor = Color.White;

            switch (value)
            {
                case Severity.Bajo:
                    backColor = low;
                    fontColor = foreBright;
                    break;
                case Severity.Medio:
                    backColor = medium;
                    fontColor = foreDark;
                    break;
                case Severity.Alto:
                    backColor = high;
                    fontColor = foreBright;
                    break;
                case Severity.Crítico:
                    backColor = critical;
                    fontColor = foreBright;
                    break;
                default:
                    break;
            }

            Cell(worksheet, index, row, fontColor, backColor);
        }
    }
}
