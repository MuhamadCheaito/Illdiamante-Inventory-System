using System.Windows;
using BusinessLayer;
using Ildiamnte_Inventory_System.Notifications;
using Ildiamnte_Inventory_System.ViewModel;

namespace Ildiamnte_Inventory_System.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainWindowViewModel(this);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            NotificationProvider.Close();
        }
    }
}
