using BusinessLayer;
using EntityLayer;
using Ildiamnte_Inventory_System.Model;
using Ildiamnte_Inventory_System.Views;
using System;
using System.Windows.Input;

namespace Ildiamnte_Inventory_System.ViewModel
{
    public class PartnerTransactionsViewModel : ListModel<TransactionHeadListEntity>
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public PartnerTransactionsViewModel()
        {
            TableName = "Riepilogo transazione partner";
        }

        private decimal _totalTransactions;
        public decimal TotalTransactions
        {
            get { return _totalTransactions; }
            set { SetProperty(ref _totalTransactions, value); }
        }
        protected override void RefreshList(object parameter)
        {
            log.Debug("Refresh list: " + TableName);

            List = ManageTransactions.ListPartnerTransactions();
            TotalTransactions = 0;
            foreach (var record in List)
                TotalTransactions += record.ListVariable;
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

            PartnerTransactionsDetailsViewModel PTDVM = new PartnerTransactionsDetailsViewModel(SelectedItem.Partner.Id);
            ListDetailsWindow LDW = new ListDetailsWindow() { DataContext = PTDVM };
            LDW.ShowDialog();
        }
    }
}
