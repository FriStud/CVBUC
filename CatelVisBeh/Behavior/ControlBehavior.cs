using Catel.IoC;
using Catel.MVVM;
using Catel.Windows.Controls;
using CatelVisBeh.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interactivity;

namespace CatelVisBeh.Behavior
{
    public class ControlBehavior : Behavior<UserControl>
    {

        public static readonly DependencyProperty AccessLevelProperty = DependencyProperty.RegisterAttached(
                                                                                    "AccessLevel"
                                                                                    , typeof(string)
                                                                                    , typeof(ControlBehavior)
                                                                                    , new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty VisibilityActionProperty = DependencyProperty.RegisterAttached(
                                                                                    "VisibilityAction"
                                                                                    , typeof(System.Windows.Visibility)
                                                                                    , typeof(ControlBehavior)
                                                                                    , new PropertyMetadata(System.Windows.Visibility.Visible));

        public string AccessLevel
        {
            get { return (string)GetValue(AccessLevelProperty); }
            set { SetValue(AccessLevelProperty, value); }
        }

        public Visibility VisibilityAction
        {
            get { return (System.Windows.Visibility)GetValue(VisibilityActionProperty); }
            set { SetValue(VisibilityActionProperty, value); }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            var objNot = (INotifyView)ServiceLocator.Default.GetService(typeof(INotifyView));
            objNot.Notification += OnRolesUpdates;

            var auth = (IAuthenticationProvider)ServiceLocator.Default.GetService(typeof(IAuthenticationProvider));
            if (!(auth.HasAccessToUIElement(null, null, AccessLevel)))
                AssociatedObject.Visibility = VisibilityAction;
        }


        private void OnRolesUpdates(object sender, EventArgs args)
        {
            if (((IAuthenticationProvider)sender).HasAccessToUIElement(null, null, AccessLevel))
                AssociatedObject.Visibility = Visibility.Visible;
            else
                AssociatedObject.Visibility = VisibilityAction;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            var objNot = (INotifyView)ServiceLocator.Default.GetService(typeof(INotifyView));
            objNot.Notification -= OnRolesUpdates;
        }

    }
}
