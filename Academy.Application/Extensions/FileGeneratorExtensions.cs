using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;
using Academy.Application.Statics;
using CsvHelper;
using iTextSharp.text;
using iTextSharp.text.pdf;
using OfficeOpenXml;

namespace Academy.Application.Extensions;

public static class FileGeneratorExtensions
{
    public static byte[] GenerateExcel<T>(this IEnumerable<T> data)
    {
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Sheet1");

        // Get the properties of the generic type
        var properties = typeof(T).GetProperties();

        // Add headers to the worksheet using display name
        for (int i = 0; i < properties.Length; i++)
        {
            string header = GetPropertyName(properties[i]);
            worksheet.Cells[1, i + 1].Value = header;
        }

        // Add data rows to the worksheet
        int rowIndex = 2;
        foreach (var item in data)
        {
            for (int i = 0; i < properties.Length; i++)
            {
                worksheet.Cells[rowIndex, i + 1].Value = properties[i].GetValue(item);
            }
            rowIndex++;
        }

        worksheet.Cells.AutoFitColumns();
        worksheet.View.RightToLeft = true;

        return package.GetAsByteArray();
    }


    public static byte[] GeneratePdf<T>(this IEnumerable<T> data, string? fontPath = null)
    {
        fontPath ??= FileGeneratorStatics.PdfFont;

        using MemoryStream memoryStream = new MemoryStream();
        // Create a new PDF document
        Document document = new Document();
        var properties = typeof(T).GetProperties();

        // Create a new PDF writer
        PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
        float docWidth = 0;

        for (int i = 0; i <= properties.Length; i++)
        {
            docWidth += 100f;
        }

        if (docWidth < 14400 && docWidth > 595)
        {
            document.SetPageSize(new Rectangle(docWidth, 842));
        }

        if (docWidth >= 14400)
        {
            document.SetPageSize(new Rectangle(14400, 842));
        }

        document.Open();

        BaseFont baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        Font font = new Font(baseFont);

        PdfPTable table = new PdfPTable(typeof(T).GetProperties().Length);
        table.WidthPercentage = 100;
        table.RunDirection = PdfWriter.RUN_DIRECTION_RTL;


        // Set table headers
        foreach (var property in properties)
        {
            string headerText = GetPropertyName(property);
            PdfPCell headerCell = new PdfPCell();
            Phrase phrase = new Phrase(headerText, font);
            phrase.Leading = 20f;
            headerCell.AddElement(phrase);
            headerCell.Padding = 5;
            headerCell.PaddingBottom = 20;
            table.AddCell(headerCell);
        }


        // set table rows
        foreach (var item in data)
        {
            foreach (var property in properties)
            {
                var cellValue = property.GetValue(item)?.ToString() ?? "";
                PdfPCell dataCell = new PdfPCell();

                Phrase phrase = new Phrase(cellValue, font);
                phrase.Leading = 20f;

                dataCell.AddElement(phrase);
                dataCell.Padding = 5;
                dataCell.PaddingBottom = 20;

                table.AddCell(dataCell);
            }
        }

        // Add the table to the document
        document.Add(table);

        document.Close();

        // Return the generated PDF file as a byte array
        return memoryStream.ToArray();
    }

    public static byte[] GenerateCsv<T>(this IEnumerable<T> data)
    {
        using MemoryStream memoryStream = new();
        using var writer = new StreamWriter(memoryStream);
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        var properties = typeof(T).GetProperties();
        foreach (var propertyInfo in properties)
        {
            csv.WriteField(GetPropertyName(propertyInfo));
        }
        csv.NextRecord();
        
        foreach (T item in data)
        {
            foreach (var property in typeof(T).GetProperties())
            {
                csv.WriteField(property.GetValue(item)?.ToString()?.Replace("," , "-"));
            }
            csv.NextRecord();
        }
        
        writer.Flush();
        return memoryStream.ToArray();
    }
    
    private static string GetPropertyName(PropertyInfo property)
    {
        var displayNameAttribute = property.GetCustomAttribute<DisplayNameAttribute>();
        var displayAttribute = property.GetCustomAttribute<DisplayAttribute>();
        return displayNameAttribute?.DisplayName ?? displayAttribute?.GetName() ?? property.Name;
    }
}