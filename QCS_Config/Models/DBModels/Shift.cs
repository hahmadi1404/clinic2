using System;
using System.Collections.Generic;

namespace QCS_Config.Models
{
    public partial class Shift
    {
        public int Id { get; set; }
        public string? ShiftName { get; set; }
        public int ClinicId { get; set; }
        public int DepartmentId { get; set; }
    }
}
