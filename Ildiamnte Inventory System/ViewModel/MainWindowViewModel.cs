using System;
using Ildiamnte_Inventory_System.Model;
using System.Windows;
using System.Windows.Input;
using BusinessLayer;
using Ildiamnte_Inventory_System.Views;
using Ildiamnte_Inventory_System.Notifications;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace Ildiamnte_Inventory_System.ViewModel
{
    public class MainWindowViewModel : BindableBase
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Constructors
        public MainWindowViewModel(Window mainwindow)
        {
            log.Debug(">>> Program started. <<<");

            MainWindow = mainwindow;

            try
            {
                // Set database connection.
                if (!DatabaseConnection.TestConnection())
                {
                    log.Debug("Can't connect to database. => Opening connection setup window.");
                    SetupConnectionWindow SCW = new SetupConnectionWindow();
                    SCW.ShowDialog();
                    if (!DatabaseConnection.TestConnection())
                    {
                        log.Debug("Database connection wasn't set.");
                        CloseWindow();
                        return;
                    }
                }

                // New user.
                if (UserLogin.IsEmptyUserDatabase())
                {
                    log.Debug("User datatable is empty. => Opening new user window.");
                    NotificationProvider.Info("Welcome First User!", "Please, set a username and a password.");
                    NewUserWindow NUW = new NewUserWindow();
                    NUW.ShowDialog();
                    if (UserLogin.IsEmptyUserDatabase())
                    {
                        log.Debug("User wasn't added.");
                        CloseWindow();
                        return;
                    }
                }

                // Login.
                log.Debug("Opening login window.");
                LoginWindow LW = new LoginWindow();
                LW.ShowDialog();
                if (UserLogin.LoginedUser == "")
                {
                    log.Debug("Not logged in.");
                    CloseWindow();
                    return;
                }
            }
            catch (Exception ex)
            {
                log.Fatal("Program initialization error.", ex);
                MessageBox.Show("Program initialization error.\nCheck RedLog.txt for more information.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                CloseWindow();
                return;
            }
        }
        #endregion
        #region Menus and Views
        private string[] _mainMenu = new string[] { "Tabelle", "Transazioni", "Elenchi", "Impostazioni" };
        private string[] _tablesMenu = new string[] { "Prodotti", "Categorie di Prodotto", "Partner" };
        private ProductsViewModel _products = new ProductsViewModel();
        private ProductCategoriesViewModel _productCategories = new ProductCategoriesViewModel();
        private PartnersViewModel _partners = new PartnersViewModel();
        private string[] _transactionsMenu = new string[] { "In arrivo", "In uscita" };
        private TransactionsViewModel _incomingTransactions = new TransactionsViewModel(true);
        private TransactionsViewModel _outgoingTransactions = new TransactionsViewModel(false);
        private string[] _listsMenu = new string[] { "Inventario", "Transazioni con i partner" };
        private InventoryViewModel _inventory = new InventoryViewModel();
        private PartnerTransactionsViewModel _partnerTransactions = new PartnerTransactionsViewModel();
        private string[] _settingsMenu = new string[] { "Utenti", "Banca dati" };
        private UsersViewModel _users = new UsersViewModel();
        private SetupConnectionViewModel _setupConnection = new SetupConnectionViewModel();
        #endregion
        #region Change Menu
        public string[] MainMenu { get { return _mainMenu; } }

        private string[] _currentMenu;
        public string[] CurrentMenu
        {
            get { return _currentMenu; }
            set { SetProperty(ref _currentMenu, value); }
        }

        private ICommand _switchMenuCommand;
        public ICommand SwitchMenuCommand
        {
            get
            {
                if (_switchMenuCommand == null) _switchMenuCommand = new RelayCommand(new Action<object>(SwitchMenu));
                return _switchMenuCommand;
            }
            set { SetProperty(ref _switchMenuCommand, value); }
        }

        private void SwitchMenu(object parameter)
        {
            string destination = (string)parameter;
            log.Debug(string.Format("Switch menu to: {0}", destination));
            switch (destination)
            {
                case "Tabelle":
                    CurrentMenu = _tablesMenu;
                    break;
                case "Transazioni":
                    CurrentMenu = _transactionsMenu;
                    break;
                case "Elenchi":
                    CurrentMenu = _listsMenu;
                    break;
                case "Impostazioni":
                    CurrentMenu = _settingsMenu;
                    break;
                default:
                    CurrentMenu = null;
                    break;
            }
            CurrentViewModel = null;
        }
        #endregion
        #region Change View
        private BindableBase _currentViewModel;
        public BindableBase CurrentViewModel
        {
            get
            {
                return _currentViewModel;
            }
            set { SetProperty(ref _currentViewModel, value); }
        }
        private ICommand _switchViewCommand;
        public ICommand SwitchViewCommand
        {
            get
            {
                if (_switchViewCommand == null) _switchViewCommand = new RelayCommand(new Action<object>(Navigate));
                return _switchViewCommand;
            }
            set { SetProperty(ref _switchViewCommand, value); }
        }
        private void Navigate(object parameter)
        {
            string destination = (string)parameter;
            log.Debug(string.Format("Navigate to: {0}", destination));
            switch (destination)
            {
                case "Prodotti":
                    CurrentViewModel = _products;
                    ((RelayCommand)_products.RefreshListCommand).CheckAndExecute(_products);
                    break;
                case "Categorie di Prodotto":
                    CurrentViewModel = _productCategories;
                    ((RelayCommand)_productCategories.RefreshListCommand).CheckAndExecute(_productCategories);
                    break;
                case "Partner":
                    CurrentViewModel = _partners;
                    ((RelayCommand)_partners.RefreshListCommand).CheckAndExecute(_partners);
                    break;
                case "Utenti":
                    CurrentViewModel = _users;
                    ((RelayCommand)_users.RefreshListCommand).CheckAndExecute(_users);
                    break;
                case "In arrivo":
                    CurrentViewModel = _incomingTransactions;
                    ((RelayCommand)_incomingTransactions.RefreshListCommand).CheckAndExecute(_incomingTransactions);
                    break;
                case "In uscita":
                    CurrentViewModel = _outgoingTransactions;
                    ((RelayCommand)_outgoingTransactions.RefreshListCommand).CheckAndExecute(_outgoingTransactions);
                    break;
                case "Inventario":
                    CurrentViewModel = _inventory;
                    ((RelayCommand)_inventory.RefreshListCommand).CheckAndExecute(_inventory);
                    break;
                case "Transazioni con i partner":
                    CurrentViewModel = _partnerTransactions;
                    ((RelayCommand)_partnerTransactions.RefreshListCommand).CheckAndExecute(_partnerTransactions);
                    break;
                case "Banca dati":
                    CurrentViewModel = _setupConnection;
                    break;
                default:
                    CurrentViewModel = null;
                    break;
            }
        }
        #endregion
        #region Close Main window
        private Window _mainWindow;
        public Window MainWindow
        {
            get { return _mainWindow; }
            set { SetProperty(ref _mainWindow, value); }
        }
        private ICommand _closeMainWindowCommand;
        public ICommand CloseMainWindowCommand
        {
            get
            {
                if (_closeMainWindowCommand == null) _closeMainWindowCommand = new RelayCommand(CloseWindow);
                return _closeMainWindowCommand;
            }
            set { SetProperty(ref _closeMainWindowCommand, value); }
        }
        private void CloseWindow(object parameter = null)
        {
            log.Debug("Close main window.");
            MainWindow?.Close();
        }
        #endregion
    }
}
