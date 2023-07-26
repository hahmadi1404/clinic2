using System;
using System.Collections.Generic;

namespace QCS_Config.Models
{
    public partial class Insurance
    {
        public int Id { get; set; }
        public int ClinicId { get; set; }
        public string? InsuranceName { get; set; }
    }
}
