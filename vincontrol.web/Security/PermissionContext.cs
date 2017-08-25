using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using vincontrol.Application.ViewModels.AccountManagement;
using vincontrol.Data.Model;
using Vincontrol.Web.Handlers;
using Vincontrol.Web.Models;

namespace Vincontrol.Web.Security
{
    public class PermissionContext : IPermissionContext
    {
        #region Fields

        //private readonly IUserGroupRepository _userGroupRepository;
        //private readonly IGroupPermissionRepository _permissionRepository;

        public const string NO_ACCESS = "NONE";
        public const string ALL_ACCESS = "ALLACCESS";
        public const string READONLY = "READONLY";

        #endregion

        #region Constructor

        //public PermissionContext(IUserGroupRepository userGroupRepository, IGroupPermissionRepository permissionRepository)
        //{
        //    _userGroupRepository = userGroupRepository;
        //    _permissionRepository = permissionRepository;
        //}

        public PermissionContext()
        {
            //_userGroupRepository = new UserGroupRepository();
            //_permissionRepository = new GroupPermissionRepository();
        }
        #endregion

        #region IPermissionContext Members

        public Dictionary<string, string> PermissionData { get; private set; }

        public string Username { get; private set; }

        public void Build(HttpContextBase context)
        {
            var currentUserSession = SessionHandler.CurrentUser;

            if (currentUserSession != null && currentUserSession.AccessDealerPermissions != null)
            {
                var currentUser = (UserRoleViewModel) currentUserSession;

                Username = currentUser.Username;
               
                PermissionData = new Dictionary<string, string>();

                using (var dbContext = new VincontrolEntities())
                {
                    var roles = currentUser.AccessDealerPermissions.Select(x=>x.RoleId).Distinct().ToList();

                    var groupPermissions = dbContext.RolePermissions.Select(gp => new { gp.RoleId, gp.PermissionCode, gp.PermissionValue }).ToList();

                    foreach (var role in roles)
                    {
                        var groupRolePermissions = groupPermissions.Where(gp => gp.RoleId == role);

                        foreach (var g in groupRolePermissions)
                        {
                            if (!PermissionData.ContainsKey(g.PermissionCode.PermissionCodeText) && g.PermissionValue.PermissionValueText != NO_ACCESS)
                                PermissionData.Add(g.PermissionCode.PermissionCodeText, g.PermissionValue.PermissionValueText);
                            else if (PermissionData.ContainsKey(g.PermissionCode.PermissionCodeText) &&
                                     PermissionData[g.PermissionCode.PermissionCodeText] != ALL_ACCESS)
                                PermissionData[g.PermissionCode.PermissionCodeText] = g.PermissionValue.PermissionValueText;
                        }
                    }
                }
            }
        
        
        }

        //private void Buid(int roleId)
        //{
        //    using (var dbContext = new VincontrolEntities())
        //    {
        //        var groupPermissions =
        //            dbContext.RolePermissions.Where(gp => gp.RoleId == roleId)
        //                     .Select(gp => new {gp.RoleId, gp.PermissionCode, gp.PermissionValue});
              
        //        foreach (var g in groupPermissions)
        //        {
        //            if (!PermissionData.ContainsKey(g.PermissionCode.PermissionCodeText) && g.PermissionValue.PermissionValueText != NO_ACCESS)
        //                PermissionData.Add(g.PermissionCode.PermissionCodeText, g.PermissionValue.PermissionValueText);
        //            else if (PermissionData.ContainsKey(g.PermissionCode.PermissionCodeText) &&
        //                     PermissionData[g.PermissionCode.PermissionText] != ALL_ACCESS)
        //                PermissionData[g.PermissionCode.PermissionCodeText] = g.PermissionValue.PermissionValueText;
        //        }
                
        //    }
        //}

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
            int indx = idName.IndexOf("\\", System.StringComparison.Ordinal);
            idName = idName.Substring(indx + 1);
            return idName.Trim();
        }
    }
}