using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using PresentationLayer.ViewModels;
using PresentationLayer.Views;

namespace PresentationLayer.Services
{
    public class ViewTypeToImageSourceConverter : IValueConverter //Changes teh icon when menu button pressed
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is RegisterPrivateCustomerViewModel)
            {
                return "Assets/register_customer_pressed.png";
            }
            else if (value is RegisterCompanyCustomerViewModel)
            {
                return "Assets/register_company_pressed.png";
            }
            else if (value is ShowProspectsViewModel)
            {
                return "Assets/show_prospects_pressed.png";
            }
            else if (value is ExportBillingInformationViewModel)
            {
                return "Assets/export_bill_pressed.png";
            }
            else if (value is CalculateComissionViewModel)
            {
                return "Assets/calculate_comission_pressed.png";
            }
            else if (value is RegisterPreliminaryInsuranceViewModel)
            {
                return "Assets/insurance_pressed.png";
            }
            else if (value is SalesStatisticsViewModel)
            {
                return "Assets/SalesStatistics_pressed.png";
            }
            else if (value is RegisterUserViewModel)
            {
                return "Assets/add_user_pressed.png";
            }
            else if (value is CompanyCustomerProfileViewModel||value is PrivateCustomerProfileViewModel||value is SearchCustomerProfileViewModel)
            {
                return "Assets/customer_profile_pressed.png";
            }
            return "";
        }

        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture
        )
        {
            throw new NotImplementedException();
        }
    }
}
