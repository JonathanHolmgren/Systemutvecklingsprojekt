using System.Collections.ObjectModel;
using LiveChartsCore;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using Models.SalesStatistics;
using PresentationLayer.Models;
using ServiceLayer;
using SkiaSharp;

namespace PresentationLayer.ViewModels;

public class SalesStatisticsViewModel : ObservableObject
{
    private SalesStatisticsController salesStatisticsController;
    public ObservableCollection<int> AvailableYears { get; set; }

    public SalesStatisticsViewModel()
    {
        salesStatisticsController = new SalesStatisticsController();
        AvailableYears = new ObservableCollection<int>(
            Enumerable.Range(DateTime.Now.Year - 10, 11)
        );
        SelectedYear = DateTime.Now.Year;
        SalesReport = salesStatisticsController.GetSalesReport(SelectedYear);
    }

    private SalesPersonData selectedSalesPerson;

    public SalesPersonData SelectedSalesPerson
    {
        get => selectedSalesPerson;
        set
        {
            selectedSalesPerson = value;
            OnPropertyChanged();
            UpdateChartData();
            GenerateChartData();
        }
    }

    private SalesReport salesReport = new SalesReport();

    public SalesReport SalesReport
    {
        get => salesReport;
        set
        {
            salesReport = value;
            OnPropertyChanged();
        }
    }

    private int selectedYear;
    public int SelectedYear
    {
        get => selectedYear;
        set
        {
            if (selectedYear != value)
            {
                selectedYear = value;
                OnPropertyChanged(nameof(SelectedYear));
                SalesReport = salesStatisticsController.GetSalesReport(SelectedYear);
            }
        }
    }

    private ObservableCollection<ISeries> series = new ObservableCollection<ISeries>();
    public ObservableCollection<ISeries> Series
    {
        get => series;
        set
        {
            series = value;
            OnPropertyChanged();
        }
    }

    private void UpdateChartData()
    {
        if (SelectedSalesPerson == null)
        {
            Series.Clear();
            return;
        }

        var monthlyTotalSales = new int[12];

        foreach (var sale in SelectedSalesPerson.MonthlySalesPrivate)
        {
            int monthIndex = sale.ActiveDate.Month - 1;
            monthlyTotalSales[monthIndex] += sale.TotalSales;
        }

        foreach (var sale in SelectedSalesPerson.MonthlySalesCompany)
        {
            int monthIndex = sale.ActiveDate.Month - 1;
            monthlyTotalSales[monthIndex] += sale.TotalSales;
        }

        // Uppdatera Series med nya data
        Series.Clear();
        Series.Add(
            new LineSeries<int>
            {
                Values = monthlyTotalSales,
                Name = "Total Sales",
                LineSmoothness = 0,
                GeometrySize = 0,
            }
        );
    }

    public LabelVisual Title { get; set; } =
        new LabelVisual
        {
            Text = "My chart title",
            TextSize = 25,
            Padding = new LiveChartsCore.Drawing.Padding(15),
        };

    public Axis[] XAxes { get; set; } =
        new Axis[]
        {
            new Axis()
            {
                Labels = new List<string>
                {
                    "Jan",
                    "Feb",
                    "Mar",
                    "Apr",
                    "May",
                    "Jun",
                    "Jul",
                    "Aug",
                    "Sep",
                    "Oct",
                    "Nov",
                    "Dec",
                },
                Labeler = value => $"Month {value}",
                LabelsRotation = 15,
            },
        };

    private ObservableCollection<ISeries> chartWithTrend = new ObservableCollection<ISeries>();

    public ObservableCollection<ISeries> ChartWithTrend
    {
        get => chartWithTrend;
        set
        {
            chartWithTrend = value;
            OnPropertyChanged();
        }
    }

    private void GenerateChartData()
    {
        var salesPerson = SelectedSalesPerson;
        if (salesPerson == null)
        {
            chartWithTrend.Clear();
            return;
        }

        var totalMonthlySales = new double[12];
        for (int month = 0; month < 12; month++)
        {
            var privateSales = salesPerson
                .MonthlySalesPrivate.Where(m => m.ActiveDate.Month == month + 1)
                .Sum(m => m.TotalSales);
            var companySales = salesPerson
                .MonthlySalesCompany.Where(m => m.ActiveDate.Month == month + 1)
                .Sum(m => m.TotalSales);
            totalMonthlySales[month] = privateSales + companySales;
        }

        var trendlineValues = CalculateTrendline(totalMonthlySales);

        ChartWithTrend.Clear();
        ChartWithTrend.Add(
            new ColumnSeries<double> { Values = totalMonthlySales, Name = "Totala försäljningar" }
        );
        ChartWithTrend.Add(
            new LineSeries<double>
            {
                Values = trendlineValues,
                Name = "TrendLinje",
                Fill = null,
            }
        );
    }

    private double[] CalculateTrendline(double[] sales)
    {
        int n = sales.Length;
        double[] trendline = new double[n];
        if (n == 0)
            return trendline;

        double[] months = Enumerable.Range(1, n).Select(x => (double)x).ToArray();

        double meanX = months.Average();
        double meanY = sales.Average();

        double numerator = 0;
        double denominator = 0;

        for (int i = 0; i < n; i++)
        {
            numerator += (months[i] - meanX) * (sales[i] - meanY);
            denominator += (months[i] - meanX) * (months[i] - meanX);
        }

        double a = numerator / denominator;
        double b = meanY - a * meanX;

        for (int i = 0; i < n; i++)
        {
            trendline[i] = a * months[i] + b;
        }

        return trendline;
    }
}
