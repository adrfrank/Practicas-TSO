using ControlDeProcesos.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ControlDeProcesos
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ProcesosDulcesVM vm;
        public ProcesosDulcesVM ViewModel { get {
                return vm;
            }
            set { vm = value; DataContext = value; }
        }

        public MainWindow()
        {
            InitializeComponent();
            ViewModel = new ProcesosDulcesVM();
        }
    }
}
