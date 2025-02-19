using OfficeOpenXml.Table.PivotTable;
using OfficeOpenXml;

namespace ParseTenable.Excel
{
    internal static class PivotTable
    {
        /// <summary>
        /// Creates a pivot table
        /// </summary>
        /// <param name="worksheet"></param>
        /// <param name="worksheetName"></param>
        /// <param name="fieldName"></param>
        /// <param name="graphName"></param>
        /// <param name="serie"></param>
        /// <param name="xSerie"></param>
        public static void Create(ExcelPackage excelfile, ExcelWorksheet worksheet, string worksheetName, string fieldName, string graphName, string serie, string xSerie)
        {
            var pivotWorksheet = excelfile.Workbook.Worksheets.Add(worksheetName);
            var range = worksheet.Dimension;

            var sourceRange = worksheet.Cells[1, 1, range.Rows, range.Columns];

            var pivotTable = pivotWorksheet.PivotTables.Add(pivotWorksheet.Cells["A1"], sourceRange, "");

            pivotTable.RowFields.Add(pivotTable.Fields[fieldName]);
            var dataField = pivotTable.DataFields.Add(pivotTable.Fields[fieldName]);
            dataField.Function = DataFieldFunctions.Count;

            Graphs.Create(pivotWorksheet, graphName, serie, xSerie);
        }
    }
}
