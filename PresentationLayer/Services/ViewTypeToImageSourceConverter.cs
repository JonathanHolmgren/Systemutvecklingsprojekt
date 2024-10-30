using PresentationLayer.Views;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PresentationLayer.Services
{
    public class ViewTypeToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is RegisterPrivateCustomerView)
            {
                return "Assets/register_customer_pressed.png";
            }
            else if (value is RegisterCompanyCustomer)
            {
                return "Assets/register_company_pressed.png";
            }
            else if (value is ShowProspectsView)
            {
                return "Assets/show_prospects_pressed.png";
            }
            else if (value is ExportBillingInformationView)
            {
                return "Assets/export_bill_pressed.png";
            }
            else if (value is CalculateComissionView)
            {
                return "Assets/calculate_comission_pressed.png";
            }
            return "";
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
