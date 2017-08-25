using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace WhitmanEnterpriseMVC.Security
{
    /// <summary>
    /// Authorizes the current logged in user against permissions.
    /// Using the PermissionCode and AcceptedValues properties, we can
    /// verify that the currently logged in user has one of the AcceptedValues for the specified PermissionCode.
    /// </summary>
    /// <example>
    /// <para>Single Accepted Values:</para>
    /// <para>[VinControlAuthorization(PermissionCode = "AMMRETSE", AcceptedValues = "ALLACCESS")]</para>
    /// <para>Multiple Accepted Values:</para>
    /// <para>[VinControlAuthorization(PermissionCode = "AMMRETSE", AcceptedValues = "READONLY, ALLACCESS")]</para>
    /// </example>
    public class VinControlAuthorizationAttribute : AuthorizeAttribute
    {
        private string _acceptedValues;
        private string[] _acceptedValuesSplit = new string[0];
        private string _permissionCode;
        private Dictionary<string, string> _userPermissions = new Dictionary<string, string>();

        /// <summary>
        /// The permission code to use for value checking
        /// </summary>
        public string PermissionCode
        {
            get { return _permissionCode ?? string.Empty; }
            set { _permissionCode = value; }
        }

        /// <summary>
        /// Comma seperated list of permission values that are accepted for the supplied permission code
        /// </summary>
        public string AcceptedValues
        {
            get
            {
                return _acceptedValues ?? string.Empty;
            }
            set
            {
                _acceptedValues = value;
                _acceptedValuesSplit = SplitString(value);
            }
        }

        protected override bool AuthorizeCore(System.Web.HttpContextBase httpContext)
        {
            //if the base authroization doesn't pass, there is no need to continue with the athena specific authorization
            if (!base.AuthorizeCore(httpContext))
                return false;

            if (!string.IsNullOrEmpty(_permissionCode) && !_userPermissions.ContainsKey(_permissionCode))
                return false;

            if (_acceptedValuesSplit.Length > 0 && !_acceptedValuesSplit.Contains(_userPermissions[_permissionCode]))
                return false;

            return true;
        }

        public IEnumerable<Type> GetAllBaseTypes(Type child)
        {
            var parents = new List<Type>();

            var currentParent = child.BaseType;
            while (currentParent != null)
            {
                parents.Add(currentParent);
                currentParent = currentParent.BaseType;
            }
            return parents;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var parents = GetAllBaseTypes(filterContext.Controller.GetType());
            if (!parents.Contains(typeof(SecurityController)))
                throw new NotSupportedException(string.Format("Unable to authorize: {0} does not implement {1}", filterContext.Controller.GetType(), typeof(SecurityController)));

            var currentController = (SecurityController)filterContext.Controller;
            _userPermissions = currentController.PermissionsContext.PermissionData;

            base.OnAuthorization(filterContext);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            base.HandleUnauthorizedRequest(filterContext);
            filterContext.HttpContext.Response.Redirect(string.Format("~/Security/Unauthorized?username={0}", filterContext.HttpContext.User.Identity.Name));
        }

        internal static string[] SplitString(string original)
        {
            if (String.IsNullOrEmpty(original))
            {
                return new string[0];
            }

            var split = from piece in original.Split(',')
                        let trimmed = piece.Trim()
                        where !String.IsNullOrEmpty(trimmed)
                        select trimmed;
            return split.ToArray();
        }
    }
}