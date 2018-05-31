namespace CatelVisBeh.ViewModels
{
    using Catel.Fody;
    using Catel.IoC;
    using Catel.MVVM;
    using CatelVisBeh.Login;
    using CatelVisBeh.Models;
    using CatelVisBeh.Services.Interfaces;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;

    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
           
        }

       


        public override string Title { get { return "CatelVisBeh"; } }

        // TODO: Register models with the vmpropmodel codesnippet
        // TODO: Register view model properties with the vmprop or vmpropviewmodeltomodel codesnippets
        // TODO: Register commands with the vmcommand or vmcommandwithcanexecute codesnippets


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
