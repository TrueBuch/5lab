public static class DatabaseUtil
{
    public static void AddAccount(this Database database)
    {
        string name = Util.Parse<string>("Введите ФИО: ");
        DateTime date = Util.Parse<DateTime>("Введите дату открытия: ");
        database.GenerateId<Account>();

        Account account = new(name, date);

        database.Add(account);
    }

    public static void AddAccrual(this Database database)
    {
        int accountId = Util.Parse<int>("Введите ID счета: ");
        if (!database.Has<Account>(accountId))
        {
            Console.WriteLine("Такого счета не существует");
            Console.ReadKey();
            return;
        }

        int currencyId = Util.Parse<int>("Введите ID валюты: ");
        if (!database.Has<CurrencyRate>(currencyId))
        {
            Console.WriteLine("Такой валюты не сущесвует");
            Console.ReadKey();
            return;
        }

        decimal value = Util.Parse<decimal>("Введите сумму");
        DateTime dateTime = Util.Parse<DateTime>("Введите дату");

        Accrual accrual = new(accountId, currencyId, dateTime, value);
        database.Add(accrual);
    }

    public static void AddRate(this Database database)
    {
        string code = Util.Parse<string>("Введите код валюты: ");
        string name = Util.Parse<string>("Введите название валюты: ");
        decimal rate = Util.Parse<decimal>("Введите курс: ");

        if (rate <= 0)
        {
            Console.WriteLine("курс не может быть отрицательным");
            Console.ReadKey();

            return;
        }  

        CurrencyRate currencyRate = new(code, name, rate);
        database.Add(currencyRate);
    } 
}