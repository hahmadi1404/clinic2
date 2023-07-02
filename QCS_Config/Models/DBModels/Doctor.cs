using System;
using System.Collections.Generic;

namespace QCS_Config.Models
{
    public partial class Doctor
    {
        public int DrId { get; set; }
        public int ClinikId { get; set; }
        public string? DrName { get; set; }
        public string? DrSpecialty { get; set; }
        public string? Days { get; set; }
        public string? Image { get; set; }
        public int? SectionId { get; set; }
    }
}
