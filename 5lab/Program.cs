using System.Globalization;
using System.Text;

public class Program
{
    public static void Main()
    {
        
        Console.InputEncoding = Encoding.UTF8;
        Console.OutputEncoding = Encoding.UTF8;
        
        Database database = new();

        while (true)
        {
            Console.Clear();

            Console.WriteLine("1 — Чтение базы данных из Excel файла");
            Console.WriteLine("2 — Просмотр базы данных");
            Console.WriteLine("3 — Удаление элементов");
            Console.WriteLine("4 — Добавление элементов");
            Console.WriteLine("5 — Выполнение запросов");
            Console.WriteLine("6 — Сохранение базы данных в Excel файл");

            int task = Util.Parse<int>("Номер задачи: ");

            switch (task)
            {
                case 1:
                    {
                        database = Load();
                        break;
                    }
                case 2:
                    {
                        if (!database)
                        {
                            Console.WriteLine("База данных не загружена.");
                            Console.ReadKey();
                            break;
                        }

                        Console.Clear();

                        Console.WriteLine("1 — Счета");
                        Console.WriteLine("2 — Поступления");
                        Console.WriteLine("3 — Курс валют");

                        int choice = Util.Parse<int>("Номер таблицы: ");
                        Console.Clear();
                        switch (choice)
                        {
                            case 1: PrintAccounts(database.Table<Account>()); break;
                            case 2: PrintAccruals(database.Table<Accrual>()); break;
                            case 3: PrintRates(database.Table<CurrencyRate>()); break;
                        }

                        Console.ReadKey();
                        break;
                    }
                case 3:
                    {
                        if (!database)
                        {
                            Console.WriteLine("База данных не загружена.");
                            Console.ReadKey();
                            break;
                        }

                        Console.WriteLine("Выберите таблицу:");
                        Console.WriteLine("1 — Счета");
                        Console.WriteLine("2 — Начисления");
                        Console.WriteLine("3 — Курсы валют");

                        int tableChoice = Util.Parse<int>("Ваш выбор: ");

                        int id = Util.Parse<int>("Введите ID: ");

                        bool removed = tableChoice switch
                        {
                            1 => database.Remove<Account>(id),
                            2 => database.Remove<Accrual>(id),
                            3 => database.Remove<CurrencyRate>(id),
                            _ => false
                        };

                        if (!removed)
                            Console.WriteLine("Запись не существует.");
                        else
                            Console.WriteLine("Удалено.");

                        break;
                    }
                case 4:
                {
                    if (!database)
                    {
                        Console.WriteLine("База данных не загружена.");
                        Console.ReadKey();
                        break;
                    }

                    Console.WriteLine("Добавить запись в таблицу:");
                    Console.WriteLine("1 — Счета");
                    Console.WriteLine("2 — Начисления");
                    Console.WriteLine("3 — Курсы валют");

                    int choice = Util.Parse<int>("Ваш выбор: ");

                    switch (choice)
                        {
                            case 1: database.AddAccount(); break;
                            case 2: database.AddAccrual(); break;
                            case 3: database.AddRate(); break;
                        }
                    Console.ReadKey();
                    break;
                }
                case 5:
                {
                    if (!database)
                    {
                        Console.WriteLine("База данных не загружена.");
                        Console.ReadKey();
                        break;
                    }

                    Console.Clear();
                    Console.WriteLine("Запросы:");
                    Console.WriteLine("1 — Счета, открытые после заданной даты (1 таблица, перечень)");
                    Console.WriteLine("2 — Сумма начислений по счету (2 таблицы, одно значение)");
                    Console.WriteLine("3 — Сумма средств по каждому владельцу в рублях (3 таблицы, перечень)");
                    Console.WriteLine("4 — Владелец с максимальной суммой в рублях за период (3 таблицы, одно значение)");

                    int q = Util.Parse<int>("Выберите запрос: ");
                    Console.Clear();

                    switch (q)
                    {
                        case 1: Queries.AccountsOpenedAfter(database);break;
                        case 2: Queries.TotalAccrualsForAccount(database);break;
                        case 3: Queries.PrintAccountsTotalInRubles(database);break;
                        case 4: Queries.MaxHolderInRubles(database); break;
                        default: Console.WriteLine("Неверный номер запроса.");break;
                    }

                    Console.ReadKey();
                    break;
                }
                case 6:
                    {
                        if (!database)
                        {
                            Console.WriteLine("База данных не загружена.");
                            Console.ReadKey();
                            break;
                        }

                        ExcelWriter.SaveDatabase("LR5-var13.xls", database);
                        Console.WriteLine("База данных сохранена.");
                        Console.ReadKey();
                        break;
                    }
            }
        }
    }

