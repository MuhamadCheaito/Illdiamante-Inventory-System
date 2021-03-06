using Ildiamnte_Inventory_System.Model;
using EntityLayer;
using BusinessLayer;
using System;
using System.Windows.Input;
using Ildiamnte_Inventory_System.Views;

namespace Ildiamnte_Inventory_System.ViewModel
{
    public class InventoryViewModel : ListModel<TransactionBodyListEntity>
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public InventoryViewModel()
        {
            TableName = "Inventario elenco";
        }
        protected override void RefreshList(object parameter)
        {
            log.Debug("Refresh list: " + TableName);

            List = ManageTransactions.ListInventory();
        }

        private ICommand _detailsCommand;
        public ICommand DetailsCommand
        {
            get
            {
                if (_detailsCommand == null) _detailsCommand = new RelayCommand(new Action<object>(Details), new Predicate<object>(DetailsCanExecute));
                return _detailsCommand;
            }
            set { SetProperty(ref _detailsCommand, value); }
        }

        private bool DetailsCanExecute(object parameter)
        {
            return ItemSelected(parameter);
        }

        private void Details(object parameter)
        {
            log.Debug(TableName + " - details button");

            InventoryDetailsViewModel IDVM = new InventoryDetailsViewModel(SelectedItem.Product.Id);
            ListDetailsWindow LDW = new ListDetailsWindow() { DataContext = IDVM };
            LDW.ShowDialog();
        }
    }
}
