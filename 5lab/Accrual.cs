public class Accrual : DatabaseEntry
{
    public int AccountID { get; set; }
    public int CurrencyID { get; set; }
    public DateTime OperationDate {get; set;}
    public decimal Amount {get; set;}

    public Accrual(int accId, int curId, DateTime date, decimal amount)
    {
        AccountID = accId;
        CurrencyID = curId;
        OperationDate = date;
        Amount = amount;
    }
    public Accrual(int id, int accId, int curId, DateTime date, decimal amount)
    {
        ID = id;
        AccountID = accId;
        CurrencyID = curId;
        OperationDate = date;
        Amount = amount;
    }
}