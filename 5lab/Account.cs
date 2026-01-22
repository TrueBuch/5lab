public class Account : DatabaseEntry
{
    public string FullName { get; set; }
    public DateTime OpenDate { get; set; }

    public Account(int id, string fullName, DateTime openDate)
    {
        ID = id;
        FullName = fullName;
        OpenDate = openDate;
    }

    public Account(string fullName, DateTime openDate)
    {
        FullName = fullName;
        OpenDate = openDate;
    }

    public override string ToString()
    {
        return $"{ID} {FullName} {OpenDate:dd.MM.yyyy}";
    }
}
