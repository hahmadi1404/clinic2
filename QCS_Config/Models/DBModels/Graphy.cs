using System;
using System.Collections.Generic;

namespace QCS_Config.Models
{
    public partial class Graphy
    {
        public int GraphyId { get; set; }
        public int ClinicId { get; set; }
        public string Mobile { get; set; } = null!;
        public long NationalCode { get; set; }
        public string? DosierIdReq { get; set; }
        public string? FullNameReq { get; set; }
        public long? NationalCodeReq { get; set; }
        public string? MobileReq { get; set; }
        public string? FromDate { get; set; }
        public string? ToDate { get; set; }
        public string? RequestDate { get; set; }
        public string? GraphyPath { get; set; }
    }
}
