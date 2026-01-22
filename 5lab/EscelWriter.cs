using System.Drawing;
using Aspose.Cells;

public static class ExcelWriter
{
    private static CellsFactory _factory;
    private static Style _header;
    private static Style _default;
    private static Style _date;

    static ExcelWriter()
    {
        _factory = new();

        _header = _factory.CreateStyle();
        _default = _factory.CreateStyle();
        _date = _factory.CreateStyle();
        
        InitHeaderStyle();
        InitDefaultStyle();
        InitDateStyle();
    }

    private static void InitHeaderStyle()
    {
        _header.Font.Name = "Calibri";
        _header.Font.Size = 11;
        _header.Font.IsBold = true;

        _header.HorizontalAlignment = TextAlignmentType.Center;
        _header.VerticalAlignment = TextAlignmentType.Center;

        _header.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
        _header.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;
        _header.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
        _header.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;

        _header.Borders.SetColor(Color.Black);

    }

    private static void InitDefaultStyle()
    {
        _default.Font.Size = 11;
        _default.Font.Name = "Calibri";
    }

    private static void InitDateStyle()
    {
        _date.Custom = "dd.MM.yyyy";    
    }   
    public static void SaveDatabase(string path, Database database)
    {
        Workbook wb = new();

        wb.Worksheets.Clear();
        Worksheet accounts = wb.Worksheets.Add("Счета");
        accounts.Cells.StyleDefault();
        accounts.WriteAccounts(database.Table<Account>());

        Worksheet accruals = wb.Worksheets.Add("Поступления");
        accruals.Cells.StyleDefault();
        accruals.WriteAccruals(database.Table<Accrual>());

        Worksheet rates = wb.Worksheets.Add("Курс валют");
        rates.Cells.StyleDefault();
        rates.WriteCurrencyRates(database.Table<CurrencyRate>());

        wb.Save(path);
    }

    private static void WriteAccounts(this Worksheet sheet, List<Account> list)
    {
        Cells cells = sheet.Cells;

        cells[0, 0].PutValue("ID");
        cells[0, 1].PutValue("ФИО");
        cells[0, 2].PutValue("Дата открытия");

        cells.StyleHeader();

        Style dateStyle = sheet.Workbook.CreateStyle();

        int row = 1;

        foreach (var a in list)
        {
            cells[row, 0].PutValue(a.ID);
            cells[row, 1].PutValue(a.FullName);
            cells[row, 2].PutValue(a.OpenDate);
            cells[row, 2].StyleDate();
            row++;
        }
    }

    private static void WriteAccruals(this Worksheet sheet, List<Accrual> list)
    {
        Cells cells = sheet.Cells;

        cells[0, 0].PutValue("ID");
        cells[0, 1].PutValue("ID счёта");
        cells[0, 2].PutValue("ID валюты");
        cells[0, 3].PutValue("Дата");
        cells[0, 4].PutValue("Сумма");

        cells.StyleHeader();

        int row = 1;

        foreach (var a in list)
        {
            cells[row, 0].PutValue(a.ID);
            cells[row, 1].PutValue(a.AccountID);
            cells[row, 2].PutValue(a.CurrencyID);
            cells[row, 3].PutValue(a.OperationDate.ToString("dd.MM.yyyy"));
            cells[row, 3].StyleDate();
            cells[row, 4].PutValue(a.Amount);
            row++;
        }
    }

    private static void WriteCurrencyRates(this Worksheet sheet, List<CurrencyRate> list)
    {
        Cells cells = sheet.Cells;

        cells[0, 0].PutValue("ID");
        cells[0, 1].PutValue("Буквенный Код");
        cells[0, 2].PutValue("Курс");
        cells[0, 3].PutValue("Полное наименование");

        cells.StyleHeader();

        int row = 1;

        foreach (var r in list)
        {
            cells[row, 0].PutValue(r.ID);
            cells[row, 1].PutValue(r.Code);
            cells[row, 2].PutValue(r.Rate);
            cells[row, 3].PutValue(r.Name);
            row++;
        }
    }

    private static void StyleHeader(this Cells cells)
    {
        StyleFlag flag = new() 
        {
            All = true 
        };

        for (int c = 0; c <= cells.MaxDataColumn; c++)
        {
            Cell cell = cells[0, c];
            cell.SetStyle(_header, flag);
        }
        
    }


    private static void StyleDefault(this Cells cells)
    {
        StyleFlag flag = new() 
        {
            All = true 
        };

        cells.ApplyStyle(_default, flag);
    }

    private static void StyleDate(this Cell cell)
    {
        StyleFlag flag = new() 
        { 
            NumberFormat = true 
        };

        cell.SetStyle(_date, flag);
    }
}