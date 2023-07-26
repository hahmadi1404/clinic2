using System;
using System.Collections.Generic;

namespace QCS_Config.Models
{
    public partial class Bill
    {
        public int BillId { get; set; }
        public int ClinicId { get; set; }
        public string Mobile { get; set; } = null!;
        public long NationalCode { get; set; }
        public string? DosierIdReq { get; set; }
        public string? FullNameReq { get; set; }
        public long? NationalCodeReq { get; set; }
        public string? MobileReq { get; set; }
        public string? FromDatePersian { get; set; }
        public string? ToDatePersian { get; set; }
        public string? RequestDatePersian { get; set; }
        public string? BillPath { get; set; }
        public int? Status { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public DateTime? RequestDate { get; set; }
    }
}
