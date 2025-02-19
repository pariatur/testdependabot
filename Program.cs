using ParseTenable;

var parser = new Parser();
parser.Read();
parser.PrepareVulnerabilities();
parser.PrepareAssets();
parser.ExportToExcel();