    public static Database Load()
    {
        if (!ExcelReader.TryLoadDatabase("LR5-var13.xls", out Database result))
        {
            Console.WriteLine("Excel файла не существует");
        }

        return result;
    }

    public static void PrintAccounts(List<Account> accounts)
    {
        int[] widths = [5, 25, 12];

        Console.WriteLine(Util.FormatRow(["ID", "ФИО", "Открыт"],widths));

        Console.WriteLine(new string('-', widths.Sum() + 2 * widths.Length + 1));

        foreach (var a in accounts)
        {
            Console.WriteLine(Util.FormatRow(
                [
                    a.ID.ToString(),
                    a.FullName,
                    a.OpenDate.ToString("dd.MM.yyyy")
                ],widths
            ));
        }
    }

    public static void PrintAccruals(List<Accrual> accruals)
    {
        int[] widths = [5, 10, 11, 12, 10];

        Console.WriteLine(Util.FormatRow(["ID", "ID счёта", "ID валюты", "Дата", "Сумма"],widths));

        Console.WriteLine(new string('-', widths.Sum() + 2 * widths.Length + 1));

        foreach (var a in accruals)
        {
            Console.WriteLine(Util.FormatRow(
                [
                    a.ID.ToString(),
                    a.AccountID.ToString(),
                    a.CurrencyID.ToString(),
                    a.OperationDate.ToString("dd.MM.yyyy"),
                    a.Amount.ToString()
                ],widths
            ));
        }
    }

    public static void PrintRates(List<CurrencyRate> rates)
    {

        int[] widths = [5, 5, 10, 48];

        Console.WriteLine(Util.FormatRow(["ID", "Код", "Курс", "Наименование"],widths));

        Console.WriteLine(new string('-', widths.Sum() + 2 * widths.Length + 1));

        foreach (var r in rates)
        {
            Console.WriteLine(Util.FormatRow(
                [
                    r.ID.ToString(),
                    r.Code,
                    r.Rate.ToString(CultureInfo.InvariantCulture),
                    r.Name,
                ],widths
            ));
        }
    }
}

public static class Util
{
    public static T Parse<T>(string text) where T : IParsable<T>
    {
        while (true)
        {
            Console.Write(text);
            string? result = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(result))
            {
                Console.WriteLine("Пустой ввод недопустим.");
                Console.ReadKey();
                continue;
            }

            IFormatProvider culture = typeof(T) == typeof(DateTime) ? CultureInfo.GetCultureInfo("ru-RU") : CultureInfo.InvariantCulture;

            if (!T.TryParse(result, culture, out T? value))
            {
                if (typeof(T) == typeof(DateTime))
                    Console.WriteLine("Неверный формат даты. Используйте dd.MM.yyyy");
                else
                    Console.WriteLine("Invalid input, try again!");

                Console.ReadKey();
                continue;
            }

            return value;
        }
    }

    public static string FormatRow(string[] cols, int[] widths)
    {
        string result = "";
        for (int i = 0; i < cols.Length; i++)
            result += "| " + cols[i].PadRight(widths[i]);

        return result + "|";
    }
}