using System;
using System.Collections.Generic;

namespace QCS_Config.Models.DBModels
{
    public partial class UserT
    {
        public int UserId { get; set; }
        public string Name { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public DateTime? CreateTime { get; set; }
        public bool? State { get; set; }
        public int? ClinicId { get; set; }
        public int Owner { get; set; }
    }
}
