using System.Collections.ObjectModel;

namespace Models.SalesStatistics;

public class SalesReport
{
    public int Year { get; set; }
    public ObservableCollection<SalesPersonData> Sales { get; set; } =
        new ObservableCollection<SalesPersonData>();
}
