using System;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Windows;
using Catel.MVVM;
using Catel.Data;
using Catel.IoC;
using CatelVisBeh.Services.Interfaces;
using CatelVisBeh.Authentication;
using CatelVisBeh.Utils;

namespace CatelVisBeh.Models
{
    public class UserServiceModel : ViewModelBase, IUserService//, IRequestFocus
    {
        #region Constructors

        public UserServiceModel()
        {
            this.UserList = new UserList();



        }

        #endregion Constructors

        #region Properties
        public UserList UserList { get; set; }

        /// <summary>
        /// Default user to use in no login needed.
        /// </summary>
        public User DefaultUser
        {
            get
            {
                if (GetValue<User>(DefaultUserProperty) == null)
                    SetValue(DefaultUserProperty, new User { Password = EncryptPass("default") });

                return GetValue<User>(DefaultUserProperty);
            }
            private set { SetValue(DefaultUserProperty, value); }
        }
        public static readonly PropertyData DefaultUserProperty = RegisterProperty("DefaultUser", typeof(User));

        /// <summary>
        /// Property defualt user with public access.
        /// </summary>
        public User LoggedUser
        {
            get
            {
                if (GetValue<User>(LoggedUserProperty) == null)
                    SetValue(LoggedUserProperty, DefaultUser);

                return GetValue<User>(LoggedUserProperty);
            }
            set { SetValue(LoggedUserProperty, value); }
        }
        public static readonly PropertyData LoggedUserProperty = RegisterProperty("LoggedUser", typeof(User));


        #endregion Properties

        #region Member Functions

        /// <summary>
        /// Creates and store a new user.
        /// </summary>
        public User CreateUser()
        {
            var user = new User();
            user.Password = EncryptPass(user.Password);
            UserList.AllUsers.Add(user);
            return user;
        }

        /// <summary>
        /// Creates and store a new user.
        /// </summary>
        public void CreateUser(string name, string password, Roles.Roles level)
        {
            var user = new User
            {
                Name = name,
                Password = EncryptPass(password),
                AccessLevel = level
            };
            UserList.AllUsers.Add(user);
            UserList.SaveToFile();
        }

        /// <summary>
        /// Delete selected user. Capability of root.
        /// </summary>
        /// <param name="name"></param>
        public void DeleteUser(User user)
        {
            if (user != null && user != LoggedUser)
            {
                UserList.AllUsers.Remove(user);
                UserList.SaveToFile();
            }
        }

        /// <summary>
        /// Log off.
        /// </summary>
        public void LoggOff()
        {
            if (LoggedUser != DefaultUser)
            {
                // PresentationCore.TryMessageBoxInMainWindow(string.Format($"Odhlasenie uzivatela : {LoggedUser.Name}"), "ODHLASENIE", MessageBoxButton.OK, MessageBoxImage.Information);
                LoggedUser = DefaultUser;
                ((AuthProvider)ServiceLocator.Default.GetService(typeof(IAuthenticationProvider))).Role = LoggedUser.AccessLevel;
            }

            /// SecurityConverter.UpdateAccess();
        }

        /// <summary>
        /// Encrypt string.
        /// </summary>
        /// <param name="passToEncrypt"></param>
        /// <returns>Encrypted password.</returns>
        public static string EncryptPass(string passToEncrypt)
        {
            HashAlgorithm sha = new SHA1CryptoServiceProvider();
            var charArray = passToEncrypt.ToCharArray();
            var byteArray = charArray.ToList().ConvertAll(p => (byte)p).ToArray();
            var encryptedBytes = sha.ComputeHash(byteArray);

            var sb = new StringBuilder();

            foreach (var enc in encryptedBytes)
            {
                sb.Append(enc);
            }

            return sb.ToString();
        }


        /// <summary>
        /// Log in.
        /// </summary>
        public bool LogIn(string name, string password)
        {
            bool retVal = false;

            if (Checker.IsOkay(password))
                if (Checker.IsOkay(name))
                {
                    var users = UserList.AllUsers.ToList();
                    var user = users.Find(x => x.Name.Equals(name));
                    if (user != null && EncryptPass(password).Equals(user.Password))
                    {
                        LoggedUser = user;
                        password = string.Empty;
                        ((AuthProvider)ServiceLocator.Default.GetService(typeof(IAuthenticationProvider))).Role = LoggedUser.AccessLevel;
                        retVal = true;

                    }

                }
            return retVal;
        }

        /// <summary>
        /// Used to log internaly for developer purposes or to take shortcut to log in.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <param name="level"></param>
        public void LogInInternaly(string name, string password, Roles.Roles level)
        {

            var users = UserList.AllUsers.ToList();
            var user = users.Find(x => x.Name.Equals(name));

            if (user != null /*&& EncryptPass(Password).Equals(user.Password)*/)
            {
                LoggedUser = user;
            }
            else if (user == null)
            {
                LoggedUser = CreateUser();
            }
            ///SecurityConverter.UpdateAccess();
        }

        /// <summary>
        /// Nullify helping prop. Save Changes on users to file on hardisk. Notify that change via dialog.
        /// </summary>
        private void NullSaveNotify(string Password, string NewPass, string VerifyNewPass)
        {
            UserList.SaveToFile();
            Password = string.Empty;
            NewPass = string.Empty;
            VerifyNewPass = string.Empty;
        }

        /// <summary>
        /// Changes password. Changing password formula differs if root/non-root user.
        /// </summary>
        /// <param name="asRoot"></param>
        public void ChangePassword(string Password, string NewPass, string VerifyNewPass)
        {
            if (EncryptPass(Password) == LoggedUser.Password)
            {
                if (Checker.IsOkay(NewPass))
                    if (Checker.IsOkay(VerifyNewPass))
                        if (NewPass == VerifyNewPass)
                        {
                            LoggedUser.Password = EncryptPass(NewPass);
                            NullSaveNotify(Password, NewPass, VerifyNewPass);
                        }
            }
        }

        #endregion Member Functions

        public bool LogInternaly(string name)
        {
            throw new NotImplementedException();
        }
    }
}
