namespace Academy.Application.Statics;

public static class FileGeneratorStatics
{
    public const string ExcelContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    public const string ExcelFileName = "report.xlsx";
    public const string PdfContentType = "application/pdf";
    public const string PdfFileName = "report.pdf";
    public const string CsvFileName = "report.csv";
    public const string CsvContentType = "text/csv";
    public const string JsonContentType = "application/json";
    public const string JsonFileName = "data.json";
    
    public static readonly string PdfFont = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/shared/fonts/arial/arial.ttf");
}