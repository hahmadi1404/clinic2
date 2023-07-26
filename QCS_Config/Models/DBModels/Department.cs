using System;
using System.Collections.Generic;

namespace QCS_Config.Models
{
    public partial class Department
    {
        public int Id { get; set; }
        public string? DepartmentName { get; set; }
        public int ClinicId { get; set; }
    }
}
