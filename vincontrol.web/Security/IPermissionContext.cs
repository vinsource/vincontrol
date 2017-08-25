using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Vincontrol.Web.Security
{
    public interface IPermissionContext
    {
        /// <summary>
        /// Collection of Permission Codes and Values
        /// </summary>
        Dictionary<string, string> PermissionData { get; }

        /// <summary>
        /// The currently logged in user
        /// </summary>
        string Username { get; }
        bool HasReadAccess(string permissionCode);
        bool HasWriteAccess(string permissionCode);
        /// <summary>
        /// Builds permission data for the currently logged in user
        /// </summary>
        /// <param name="context">The current HttpContext</param>
        void Build(HttpContextBase context);
    }
}
