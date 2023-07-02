using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class UserModel
    {
        //public long UserID  { get; set; }
        public int UserID { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public DateTime CreateTime { get; set; }
        public bool isAdmin { get; set; }
        public int owner { get; set; }
    }

    public class UserPermissionModel
    {
        public long UserId { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public DateTime CreateTime { get; set; }
        public string Permission { get; set; }
        public bool isAdmin { get; set; }
        public int Owner { get; set; }
    }

    public class UserPermissionsModel
    {
        public string Name { get; set; }
        public string UserName { get; set; }
        public string password { get; set; }
        public List<string> Permissions { get; set; }

        public bool isAdmin { get; set; }
    }
}