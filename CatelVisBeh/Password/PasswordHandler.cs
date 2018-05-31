using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CatelVisBeh.Password
{
    public static class PasswordHandler
    {

        public static readonly DependencyProperty BoundPassword = DependencyProperty.RegisterAttached("BoundPassword",
                                                                                                        typeof(string),
                                                                                                        typeof(PasswordHandler),
                                                                                                        new PropertyMetadata(string.Empty,
                                                                                                                             OnBoundPasswordChanged));

        public static readonly DependencyProperty BindPassword = DependencyProperty.RegisterAttached("BindPassword",
                                                                                                      typeof(bool),
                                                                                                      typeof(PasswordHandler),
                                                                                                      new PropertyMetadata(false,
                                                                                                                            OnBindPasswordChanged));

        private static readonly DependencyProperty UpdatingPassword = DependencyProperty.RegisterAttached("UpdatingPassword",
                                                                                                          typeof(bool),
                                                                                                          typeof(PasswordHandler),
                                                                                                          new PropertyMetadata(false));


        private static void OnBindPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PasswordBox pb)
            {
                var wasBound = (bool)e.OldValue;
                var needToBind = (bool)e.NewValue;

                if (wasBound)
                    pb.PasswordChanged -= HandlePasswordChanged;

                if (needToBind)
                    pb.PasswordChanged += HandlePasswordChanged;
            }
        }

        private static void OnBoundPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PasswordBox box = d as PasswordBox;

            if (d != null || GetBindPassword(d))
            {
                box.PasswordChanged -= HandlePasswordChanged;
                string newPass = (string)e.NewValue;

                if (!GetUpdatingPassword(box))
                {
                    box.Password = newPass;
                }

                box.PasswordChanged += HandlePasswordChanged;
            }
        }

        private static void HandlePasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox box = sender as PasswordBox;
            SetUpdatingPassword(box, true);

            SetBoundPassword(box, box.Password);
            SetUpdatingPassword(box, false);
        }



        public static void SetBoundPassword(DependencyObject box, string password)
        {
            box.SetValue(BoundPassword, password);
        }

        public static string GetBoundPassword(DependencyObject dp)
        {
            return (string)dp.GetValue(BoundPassword);
        }

        public static void SetBindPassword(DependencyObject dp, bool value)
        {
            dp.SetValue(BindPassword, value);
        }

        public static bool GetBindPassword(DependencyObject d)
        {
            return (bool)d.GetValue(BindPassword);
        }

        private static bool GetUpdatingPassword(DependencyObject box)
        {
            return (bool)box.GetValue(UpdatingPassword);
        }
        private static void SetUpdatingPassword(DependencyObject DO, bool v)
        {
            DO.SetValue(UpdatingPassword, v);
        }

    }
}
