using Catel;
using Catel.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace CatelVisBeh.Authentication
{
    using CatelVisBeh.Roles;
    using CatelVisBeh.Utils;

    public class AuthProvider : IAuthenticationProvider, CatelVisBeh.Services.Interfaces.INotifyView
    {
        #region Properties
        private Roles _Role;
        public Roles Role { get { return _Role; }
            set
            {
                _Role = value;
                OnNotification();
            }
        }
        public ICollection<string> KeyWords { get; set; }

        public event EventHandler Notification;

        #endregion

        #region IAuthenticationProvider Members
        public bool CanCommandBeExecuted(ICatelCommand command, object commandParameter)
        {
            return true;
        }

        public bool HasAccessToUIElement(FrameworkElement element, object tag, object authenticationTag)
        {
            var authe = authenticationTag as string;
            string[] words = System.Text.RegularExpressions.Regex.Split(authe.Trim(), @"\W+");

            Roles _authenticatingTag = Roles.None;

            if(words[0].ToLower() == "only" && words.Length >= 2 )
            {
                _authenticatingTag = ColFlag(words.Skip(1));
                if (_authenticatingTag.HasFlag(Role) && (Role != Roles.None)) return true;
            }
            else if (words.Length == 1)
            {
                _authenticatingTag = EnumUtil.GetEnumType<Roles>(words[0]);
                if (Role >= _authenticatingTag) return true;
            }

            return false;
        }

        

        private Roles ColFlag(IEnumerable<string> words)
        {
            Roles x = Roles.None;

            foreach (var item in words)
            {
                x |= EnumUtil.GetEnumType<Roles>(item);
            }

            return x;
        }
        #endregion

        #region Implements INotifyView

        public void OnNotification()
        {
            Notification?.Invoke(this, new EventArgs());
        }

        #endregion
    }
}
