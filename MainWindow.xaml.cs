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

namespace Celochisl
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Branch_Click(object sender, RoutedEventArgs e)
        {
            BranchC branchC = new BranchC();
            branchC.Show();
        }

        private void Gomori_Click(object sender, RoutedEventArgs e)
        {
            Gomori gomori = new Gomori();
            gomori.Show();
        }
    }
}