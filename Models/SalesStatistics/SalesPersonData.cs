using System.Collections.ObjectModel;

namespace Models.SalesStatistics;

public class SalesPersonData
{
    public string? SalesPersonName { get; set; }
    public ObservableCollection<MonthlySalesDataPrivate> MonthlySalesPrivate { get; set; } =
        new ObservableCollection<MonthlySalesDataPrivate>();

    public ObservableCollection<MonthlySalesDataCompany> MonthlySalesCompany { get; set; } =
        new ObservableCollection<MonthlySalesDataCompany>();

    public int TotalSalesPrivate => MonthlySalesPrivate.Sum(m => m.TotalSales);
    public double AverageMonthlySalesPrivate =>
        MonthlySalesPrivate.Count > 0 ? TotalSalesPrivate / 12.0 : 0;

    public int TotalSalesCompany => MonthlySalesCompany.Sum(m => m.TotalSales);
    public double AverageMonthlySalesCompany =>
        MonthlySalesCompany.Count > 0 ? TotalSalesCompany / 12.0 : 0;
}
