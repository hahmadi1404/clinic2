using System;
using System.Collections.Generic;

namespace QCS_Config.Models
{
    public partial class ReserveStatus
    {
        public int StatusId { get; set; }
        public int ClinicId { get; set; }
        public string? StatusName { get; set; }
    }
}
