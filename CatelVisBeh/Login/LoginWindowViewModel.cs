using Catel.Fody;
using Catel.IoC;
using Catel.MVVM;
using CatelVisBeh.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatelVisBeh.Login
{
    public class LoginWindowViewModel : ViewModelBase
    {
        public override string Title => "Login Window View Model";

        public LoginWindowViewModel()
        {
            AccessLayer = (IUserService)ServiceLocator.Default.GetService(typeof(IUserService));

            LoginCommand = new Command(OnLoginExecute);
        }

        [Model]
        [Expose("UserList")]
        [Expose("LoggedUser")]
        public IUserService AccessLayer { get; private set; }

        public Command LoginCommand { get; private set; }
       
        private void OnLoginExecute()
        {
            if (AccessLayer.LogIn(this.Name, this.Password))
                this.SaveAndCloseViewModelAsync();
        }

        public string Name { get; set; }
        public string Password { get; set; }

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();
        }

        protected override async Task CloseAsync()
        {
            await base.CloseAsync();
        }

    }
}
