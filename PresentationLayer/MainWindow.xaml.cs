using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ServiceLayer;

namespace PresentationLayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MyService _myService;
        
        public MainWindow() 
            : this(null) // Anropa den andra konstruktorn, men skicka null för MyService
        {
        }
        public MainWindow(MyService myService)
        {
            InitializeComponent();
            _myService = myService;
        }

    }
}