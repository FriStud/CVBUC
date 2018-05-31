using Catel.Fody;
using Catel.IoC;
using Catel.MVVM;
using CatelVisBeh.Login;
using CatelVisBeh.Models;
using CatelVisBeh.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CatelVisBeh.ViewModels
{
    public class ActionViewModel : ViewModelBase
    {

        public ActionViewModel()
        {
            Model = new ActionModel();

            ChangeUserRole = new Command(OnChangeUserRoleExecute);

            LogIn = new Command(OnLogInExecute); // move to constructor this initialization...
        }

        public override string Title { get { return "CatelVisBeh"; } }

        [Model]
        [Expose("SelectedRole")]
        [Expose("AllRoles")]
        public ActionModel Model { get; private set; }

        // TODO: Register models with the vmpropmodel codesnippet
        // TODO: Register view model properties with the vmprop or vmpropviewmodeltomodel codesnippets
        // TODO: Register commands with the vmcommand or vmcommandwithcanexecute codesnippets


        public Command ChangeUserRole { get; private set; }

        private void OnChangeUserRoleExecute()
        {
            ((INotifyView)ServiceLocator.Default.GetService(typeof(INotifyView))).Role = Model.SelectedRole;
        }
        //--------------------------------Command ChangeUserRole-------------------------------------


        #region  ----------- VM_Command_LogIn -----------

        public Command LogIn { get; private set; }

        private void OnLogInExecute()
        {
            var window = new LoginWindow
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                Owner = Application.Current.MainWindow,
                DataContext = new LoginWindowViewModel()
            };

            window.ShowDialog();
        }
        #endregion //  ----------- VM_Command_LogIn ---------

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            // TODO: subscribe to events here
        }

        protected override async Task CloseAsync()
        {
            // TODO: unsubscribe from events here

            await base.CloseAsync();
        }
    }
}
