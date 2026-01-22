public static class Queries
{
    public static void MaxHolderInRubles(Database db)
    {
        DateTime from = Util.Parse<DateTime>("Введите начальную дату (dd.MM.yyyy): ");
        DateTime to = Util.Parse<DateTime>("Введите конечную дату (dd.MM.yyyy): ");

        List<Account> accounts = db.Table<Account>();
        List<Accrual> accruals = db.Table<Accrual>();
        List<CurrencyRate> rates = db.Table<CurrencyRate>();

        var result = accruals
            .Where(a => a.OperationDate >= from && a.OperationDate <= to)
            .Join(
                rates,
                accr => accr.CurrencyID,
                rate => rate.ID,
                (accr, rate) => new
                {
                    accr.AccountID,
                    Rubles = accr.Amount * rate.Rate
                })
            .Join(
                accounts,
                a => a.AccountID,
                acc => acc.ID,
                (a, acc) => new
                {
                    acc.FullName,
                    a.Rubles
                })
            .GroupBy(x => x.FullName)
            .Select(g => new
            {
                Holder = g.Key,
                Total  = g.Sum(x => x.Rubles)
            })
            .OrderByDescending(x => x.Total)
            .FirstOrDefault()?.Holder.Split('.')[0];

        Console.WriteLine(result);
        
        var names = accounts;
    }

    public static void PrintAccountsTotalInRubles(Database db)
    {
        List<Account> accounts = db.Table<Account>();
        List<Accrual> accruals = db.Table<Accrual>();
        List<CurrencyRate> rates = db.Table<CurrencyRate>();

        var result = accruals.
        Join(
            rates,
            accr => accr.CurrencyID,
            rate => rate.ID,
            (accr, rate) => new
            {
                accr.AccountID,
                Rubles = accr.Amount * rate.Rate
            })
        .Join(
            accounts,
            a => a.AccountID,
            accr => accr.ID,
            (a, acc) => new
            {
                acc.ID,
                acc.FullName,
                a.Rubles
            })
        .OrderBy(a => a.ID)
        .GroupBy(x => x.FullName)
        .Select(x => new
        {
            FullName = x.Key,
            Rubles = x.Sum(x => x.Rubles)  
        });

        int[] widths = [24, 16];

        Console.WriteLine(Util.FormatRow(["Имя", "Сумма в рублях"],widths));

        Console.WriteLine(new string('-', widths.Sum() + 2 * widths.Length + 1));

        foreach (var item in result)
        {
            Console.WriteLine(Util.FormatRow(
                [
                    item.FullName,
                    item.Rubles.ToString()
                ],widths
            ));
        }
    }

    public static void AccountsOpenedAfter(Database db)
    {
        DateTime date = Util.Parse<DateTime>("Введите дату: ");

        var result = db.Table<Account>()
            .Where(a => a.OpenDate > date);

        foreach (var a in result)
            Console.WriteLine(a);
    }

    public static void TotalAccrualsForAccount(Database db)
    {
        int accountId = Util.Parse<int>("Введите ID счета: ");

        var sum = db.Table<Accrual>()
            .Where(a => a.AccountID == accountId)
            .Sum(a => a.Amount);

        Console.WriteLine($"Сумма начислений: {sum}");
    }
}