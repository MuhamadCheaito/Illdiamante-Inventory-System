using System;
using System.Windows;
using Microsoft.Win32;
using BusinessLayer;
using Ildiamnte_Inventory_System.ViewModel;

namespace Ildiamnte_Inventory_System.Views
{
    /// <summary>
    /// Interaction logic for SetupConnectionWindow.xaml
    /// </summary>
    public partial class SetupConnectionWindow : Window
    {
        public SetupConnectionWindow()
        {
            InitializeComponent();
            SetupConnectionViewModel context = new SetupConnectionViewModel() { SetupWindow = this };
            this.DataContext = context;
        }
    }
}
