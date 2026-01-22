public class CurrencyRate : DatabaseEntry
{
    public string Code { get; set; }
    public string Name { get; set; }
    public decimal Rate { get; set; }

    public CurrencyRate(string code, string name, decimal rate)
    {
        Code = code;
        Name = name.Replace("\n", "").Replace("\r", "");
        Rate = rate;
    }

    public CurrencyRate(int id, string code, string name, decimal rate)
    {
        ID = id;
        Code = code;
        Name = name.Replace("\n", "").Replace("\r", "");
        Rate = rate;
    }

    public override string ToString()
    {
        return $"ID={ID}; {Code} ({Name}); Курс={Rate}";
    }
}
