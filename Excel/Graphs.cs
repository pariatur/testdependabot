using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;
using System.Drawing;

namespace ParseTenable.Excel
{
    /// <summary>
    /// Excel graphs
    /// </summary>
    internal static class Graphs
    {
        /// <summary>
        /// Creates a graph
        /// </summary>
        /// <param name="worksheet"></param>
        /// <param name="text"></param>
        /// <param name="serie"></param>
        /// <param name="xSerie"></param>
        /// <returns></returns>
        public static ExcelWorksheet Create(ExcelWorksheet worksheet, string text, string serie, string xSerie)
        {
            var chart = worksheet.Drawings.AddPieChart("", ePieChartType.Pie3D);

            var series = chart.Series.Add(worksheet.Cells[serie], worksheet.Cells[xSerie]);
            var pieSeries = series;

            pieSeries.DataLabel.Font.Bold = true;
            pieSeries.DataLabel.Font.Fill.Color = Color.Black;
            pieSeries.DataLabel.ShowValue = true;
            pieSeries.DataLabel.ShowPercent = true;
            pieSeries.DataLabel.ShowLeaderLines = true;
            pieSeries.DataLabel.Separator = " - ";
            pieSeries.DataLabel.Position = eLabelPosition.BestFit;

            chart.ShowDataLabelsOverMaximum = true;
            chart.Title.Text = text;

            chart.SetPosition(0, 150);
            chart.SetSize(600, 400);

            return worksheet;
        }
    }
}
