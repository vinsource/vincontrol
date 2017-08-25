using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;


namespace WhitmanEnterpriseMVC.Security
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
            Username = GetUsername(context.User).ToLower();
            PermissionData = new Dictionary<string, string>();
            
            //Get the user groups of the logged in user.  Typicaly this will only be one, but some users do belong to multiple groups.
            //When a user does belong to multiple groups there is a potential for conflicts among the same permission codes.  When this happens, the permisison value with the greates weight
            //will tae precedence
            using (var dbContext = new DatabaseModel.whitmanenterprisewarehouseEntities())
            {
                var groups = dbContext.vincontrolusergroups.Select(
                    ug => new { ug.vincontrolgroup.groupid, ug.vincontroluser.username })
                    .Where(g => g.username.ToLower() == Username);

                foreach (var userGroup in groups)
                {
                    Buid(userGroup.groupid);
                }
            }
        }

        private void Buid(int groupId)
        {
            using (var dbContext = new DatabaseModel.whitmanenterprisewarehouseEntities())
            {
                var groupPermissions =
                    dbContext.vincontrolgrouppermissions
                    .Select(gp => new { gp.vincontrolgroup.groupid, gp.vincontrolpermissioncode.permissioncode, gp.vincontrolpermissionvalue.permissionvalue })
                    .Where(gp => gp.groupid == groupId);

                foreach (var g in groupPermissions)
                {
                    if (!PermissionData.ContainsKey(g.permissioncode) && g.permissionvalue != NO_ACCESS)
                        PermissionData.Add(g.permissioncode, g.permissionvalue);
                    else if (PermissionData.ContainsKey(g.permissioncode) && PermissionData[g.permissioncode] != ALL_ACCESS)
                        PermissionData[g.permissioncode] = g.permissionvalue;
                }
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