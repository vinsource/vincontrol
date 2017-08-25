using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace vincontrol.Backend.Helper
{
    public class Permission
    {
        private List<string> roles = new List<string>();
        public List<string> Roles
        {
            get { return roles; }
            set { roles = value; }
        }
    }

    public class PermissionBehavior
    {
        // Static behaviour parameters
        public static readonly Dictionary<FrameworkElement, PermissionBehavior> Instances = new Dictionary<FrameworkElement, PermissionBehavior>();

        public static readonly DependencyProperty PermissionProperty =
            DependencyProperty.RegisterAttached("Permission", typeof(Permission), typeof(PermissionBehavior),
                                                new PropertyMetadata(OnPermissionPropertyChanged));

        public static readonly DependencyProperty RoleProperty =
            DependencyProperty.RegisterAttached("Role", typeof(string), typeof(PermissionBehavior),
                                                new PropertyMetadata(OnRolePropertyChanged));

        private static void OnPermissionPropertyChanged(DependencyObject dependencyObject,
                                                      DependencyPropertyChangedEventArgs e)
        {
            SetPermission(dependencyObject, (Permission)e.NewValue);
        }

        public static void SetPermission(DependencyObject obj, Permission value)
        {
            var behavior = GetAttachedBehavior(obj as FrameworkElement);
            behavior.AssociatedObject = obj as FrameworkElement;
            obj.SetValue(PermissionProperty, value);

            behavior.CurrentPermission = value;
            behavior.UpdateVisibility();
        }

        public static object GetPermission(DependencyObject obj)
        {
            return obj.GetValue(PermissionProperty);
        }

        private static void OnRolePropertyChanged(DependencyObject dependencyObject,
                                                      DependencyPropertyChangedEventArgs e)
        {
            SetRole(dependencyObject, (string)e.NewValue);
        }

        public static void SetRole(DependencyObject obj, string value)
        {
            var behavior = GetAttachedBehavior(obj as FrameworkElement);
            behavior.AssociatedObject = obj as FrameworkElement;
            obj.SetValue(RoleProperty, value);

            behavior.RoleList = value.Split(',').ToList();
            behavior.UpdateVisibility();
        }

        public static object GetRole(DependencyObject obj)
        {
            return obj.GetValue(RoleProperty);
        }

        private static PermissionBehavior GetAttachedBehavior(FrameworkElement obj)
        {
            if (!Instances.ContainsKey(obj))
            {
                Instances[obj] = new PermissionBehavior { AssociatedObject = obj };
            }

            return Instances[obj];
        }

        static PermissionBehavior()
        {

        }

        // Class instance parameters
        private FrameworkElement AssociatedObject { get; set; }
        private Permission CurrentPermission { get; set; }
        private List<string> RoleList { get; set; }

        private void UpdateVisibility()
        {
            if (RoleList == null || CurrentPermission == null)
                return;

            if (RoleList.Intersect(CurrentPermission.Roles).Any())
            {
                AssociatedObject.Visibility = Visibility.Visible;
                AssociatedObject.IsEnabled = true;
            }
            else
            {
                AssociatedObject.Visibility = Visibility.Hidden;
                AssociatedObject.IsEnabled = false;
            }
        }
    }
}
