namespace Models.SalesStatistics;

public class MonthlySalesDataPrivate
{
    public DateTime ActiveDate { get; set; }
    public int ChildrenSales { get; set; }
    public int AdultSales { get; set; }
    public int LifeSales { get; set; }

    public int TotalSales => ChildrenSales + AdultSales + LifeSales;

    public string MonthString
    {
        get
        {
            string month = ActiveDate.ToString("MMMM");
            return char.ToUpper(month[0]) + month.Substring(1);
        }
    }
}
