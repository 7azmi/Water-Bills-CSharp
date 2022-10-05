using System.Globalization;
using System.Text;
using d = System.Drawing;
using iTextSharp.text;
using iTextSharp.text.pdf;


namespace Water_BIlls;

public class PDFGenerator
{
    BaseFont bf => BaseFont.CreateFont(Environment.GetEnvironmentVariable("windir") + @"\fonts\Arial.ttf", BaseFont.IDENTITY_H, true);
    Font Black => new (bf, 9, Font.BOLD);
    Font Red => new (bf, 9, Font.BOLD, BaseColor.RED);
    Font Blue => new (bf, 10, Font.BOLD, BaseColor.BLUE);

    private const string Directory = @"C:\Users\a7h30\Desktop\nested_tables.pdf";

    public void Print(BillCounter.User[] users)
    {
        //registry stuff that I don't know
        var ppp = CodePagesEncodingProvider.Instance;
        Encoding.RegisterProvider(ppp);

        var document = new Document();
        
        PdfWriter.GetInstance(document, new FileStream(Directory, FileMode.Create));
        
        document.Open();

        foreach (var user in users)
        {
            WriteUserSummary(user);
        }

        document.Close();


        void WriteUserSummary(BillCounter.User user)
        {
            float[] columnWidths = {60f, 45, 55, 30f, 25f, 50f};

            var userAndDateTable = CreateTable(new[] { 1f, 2f });

            userAndDateTable.AddCell(CreateCell(user.Name, userAndDateTable, false, horizontalAlignment: Element.ALIGN_LEFT));
            userAndDateTable.AddCell(CreateCell(DateTime.Now.ToString("yyyy-MM-dd"), userAndDateTable, false, horizontalAlignment: Element.ALIGN_RIGHT));

            document.Add(userAndDateTable);
            
            //units info
            var usageTable = new PdfPTable(columnWidths);

            var unitValueTable = CreateTable(1);
                var lastAndNewUnitValueTable = CreateTable(2);
            
            var unitDifferenceTable = CreateTable(1);
            var unitCostTable = CreateTable(1);
            var costTable = CreateTable(1);
            var arrearsTable = CreateTable(1);
            var totalTable = CreateTable(1);

            unitValueTable.AddCell(CreateCell("قراءة العداد",usageTable, true, true));
            lastAndNewUnitValueTable.AddCell(CreateCell("السابقة",usageTable));
            lastAndNewUnitValueTable.AddCell(CreateCell("الحالية", usageTable));
            lastAndNewUnitValueTable.AddCell(CreateCell(user.LastUnit.ToString(),usageTable));
            lastAndNewUnitValueTable.AddCell(CreateCell(user.CurrentUnit.ToString(),usageTable));
            unitValueTable.AddCell(lastAndNewUnitValueTable);
            

            unitDifferenceTable.AddCell(CreateCell("الفارق",usageTable, true, true));
            unitDifferenceTable.AddCell(CreateCell(user.UnitDifference.ToString(),usageTable));
            
            unitCostTable.AddCell(CreateCell("سعر الوحدة",usageTable, true, true));
            unitCostTable.AddCell(CreateCell(user.UnitCost.ToString("0"),usageTable));
            
            costTable.AddCell(CreateCell("القيمة",usageTable, true, true));
            costTable.AddCell(CreateCell(user.Cost.ToString("0"),usageTable));

            arrearsTable.AddCell(CreateCell("متأخرات",usageTable, true, true));
            arrearsTable.AddCell(CreateCell(user.Arrears.ToString("0"),usageTable, Red));

            totalTable.AddCell(CreateCell("إجمالي",usageTable, true, true));
            totalTable.AddCell(CreateCell(user.Total + " ريال",usageTable, Blue));
            
            usageTable.AddCell(unitValueTable);
            usageTable.AddCell(unitDifferenceTable);
            usageTable.AddCell(unitCostTable);
            usageTable.AddCell(costTable);
            usageTable.AddCell(arrearsTable);
            usageTable.AddCell(totalTable);
            
            document.Add(usageTable);

            document.Add(new Paragraph("\n\n"));//spacing :)
        }
        
    }
    
    PdfPTable CreateTable(int columns, bool rightToLeft = true)
    {
        var table =new PdfPTable(columns);
        if (rightToLeft) RTL(table);
        return table;
    }
    PdfPTable CreateTable(float[] widthRelatives, bool rightToLeft = true)
    {
        var table =new PdfPTable(widthRelatives);
        if (rightToLeft) RTL(table);
        return table;
    }
    private PdfPCell CreateCell(string text, PdfPTable table, Font font, bool border = true, bool grayFill = false,
        bool rightToLeft = true, float padding = 4, int horizontalAlignment = Element.ALIGN_CENTER, int verticalAlignment = Element.ALIGN_MIDDLE)
    {
        var cell = new PdfPCell(new Paragraph(text, font));
                
        if (rightToLeft) RTL(table);
        else LTR(table);
                

        cell.Padding = padding;
        if (!border) cell.Border = Rectangle.NO_BORDER;
                
        if(grayFill) cell.GrayFill = 0.9f;
                
        cell.HorizontalAlignment = horizontalAlignment;
        cell.VerticalAlignment = verticalAlignment;
                
        return cell;
    }

    private PdfPCell CreateCell(string text, PdfPTable table, bool border = true, bool grayFill = false, bool rightToLeft = true, float padding = 4, int horizontalAlignment = Element.ALIGN_CENTER, int verticalAlignment = Element.ALIGN_MIDDLE)
    {
        return CreateCell(text, table, Black, border, grayFill, rightToLeft, padding, horizontalAlignment, verticalAlignment);
    }
    void RTL(PdfPTable table) => table.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
    void LTR(PdfPTable table) => table.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
}