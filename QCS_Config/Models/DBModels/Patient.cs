using System;
using System.Collections.Generic;

namespace QCS_Config.Models
{
    public partial class Patient
    {
        public int Id { get; set; }
        public string? DosierId { get; set; }
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public string NationalCode { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public int InsuranceId { get; set; }
        public int Gender { get; set; }
    }
}
