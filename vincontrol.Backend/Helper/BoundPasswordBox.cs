﻿using System.Windows;
using System.Windows.Controls;

namespace vincontrol.Backend.Helper
{
    public class BoundPasswordBox
    {
        #region BoundPassword
        private static bool _updating;

        /// <summary>
        /// BoundPassword Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty BoundPasswordProperty =
            DependencyProperty.RegisterAttached("BoundPassword",
                typeof(string),
                typeof(BoundPasswordBox),
                new FrameworkPropertyMetadata(string.Empty, OnBoundPasswordChanged));

        /// <summary>
        /// Gets the BoundPassword property.
        /// </summary>
        public static string GetBoundPassword(DependencyObject d)
        {
            return (string)d.GetValue(BoundPasswordProperty);
        }

        /// <summary>
        /// Sets the BoundPassword property.
        /// </summary>
        public static void SetBoundPassword(DependencyObject d, string value)
        {
            d.SetValue(BoundPasswordProperty, value);
        }

        /// <summary>
        /// Handles changes to the BoundPassword property.
        /// </summary>
        private static void OnBoundPasswordChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var password = d as PasswordBox;
            if (password != null)
            {
                // Disconnect the handler while we're updating.
                password.PasswordChanged -= PasswordChanged;
            }

            if (e.NewValue != null)
            {
                if (!_updating)
                {
                    if (password != null) password.Password = e.NewValue.ToString();
                }
            }
            else
            {
                if (password != null) password.Password = string.Empty;
            }
            // Now, reconnect the handler.
            if (password != null) password.PasswordChanged += PasswordChanged;
        }

        /// <summary>
        /// Handles the password change event.
        /// </summary>
        static void PasswordChanged(object sender, RoutedEventArgs e)
        {
            var password = sender as PasswordBox;
            _updating = true;
            if (password != null) SetBoundPassword(password, password.Password);
            _updating = false;
        }

        #endregion
    }
}
