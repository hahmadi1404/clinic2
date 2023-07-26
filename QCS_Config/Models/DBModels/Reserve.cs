using System;
using System.Collections.Generic;

namespace QCS_Config.Models
{
    public partial class Reserve
    {
        public int ReserveId { get; set; }
        public int ClinicId { get; set; }
        public string Mobile { get; set; } = null!;
        public long NationalCode { get; set; }
        public string? DosierIdReq { get; set; }
        public string? FullNameReq { get; set; }
        public long? NationalCodeReq { get; set; }
        public string? MobileReq { get; set; }
        public int? GenderReq { get; set; }
        public int? AgeReq { get; set; }
        public string? ReserveDatePersian { get; set; }
        public string? Status { get; set; }
        public int? ShiftId { get; set; }
        public int? DrId { get; set; }
        public int? InsuranceId { get; set; }
        public int? SectionId { get; set; }
        public string? CreateDatePersian { get; set; }
        public DateTime? ReserveDate { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}
