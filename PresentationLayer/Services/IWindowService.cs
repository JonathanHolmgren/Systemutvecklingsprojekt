using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationLayer.Services
{
    public interface IWindowService
    {
        void Show<TViewModel>(TViewModel model);
        bool ShowDialog<TViewModel>(TViewModel model);
        void ShowWindow<TViewModel>(TViewModel viewModel);

    }
}
