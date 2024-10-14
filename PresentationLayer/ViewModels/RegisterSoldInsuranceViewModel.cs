using PresentationLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using System.ComponentModel;

namespace PresentationLayer.ViewModels
{
    public class RegisterSoldInsuranceViewModel: ObservableObject
    {
        Insurance testInsurance1 = new Insurance(InsuranceStatus.Active);
        Insurance testInsurance2 = new Insurance(InsuranceStatus.Preliminary);

        private IList<Insurance> _testInsurances;
        public IList<Insurance> TestInsurance
        {
            get => _testInsurances;
            set
            {
                _testInsurances = value;
                OnPropertyChanged(nameof(TestInsurance));
            }

        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public RegisterSoldInsuranceViewModel()
        {
            
           
            TestInsurance = new List<Insurance> { testInsurance1, testInsurance2 };
        }

    }
}
