using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Web;
using vincontrol.Data.Interface;
using vincontrol.Data.Repository;

namespace vincontrol.Security
{
    public class PermissionContext : IPermissionContext
    {
        #region Fields
        private IUnitOfWork _unitOfWork;

        public const string NO_ACCESS = "NONE";
        public const string ALL_ACCESS = "ALLACCESS";
        public const string READONLY = "READONLY";

        #endregion

        #region Constructor

        public PermissionContext() : this(new SqlUnitOfWork()) { }

        public PermissionContext(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region IPermissionContext Members

        public Dictionary<string, string> PermissionData { get; private set; }

        public string Username { get; private set; }

        public void Build(HttpContextBase context)
        {
            Username = GetUsername(context.User).ToLower();
            PermissionData = new Dictionary<string, string>();

            //Get the user groups of the logged in user.  Typicaly this will only be one, but some users do belong to multiple groups.
            //When a user does belong to multiple groups there is a potential for conflicts among the same permission codes.  When this happens, the permisison value with the greates weight
            //will tae precedence
            var groups = _unitOfWork.ExtendedUser.GetAll().Select(ug => new { ug.UserPermissions.FirstOrDefault().RoleId, ug.UserName }).Where(g => g.UserName.ToLower() == Username);

            foreach (var userGroup in groups)
            {
                Buid(userGroup.RoleId);
            }
        }

        private void Buid(int groupId)
        {
            var groupPermissions = _unitOfWork.GroupPermission.GetAll()
                    .Select(gp => new { gp.RoleId, gp.PermissionCode.PermissionCodeText, gp.PermissionValue.PermissionValueText })
                    .Where(gp => gp.RoleId == groupId);

            foreach (var g in groupPermissions)
            {
                if (!PermissionData.ContainsKey(g.PermissionCodeText) && g.PermissionValueText != NO_ACCESS)
                    PermissionData.Add(g.PermissionCodeText, g.PermissionValueText);
                else if (PermissionData.ContainsKey(g.PermissionCodeText) && PermissionData[g.PermissionValueText] != ALL_ACCESS)
                    PermissionData[g.PermissionCodeText] = g.PermissionValueText;
            }
        }

        public bool HasReadAccess(string permissionCode)
        {
            return PermissionData.ContainsKey(permissionCode);
        }

        public bool HasWriteAccess(string permissionCode)
        {
            return (PermissionData.ContainsKey(permissionCode) && PermissionData[permissionCode] == ALL_ACCESS);
        }
        #endregion

        /// <summary>
        /// Strips out the expected "[DomainName]\\" portion of the windows username
        /// </summary>
        private string GetUsername(IPrincipal p)
        {
            string idName = p.Identity.Name;
            int indx = idName.IndexOf("\\");
            idName = idName.Substring(indx + 1);
            return idName.Trim();
        }
    }
}
