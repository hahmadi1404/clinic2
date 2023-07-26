using System;
using System.Collections.Generic;

namespace QCS_Config.Models
{
    public partial class UserT
    {
        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public DateTime? CreateTime { get; set; }
        public bool? Status { get; set; }
        public int? ClinicId { get; set; }
        public int? Owner { get; set; }
    }
}
