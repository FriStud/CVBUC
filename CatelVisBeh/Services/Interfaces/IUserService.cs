using Catel.MVVM;
using CatelVisBeh.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatelVisBeh.Services.Interfaces
{
   public interface IUserService 
    {
        User LoggedUser { get; }
        UserList UserList { get;}

        // return true if is logged, called only in luncher
        bool LogInternaly(string name);

        // use for WPF login
        bool LogIn(string name, string password);

        void CreateUser(string name, string password,  Roles.Roles level);

        void LoggOff();

        void DeleteUser(User name);

        void ChangePassword(string Password,string NewPass,string VerifyNewPass);

        User CreateUser();

    }
}
