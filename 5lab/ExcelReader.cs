using Aspose.Cells;

public static class ExcelReader
{
    public static bool TryLoadDatabase(string path, out Database database)
    {
        Workbook wb;

        database = new();

        try
        {
            wb = new(path);
        }
        catch
        {
            return false;
        }

        WorksheetCollection wbCollection = wb.Worksheets;

        database.Register(ReadAccounts(wbCollection["Счета"]));
        database.Register(ReadAccruals(wbCollection["Поступления"]));
        database.Register(ReadCurrencyRates(wbCollection["Курс валют"]));

        return true;
    }

    private static List<Account> ReadAccounts(Worksheet sheet)
    {
        List<Account> list = new();

        for (int i = 0; i <= sheet.Cells.MaxDataRow; i++)
        {
            Cells cells = sheet.Cells;

            if (!int.TryParse(cells[i, 0].Value?.ToString(), out int id))
            {
                continue;
            }

            string? name = cells[i, 1].Value?.ToString();
            if (string.IsNullOrEmpty(name))
            {
                continue;
            }

            if (!DateTime.TryParse(sheet.Cells[i, 2].Value?.ToString(), out DateTime openDate))
            {
                continue;
            }

            Account account = new(id, name, openDate);
            list.Add(account);
        }

        return list;
    }

    private static List<Accrual> ReadAccruals(Worksheet sheet)
    {
        List<Accrual> list = new();

        Cells cells = sheet.Cells;

        for (int i = 0; i <= sheet.Cells.MaxDataRow; i++)
        {
            if (!int.TryParse(cells[i, 0].Value?.ToString(), out int id))
                continue;

            if (!int.TryParse(cells[i, 1].Value?.ToString(), out int accountId))
                continue;

            if (!int.TryParse(cells[i, 2].Value?.ToString(), out int currencyId))
                continue;

            if (!DateTime.TryParse(cells[i, 3].Value?.ToString(), out DateTime opDate))
                continue;

            if (!decimal.TryParse(cells[i, 4].Value?.ToString(), out decimal amount))
                continue;

            Accrual accrual = new(id, accountId, currencyId, opDate, amount);
            list.Add(accrual);
        }

        return list;
    }

    private static List<CurrencyRate> ReadCurrencyRates(Worksheet sheet)
    {
        List<CurrencyRate> list = new();

        Cells cells = sheet.Cells;

        for (int i = 0; i <= sheet.Cells.MaxDataRow; i++)
        {
            if (!int.TryParse(cells[i, 0].Value?.ToString(), out int id))
                continue;

            string? code = cells[i, 1].Value?.ToString();
            if (string.IsNullOrWhiteSpace(code))
                continue;

            if (!decimal.TryParse(cells[i, 2].Value?.ToString(), out decimal rate))
                continue;

            string? name = cells[i, 3].Value?.ToString();
            if (string.IsNullOrWhiteSpace(name))
                continue;


            CurrencyRate currency = new(id, code, name, rate);
            list.Add(currency);
        }

        return list;
    }
}