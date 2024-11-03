namespace Models.SalesStatistics;

public class MonthlySalesDataCompany
{
    public DateTime ActiveDate { get; set; }
    public int PropertyEquipmentInsurance { get; set; }
    public int VehicleInsurance { get; set; }
    public int LiabilityInsurance { get; set; }

    public int TotalSales => PropertyEquipmentInsurance + VehicleInsurance + LiabilityInsurance;
    public string MonthString
    {
        get
        {
            string month = ActiveDate.ToString("MMMM");
            return char.ToUpper(month[0]) + month.Substring(1);
        }
    }
}
