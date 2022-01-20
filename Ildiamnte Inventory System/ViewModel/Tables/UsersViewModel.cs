using Ildiamnte_Inventory_System.Model;
using Ildiamnte_Inventory_System.Notifications;
using System;
using EntityLayer;
using BusinessLayer;
using Ildiamnte_Inventory_System.Views;

namespace Ildiamnte_Inventory_System.ViewModel
{
    public class UsersViewModel : TableModel<UserEntity>
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public UsersViewModel()
        {
            ItemName = "utenti";
            TableName = "Utenti";
        }
        protected override void DeleteItem(object parameter)
        {
            log.Debug("Delete " + ItemName + " button");

            string UserID = SelectedItem.Username;
            UserLogin.RemoveUser(UserID);
            NotificationProvider.Info("User deleted", String.Format("Username: {0}", UserID));
            RefreshList(parameter);
        }

        protected override void EditItem(object parameter)
        {
            log.Debug("Edit " + ItemName + " button");

            string UserID = SelectedItem.Username;
            EditUserViewModel EUVM = new EditUserViewModel(UserID);
            EditUserWindow EUV = new EditUserWindow() { DataContext = EUVM };
            EUVM.EditWindow = EUV;
            EUV.ShowDialog();
            RefreshList(parameter);
        }

        protected override void NewItem(object parameter)
        {
            log.Debug("New " + ItemName + " button");

            NewUserWindow NUW = new NewUserWindow();
            NUW.ShowDialog();
            RefreshList(parameter);
        }

        protected override void RefreshList(object parameter)
        {
            log.Debug("Refresh " + ItemName + " list");

            List = UserLogin.ListUsers();
        }
    }
}
